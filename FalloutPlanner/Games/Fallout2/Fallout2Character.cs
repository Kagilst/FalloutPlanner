using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FalloutPlanner.Games.Fallout2.Perks;
using FalloutPlanner.Games.Fallout2.SkillBooks;
using FalloutPlanner.Games.Fallout2.Traits;

namespace FalloutPlanner.Games.Fallout2
{
    public class Fallout2Character : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private Stack<Fallout2CharacterStats> _history = new();
        private readonly Stack<Dictionary<string, int>> _skillIncreaseHistory = new();
        private readonly Stack<Fallout2Perk?> _perkHistory = new();
        private readonly Stack<Dictionary<string, int>> _skillBookCountHistory = new();

        private Fallout2CharacterStats _namedBaseline;

        private readonly List<CharacterActionState> _actionStates = new();
        public ObservableCollection<string> ActionLog { get; } = new();
        public string? ActiveSaveId { get; set; }
        public bool IsDirty { get; private set; }
        public void MarkClean() => IsDirty = false;
        public void MarkDirty() => IsDirty = true;

        public string GetSpecialLine(bool starting = false)
        {
            var stats = starting && _namedBaseline != null ? _namedBaseline : Current;
            return $"S {stats.Strength} / P {stats.Perception} / E {stats.Endurance} / " +
                   $"C {stats.Charisma} / I {stats.Intelligence} / A {stats.Agility} / L {stats.Luck}";
        }

        public string GetTaggedSkillsLine()
        {
            if (_taggedSkillNames == null || _taggedSkillNames.Count == 0)
                return "";
            return string.Join(", ", _taggedSkillNames.OrderBy(FormatSkillName).Select(FormatSkillName));
        }

        public Fallout2CharacterStats Current => _history.Peek();
        private Dictionary<string, int> SkillIncreasesThisLevel => _skillIncreaseHistory.Peek();
        private Dictionary<string, int> SkillBookCounts => _skillBookCountHistory.Peek();

        // Perk Logic
        public Dictionary<string, int> TakenPerks { get; } = new();

        public bool CanTakePerk(Fallout2Perk perk, int? levelOverride = null)
        {
            if (!perk.MeetsStatRequirements(this, levelOverride))
                return false;

            if (TakenPerks.ContainsKey(perk.Name) &&
                TakenPerks[perk.Name] >= perk.MaxRank)
                return false;

            return true;
        }

        public bool CanTakePerkOnLevelUp(Fallout2Perk perk) =>
            CanTakePerk(perk, Level + 1);

        public void TakePerk(Fallout2Perk perk)
        {
            if (!CanTakePerk(perk))
                return;

            ApplyPerk(perk);
            OnPropertyChanged(null);
        }

        private void ApplyPerk(Fallout2Perk perk)
        {
            if (!TakenPerks.ContainsKey(perk.Name))
                TakenPerks[perk.Name] = 0;

            TakenPerks[perk.Name]++;
            perk.ApplyEffect?.Invoke(this);
        }

        private void UntakePerk(Fallout2Perk perk)
        {
            if (!TakenPerks.ContainsKey(perk.Name))
                return;

            TakenPerks[perk.Name]--;
            if (TakenPerks[perk.Name] <= 0)
                TakenPerks.Remove(perk.Name);
        }

        // Skill Book Logic
        public int GetSkillBookCount(string bookName) =>
            SkillBookCounts.TryGetValue(bookName, out var n) ? n : 0;

        public bool HasComprehension => GetPerkRank("Comprehension") > 0;

        public bool TryReadSkillBook(string bookName)
        {
            var book = Fallout2SkillBookRepository.GetBookType(bookName);
            if (book == null)
                return false;

            int currentSkill = GetSkillValue(book.SkillPropertyName);
            if (currentSkill >= 300)
                return false;

            int gain = Fallout2SkillBookRepository.CalculateSkillGain(currentSkill, HasComprehension);

            RecordLoggedAction($"Read: {bookName} (+{gain}% {book.SkillName})", () =>
            {
                TryAddSkillPointsFromPerk(book.SkillPropertyName, gain);
                if (!SkillBookCounts.ContainsKey(bookName))
                    SkillBookCounts[bookName] = 0;
                SkillBookCounts[bookName]++;
            });

            return true;
        }

        /// <summary>
        /// FO2 skill-point cost to raise a skill one step from its current %.
        /// Tagged skills still pay this once and gain +2%.
        /// </summary>
        public static int GetSkillPointCost(int currentSkillPercent)
        {
            if (currentSkillPercent <= 100) return 1;
            if (currentSkillPercent <= 125) return 2;
            if (currentSkillPercent <= 150) return 3;
            if (currentSkillPercent <= 175) return 4;
            if (currentSkillPercent <= 200) return 5;
            return 6;
        }

        private int GetSkillInvestment(string skillName) =>
            _skillInvestment.TryGetValue(skillName, out var n) ? n : 0;

        private void AddSkillInvestment(string skillName, int delta)
        {
            int next = GetSkillInvestment(skillName) + delta;
            if (next <= 0)
                _skillInvestment.Remove(skillName);
            else
                _skillInvestment[skillName] = next;
        }

