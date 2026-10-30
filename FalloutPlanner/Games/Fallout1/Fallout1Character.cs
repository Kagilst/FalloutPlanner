using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FalloutPlanner.Games.Fallout1.Perks;
using FalloutPlanner.Games.Fallout1.SkillBooks;
using FalloutPlanner.Games.Fallout1.Traits;

namespace FalloutPlanner.Games.Fallout1
{
    public class Fallout1Character : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private Stack<Fallout1CharacterStats> _history = new();
        private readonly Stack<Dictionary<string, int>> _skillIncreaseHistory = new();
        private readonly Stack<Fallout1Perk?> _perkHistory = new();
        private readonly Stack<Dictionary<string, int>> _skillBookCountHistory = new();

        private Fallout1CharacterStats? _namedBaseline;
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
            var tags = (_namedBaseline ?? Current).TaggedSkills;
            if (tags == null || tags.Count == 0)
                return "";
            return string.Join(", ", tags.OrderBy(n => n).Select(FormatTaggedSkillName));
        }

        private static string FormatTaggedSkillName(string propertyName) => propertyName switch
        {
            nameof(SmallGuns) => "Small Guns",
            nameof(BigGuns) => "Big Guns",
            nameof(EnergyWeapons) => "Energy Weapons",
            nameof(Unarmed) => "Unarmed",
            nameof(MeleeWeapons) => "Melee Weapons",
            nameof(Throwing) => "Throwing",
            nameof(FirstAid) => "First Aid",
            nameof(Doctor) => "Doctor",
            nameof(Sneak) => "Sneak",
            nameof(Lockpick) => "Lockpick",
            nameof(Steal) => "Steal",
            nameof(Traps) => "Traps",
            nameof(Science) => "Science",
            nameof(Repair) => "Repair",
            nameof(Speech) => "Speech",
            nameof(Barter) => "Barter",
            nameof(Gambling) => "Gambling",
            nameof(Outdoorsman) => "Outdoorsman",
            _ => propertyName
        };

        public Fallout1CharacterStats Current => _history.Peek();
        private Dictionary<string, int> SkillIncreasesThisLevel => _skillIncreaseHistory.Peek();
        private Dictionary<string, int> SkillBookCounts => _skillBookCountHistory.Peek();

        // Perk Logic
        public Dictionary<string, int> TakenPerks { get; } = new();

        public IReadOnlyList<string> OwnedPerkNames =>
            TakenPerks.SelectMany(kvp => Enumerable.Repeat(kvp.Key, kvp.Value)).ToList();

        public bool CanTakePerk(Fallout1Perk perk, int? levelOverride = null)
        {
            if (!perk.MeetsStatRequirements(this, levelOverride))
                return false;

            if (TakenPerks.ContainsKey(perk.Name) && TakenPerks[perk.Name] >= perk.MaxRank)
                return false;

            return true;
        }

        public bool CanTakePerkOnLevelUp(Fallout1Perk perk) => CanTakePerk(perk, Level + 1);

        public void TakePerk(Fallout1Perk perk)
        {
            if (!CanTakePerk(perk))
                return;

            ApplyPerk(perk);
            OnPropertyChanged(null);
        }

        private void ApplyPerk(Fallout1Perk perk)
        {
            if (!TakenPerks.ContainsKey(perk.Name))
                TakenPerks[perk.Name] = 0;

            TakenPerks[perk.Name]++;
            perk.ApplyEffect?.Invoke(this);
        }

        public int GetPerkRank(string perkName) =>
            TakenPerks.TryGetValue(perkName, out var rank) ? rank : 0;

        // Skill Book Logic
        public int GetSkillBookCount(string bookName) =>
            SkillBookCounts.TryGetValue(bookName, out var n) ? n : 0;

        public int SkillBookPointsPerBook => GetPerkRank("Comprehension") > 0 ? 2 : 1;

        public bool TryAddSkillBook(string bookName)
        {
            var book = Fallout1SkillBookRepository.GetBook(bookName);
            if (book == null) return false;

            int current = GetSkillBookCount(bookName);
            if (book.MaxBooks >= 0 && current >= book.MaxBooks)
                return false;

            RecordLoggedAction($"Read: {bookName}", () =>
            {
                ApplySkillBookCountDelta(book, +1);
            });
            return true;
        }

        private void ApplySkillBookCountDelta(Fallout1SkillBook book, int deltaBooks)
        {
            if (deltaBooks == 0)
                return;

            int current = GetSkillBookCount(book.Name);
            int newCount = current + deltaBooks;
            if (book.MaxBooks >= 0)
                newCount = Math.Clamp(newCount, 0, book.MaxBooks);
            else
                newCount = Math.Max(0, newCount);

            int appliedDelta = newCount - current;
            if (appliedDelta == 0)
                return;

            TryAddSkillPoints(book.SkillPropertyName, appliedDelta * SkillBookPointsPerBook);

            if (newCount == 0)
                SkillBookCounts.Remove(book.Name);
            else
                SkillBookCounts[book.Name] = newCount;

            OnPropertyChanged(null);
        }

        // Skill Logic
        public bool ModifySkill(string skillName, int amount)
        {
            var property = GetType().GetProperty(skillName);
            if (property == null || property.PropertyType != typeof(int))
                return false;

            int currentValue = (int)property.GetValue(this)!;
            int newValue = currentValue + amount;
            bool isTagged = Current.TaggedSkills.Contains(skillName);

            if (amount > 0)
            {
                int cost = isTagged ? 1 : 1; // In FO1, tagged skills give 2% per point spent
                int gain = isTagged ? 2 : 1;

                if (SkillPoints < cost || currentValue >= 300)
                    return false;

                property.SetValue(this, Math.Min(newValue + (isTagged ? 1 : 0), 300));
                SkillPoints -= cost;

                if (!SkillIncreasesThisLevel.ContainsKey(skillName))
                    SkillIncreasesThisLevel[skillName] = 0;
                SkillIncreasesThisLevel[skillName] += gain;

                NotifySkillChanged(skillName);
                return true;
            }

            if (amount < 0)
            {
                if (!SkillIncreasesThisLevel.ContainsKey(skillName) ||
                    SkillIncreasesThisLevel[skillName] <= 0)
                    return false;

                int gain = isTagged ? 2 : 1;
                property.SetValue(this, currentValue - gain);
                SkillPoints++;
                SkillIncreasesThisLevel[skillName] -= gain;

                NotifySkillChanged(skillName);
                return true;
            }

            return false;
        }

        private void NotifySkillChanged(string skillName)
        {
            OnPropertyChanged(skillName);
            OnPropertyChanged(nameof(SkillPoints));
            OnPropertyChanged(nameof(CanLevelUp));
            NotifyAllSkillButtonStates();
        }

        private void NotifyAllSkillButtonStates()
        {
            OnPropertyChanged(nameof(CanIncreaseSmallGuns)); OnPropertyChanged(nameof(CanDecreaseSmallGuns));
            OnPropertyChanged(nameof(CanIncreaseBigGuns)); OnPropertyChanged(nameof(CanDecreaseBigGuns));
            OnPropertyChanged(nameof(CanIncreaseEnergyWeapons)); OnPropertyChanged(nameof(CanDecreaseEnergyWeapons));
            OnPropertyChanged(nameof(CanIncreaseUnarmed)); OnPropertyChanged(nameof(CanDecreaseUnarmed));
            OnPropertyChanged(nameof(CanIncreaseMeleeWeapons)); OnPropertyChanged(nameof(CanDecreaseMeleeWeapons));
            OnPropertyChanged(nameof(CanIncreaseThrowing)); OnPropertyChanged(nameof(CanDecreaseThrowing));
            OnPropertyChanged(nameof(CanIncreaseFirstAid)); OnPropertyChanged(nameof(CanDecreaseFirstAid));
            OnPropertyChanged(nameof(CanIncreaseDoctor)); OnPropertyChanged(nameof(CanDecreaseDoctor));
            OnPropertyChanged(nameof(CanIncreaseSneak)); OnPropertyChanged(nameof(CanDecreaseSneak));
            OnPropertyChanged(nameof(CanIncreaseLockpick)); OnPropertyChanged(nameof(CanDecreaseLockpick));
            OnPropertyChanged(nameof(CanIncreaseSteal)); OnPropertyChanged(nameof(CanDecreaseSteal));
            OnPropertyChanged(nameof(CanIncreaseTraps)); OnPropertyChanged(nameof(CanDecreaseTraps));
            OnPropertyChanged(nameof(CanIncreaseScience)); OnPropertyChanged(nameof(CanDecreaseScience));
            OnPropertyChanged(nameof(CanIncreaseRepair)); OnPropertyChanged(nameof(CanDecreaseRepair));
            OnPropertyChanged(nameof(CanIncreaseSpeech)); OnPropertyChanged(nameof(CanDecreaseSpeech));
            OnPropertyChanged(nameof(CanIncreaseBarter)); OnPropertyChanged(nameof(CanDecreaseBarter));
            OnPropertyChanged(nameof(CanIncreaseGambling)); OnPropertyChanged(nameof(CanDecreaseGambling));
            OnPropertyChanged(nameof(CanIncreaseOutdoorsman)); OnPropertyChanged(nameof(CanDecreaseOutdoorsman));
        }

        public bool CanIncreaseSkill(string skillName)
        {
            var property = GetType().GetProperty(skillName);
            if (property == null) return false;
            int value = (int)property.GetValue(this)!;
            return SkillPoints > 0 && value < 300;
        }

        public bool CanDecreaseSkill(string skillName)
        {
            return SkillIncreasesThisLevel.ContainsKey(skillName) &&
                   SkillIncreasesThisLevel[skillName] > 0;
        }

        // Skill CanIncrease/CanDecrease properties
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
        public string Name
        {
            get => Current.CharacterName;
            set
            {
                if (Current.CharacterName != value)
                {
                    Current.CharacterName = value;
                    OnPropertyChanged();
                }
            }
        }

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
                    Current.Age = Math.Clamp(value, 16, 35);
                    OnPropertyChanged();
                }
            }
        }

        public List<string> Traits => Current.Traits;
        public bool HasTrait(string traitName) => Traits.Contains(traitName);
        public int TraitCount => Traits.Count;

        public bool TryAddTrait(string traitName)
        {
            if (Traits.Count >= 2 || Traits.Contains(traitName))
                return false;

            Traits.Add(traitName);
            var trait = Fallout1TraitRepository.GetTrait(traitName);
            if (trait?.HasPermanentEffect == true)
                trait.ApplyEffect?.Invoke(this);

            RecalculateDerivedStats();
            OnPropertyChanged(nameof(Traits));
            OnPropertyChanged(nameof(TraitCount));
            return true;
        }

        public bool TryRemoveTrait(string traitName)
        {
            if (!Traits.Contains(traitName))
                return false;

            var trait = Fallout1TraitRepository.GetTrait(traitName);
            if (trait?.HasPermanentEffect == true)
                trait.RemoveEffect?.Invoke(this);

            Traits.Remove(traitName);
            RecalculateDerivedStats();
            OnPropertyChanged(nameof(Traits));
            OnPropertyChanged(nameof(TraitCount));
            return true;
        }

        public HashSet<string> TaggedSkillNames => Current.TaggedSkills;
        public int TaggedSkillCount => Current.TaggedSkills.Count;

        public bool IsSkillTagged(string skillName) => Current.TaggedSkills.Contains(skillName);

        public bool ModifyTaggedSkill(string skillName, bool isToggled)
        {
            if (isToggled && TaggedSkillCount >= 3)
                return false;

            if (isToggled)
            {
                if (!Current.TaggedSkills.Add(skillName))
                    return true;
            }
            else
            {
                if (!Current.TaggedSkills.Remove(skillName))
                    return true;
            }

            RecalculateDerivedStats();
            OnPropertyChanged(nameof(TaggedSkillCount));
            OnPropertyChanged(nameof(CanConfirm));
            return true;
        }

        public Fallout1Character(Fallout1CharacterStats baseStats)
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

        /// <summary>
        /// Collapses history when returning from the level page to character creation.
        /// </summary>
        public void RevertToCreationState()
        {
            var working = (_namedBaseline ?? Current).Clone();
            working.SkillPoints = 0;
            _history.Clear();
            _skillIncreaseHistory.Clear();
            _perkHistory.Clear();
            _skillBookCountHistory.Clear();
            TakenPerks.Clear();
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
            var lastPerk = _perkHistory.Count > 0 ? _perkHistory.Peek() : null;
            return new CharacterActionState
            {
                Stats = CreateCurrentSnapshot(),
                SkillIncreases = new Dictionary<string, int>(SkillIncreasesThisLevel),
                TakenPerks = new Dictionary<string, int>(TakenPerks),
                LastPerkName = lastPerk?.Name,
                SkillBookCounts = new Dictionary<string, int>(SkillBookCounts)
            };
        }

        public void RestoreActionState(CharacterActionState state)
        {
            TakenPerks.Clear();
            foreach (var kvp in state.TakenPerks)
                TakenPerks[kvp.Key] = kvp.Value;

            Fallout1Perk? lastPerk = null;
            if (!string.IsNullOrEmpty(state.LastPerkName))
                lastPerk = Fallout1PerkRepository.AllPerks.FirstOrDefault(p => p.Name == state.LastPerkName);

            _history.Clear();
            _skillIncreaseHistory.Clear();
            _perkHistory.Clear();
            _skillBookCountHistory.Clear();

            _history.Push((_namedBaseline ?? state.Stats).Clone());
            _history.Push(state.Stats.Clone());
            _skillIncreaseHistory.Push(new Dictionary<string, int>());
            _skillIncreaseHistory.Push(new Dictionary<string, int>(state.SkillIncreases));
            _perkHistory.Push(null);
            _perkHistory.Push(lastPerk);
            _skillBookCountHistory.Push(new Dictionary<string, int>());
            _skillBookCountHistory.Push(new Dictionary<string, int>(state.SkillBookCounts));

            OnPropertyChanged(null);
            OnPropertyChanged(nameof(CanLevelUp));
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

        /// <summary>
        /// Undoes the most recent logged action. Returns false when only
        /// "Created Character" remains (UI should return to creation).
        /// </summary>
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

            ActiveSaveId = saveId;
            MarkClean();
            OnPropertyChanged(null);
        }

        private static CharacterActionState CloneState(CharacterActionState state) =>
            System.Text.Json.JsonSerializer.Deserialize<CharacterActionState>(
                System.Text.Json.JsonSerializer.Serialize(state, FalloutPlanner.Saves.SaveJsonOptions.Options),
                FalloutPlanner.Saves.SaveJsonOptions.Options)!;

        private Fallout1CharacterStats CreateCurrentSnapshot()
        {
            return new Fallout1CharacterStats
            {
                Strength = Strength,
                Perception = Perception,
                Endurance = Endurance,
                Charisma = Charisma,
                Intelligence = Intelligence,
                Agility = Agility,
                Luck = Luck,
                CharPoints = CharPoints,

                CharacterName = Name,
                Level = Current.Level,
                IsMale = Current.IsMale,
                Age = Current.Age,
                Traits = new List<string>(Current.Traits),
                TaggedSkills = new HashSet<string>(Current.TaggedSkills),

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

                HitPoints = HitPoints,
                ArmorClass = ArmorClass,
                ActionPoints = ActionPoints,
                CarryWeight = CarryWeight,
                MeleeDamage = MeleeDamage,
                DamageResistance = DamageResistance,
                PoisonResistance = PoisonResistance,
                RadiationResistance = RadiationResistance,
                Sequence = Sequence,
                HealingRate = HealingRate,
                CriticalChance = CriticalChance,
                SkillPoints = SkillPoints
            };
        }

        // SPECIAL Properties
        public int Strength
        {
            get => Current.Strength;
            set { if (Current.Strength != value) { Current.Strength = value; RecalculateDerivedStats(); OnPropertyChanged(); } }
        }
        public int Perception
        {
            get => Current.Perception;
            set { if (Current.Perception != value) { Current.Perception = value; RecalculateDerivedStats(); OnPropertyChanged(); } }
        }
        public int Endurance
        {
            get => Current.Endurance;
            set { if (Current.Endurance != value) { Current.Endurance = value; RecalculateDerivedStats(); OnPropertyChanged(); } }
        }
        public int Charisma
        {
            get => Current.Charisma;
            set { if (Current.Charisma != value) { Current.Charisma = value; RecalculateDerivedStats(); OnPropertyChanged(); } }
        }
        public int Intelligence
        {
            get => Current.Intelligence;
            set { if (Current.Intelligence != value) { Current.Intelligence = value; RecalculateDerivedStats(); OnPropertyChanged(); } }
        }
        public int Agility
        {
            get => Current.Agility;
            set { if (Current.Agility != value) { Current.Agility = value; RecalculateDerivedStats(); OnPropertyChanged(); } }
        }
        public int Luck
        {
            get => Current.Luck;
            set { if (Current.Luck != value) { Current.Luck = value; RecalculateDerivedStats(); OnPropertyChanged(); } }
        }
        public int CharPoints
        {
            get => Current.CharPoints;
            set { if (Current.CharPoints != value) { Current.CharPoints = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanConfirm)); } }
        }

        public void ModifySpecial(string statName, int amount)
        {
            var property = GetType().GetProperty(statName);
            if (property == null || property.PropertyType != typeof(int))
                return;

            int currentValue = (int)property.GetValue(this)!;
            int newValue = currentValue + amount;

            if (newValue < 1 || newValue > 10)
                return;

            if (amount > 0)
            {
                if (CharPoints <= 0) return;
                CharPoints--;
            }
            else
            {
                CharPoints++;
            }

            property.SetValue(this, newValue);
            RecalculateDerivedStats();
        }

        // Derived Stats
        public int HitPoints
        {
            get => Current.HitPoints;
            set { if (Current.HitPoints != value) { Current.HitPoints = value; OnPropertyChanged(); } }
        }
        public int ArmorClass
        {
            get => Current.ArmorClass;
            set { if (Current.ArmorClass != value) { Current.ArmorClass = value; OnPropertyChanged(); } }
        }
        public int ActionPoints
        {
            get => Current.ActionPoints;
            set { if (Current.ActionPoints != value) { Current.ActionPoints = value; OnPropertyChanged(); } }
        }
        public int CarryWeight
        {
            get => Current.CarryWeight;
            set { if (Current.CarryWeight != value) { Current.CarryWeight = value; OnPropertyChanged(); } }
        }
        public int MeleeDamage
        {
            get => Current.MeleeDamage;
            set { if (Current.MeleeDamage != value) { Current.MeleeDamage = value; OnPropertyChanged(); } }
        }
        public int DamageResistance
        {
            get => Current.DamageResistance;
            set { if (Current.DamageResistance != value) { Current.DamageResistance = value; OnPropertyChanged(); } }
        }
        public int PoisonResistance
        {
            get => Current.PoisonResistance;
            set { if (Current.PoisonResistance != value) { Current.PoisonResistance = value; OnPropertyChanged(); } }
        }
        public int RadiationResistance
        {
            get => Current.RadiationResistance;
            set { if (Current.RadiationResistance != value) { Current.RadiationResistance = value; OnPropertyChanged(); } }
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
        public int CriticalChance
        {
            get => Current.CriticalChance;
            set { if (Current.CriticalChance != value) { Current.CriticalChance = value; OnPropertyChanged(); } }
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

        // Skills
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

        private int CalculateSkill(int baseValue, string skillName)
        {
            int value = baseValue;

            // Tagged skills get +20%
            if (Current.TaggedSkills.Contains(skillName))
                value += 20;

            // Gifted trait: -10% to all skills
            if (HasTrait(Fallout1TraitRepository.Gifted))
                value -= 10;

            // Good Natured trait effects
            if (HasTrait(Fallout1TraitRepository.GoodNatured))
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

            return Math.Max(0, value);
        }

        public void RecalculateDerivedStats()
        {
            var stats = Current;
            int ST = stats.Strength;
            int PE = stats.Perception;
            int EN = stats.Endurance;
            int CH = stats.Charisma;
            int IN = stats.Intelligence;
            int AG = stats.Agility;
            int LK = stats.Luck;

            // HP = 15 + ST + (2*EN); level-up adds (2 + EN/2)
            int baseHP = 15 + ST + (2 * EN);
            int levelHP = (stats.Level - 1) * (2 + EN / 2);
            stats.HitPoints = baseHP + levelHP;

            // AC = AG (0 if Kamikaze)
            stats.ArmorClass = HasTrait(Fallout1TraitRepository.Kamikaze) ? 0 : AG;

            // AP = 5 + AG/2 (-2 if Bruiser)
            stats.ActionPoints = 5 + (AG / 2);
            if (HasTrait(Fallout1TraitRepository.Bruiser))
                stats.ActionPoints -= 2;
            stats.ActionPoints = Math.Max(5, stats.ActionPoints);

            // Carry Weight = 25 + (25*ST) or Small Frame: 25 + (15*ST)
            int cwMult = HasTrait(Fallout1TraitRepository.SmallFrame) ? 15 : 25;
            stats.CarryWeight = 25 + (cwMult * ST);

            // Melee Damage = max(1, ST-5) (+4 Heavy Handed)
            stats.MeleeDamage = Math.Max(1, ST - 5);
            if (HasTrait(Fallout1TraitRepository.HeavyHanded))
                stats.MeleeDamage += 4;

            // DR = 0%
            stats.DamageResistance = 0;

            // Poison Res = 5*EN% (0 if Fast Metabolism)
            stats.PoisonResistance = HasTrait(Fallout1TraitRepository.FastMetabolism) ? 0 : 5 * EN;

            // Rad Res = 2*EN% (0 if Fast Metabolism)
            stats.RadiationResistance = HasTrait(Fallout1TraitRepository.FastMetabolism) ? 0 : 2 * EN;

            // Sequence = 2*PE (+5 Kamikaze)
            stats.Sequence = 2 * PE;
            if (HasTrait(Fallout1TraitRepository.Kamikaze))
                stats.Sequence += 5;

            // Healing Rate = max(1, EN/3) (×2 Fast Metabolism)
            stats.HealingRate = Math.Max(1, EN / 3);
            if (HasTrait(Fallout1TraitRepository.FastMetabolism))
                stats.HealingRate *= 2;

            // Crit Chance = Luck% (+10% Finesse)
            stats.CriticalChance = LK;
            if (HasTrait(Fallout1TraitRepository.Finesse))
                stats.CriticalChance += 10;

            // Calculate base skills using FO1 formulas
            stats.SmallGuns = CalculateSkill(5 + (4 * AG), nameof(SmallGuns));
            stats.BigGuns = CalculateSkill(2 * AG, nameof(BigGuns));
            stats.EnergyWeapons = CalculateSkill(2 * AG, nameof(EnergyWeapons));
            stats.Unarmed = CalculateSkill(30 + (2 * ((AG + ST) / 2)), nameof(Unarmed));
            stats.MeleeWeapons = CalculateSkill(20 + (2 * ((AG + ST) / 2)), nameof(MeleeWeapons));
            stats.Throwing = CalculateSkill(4 * AG, nameof(Throwing));
            stats.FirstAid = CalculateSkill(2 * (PE + IN), nameof(FirstAid));
            stats.Doctor = CalculateSkill(5 + (PE + IN), nameof(Doctor));
            stats.Sneak = CalculateSkill(5 + (3 * AG), nameof(Sneak));
            stats.Lockpick = CalculateSkill(10 + (PE + AG), nameof(Lockpick));
            stats.Steal = CalculateSkill(3 * AG, nameof(Steal));
            stats.Traps = CalculateSkill(10 + (PE + AG), nameof(Traps));
            stats.Science = CalculateSkill(4 * IN, nameof(Science));
            stats.Repair = CalculateSkill(3 * IN, nameof(Repair));
            stats.Speech = CalculateSkill(5 * CH, nameof(Speech));
            stats.Barter = CalculateSkill(4 * CH, nameof(Barter));
            stats.Gambling = CalculateSkill(5 * LK, nameof(Gambling));
            stats.Outdoorsman = CalculateSkill(2 * (EN + IN), nameof(Outdoorsman));

            OnPropertyChanged(null);
            OnPropertyChanged(nameof(SkillPointsPerLevel));
        }

        // Confirmation and Level Up
        public bool CanConfirm => CharPoints == 0 && TaggedSkillCount == 3;
        public int MaxLevel => 21;
        public bool IsMaxLevel => Level >= MaxLevel;

        public bool AllSkillsAtMax =>
            SmallGuns >= 300 && BigGuns >= 300 && EnergyWeapons >= 300 &&
            Unarmed >= 300 && MeleeWeapons >= 300 && Throwing >= 300 &&
            FirstAid >= 300 && Doctor >= 300 && Sneak >= 300 &&
            Lockpick >= 300 && Steal >= 300 && Traps >= 300 &&
            Science >= 300 && Repair >= 300 && Speech >= 300 &&
            Barter >= 300 && Gambling >= 300 && Outdoorsman >= 300;

        public bool CanLevelUp => !IsMaxLevel && (SkillPoints == 0 || AllSkillsAtMax);

        // Perk every 3 levels (or 4 with Skilled)
        public int PerkInterval => HasTrait(Fallout1TraitRepository.Skilled) ? 4 : 3;
        public bool NextLevelRequiresPerk => ((Level + 1) % PerkInterval) == 0;

        // Skill points per level = 5 + (2*IN) (+5 Skilled) (-5 Gifted)
        public int SkillPointsPerLevel
        {
            get
            {
                int points = 5 + (2 * Intelligence);
                if (HasTrait(Fallout1TraitRepository.Skilled))
                    points += 5;
                if (HasTrait(Fallout1TraitRepository.Gifted))
                    points -= 5;
                // Educated perk: +2 per rank
                points += GetPerkRank("Educated") * 2;
                return Math.Max(1, points);
            }
        }

        private static string FormatSkillSpendSummary(Dictionary<string, int> increases)
        {
            if (increases == null || increases.Count == 0)
                return "";
            return string.Join(", ",
                increases.Where(k => k.Value > 0)
                    .OrderBy(k => k.Key)
                    .Select(k => $"{k.Key} +{k.Value}"));
        }

        public void LevelUp(Fallout1Perk? perk = null)
        {
            int nextLevel = Level + 1;
            bool needsPerk = (nextLevel % PerkInterval) == 0;
            string spendSummary = FormatSkillSpendSummary(SkillIncreasesThisLevel);

            string description;
            if (needsPerk && perk != null)
            {
                description = string.IsNullOrEmpty(spendSummary)
                    ? $"Level Up: Level {nextLevel}, Perk: {perk.Name}"
                    : $"Level Up: Level {nextLevel}, {spendSummary}, Perk: {perk.Name}";
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
                newStats.SkillPoints = SkillPointsPerLevel;

                // Recalculate HP for new level
                int baseHP = 15 + newStats.Strength + (2 * newStats.Endurance);
                int levelHP = (newStats.Level - 1) * (2 + newStats.Endurance / 2);
                newStats.HitPoints = baseHP + levelHP;

                _history.Push(newStats);
                _skillIncreaseHistory.Push(new Dictionary<string, int>());
                _skillBookCountHistory.Push(new Dictionary<string, int>(SkillBookCounts));

                if (needsPerk && perk != null)
                {
                    ApplyPerk(perk);
                    _perkHistory.Push(perk);
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

        public bool TryAddSkillPoints(string skillName, int amount)
        {
            var property = GetType().GetProperty(skillName);
            if (property == null) return false;

            int current = (int)property.GetValue(this)!;
            int updated = Math.Clamp(current + amount, 0, 300);
            if (updated == current) return false;

            property.SetValue(this, updated);
            OnPropertyChanged(skillName);
            return true;
        }
    }
}