        // Skill Logic (FO2 wiki):
        // - Untagged: +1% per purchase; Tagged: +2% per purchase
        // - Cost scales with current skill % (1–6 SP); tagged still pays that cost once
        public bool ModifySkill(string skillName, int amount)
        {
            var property = GetType().GetProperty(skillName);

            if (property == null || property.PropertyType != typeof(int))
                return false;

            int currentValue = (int)property.GetValue(this);
            bool isTagged = _taggedSkillNames.Contains(skillName);
            int gain = isTagged ? 2 : 1;

            if (amount > 0)
            {
                if (currentValue >= 300)
                    return false;

                int cost = GetSkillPointCost(currentValue);
                if (SkillPoints < cost)
                    return false;

                int newValue = Math.Min(currentValue + gain, 300);
                int appliedGain = newValue - currentValue;
                if (appliedGain <= 0)
                    return false;

                AddSkillInvestment(skillName, appliedGain);
                property.SetValue(this, newValue);
                SkillPoints -= cost;

                if (!SkillIncreasesThisLevel.ContainsKey(skillName))
                    SkillIncreasesThisLevel[skillName] = 0;
                SkillIncreasesThisLevel[skillName]++; // purchases this level (for Undo -)

                NotifySkillChanged(skillName);
                return true;
            }

            if (amount < 0)
            {
                if (!SkillIncreasesThisLevel.ContainsKey(skillName) ||
                    SkillIncreasesThisLevel[skillName] <= 0)
                    return false;

                // Reverse one purchase: refund cost based on value before that purchase
                int previousValue = Math.Max(0, currentValue - gain);
                int refund = GetSkillPointCost(previousValue);
                int appliedLoss = currentValue - previousValue;

                AddSkillInvestment(skillName, -appliedLoss);
                property.SetValue(this, previousValue);
                SkillPoints += refund;
                SkillIncreasesThisLevel[skillName]--;

                NotifySkillChanged(skillName);
                return true;
            }

            return false;
        }

        private void NotifyAllSkillButtonStates()
        {
            OnPropertyChanged(nameof(CanIncreaseSmallGuns));
            OnPropertyChanged(nameof(CanDecreaseSmallGuns));
            OnPropertyChanged(nameof(CanIncreaseBigGuns));
            OnPropertyChanged(nameof(CanDecreaseBigGuns));
            OnPropertyChanged(nameof(CanIncreaseEnergyWeapons));
            OnPropertyChanged(nameof(CanDecreaseEnergyWeapons));
            OnPropertyChanged(nameof(CanIncreaseUnarmed));
            OnPropertyChanged(nameof(CanDecreaseUnarmed));
            OnPropertyChanged(nameof(CanIncreaseMeleeWeapons));
            OnPropertyChanged(nameof(CanDecreaseMeleeWeapons));
            OnPropertyChanged(nameof(CanIncreaseThrowing));
            OnPropertyChanged(nameof(CanDecreaseThrowing));
            OnPropertyChanged(nameof(CanIncreaseFirstAid));
            OnPropertyChanged(nameof(CanDecreaseFirstAid));
            OnPropertyChanged(nameof(CanIncreaseDoctor));
            OnPropertyChanged(nameof(CanDecreaseDoctor));
            OnPropertyChanged(nameof(CanIncreaseSneak));
            OnPropertyChanged(nameof(CanDecreaseSneak));
            OnPropertyChanged(nameof(CanIncreaseLockpick));
            OnPropertyChanged(nameof(CanDecreaseLockpick));
            OnPropertyChanged(nameof(CanIncreaseSteal));
            OnPropertyChanged(nameof(CanDecreaseSteal));
            OnPropertyChanged(nameof(CanIncreaseTraps));
            OnPropertyChanged(nameof(CanDecreaseTraps));
            OnPropertyChanged(nameof(CanIncreaseScience));
            OnPropertyChanged(nameof(CanDecreaseScience));
            OnPropertyChanged(nameof(CanIncreaseRepair));
            OnPropertyChanged(nameof(CanDecreaseRepair));
            OnPropertyChanged(nameof(CanIncreaseSpeech));
            OnPropertyChanged(nameof(CanDecreaseSpeech));
            OnPropertyChanged(nameof(CanIncreaseBarter));
            OnPropertyChanged(nameof(CanDecreaseBarter));
            OnPropertyChanged(nameof(CanIncreaseGambling));
            OnPropertyChanged(nameof(CanDecreaseGambling));
            OnPropertyChanged(nameof(CanIncreaseOutdoorsman));
            OnPropertyChanged(nameof(CanDecreaseOutdoorsman));
        }

        private void NotifySkillChanged(string skillName)
        {
            OnPropertyChanged(skillName);
            OnPropertyChanged(nameof(SkillPoints));
            OnPropertyChanged(nameof(CanLevelUp));
            NotifyAllSkillButtonStates();
        }

        public bool CanIncreaseSkill(string skillName)
        {
            var property = GetType().GetProperty(skillName);
            if (property == null) return false;
            int value = (int)property.GetValue(this);
            if (value >= 300) return false;
            int cost = GetSkillPointCost(value);
            return SkillPoints >= cost;
        }

        public bool CanDecreaseSkill(string skillName)
        {
            return SkillIncreasesThisLevel.ContainsKey(skillName) &&
                   SkillIncreasesThisLevel[skillName] > 0;
        }

        // Skill Can Increase/Decrease properties (FO2 - 18 skills)
        public bool CanIncreaseSmallGuns => CanIncreaseSkill(nameof(SmallGuns));
        public bool CanDecreaseSmallGuns => CanDecreaseSkill(nameof(SmallGuns));
        public bool CanIncreaseBigGuns => CanIncreaseSkill(nameof(BigGuns));
        public bool CanDecreaseBigGuns => CanDecreaseSkill(nameof(BigGuns));
        public bool CanIncreaseEnergyWeapons => CanIncreaseSkill(nameof(EnergyWeapons));
        public bool CanDecreaseEnergyWeapons => CanDecreaseSkill(nameof(EnergyWeapons));
        public bool CanIncreaseUnarmed => CanIncreaseSkill(nameof(Unarmed));
        public bool CanDecreaseUnarmed => CanDecreaseSkill(nameof(Unarmed));
        public bool CanIncreaseMeleeWeapons => CanIncreaseSkill(nameof(MeleeWeapons));
        public bool CanDecreaseMeleeWeapons => CanDecreaseSkill(nameof(MeleeWeapons));
        public bool CanIncreaseThrowing => CanIncreaseSkill(nameof(Throwing));
        public bool CanDecreaseThrowing => CanDecreaseSkill(nameof(Throwing));
        public bool CanIncreaseFirstAid => CanIncreaseSkill(nameof(FirstAid));
        public bool CanDecreaseFirstAid => CanDecreaseSkill(nameof(FirstAid));
        public bool CanIncreaseDoctor => CanIncreaseSkill(nameof(Doctor));
        public bool CanDecreaseDoctor => CanDecreaseSkill(nameof(Doctor));
        public bool CanIncreaseSneak => CanIncreaseSkill(nameof(Sneak));
        public bool CanDecreaseSneak => CanDecreaseSkill(nameof(Sneak));
        public bool CanIncreaseLockpick => CanIncreaseSkill(nameof(Lockpick));
        public bool CanDecreaseLockpick => CanDecreaseSkill(nameof(Lockpick));
        public bool CanIncreaseSteal => CanIncreaseSkill(nameof(Steal));
        public bool CanDecreaseSteal => CanDecreaseSkill(nameof(Steal));
        public bool CanIncreaseTraps => CanIncreaseSkill(nameof(Traps));
        public bool CanDecreaseTraps => CanDecreaseSkill(nameof(Traps));
        public bool CanIncreaseScience => CanIncreaseSkill(nameof(Science));
        public bool CanDecreaseScience => CanDecreaseSkill(nameof(Science));
        public bool CanIncreaseRepair => CanIncreaseSkill(nameof(Repair));
        public bool CanDecreaseRepair => CanDecreaseSkill(nameof(Repair));
        public bool CanIncreaseSpeech => CanIncreaseSkill(nameof(Speech));
        public bool CanDecreaseSpeech => CanDecreaseSkill(nameof(Speech));
        public bool CanIncreaseBarter => CanIncreaseSkill(nameof(Barter));
        public bool CanDecreaseBarter => CanDecreaseSkill(nameof(Barter));
        public bool CanIncreaseGambling => CanIncreaseSkill(nameof(Gambling));
        public bool CanDecreaseGambling => CanDecreaseSkill(nameof(Gambling));
        public bool CanIncreaseOutdoorsman => CanIncreaseSkill(nameof(Outdoorsman));
        public bool CanDecreaseOutdoorsman => CanDecreaseSkill(nameof(Outdoorsman));

        // Constructor
        public string Name { get; set; }
        public int Level => Current.Level;
        public bool IsMale
        {
            get => Current.IsMale;
            set
            {
                if (Current.IsMale != value)
                {
                    Current.IsMale = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Age
        {
            get => Current.Age;
            set
            {
                if (Current.Age != value)
                {
                    Current.Age = Math.Clamp(value, 16, 65);
                    OnPropertyChanged();
                }
            }
        }

        public List<string> Traits => Current.Traits;

        public bool HasTrait(string traitName) => Traits.Contains(traitName);

        public Fallout2Character(Fallout2CharacterStats baseStats)
        {
            _history.Push(baseStats.Clone());
            _skillIncreaseHistory.Push(new Dictionary<string, int>());
            _perkHistory.Push(null);
            _skillBookCountHistory.Push(new Dictionary<string, int>());
            RecalculateDerivedStats();
            OnPropertyChanged(null);
        }

        public void SaveInitialState()
        {
            SkillPoints = SkillPointsPerLevel;

            var snapshot = CreateCurrentSnapshot();
            _namedBaseline = snapshot.Clone();

            _history.Clear();
            _skillIncreaseHistory.Clear();
            _perkHistory.Clear();
            _skillBookCountHistory.Clear();
            TakenPerks.Clear();
            _skillInvestment.Clear();

            _history.Push(snapshot.Clone());
            _history.Push(snapshot);
            _skillIncreaseHistory.Push(new Dictionary<string, int>());
            _skillIncreaseHistory.Push(new Dictionary<string, int>());
            _perkHistory.Push(null);
            _perkHistory.Push(null);
            _skillBookCountHistory.Push(new Dictionary<string, int>());
            _skillBookCountHistory.Push(new Dictionary<string, int>());

            ResetActionLog($"Created Character: {Name}");
            OnPropertyChanged(null);
        }

        public void RevertToCreationState()
        {
            var working = (_namedBaseline ?? Current).Clone();
            working.SkillPoints = 0;
            _history.Clear();
            _skillIncreaseHistory.Clear();
            _perkHistory.Clear();
            _skillBookCountHistory.Clear();
            TakenPerks.Clear();
            _skillInvestment.Clear();
            _history.Push(working);
            _skillIncreaseHistory.Push(new Dictionary<string, int>());
            _perkHistory.Push(null);
            _skillBookCountHistory.Push(new Dictionary<string, int>());
            ActionLog.Clear();
            _actionStates.Clear();
            OnPropertyChanged(null);
        }

        private void ResetActionLog(string createdEntry)
        {
            ActionLog.Clear();
            _actionStates.Clear();
            ActionLog.Add(createdEntry);
            _actionStates.Add(CaptureActionState());
        }

        public CharacterActionState CaptureActionState()
        {
            return new CharacterActionState
            {
                Stats = CreateCurrentSnapshot(),
                SkillIncreases = new Dictionary<string, int>(SkillIncreasesThisLevel),
                TakenPerks = new Dictionary<string, int>(TakenPerks),
                SkillBookCounts = new Dictionary<string, int>(SkillBookCounts),
                SkillInvestment = new Dictionary<string, int>(_skillInvestment),
                TaggedSkillNames = new HashSet<string>(_taggedSkillNames)
            };
        }

        public void RestoreActionState(CharacterActionState state)
        {
            TakenPerks.Clear();
            foreach (var kvp in state.TakenPerks)
                TakenPerks[kvp.Key] = kvp.Value;

            _skillInvestment = new Dictionary<string, int>(state.SkillInvestment);
            _taggedSkillNames = new HashSet<string>(state.TaggedSkillNames ?? new HashSet<string>());

            _history.Clear();
            _skillIncreaseHistory.Clear();
            _perkHistory.Clear();
            _skillBookCountHistory.Clear();

            _history.Push((_namedBaseline ?? state.Stats).Clone());
            _history.Push(state.Stats.Clone());
            _skillIncreaseHistory.Push(new Dictionary<string, int>());
            _skillIncreaseHistory.Push(new Dictionary<string, int>(state.SkillIncreases));
            _perkHistory.Push(null);
            _perkHistory.Push(null);
            _skillBookCountHistory.Push(new Dictionary<string, int>());
            _skillBookCountHistory.Push(new Dictionary<string, int>(state.SkillBookCounts));

            OnPropertyChanged(null);
        }

        private void RecordLoggedAction(string description, Action apply)
        {
            if (_actionStates.Count > 0)
                _actionStates[0] = CaptureActionState();

            apply();

            _actionStates.Insert(0, CaptureActionState());
            ActionLog.Insert(0, description);
            IsDirty = true;
        }

        public bool TryUndoLastAction()
        {
            if (ActionLog.Count <= 1 || _actionStates.Count <= 1)
                return false;

            ActionLog.RemoveAt(0);
            _actionStates.RemoveAt(0);
            RestoreActionState(_actionStates[0]);
            return true;
        }

        public FalloutPlanner.Saves.Adapters.ClassicGameSavePayload<CharacterActionState> CreateSavePayload()
        {
            var payload = new FalloutPlanner.Saves.Adapters.ClassicGameSavePayload<CharacterActionState>
            {
                Current = CaptureActionState(),
                Baseline = _namedBaseline == null
                    ? null
                    : new CharacterActionState { Stats = _namedBaseline.Clone() },
                ActionLogNewestFirst = ActionLog.ToList()
            };
            foreach (var state in _actionStates)
                payload.ActionStates.Add(CloneState(state));
            return payload;
        }

        public void LoadFromSavePayload(
            FalloutPlanner.Saves.Adapters.ClassicGameSavePayload<CharacterActionState> payload,
            string? saveId)
        {
            if (payload.Baseline?.Stats != null)
                _namedBaseline = payload.Baseline.Stats.Clone();
            else if (payload.Current?.Stats != null)
                _namedBaseline = payload.Current.Stats.Clone();

            ActionLog.Clear();
            _actionStates.Clear();

            if (payload.ActionStates.Count > 0 && payload.ActionLogNewestFirst.Count > 0)
            {
                foreach (var entry in payload.ActionLogNewestFirst)
                    ActionLog.Add(entry);
                foreach (var state in payload.ActionStates)
                    _actionStates.Add(CloneState(state));
                RestoreActionState(_actionStates[0]);
            }
            else if (payload.Current != null)
            {
                RestoreActionState(CloneState(payload.Current));
                ResetActionLog($"Created Character: {Name}");
            }

            if (!string.IsNullOrWhiteSpace(Current.CharacterName))
                Name = Current.CharacterName;

            ActiveSaveId = saveId;
            MarkClean();
            OnPropertyChanged(null);
        }

        private static CharacterActionState CloneState(CharacterActionState state) =>
            System.Text.Json.JsonSerializer.Deserialize<CharacterActionState>(
                System.Text.Json.JsonSerializer.Serialize(state, FalloutPlanner.Saves.SaveJsonOptions.Options),
                FalloutPlanner.Saves.SaveJsonOptions.Options)!;

        private Fallout2CharacterStats CreateCurrentSnapshot()
        {
            return new Fallout2CharacterStats
            {
                Strength = Strength,
                Perception = Perception,
                Endurance = Endurance,
                Charisma = Charisma,
                Intelligence = Intelligence,
                Agility = Agility,
                Luck = Luck,
                Points = Points,

                CharacterName = Name,
                Level = Current.Level,
                IsMale = Current.IsMale,
                Age = Current.Age,
                Traits = new List<string>(Current.Traits),
                TaggedSkills = TaggedSkills,

                SmallGuns = SmallGuns,
                BigGuns = BigGuns,
                EnergyWeapons = EnergyWeapons,
                Unarmed = Unarmed,
                MeleeWeapons = MeleeWeapons,
                Throwing = Throwing,
                FirstAid = FirstAid,
                Doctor = Doctor,
                Sneak = Sneak,
                Lockpick = Lockpick,
                Steal = Steal,
                Traps = Traps,
                Science = Science,
                Repair = Repair,
                Speech = Speech,
                Barter = Barter,
                Gambling = Gambling,
                Outdoorsman = Outdoorsman,

                ActionPoints = ActionPoints,
                HitPoints = HitPoints,
                MaxHitPoints = MaxHitPoints,
                CarryWeight = CarryWeight,
                MeleeDamage = MeleeDamage,
                PoisonResist = PoisonResist,
                RadiationResist = RadiationResist,
                CriticalChance = CriticalChance,
                ArmorClass = ArmorClass,
                DamageResist = DamageResist,
                Sequence = Sequence,
                HealingRate = HealingRate,
                PerkRate = PerkRate,
                SkillPoints = SkillPoints
            };
        }

        // SPECIAL
        public void ModifySpecial(string statName, int amount)
        {
            var property = GetType().GetProperty(statName);

            if (property == null || property.PropertyType != typeof(int))
                return;

            int currentValue = (int)property.GetValue(this);
            int newValue = currentValue + amount;

            if (newValue < 1 || newValue > 10)
                return;

            if (amount > 0)
            {
                if (Points <= 0)
                    return;
                Points -= 1;
            }
            else
            {
                Points += 1;
            }

            property.SetValue(this, newValue);
            RecalculateDerivedStats();
        }

        // TAG logic
        private HashSet<string> _taggedSkillNames = new();
        /// <summary>Permanent skill % from spending points / books / perks (kept across SPECIAL recalc).</summary>
        private Dictionary<string, int> _skillInvestment = new();

        public bool ModifyTaggedSkill(string skillName, bool isToggled)
        {
            if (isToggled && TaggedSkills >= 3)
                return false;

            if (isToggled)
            {
                if (!_taggedSkillNames.Add(skillName))
                    return true;
                TaggedSkills++;
            }
            else
            {
                if (!_taggedSkillNames.Remove(skillName))
                    return true;
                TaggedSkills--;
            }

            RecalculateDerivedStats();
            OnPropertyChanged(nameof(TaggedSkills));
            OnPropertyChanged(nameof(CanConfirm));

            return true;
        }

        // SPECIAL Properties
        public int Strength
        {
            get => Current.Strength;
            set
            {
                if (Current.Strength != value)
                {
                    Current.Strength = value;
                    RecalculateDerivedStats();
                    OnPropertyChanged();
                }
            }
        }

        public int Perception
        {
            get => Current.Perception;
            set
            {
                if (Current.Perception != value)
                {
                    Current.Perception = value;
                    RecalculateDerivedStats();
                    OnPropertyChanged();
                }
            }
        }

        public int Endurance
        {
            get => Current.Endurance;
            set
            {
                if (Current.Endurance != value)
                {
                    Current.Endurance = value;
                    RecalculateDerivedStats();
                    OnPropertyChanged();
                }
            }
        }

        public int Charisma
        {
            get => Current.Charisma;
            set
            {
                if (Current.Charisma != value)
                {
                    Current.Charisma = value;
                    RecalculateDerivedStats();
                    OnPropertyChanged();
                }
            }
        }

        public int Intelligence
        {
            get => Current.Intelligence;
            set
            {
                if (Current.Intelligence != value)
                {
                    Current.Intelligence = value;
                    RecalculateDerivedStats();
                    OnPropertyChanged();
                }
            }
        }

        public int Agility
        {
            get => Current.Agility;
            set
            {
                if (Current.Agility != value)
                {
                    Current.Agility = value;
                    RecalculateDerivedStats();
                    OnPropertyChanged();
                }
            }
        }

        public int Luck
        {
            get => Current.Luck;
            set
            {
                if (Current.Luck != value)
                {
                    Current.Luck = value;
                    RecalculateDerivedStats();
                    OnPropertyChanged();
                }
            }
        }

        public int Points
        {
            get => Current.Points;
            set
            {
                if (Current.Points != value)
                {
                    Current.Points = value;
                    RecalculateDerivedStats();
                    OnPropertyChanged();
                }
            }
        }

        // Derived Stats Properties
        public int ActionPoints
        {
            get => Current.ActionPoints;
            set { if (Current.ActionPoints != value) { Current.ActionPoints = value; OnPropertyChanged(); } }
        }

        public int HitPoints
        {
            get => Current.HitPoints;
            set { if (Current.HitPoints != value) { Current.HitPoints = value; OnPropertyChanged(); } }
        }

        public int MaxHitPoints
        {
            get => Current.MaxHitPoints;
            set { if (Current.MaxHitPoints != value) { Current.MaxHitPoints = value; OnPropertyChanged(); } }
        }

        public int CarryWeight
        {
            get => Current.CarryWeight;
            set { if (Current.CarryWeight != value) { Current.CarryWeight = value; OnPropertyChanged(); } }
        }

        public int CriticalChance
        {
            get => Current.CriticalChance;
            set { if (Current.CriticalChance != value) { Current.CriticalChance = value; OnPropertyChanged(); } }
        }

        public int ArmorClass
        {
            get => Current.ArmorClass;
            set { if (Current.ArmorClass != value) { Current.ArmorClass = value; OnPropertyChanged(); } }
        }

        public int DamageResist
        {
            get => Current.DamageResist;
            set { if (Current.DamageResist != value) { Current.DamageResist = value; OnPropertyChanged(); } }
        }

        public int MeleeDamage
        {
            get => Current.MeleeDamage;
            set { if (Current.MeleeDamage != value) { Current.MeleeDamage = value; OnPropertyChanged(); } }
        }

        public int PoisonResist
        {
            get => Current.PoisonResist;
            set { if (Current.PoisonResist != value) { Current.PoisonResist = value; OnPropertyChanged(); } }
        }

        public int RadiationResist
        {
            get => Current.RadiationResist;
            set { if (Current.RadiationResist != value) { Current.RadiationResist = value; OnPropertyChanged(); } }
        }

        public int Sequence
        {
            get => Current.Sequence;
            set { if (Current.Sequence != value) { Current.Sequence = value; OnPropertyChanged(); } }
        }

        public int HealingRate
        {
            get => Current.HealingRate;
            set { if (Current.HealingRate != value) { Current.HealingRate = value; OnPropertyChanged(); } }
        }

        public int PerkRate
        {
            get => Current.PerkRate;
            set { if (Current.PerkRate != value) { Current.PerkRate = value; OnPropertyChanged(); } }
        }

        public int SkillPoints
        {
            get => Current.SkillPoints;
            set
            {
                if (Current.SkillPoints != value)
                {
                    Current.SkillPoints = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanLevelUp));
                    NotifyAllSkillButtonStates();
                }
            }
        }

        public int TaggedSkills
        {
            get => Current.TaggedSkills;
            set
            {
                if (Current.TaggedSkills != value)
                {
                    Current.TaggedSkills = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanConfirm));
                }
            }
        }

        // Skills Properties (FO2 - 18 skills)
        public int SmallGuns
        {
            get => Current.SmallGuns;
            set { if (Current.SmallGuns != value) { Current.SmallGuns = value; OnPropertyChanged(); } }
        }

        public int BigGuns
        {
            get => Current.BigGuns;
            set { if (Current.BigGuns != value) { Current.BigGuns = value; OnPropertyChanged(); } }
        }

        public int EnergyWeapons
        {
            get => Current.EnergyWeapons;
            set { if (Current.EnergyWeapons != value) { Current.EnergyWeapons = value; OnPropertyChanged(); } }
        }

        public int Unarmed
        {
            get => Current.Unarmed;
            set { if (Current.Unarmed != value) { Current.Unarmed = value; OnPropertyChanged(); } }
        }

        public int MeleeWeapons
        {
            get => Current.MeleeWeapons;
            set { if (Current.MeleeWeapons != value) { Current.MeleeWeapons = value; OnPropertyChanged(); } }
        }

        public int Throwing
        {
            get => Current.Throwing;
            set { if (Current.Throwing != value) { Current.Throwing = value; OnPropertyChanged(); } }
        }

        public int FirstAid
        {
            get => Current.FirstAid;
            set { if (Current.FirstAid != value) { Current.FirstAid = value; OnPropertyChanged(); } }
        }

        public int Doctor
        {
            get => Current.Doctor;
            set { if (Current.Doctor != value) { Current.Doctor = value; OnPropertyChanged(); } }
        }

        public int Sneak
        {
            get => Current.Sneak;
            set { if (Current.Sneak != value) { Current.Sneak = value; OnPropertyChanged(); } }
        }

        public int Lockpick
        {
            get => Current.Lockpick;
            set { if (Current.Lockpick != value) { Current.Lockpick = value; OnPropertyChanged(); } }
        }

        public int Steal
        {
            get => Current.Steal;
            set { if (Current.Steal != value) { Current.Steal = value; OnPropertyChanged(); } }
        }

        public int Traps
        {
            get => Current.Traps;
            set { if (Current.Traps != value) { Current.Traps = value; OnPropertyChanged(); } }
        }

        public int Science
        {
            get => Current.Science;
            set { if (Current.Science != value) { Current.Science = value; OnPropertyChanged(); } }
        }

        public int Repair
        {
            get => Current.Repair;
            set { if (Current.Repair != value) { Current.Repair = value; OnPropertyChanged(); } }
        }

        public int Speech
        {
            get => Current.Speech;
            set { if (Current.Speech != value) { Current.Speech = value; OnPropertyChanged(); } }
        }

        public int Barter
        {
            get => Current.Barter;
            set { if (Current.Barter != value) { Current.Barter = value; OnPropertyChanged(); } }
        }

        public int Gambling
        {
            get => Current.Gambling;
            set { if (Current.Gambling != value) { Current.Gambling = value; OnPropertyChanged(); } }
        }

        public int Outdoorsman
        {
            get => Current.Outdoorsman;
            set { if (Current.Outdoorsman != value) { Current.Outdoorsman = value; OnPropertyChanged(); } }
        }

        // Skill calculation with tags, traits, and invested skill points
        private int CalculateSkill(int baseValue, string skillName)
        {
            int value = baseValue;

            // Tagged skill gets +20%
            if (_taggedSkillNames.Contains(skillName))
                value += 20;

            // Skilled trait: +10% to all skills
            if (HasTrait(Fallout2TraitRepository.Skilled))
                value += 10;

            // Gifted trait: -10% to all skills
            if (HasTrait(Fallout2TraitRepository.Gifted))
                value -= 10;

            // Good Natured trait
            if (HasTrait(Fallout2TraitRepository.GoodNatured))
            {
                switch (skillName)
                {
                    case nameof(FirstAid):
                    case nameof(Doctor):
                    case nameof(Speech):
                    case nameof(Barter):
                        value += 15;
                        break;
                    case nameof(SmallGuns):
                    case nameof(BigGuns):
                    case nameof(EnergyWeapons):
                    case nameof(Unarmed):
                    case nameof(MeleeWeapons):
                    case nameof(Throwing):
                        value -= 10;
                        break;
                }
            }

            value += GetSkillInvestment(skillName);

            return Math.Clamp(value, 0, 300);
        }

        public void RecalculateDerivedStats()
        {
            var stats = Current;

            // FO2 Derived Stat Formulas
            // AP = 5 + AG/2 (Bruiser -2)
            stats.ActionPoints = 5 + (stats.Agility / 2);
            if (HasTrait(Fallout2TraitRepository.Bruiser))
                stats.ActionPoints -= 2;

            // HP = 15 + ST + (2*EN) + Level*EN/2
            stats.MaxHitPoints = 15 + stats.Strength + (2 * stats.Endurance) + (stats.Level * stats.Endurance / 2);
            stats.HitPoints = stats.MaxHitPoints;

            // AC = AG (Kamikaze sets to 0)
            stats.ArmorClass = stats.Agility;
            if (HasTrait(Fallout2TraitRepository.Kamikaze))
                stats.ArmorClass = 0;

            // Melee Damage = ST - 5 (min 1) (Heavy Handed +4)
            stats.MeleeDamage = Math.Max(1, stats.Strength - 5);
            if (HasTrait(Fallout2TraitRepository.HeavyHanded))
                stats.MeleeDamage += 4;

            // Carry Weight = 25 + 25*ST (Small Frame -25)
            stats.CarryWeight = 25 + (25 * stats.Strength);
            if (HasTrait(Fallout2TraitRepository.SmallFrame))
                stats.CarryWeight -= 25;

            // Sequence = 2*PE (Kamikaze +5)
            stats.Sequence = 2 * stats.Perception;
            if (HasTrait(Fallout2TraitRepository.Kamikaze))
                stats.Sequence += 5;

            // Healing Rate = EN/3 (min 1) (Fast Metabolism +2)
            stats.HealingRate = Math.Max(1, stats.Endurance / 3);
            if (HasTrait(Fallout2TraitRepository.FastMetabolism))
                stats.HealingRate += 2;

            // Critical Chance = LK (Finesse +10)
            stats.CriticalChance = stats.Luck;
            if (HasTrait(Fallout2TraitRepository.Finesse))
                stats.CriticalChance += 10;

            // Poison Resist = 5*EN (Fast Metabolism sets to 0)
            stats.PoisonResist = 5 * stats.Endurance;
            if (HasTrait(Fallout2TraitRepository.FastMetabolism))
                stats.PoisonResist = 0;

            // Rad Resist = 2*EN (Fast Metabolism sets to 0)
            stats.RadiationResist = 2 * stats.Endurance;
            if (HasTrait(Fallout2TraitRepository.FastMetabolism))
                stats.RadiationResist = 0;

            // Perk Rate (default 3, Skilled makes it 4)
            stats.PerkRate = HasTrait(Fallout2TraitRepository.Skilled) ? 4 : 3;

            // Skill calculations using FO2 formulas
            // Small Guns = 5 + 4*AG
            stats.SmallGuns = CalculateSkill(5 + (4 * stats.Agility), nameof(SmallGuns));
            // Big Guns = 2*AG
            stats.BigGuns = CalculateSkill(2 * stats.Agility, nameof(BigGuns));
            // Energy Weapons = 2*AG
            stats.EnergyWeapons = CalculateSkill(2 * stats.Agility, nameof(EnergyWeapons));
            // Unarmed = 30 + 2*((AG+ST)/2)
            stats.Unarmed = CalculateSkill(30 + (2 * ((stats.Agility + stats.Strength) / 2)), nameof(Unarmed));
            // Melee Weapons = 20 + 2*((AG+ST)/2)
            stats.MeleeWeapons = CalculateSkill(20 + (2 * ((stats.Agility + stats.Strength) / 2)), nameof(MeleeWeapons));
            // Throwing = 4*AG
            stats.Throwing = CalculateSkill(4 * stats.Agility, nameof(Throwing));
            // First Aid = 2*(PE+IN)
            stats.FirstAid = CalculateSkill(2 * (stats.Perception + stats.Intelligence), nameof(FirstAid));
            // Doctor = 5 + PE + IN
            stats.Doctor = CalculateSkill(5 + stats.Perception + stats.Intelligence, nameof(Doctor));
            // Sneak = 5 + 3*AG
            stats.Sneak = CalculateSkill(5 + (3 * stats.Agility), nameof(Sneak));
            // Lockpick = 10 + PE + AG
            stats.Lockpick = CalculateSkill(10 + stats.Perception + stats.Agility, nameof(Lockpick));
            // Steal = 3*AG
            stats.Steal = CalculateSkill(3 * stats.Agility, nameof(Steal));
            // Traps = 10 + PE + AG
            stats.Traps = CalculateSkill(10 + stats.Perception + stats.Agility, nameof(Traps));
            // Science = 4*IN
            stats.Science = CalculateSkill(4 * stats.Intelligence, nameof(Science));
            // Repair = 3*IN
            stats.Repair = CalculateSkill(3 * stats.Intelligence, nameof(Repair));
            // Speech = 5*CH
            stats.Speech = CalculateSkill(5 * stats.Charisma, nameof(Speech));
            // Barter = 4*CH
            stats.Barter = CalculateSkill(4 * stats.Charisma, nameof(Barter));
            // Gambling = 5*LK
            stats.Gambling = CalculateSkill(5 * stats.Luck, nameof(Gambling));
            // Outdoorsman = 2*(EN+IN)
            stats.Outdoorsman = CalculateSkill(2 * (stats.Endurance + stats.Intelligence), nameof(Outdoorsman));

            OnPropertyChanged(null);
            OnPropertyChanged(nameof(SkillPointsPerLevel));
        }

        // Create Character Confirmation
        public bool CanConfirm => Points == 0 && TaggedSkills == 3;

        /// <summary>
        /// Max level is 99 for FO2.
        /// </summary>
        public int MaxLevel => 99;

        public bool IsMaxLevel => Level >= MaxLevel;

        public bool AllSkillsAtMax =>
            SmallGuns >= 300 && BigGuns >= 300 && EnergyWeapons >= 300 &&
            Unarmed >= 300 && MeleeWeapons >= 300 && Throwing >= 300 &&
            FirstAid >= 300 && Doctor >= 300 && Sneak >= 300 &&
            Lockpick >= 300 && Steal >= 300 && Traps >= 300 &&
            Science >= 300 && Repair >= 300 && Speech >= 300 &&
            Barter >= 300 && Gambling >= 300 && Outdoorsman >= 300;

        /// <summary>
        /// Level up when all skill points are spent (or all skills at 300) and not at max level.
        /// </summary>
        public bool CanLevelUp =>
            !IsMaxLevel && (SkillPoints == 0 || AllSkillsAtMax);

        /// <summary>
        /// Whether the next level grants a perk pick (every 3 levels, or 4 with Skilled).
        /// </summary>
        public bool NextLevelRequiresPerk => (Level + 1) % PerkRate == 0;

        /// <summary>
        /// Skill points per level = 5 + 2*IN (Skilled +5, Gifted -5)
        /// </summary>
        public int SkillPointsPerLevel
        {
            get
            {
                int points = 5 + (2 * Intelligence);
                if (HasTrait(Fallout2TraitRepository.Skilled))
                    points += 5;
                if (HasTrait(Fallout2TraitRepository.Gifted))
                    points -= 5;

                // Educated perk adds +2 per rank
                points += GetPerkRank("Educated") * 2;

                return Math.Max(1, points);
            }
        }

        public int GetPerkRank(string perkName) =>
            TakenPerks.TryGetValue(perkName, out var rank) ? rank : 0;

        public int GetSkillValue(string skillPropertyName)
        {
            var property = GetType().GetProperty(skillPropertyName);
            if (property == null || property.PropertyType != typeof(int))
                return 0;
            return (int)property.GetValue(this);
        }

        public bool TryAddSkillPointsFromPerk(string skillName, int amount)
        {
            var property = GetType().GetProperty(skillName);
            if (property == null || property.PropertyType != typeof(int))
                return false;

            int current = (int)property.GetValue(this);
            int updated = Math.Clamp(current + amount, 0, 300);
            int applied = updated - current;
            if (applied == 0)
                return false;

            AddSkillInvestment(skillName, applied);
            property.SetValue(this, updated);
            OnPropertyChanged(skillName);
            return true;
        }

        private static string FormatSkillName(string propertyName) => propertyName switch
        {
            nameof(SmallGuns) => "Small Guns",
            nameof(BigGuns) => "Big Guns",
            nameof(EnergyWeapons) => "Energy Weapons",
            nameof(MeleeWeapons) => "Melee Weapons",
            nameof(FirstAid) => "First Aid",
            _ => propertyName
        };

        private static string FormatSkillSpendSummary(Dictionary<string, int> increases)
        {
            var parts = increases
                .Where(kvp => kvp.Value > 0)
                .OrderBy(kvp => FormatSkillName(kvp.Key))
                .Select(kvp => $"{FormatSkillName(kvp.Key)} +{kvp.Value}");
            return string.Join(", ", parts);
        }

        /// <summary>
        /// Level up the character. In FO2, perks are chosen every 3 levels (4 with Skilled).
        /// </summary>
        public void LevelUp(Fallout2Perk perk = null, string tagSkill = null)
        {
            int nextLevel = Level + 1;
            bool needsPerk = nextLevel % PerkRate == 0;

            string spendSummary = FormatSkillSpendSummary(SkillIncreasesThisLevel);
            string description;
            if (needsPerk && perk != null)
            {
                string perkLabel = string.IsNullOrEmpty(tagSkill)
                    ? perk.Name
                    : $"{perk.Name} {FormatSkillName(tagSkill)}";
                description = string.IsNullOrEmpty(spendSummary)
                    ? $"Level Up: Level {nextLevel}, Perk: {perkLabel}"
                    : $"Level Up: Level {nextLevel}, {spendSummary}, Perk: {perkLabel}";
            }
            else
            {
                description = string.IsNullOrEmpty(spendSummary)
                    ? $"Level Up: Level {nextLevel}"
                    : $"Level Up: Level {nextLevel}, {spendSummary}";
            }

            RecordLoggedAction(description, () =>
            {
                var newStats = Current.Clone();
                newStats.Level++;
                // Carry unspent points (wiki); leftover only when skills are already capped
                newStats.SkillPoints = SkillPoints + SkillPointsPerLevel;

                // Recalculate HP for new level
                newStats.MaxHitPoints = 15 + newStats.Strength + (2 * newStats.Endurance) + (newStats.Level * newStats.Endurance / 2);
                newStats.HitPoints = newStats.MaxHitPoints;

                _history.Push(newStats);
                _skillIncreaseHistory.Push(new Dictionary<string, int>());
                _skillBookCountHistory.Push(new Dictionary<string, int>(SkillBookCounts));

                if (needsPerk && perk != null)
                {
                    ApplyPerk(perk);
                    _perkHistory.Push(perk);

                    if (!string.IsNullOrEmpty(tagSkill))
                        TryAddSkillPointsFromPerk(tagSkill, 20);
                }
                else
                {
                    _perkHistory.Push(null);
                }

                OnPropertyChanged(null);
                OnPropertyChanged(nameof(CanLevelUp));
            });
        }

        public bool TryUndoLevel() => TryUndoLastAction();

        public void UndoLevel()
        {
            TryUndoLastAction();
        }
    }
}
