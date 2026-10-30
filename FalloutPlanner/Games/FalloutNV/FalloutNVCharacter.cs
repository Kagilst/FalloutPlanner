using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FalloutPlanner.Games.FalloutNV.Implants;
using FalloutPlanner.Games.FalloutNV.Perks;
using FalloutPlanner.Games.FalloutNV.SkillBooks;
using FalloutPlanner.Games.FalloutNV.Traits;

namespace FalloutPlanner.Games.FalloutNV
{
    public class FalloutNVCharacter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private Stack<FalloutNVCharacterStats> _history = new();
        private readonly Stack<Dictionary<string, int>> _skillIncreaseHistory = new();
        private readonly Stack<FalloutNVPerk?> _perkHistory = new();
        private readonly Stack<HashSet<string>> _implantHistory = new();
        private readonly Stack<Dictionary<string, int>> _skillBookCountHistory = new();
        private readonly Stack<HashSet<string>> _skillBookLocationHistory = new();
        private readonly Stack<Dictionary<int, int>> _workbenchCrateHistory = new();

        private FalloutNVCharacterStats _namedBaseline;

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

        public FalloutNVCharacterStats Current => _history.Peek();
        private Dictionary<string, int> SkillIncreasesThisLevel => _skillIncreaseHistory.Peek();
        private HashSet<string> InstalledImplantNames => _implantHistory.Peek();
        private Dictionary<string, int> SkillBookCounts => _skillBookCountHistory.Peek();
        private HashSet<string> FoundSkillBookLocations => _skillBookLocationHistory.Peek();
        private Dictionary<int, int> WorkbenchCrates => _workbenchCrateHistory.Peek();

        // Workbench Crate Logic
        public bool IsWorkbenchCrateOpened(int crateNumber) => WorkbenchCrates.ContainsKey(crateNumber);
        public int GetWorkbenchCrateBooksCount(int crateNumber) =>
            WorkbenchCrates.TryGetValue(crateNumber, out var count) ? count : 0;

        public void SetWorkbenchCrateOpened(int crateNumber, int booksCount)
        {
            if (crateNumber < 1 || crateNumber > 4)
                return;
            if (WorkbenchCrates.ContainsKey(crateNumber))
                return; // Already opened

            RecordLoggedAction($"Opened Workbench Crate {crateNumber}: {booksCount} book(s)", () =>
            {
                WorkbenchCrates[crateNumber] = booksCount;
                OnPropertyChanged(null);
            });
        }

        // Implant Logic
        public bool IsImplantInstalled(string name) => InstalledImplantNames.Contains(name);
        public IReadOnlyCollection<string> InstalledImplants => InstalledImplantNames;

        /// <summary>
        /// Number of default implant slots available (based on base Endurance before END implant).
        /// </summary>
        public int ImplantSlots
        {
            get
            {
                int baseEnd = Endurance;
                if (IsImplantInstalled("EnduranceImplant"))
                    baseEnd -= 1; // END implant doesn't increase slots
                return Math.Max(0, baseEnd);
            }
        }

        /// <summary>
        /// Number of default implant slots currently used.
        /// </summary>
        public int UsedImplantSlots =>
            InstalledImplantNames.Count(n =>
            {
                var impl = FalloutNVImplantRepository.GetByName(n);
                return impl != null && impl.Category == ImplantCategory.Default;
            });

        public bool CanInstallDefaultImplant => UsedImplantSlots < ImplantSlots;

        public bool TryInstallImplant(FalloutNVImplant implant)
        {
            if (implant == null || InstalledImplantNames.Contains(implant.Name))
                return false;

            if (implant.Category == ImplantCategory.Default && !CanInstallDefaultImplant)
                return false;

            RecordLoggedAction($"Installed Implant: {implant.DisplayName}", () =>
            {
                InstalledImplantNames.Add(implant.Name);
                implant.ApplyEffect?.Invoke(this);
                OnPropertyChanged(null);
            });
            return true;
        }

        // Perk Logic
        public Dictionary<string, int> TakenPerks { get; } = new();
        private readonly Stack<Dictionary<string, QuestPerkState>> _questPerkHistory = new();
        private Dictionary<string, QuestPerkState> TakenQuestPerks => _questPerkHistory.Peek();

        public bool HasQuestPerk(string name) => TakenQuestPerks.ContainsKey(name);

        public IReadOnlyList<OwnedPerkInfo> OwnedPerks
        {
            get
            {
                var fromLevels = TakenPerks
                    .OrderBy(kvp => kvp.Key)
                    .Select(kvp =>
                    {
                        var perk = FalloutNVPerkRepository.AllPerks.FirstOrDefault(p => p.Name == kvp.Key);
                        return new OwnedPerkInfo
                        {
                            Perk = perk,
                            CurrentRank = kvp.Value,
                            DisplayName = kvp.Value > 1 && (perk?.MaxRank ?? 1) > 1
                                ? $"{kvp.Key} (Rank {kvp.Value}/{perk.MaxRank})"
                                : kvp.Value > 1
                                    ? $"{kvp.Key} (Rank {kvp.Value})"
                                    : kvp.Key
                        };
                    })
                    .Where(info => info.Perk != null);

                var fromQuest = TakenQuestPerks.Values
                    .OrderBy(s => s.DisplayName)
                    .Select(s => new OwnedPerkInfo
                    {
                        Perk = s.Perk,
                        CurrentRank = 1,
                        DisplayName = s.DisplayName,
                        DescriptionOverride = s.Description,
                        IsMiscPerk = true
                    });

                return fromLevels.Concat(fromQuest).ToList();
            }
        }

        /// <summary>
        /// Returns true if the misc perk is blocked by a mutex (another perk in the same group is owned).
        /// </summary>
        public bool IsMiscPerkBlocked(string perkName)
        {
            return FalloutNVQuestPerkRepository.IsPerkBlocked(this, perkName);
        }

        /// <summary>
        /// Gets the name of the perk that is blocking this one, or null if not blocked.
        /// </summary>
        public string GetBlockingPerk(string perkName)
        {
            return FalloutNVQuestPerkRepository.GetBlockingPerk(this, perkName);
        }

        public bool TryTakeQuestPerk(FalloutNVPerk perk, QuestPerkState? state = null, bool effectsAlreadyApplied = false)
        {
            if (perk == null || TakenQuestPerks.ContainsKey(perk.Name))
                return false;

            // Check mutex blocking
            if (IsMiscPerkBlocked(perk.Name))
                return false;

            var resolved = state ?? new QuestPerkState
            {
                Name = perk.Name,
                DisplayName = perk.Name,
                Description = perk.Description,
                Perk = perk
            };
            resolved.Perk ??= perk;
            resolved.Name = perk.Name;

            RecordLoggedAction($"Added Perk: {resolved.DisplayName}", () =>
            {
                if (!effectsAlreadyApplied)
                {
                    perk.ApplyEffect?.Invoke(this);
                }
                TakenQuestPerks[perk.Name] = resolved;
                OnPropertyChanged(null);
                OnPropertyChanged(nameof(OwnedPerks));
            });
            return true;
        }

        public bool TryRemoveQuestPerk(string perkName)
        {
            if (!TakenQuestPerks.TryGetValue(perkName, out var state))
                return false;

            // Remove any reversible stat effects
            FalloutNVQuestPerkRepository.RemoveMiscPerkEffects(this, state);

            TakenQuestPerks.Remove(perkName);
            OnPropertyChanged(null);
            OnPropertyChanged(nameof(OwnedPerks));
            return true;
        }

        // Skill Book Logic
        public int GetSkillBookCount(string bookName) =>
            SkillBookCounts.TryGetValue(bookName, out var n) ? n : 0;

        public bool IsSkillBookLocationFound(string locationId) =>
            FoundSkillBookLocations.Contains(locationId);

        /// <summary>
        /// Points per skill book: 3 normally, 4 with Comprehension.
        /// </summary>
        public int SkillBookPointsPerBook => GetPerkRank("Comprehension") > 0 ? 4 : 3;

        public bool TrySetSkillBookCount(string bookName, int newCount)
        {
            var book = FalloutNVSkillBookRepository.GetBookType(bookName);
            if (book == null)
                return false;

            int current = GetSkillBookCount(bookName);
            if (book.MaxCount >= 0)
                newCount = Math.Clamp(newCount, 0, book.MaxCount);
            else
                newCount = Math.Max(0, newCount);

            if (newCount == current)
                return false;

            if (newCount <= current)
                return false;

            int toAdd = newCount - current;
            for (int i = 0; i < toAdd; i++)
            {
                RecordLoggedAction($"Read: {bookName}", () =>
                {
                    ApplySkillBookCountDelta(book, +1);
                });
            }

            return true;
        }

        private void ApplySkillBookCountDelta(FalloutNVSkillBookType book, int deltaBooks)
        {
            if (deltaBooks == 0)
                return;

            int current = GetSkillBookCount(book.Name);
            int newCount = current + deltaBooks;
            if (book.MaxCount >= 0)
                newCount = Math.Clamp(newCount, 0, book.MaxCount);
            else
                newCount = Math.Max(0, newCount);

            int appliedDelta = newCount - current;
            if (appliedDelta == 0)
                return;

            TryAddSkillPointsFromPerk(book.SkillPropertyName, appliedDelta * SkillBookPointsPerBook);

            if (newCount == 0)
                SkillBookCounts.Remove(book.Name);
            else
                SkillBookCounts[book.Name] = newCount;

            OnPropertyChanged(null);
        }

        public bool TrySetSkillBookLocationFound(string locationId, bool found)
        {
            var loc = FalloutNVSkillBookRepository.Locations.FirstOrDefault(l => l.Id == locationId);
            if (loc == null)
                return false;

            bool currently = FoundSkillBookLocations.Contains(locationId);
            if (currently == found)
                return true;

            if (found)
                FoundSkillBookLocations.Add(locationId);
            else
                FoundSkillBookLocations.Remove(locationId);

            return true;
        }

        public bool CanTakePerk(FalloutNVPerk perk, int? levelOverride = null)
        {
            if (!perk.MeetsStatRequirements(this, levelOverride))
                return false;

            if (TakenPerks.ContainsKey(perk.Name) &&
                TakenPerks[perk.Name] >= perk.MaxRank)
                return false;

            return true;
        }

        public bool CanTakePerkOnLevelUp(FalloutNVPerk perk) =>
            CanTakePerk(perk, Level + 1);

        public void TakePerk(FalloutNVPerk perk)
        {
            if (!CanTakePerk(perk))
                return;

            ApplyPerk(perk);
            OnPropertyChanged(null);
        }

        private void ApplyPerk(FalloutNVPerk perk)
        {
            if (!TakenPerks.ContainsKey(perk.Name))
                TakenPerks[perk.Name] = 0;

            TakenPerks[perk.Name]++;
            perk.ApplyEffect?.Invoke(this);
        }

        private void UntakePerk(FalloutNVPerk perk)
        {
            if (!TakenPerks.ContainsKey(perk.Name))
                return;

            TakenPerks[perk.Name]--;
            if (TakenPerks[perk.Name] <= 0)
                TakenPerks.Remove(perk.Name);
        }

        // Skill Logic
        public bool ModifySkill(string skillName, int amount)
        {
            var property = GetType().GetProperty(skillName);

            if (property == null || property.PropertyType != typeof(int))
                return false;

            int currentValue = (int)property.GetValue(this);
            int newValue = currentValue + amount;

            if (amount > 0)
            {
                if (SkillPoints <= 0 || currentValue >= 100)
                    return false;

                property.SetValue(this, Math.Min(newValue, 100));
                SkillPoints--;

                if (!SkillIncreasesThisLevel.ContainsKey(skillName))
                    SkillIncreasesThisLevel[skillName] = 0;

                SkillIncreasesThisLevel[skillName]++;
                NotifySkillChanged(skillName);
                return true;
            }

            if (amount < 0)
            {
                if (!SkillIncreasesThisLevel.ContainsKey(skillName) ||
                    SkillIncreasesThisLevel[skillName] <= 0)
                    return false;

                property.SetValue(this, currentValue - 1);
                SkillPoints++;
                SkillIncreasesThisLevel[skillName]--;

                NotifySkillChanged(skillName);
                return true;
            }

            return false;
        }

        private void NotifyAllSkillButtonStates()
        {
            OnPropertyChanged(nameof(CanIncreaseBarter));
            OnPropertyChanged(nameof(CanIncreaseEnergyWeapons));
            OnPropertyChanged(nameof(CanIncreaseExplosives));
            OnPropertyChanged(nameof(CanIncreaseGuns));
            OnPropertyChanged(nameof(CanIncreaseLockpick));
            OnPropertyChanged(nameof(CanIncreaseMedicine));
            OnPropertyChanged(nameof(CanIncreaseMeleeWeapons));
            OnPropertyChanged(nameof(CanIncreaseRepair));
            OnPropertyChanged(nameof(CanIncreaseScience));
            OnPropertyChanged(nameof(CanIncreaseSneak));
            OnPropertyChanged(nameof(CanIncreaseSpeech));
            OnPropertyChanged(nameof(CanIncreaseSurvival));
            OnPropertyChanged(nameof(CanIncreaseUnarmed));

            OnPropertyChanged(nameof(CanDecreaseBarter));
            OnPropertyChanged(nameof(CanDecreaseEnergyWeapons));
            OnPropertyChanged(nameof(CanDecreaseExplosives));
            OnPropertyChanged(nameof(CanDecreaseGuns));
            OnPropertyChanged(nameof(CanDecreaseLockpick));
            OnPropertyChanged(nameof(CanDecreaseMedicine));
            OnPropertyChanged(nameof(CanDecreaseMeleeWeapons));
            OnPropertyChanged(nameof(CanDecreaseRepair));
            OnPropertyChanged(nameof(CanDecreaseScience));
            OnPropertyChanged(nameof(CanDecreaseSneak));
            OnPropertyChanged(nameof(CanDecreaseSpeech));
            OnPropertyChanged(nameof(CanDecreaseSurvival));
            OnPropertyChanged(nameof(CanDecreaseUnarmed));
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
            return SkillPoints > 0 && value < 100;
        }

        public bool CanDecreaseSkill(string skillName)
        {
            return SkillIncreasesThisLevel.ContainsKey(skillName) &&
                   SkillIncreasesThisLevel[skillName] > 0;
        }

        // Skill Can Increase/Decrease properties (FNV skills)
        public bool CanIncreaseBarter => CanIncreaseSkill(nameof(Barter));
        public bool CanDecreaseBarter => CanDecreaseSkill(nameof(Barter));
        public bool CanIncreaseEnergyWeapons => CanIncreaseSkill(nameof(EnergyWeapons));
        public bool CanDecreaseEnergyWeapons => CanDecreaseSkill(nameof(EnergyWeapons));
        public bool CanIncreaseExplosives => CanIncreaseSkill(nameof(Explosives));
        public bool CanDecreaseExplosives => CanDecreaseSkill(nameof(Explosives));
        public bool CanIncreaseGuns => CanIncreaseSkill(nameof(Guns));
        public bool CanDecreaseGuns => CanDecreaseSkill(nameof(Guns));
        public bool CanIncreaseLockpick => CanIncreaseSkill(nameof(Lockpick));
        public bool CanDecreaseLockpick => CanDecreaseSkill(nameof(Lockpick));
        public bool CanIncreaseMedicine => CanIncreaseSkill(nameof(Medicine));
        public bool CanDecreaseMedicine => CanDecreaseSkill(nameof(Medicine));
        public bool CanIncreaseMeleeWeapons => CanIncreaseSkill(nameof(MeleeWeapons));
        public bool CanDecreaseMeleeWeapons => CanDecreaseSkill(nameof(MeleeWeapons));
        public bool CanIncreaseRepair => CanIncreaseSkill(nameof(Repair));
        public bool CanDecreaseRepair => CanDecreaseSkill(nameof(Repair));
        public bool CanIncreaseScience => CanIncreaseSkill(nameof(Science));
        public bool CanDecreaseScience => CanDecreaseSkill(nameof(Science));
        public bool CanIncreaseSneak => CanIncreaseSkill(nameof(Sneak));
        public bool CanDecreaseSneak => CanDecreaseSkill(nameof(Sneak));
        public bool CanIncreaseSpeech => CanIncreaseSkill(nameof(Speech));
        public bool CanDecreaseSpeech => CanDecreaseSkill(nameof(Speech));
        public bool CanIncreaseSurvival => CanIncreaseSkill(nameof(Survival));
        public bool CanDecreaseSurvival => CanDecreaseSkill(nameof(Survival));
        public bool CanIncreaseUnarmed => CanIncreaseSkill(nameof(Unarmed));
        public bool CanDecreaseUnarmed => CanDecreaseSkill(nameof(Unarmed));

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

        public List<string> Traits => Current.Traits;

        public bool HasTrait(string traitName) => Traits.Contains(traitName);

        public FalloutNVCharacter(FalloutNVCharacterStats baseStats)
        {
            _history.Push(baseStats.Clone());
            _skillIncreaseHistory.Push(new Dictionary<string, int>());
            _perkHistory.Push(null);
            _implantHistory.Push(new HashSet<string>());
            _questPerkHistory.Push(new Dictionary<string, QuestPerkState>());
            _skillBookCountHistory.Push(new Dictionary<string, int>());
            _skillBookLocationHistory.Push(new HashSet<string>());
            _workbenchCrateHistory.Push(new Dictionary<int, int>());
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
            _implantHistory.Clear();
            _questPerkHistory.Clear();
            _skillBookCountHistory.Clear();
            _skillBookLocationHistory.Clear();
            _workbenchCrateHistory.Clear();
            TakenPerks.Clear();

            _history.Push(snapshot.Clone());
            _history.Push(snapshot);
            _skillIncreaseHistory.Push(new Dictionary<string, int>());
            _skillIncreaseHistory.Push(new Dictionary<string, int>());
            _perkHistory.Push(null);
            _perkHistory.Push(null);
            _implantHistory.Push(new HashSet<string>());
            _implantHistory.Push(new HashSet<string>());
            _questPerkHistory.Push(new Dictionary<string, QuestPerkState>());
            _questPerkHistory.Push(new Dictionary<string, QuestPerkState>());
            _skillBookCountHistory.Push(new Dictionary<string, int>());
            _skillBookCountHistory.Push(new Dictionary<string, int>());
            _skillBookLocationHistory.Push(new HashSet<string>());
            _skillBookLocationHistory.Push(new HashSet<string>());
            _workbenchCrateHistory.Push(new Dictionary<int, int>());
            _workbenchCrateHistory.Push(new Dictionary<int, int>());

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
            _implantHistory.Clear();
            _questPerkHistory.Clear();
            _skillBookCountHistory.Clear();
            _skillBookLocationHistory.Clear();
            _workbenchCrateHistory.Clear();
            TakenPerks.Clear();
            _history.Push(working);
            _skillIncreaseHistory.Push(new Dictionary<string, int>());
            _perkHistory.Push(null);
            _implantHistory.Push(new HashSet<string>());
            _questPerkHistory.Push(new Dictionary<string, QuestPerkState>());
            _skillBookCountHistory.Push(new Dictionary<string, int>());
            _skillBookLocationHistory.Push(new HashSet<string>());
            _workbenchCrateHistory.Push(new Dictionary<int, int>());
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
                QuestPerks = CloneQuestPerks(TakenQuestPerks),
                Implants = new HashSet<string>(InstalledImplantNames),
                SkillBookCounts = new Dictionary<string, int>(SkillBookCounts),
                SkillBookLocations = new HashSet<string>(FoundSkillBookLocations),
                WorkbenchCrates = new Dictionary<int, int>(WorkbenchCrates),
                TaggedSkillNames = new HashSet<string>(_taggedSkillNames)
            };
        }

        public void RestoreActionState(CharacterActionState state)
        {
            TakenPerks.Clear();
            foreach (var kvp in state.TakenPerks)
                TakenPerks[kvp.Key] = kvp.Value;

            _taggedSkillNames = new HashSet<string>(state.TaggedSkillNames ?? new HashSet<string>());

            _history.Clear();
            _skillIncreaseHistory.Clear();
            _perkHistory.Clear();
            _implantHistory.Clear();
            _questPerkHistory.Clear();
            _skillBookCountHistory.Clear();
            _skillBookLocationHistory.Clear();
            _workbenchCrateHistory.Clear();

            _history.Push((_namedBaseline ?? state.Stats).Clone());
            _history.Push(state.Stats.Clone());
            _skillIncreaseHistory.Push(new Dictionary<string, int>());
            _skillIncreaseHistory.Push(new Dictionary<string, int>(state.SkillIncreases));
            _perkHistory.Push(null);
            _perkHistory.Push(null);
            _implantHistory.Push(new HashSet<string>());
            _implantHistory.Push(new HashSet<string>(state.Implants));
            _questPerkHistory.Push(new Dictionary<string, QuestPerkState>());
            _questPerkHistory.Push(CloneQuestPerks(state.QuestPerks));
            _skillBookCountHistory.Push(new Dictionary<string, int>());
            _skillBookCountHistory.Push(new Dictionary<string, int>(state.SkillBookCounts));
            _skillBookLocationHistory.Push(new HashSet<string>());
            _skillBookLocationHistory.Push(new HashSet<string>(state.SkillBookLocations));
            _workbenchCrateHistory.Push(new Dictionary<int, int>());
            _workbenchCrateHistory.Push(new Dictionary<int, int>(state.WorkbenchCrates));

            OnPropertyChanged(null);
            OnPropertyChanged(nameof(OwnedPerks));
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

        private static Dictionary<string, QuestPerkState> CloneQuestPerks(Dictionary<string, QuestPerkState> source)
        {
            var copy = new Dictionary<string, QuestPerkState>();
            foreach (var kvp in source)
            {
                var perk = kvp.Value.Perk
                    ?? FalloutNVPerkRepository.AllPerks.FirstOrDefault(p => p.Name == kvp.Value.Name)
                    ?? FalloutNVQuestPerkRepository.All.FirstOrDefault(p => p.Name == kvp.Value.Name);
                copy[kvp.Key] = new QuestPerkState
                {
                    Name = kvp.Value.Name,
                    DisplayName = kvp.Value.DisplayName,
                    Description = kvp.Value.Description,
                    Perk = perk!,
                    ChoiceDetail = kvp.Value.ChoiceDetail
                };
            }
            return copy;
        }

        private FalloutNVCharacterStats CreateCurrentSnapshot()
        {
            return new FalloutNVCharacterStats
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
                Traits = new List<string>(Current.Traits),
                TaggedSkills = TaggedSkills,

                Barter = Barter,
                EnergyWeapons = EnergyWeapons,
                Explosives = Explosives,
                Guns = Guns,
                Lockpick = Lockpick,
                Medicine = Medicine,
                MeleeWeapons = MeleeWeapons,
                Repair = Repair,
                Science = Science,
                Sneak = Sneak,
                Speech = Speech,
                Survival = Survival,
                Unarmed = Unarmed,

                ActionPoints = ActionPoints,
                Health = Health,
                CarryWeight = CarryWeight,
                MeleeDamage = MeleeDamage,
                PoisonResist = PoisonResist,
                RadiationResist = RadiationResist,
                CriticalChance = CriticalChance,
                DamageResist = DamageResist,
                DamageThreshold = DamageThreshold,
                FireResist = FireResist,
                EnergyResist = EnergyResist,
                UnarmedDamage = UnarmedDamage,
                CompanionNerve = CompanionNerve,
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

        public int Health
        {
            get => Current.Health;
            set { if (Current.Health != value) { Current.Health = value; OnPropertyChanged(); } }
        }

        public int CarryWeight
        {
            get => Current.CarryWeight;
            set { if (Current.CarryWeight != value) { Current.CarryWeight = value; OnPropertyChanged(); } }
        }

        public double CriticalChance
        {
            get => Current.CriticalChance;
            set { if (Current.CriticalChance != value) { Current.CriticalChance = value; OnPropertyChanged(); } }
        }

        public int DamageResist
        {
            get => Current.DamageResist;
            set { if (Current.DamageResist != value) { Current.DamageResist = value; OnPropertyChanged(); } }
        }

        public int DamageThreshold
        {
            get => Current.DamageThreshold;
            set { if (Current.DamageThreshold != value) { Current.DamageThreshold = value; OnPropertyChanged(); } }
        }

        public int FireResist
        {
            get => Current.FireResist;
            set { if (Current.FireResist != value) { Current.FireResist = value; OnPropertyChanged(); } }
        }

        public int EnergyResist
        {
            get => Current.EnergyResist;
            set { if (Current.EnergyResist != value) { Current.EnergyResist = value; OnPropertyChanged(); } }
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

        public double MeleeDamage
        {
            get => Current.MeleeDamage;
            set { if (Current.MeleeDamage != value) { Current.MeleeDamage = value; OnPropertyChanged(); } }
        }

        public double UnarmedDamage
        {
            get => Current.UnarmedDamage;
            set { if (Current.UnarmedDamage != value) { Current.UnarmedDamage = value; OnPropertyChanged(); } }
        }

        public int CompanionNerve
        {
            get => Current.CompanionNerve;
            set { if (Current.CompanionNerve != value) { Current.CompanionNerve = value; OnPropertyChanged(); } }
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

        // Skills Properties (FNV)
        public int Barter
        {
            get => Current.Barter;
            set { if (Current.Barter != value) { Current.Barter = value; OnPropertyChanged(); } }
        }

        public int EnergyWeapons
        {
            get => Current.EnergyWeapons;
            set { if (Current.EnergyWeapons != value) { Current.EnergyWeapons = value; OnPropertyChanged(); } }
        }

        public int Explosives
        {
            get => Current.Explosives;
            set { if (Current.Explosives != value) { Current.Explosives = value; OnPropertyChanged(); } }
        }

        public int Guns
        {
            get => Current.Guns;
            set { if (Current.Guns != value) { Current.Guns = value; OnPropertyChanged(); } }
        }

        public int Lockpick
        {
            get => Current.Lockpick;
            set { if (Current.Lockpick != value) { Current.Lockpick = value; OnPropertyChanged(); } }
        }

        public int Medicine
        {
            get => Current.Medicine;
            set { if (Current.Medicine != value) { Current.Medicine = value; OnPropertyChanged(); } }
        }

        public int MeleeWeapons
        {
            get => Current.MeleeWeapons;
            set { if (Current.MeleeWeapons != value) { Current.MeleeWeapons = value; OnPropertyChanged(); } }
        }

        public int Repair
        {
            get => Current.Repair;
            set { if (Current.Repair != value) { Current.Repair = value; OnPropertyChanged(); } }
        }

        public int Science
        {
            get => Current.Science;
            set { if (Current.Science != value) { Current.Science = value; OnPropertyChanged(); } }
        }

        public int Sneak
        {
            get => Current.Sneak;
            set { if (Current.Sneak != value) { Current.Sneak = value; OnPropertyChanged(); } }
        }

        public int Speech
        {
            get => Current.Speech;
            set { if (Current.Speech != value) { Current.Speech = value; OnPropertyChanged(); } }
        }

        public int Survival
        {
            get => Current.Survival;
            set { if (Current.Survival != value) { Current.Survival = value; OnPropertyChanged(); } }
        }

        public int Unarmed
        {
            get => Current.Unarmed;
            set { if (Current.Unarmed != value) { Current.Unarmed = value; OnPropertyChanged(); } }
        }

        // Skill calculation with tags and traits
        private int CalculateSkill(int baseValue, string skillName)
        {
            int value = baseValue;

            if (_taggedSkillNames.Contains(skillName))
                value += 15;

            // Skilled trait: +5 to all skills
            if (HasTrait(FalloutNVTraitRepository.Skilled))
                value += 5;

            // Good Natured trait
            if (HasTrait(FalloutNVTraitRepository.GoodNatured))
            {
                switch (skillName)
                {
                    case nameof(Speech):
                    case nameof(Medicine):
                    case nameof(Repair):
                    case nameof(Science):
                    case nameof(Barter):
                        value += 5;
                        break;
                    case nameof(EnergyWeapons):
                    case nameof(Explosives):
                    case nameof(Guns):
                    case nameof(MeleeWeapons):
                    case nameof(Unarmed):
                        value -= 5;
                        break;
                }
            }

            return Math.Clamp(value, 0, 100);
        }

        public void RecalculateDerivedStats()
        {
            var stats = Current;

            // FNV Formulas
            // AP = AGL*3 + 65 (+10 Kamikaze via trait effect)
            stats.ActionPoints = (stats.Agility * 3) + 65;
            if (HasTrait(FalloutNVTraitRepository.Kamikaze))
                stats.ActionPoints += 10;

            // Add Action Boy/Girl bonus (+15 per rank)
            stats.ActionPoints += GetPerkRank("Action Boy/Action Girl") * 15;

            // HP = 95 + END*20 + Level*5 (+30 Life Giver)
            stats.Health = 95 + (stats.Endurance * 20) + (stats.Level * 5);
            if (GetPerkRank("Life Giver") > 0)
                stats.Health += 30;

            // CW = STR*10 + 150 (+50 Strong Back ranks, +25 Hoarder)
            stats.CarryWeight = (stats.Strength * 10) + 150;
            stats.CarryWeight += GetPerkRank("Strong Back") * 50;
            if (HasTrait(FalloutNVTraitRepository.Hoarder))
                stats.CarryWeight += 25;

            // Crit = Luck (+3 Built to Destroy, +5 Finesse)
            stats.CriticalChance = stats.Luck;
            if (HasTrait(FalloutNVTraitRepository.BuiltToDestroy))
                stats.CriticalChance += 3;
            if (GetPerkRank("Finesse") > 0)
                stats.CriticalChance += 5;

            // Melee Damage = STR * 0.5
            stats.MeleeDamage = stats.Strength * 0.5;

            // Unarmed Damage = (Unarmed skill / 20) + 0.5
            stats.UnarmedDamage = (stats.Unarmed / 20.0) + 0.5;

            // Poison Resist = END*5 - 5
            stats.PoisonResist = (stats.Endurance * 5) - 5;

            // Rad Resist = END*2 - 2
            stats.RadiationResist = (stats.Endurance * 2) - 2;

            // Companion Nerve = CHA * 5
            stats.CompanionNerve = stats.Charisma * 5;

            // DT and DR (affected by Kamikaze)
            if (HasTrait(FalloutNVTraitRepository.Kamikaze))
            {
                stats.DamageThreshold = -2;
                stats.DamageResist = -15;
            }
            else
            {
                stats.DamageThreshold = 0;
                stats.DamageResist = 0;
            }

            // Skill calculations using FNV formula: 2 + 2*SPECIAL + ceil(Luck/2)
            int luckBonus = (stats.Luck + 1) / 2; // ceil(Luck/2)

            stats.Barter = CalculateSkill(2 + luckBonus + (stats.Charisma * 2), nameof(Barter));
            stats.EnergyWeapons = CalculateSkill(2 + luckBonus + (stats.Perception * 2), nameof(EnergyWeapons));
            stats.Explosives = CalculateSkill(2 + luckBonus + (stats.Perception * 2), nameof(Explosives));
            stats.Guns = CalculateSkill(2 + luckBonus + (stats.Agility * 2), nameof(Guns));
            stats.Lockpick = CalculateSkill(2 + luckBonus + (stats.Perception * 2), nameof(Lockpick));
            stats.Medicine = CalculateSkill(2 + luckBonus + (stats.Intelligence * 2), nameof(Medicine));
            stats.MeleeWeapons = CalculateSkill(2 + luckBonus + (stats.Strength * 2), nameof(MeleeWeapons));
            stats.Repair = CalculateSkill(2 + luckBonus + (stats.Intelligence * 2), nameof(Repair));
            stats.Science = CalculateSkill(2 + luckBonus + (stats.Intelligence * 2), nameof(Science));
            stats.Sneak = CalculateSkill(2 + luckBonus + (stats.Agility * 2), nameof(Sneak));
            stats.Speech = CalculateSkill(2 + luckBonus + (stats.Charisma * 2), nameof(Speech));
            stats.Survival = CalculateSkill(2 + luckBonus + (stats.Endurance * 2), nameof(Survival));
            stats.Unarmed = CalculateSkill(2 + luckBonus + (stats.Endurance * 2), nameof(Unarmed));

            // Recalculate unarmed damage after skill recalc
            stats.UnarmedDamage = (stats.Unarmed / 20.0) + 0.5;

            OnPropertyChanged(null);
            OnPropertyChanged(nameof(SkillPointsPerLevel));
        }

        // Create Character Confirmation
        public bool CanConfirm => Points == 0 && TaggedSkills == 3;

        /// <summary>
        /// Max level is 50, or 30 if Logan's Loophole trait is taken.
        /// </summary>
        public int MaxLevel => HasTrait(FalloutNVTraitRepository.LogansLoophole) ? 30 : 50;

        public bool IsMaxLevel => Level >= MaxLevel;

        public bool AllSkillsAtMax =>
            Barter >= 100 && EnergyWeapons >= 100 && Explosives >= 100 && Guns >= 100 &&
            Lockpick >= 100 && Medicine >= 100 && MeleeWeapons >= 100 && Repair >= 100 &&
            Science >= 100 && Sneak >= 100 && Speech >= 100 && Survival >= 100 && Unarmed >= 100;

        /// <summary>
        /// Level up when all skill points are spent (or all skills at 100) and not at max level.
        /// </summary>
        public bool CanLevelUp =>
            !IsMaxLevel && (SkillPoints == 0 || AllSkillsAtMax);

        /// <summary>
        /// Whether the next level requires a perk pick (even levels only in FNV).
        /// </summary>
        public bool NextLevelRequiresPerk => (Level + 1) % 2 == 0;

        /// <summary>
        /// Skill points per level = floor(INT/2) + 10 + (Educated ? 2 : 0)
        /// </summary>
        public int SkillPointsPerLevel
        {
            get
            {
                int points = (Intelligence / 2) + 10;
                if (GetPerkRank("Educated") > 0)
                    points += 2;
                return points;
            }
        }

        public int GetPerkRank(string perkName) =>
            TakenPerks.TryGetValue(perkName, out var rank) ? rank : 0;

        public bool TryIncreaseSpecialFromPerk(string specialName)
        {
            var property = GetType().GetProperty(specialName);
            if (property == null || property.PropertyType != typeof(int))
                return false;

            int current = (int)property.GetValue(this);
            if (current >= 10)
                return false;

            switch (specialName)
            {
                case nameof(Strength):
                    Current.Strength++;
                    CarryWeight += 10;
                    MeleeDamage = Strength * 0.5;
                    break;
                case nameof(Perception):
                    Current.Perception++;
                    break;
                case nameof(Endurance):
                    Current.Endurance++;
                    Health += 20;
                    PoisonResist += 5;
                    RadiationResist += 2;
                    break;
                case nameof(Charisma):
                    Current.Charisma++;
                    CompanionNerve += 5;
                    break;
                case nameof(Intelligence):
                    Current.Intelligence++;
                    SkillPoints++;
                    OnPropertyChanged(nameof(SkillPointsPerLevel));
                    OnPropertyChanged(nameof(CanLevelUp));
                    break;
                case nameof(Agility):
                    Current.Agility++;
                    ActionPoints += 3;
                    break;
                case nameof(Luck):
                    Current.Luck++;
                    CriticalChance += 1;
                    break;
                default:
                    return false;
            }

            RecalculateDerivedStats();
            OnPropertyChanged(specialName);
            OnPropertyChanged(null);
            OnPropertyChanged(nameof(CanLevelUp));
            return true;
        }

        public bool TryDecreaseSpecialFromPerk(string specialName)
        {
            var property = GetType().GetProperty(specialName);
            if (property == null || property.PropertyType != typeof(int))
                return false;

            int current = (int)property.GetValue(this);
            if (current <= 1)
                return false;

            switch (specialName)
            {
                case nameof(Strength):
                    Current.Strength--;
                    CarryWeight -= 10;
                    MeleeDamage = Strength * 0.5;
                    break;
                case nameof(Perception):
                    Current.Perception--;
                    break;
                case nameof(Endurance):
                    Current.Endurance--;
                    Health -= 20;
                    PoisonResist -= 5;
                    RadiationResist -= 2;
                    break;
                case nameof(Charisma):
                    Current.Charisma--;
                    CompanionNerve -= 5;
                    break;
                case nameof(Intelligence):
                    Current.Intelligence--;
                    if (SkillPoints > 0)
                        SkillPoints--;
                    OnPropertyChanged(nameof(SkillPointsPerLevel));
                    OnPropertyChanged(nameof(CanLevelUp));
                    break;
                case nameof(Agility):
                    Current.Agility--;
                    ActionPoints -= 3;
                    break;
                case nameof(Luck):
                    Current.Luck--;
                    CriticalChance -= 1;
                    break;
                default:
                    return false;
            }

            RecalculateDerivedStats();
            OnPropertyChanged(specialName);
            OnPropertyChanged(null);
            return true;
        }

        public void ApplySpecialFloor(int minimum)
        {
            void Floor(string name)
            {
                var property = GetType().GetProperty(name);
                int current = (int)property.GetValue(this);
                while (current < minimum)
                {
                    TryIncreaseSpecialFromPerk(name);
                    current = (int)property.GetValue(this);
                }
            }

            Floor(nameof(Strength));
            Floor(nameof(Perception));
            Floor(nameof(Endurance));
            Floor(nameof(Charisma));
            Floor(nameof(Intelligence));
            Floor(nameof(Agility));
            Floor(nameof(Luck));
        }

        public void ApplySpecialCap(int value)
        {
            void RaiseTo(string name)
            {
                var property = GetType().GetProperty(name);
                int current = (int)property.GetValue(this);
                while (current < value)
                {
                    TryIncreaseSpecialFromPerk(name);
                    current = (int)property.GetValue(this);
                }
            }

            RaiseTo(nameof(Strength));
            RaiseTo(nameof(Perception));
            RaiseTo(nameof(Endurance));
            RaiseTo(nameof(Charisma));
            RaiseTo(nameof(Intelligence));
            RaiseTo(nameof(Agility));
            RaiseTo(nameof(Luck));
        }

        public bool TryAddSkillPointsFromPerk(string skillName, int amount)
        {
            int current = skillName switch
            {
                nameof(Barter) => Barter,
                nameof(EnergyWeapons) => EnergyWeapons,
                nameof(Explosives) => Explosives,
                nameof(Guns) => Guns,
                nameof(Lockpick) => Lockpick,
                nameof(Medicine) => Medicine,
                nameof(MeleeWeapons) => MeleeWeapons,
                nameof(Repair) => Repair,
                nameof(Science) => Science,
                nameof(Sneak) => Sneak,
                nameof(Speech) => Speech,
                nameof(Survival) => Survival,
                nameof(Unarmed) => Unarmed,
                _ => -1
            };
            if (current < 0)
                return false;

            int updated = Math.Clamp(current + amount, 0, 100);
            if (updated == current)
                return false;

            switch (skillName)
            {
                case nameof(Barter): Current.Barter = updated; break;
                case nameof(EnergyWeapons): Current.EnergyWeapons = updated; break;
                case nameof(Explosives): Current.Explosives = updated; break;
                case nameof(Guns): Current.Guns = updated; break;
                case nameof(Lockpick): Current.Lockpick = updated; break;
                case nameof(Medicine): Current.Medicine = updated; break;
                case nameof(MeleeWeapons): Current.MeleeWeapons = updated; break;
                case nameof(Repair): Current.Repair = updated; break;
                case nameof(Science): Current.Science = updated; break;
                case nameof(Sneak): Current.Sneak = updated; break;
                case nameof(Speech): Current.Speech = updated; break;
                case nameof(Survival): Current.Survival = updated; break;
                case nameof(Unarmed): Current.Unarmed = updated; break;
            }

            OnPropertyChanged(skillName);
            OnPropertyChanged(nameof(CanLevelUp));
            if (skillName == nameof(Unarmed))
                UnarmedDamage = (Unarmed / 20.0) + 0.5;
            return true;
        }

        private static string FormatSkillName(string propertyName) => propertyName switch
        {
            nameof(EnergyWeapons) => "Energy Weapons",
            nameof(MeleeWeapons) => "Melee Weapons",
            _ => propertyName
        };

        private static string FormatChoicePerkLabel(string perkName, string intenseTrainingSpecial, string tagSkill)
        {
            if (!string.IsNullOrEmpty(intenseTrainingSpecial))
                return $"{perkName} {intenseTrainingSpecial}";
            if (!string.IsNullOrEmpty(tagSkill))
                return $"{perkName} {FormatSkillName(tagSkill)}";
            return perkName;
        }

        private static string FormatSkillSpendSummary(Dictionary<string, int> increases)
        {
            var parts = increases
                .Where(kvp => kvp.Value > 0)
                .OrderBy(kvp => FormatSkillName(kvp.Key))
                .Select(kvp => $"{FormatSkillName(kvp.Key)} +{kvp.Value}");
            return string.Join(", ", parts);
        }

        /// <summary>
        /// Level up the character. In FNV, perks are only chosen on even levels.
        /// </summary>
        public void LevelUp(
            FalloutNVPerk perk = null,
            string intenseTrainingSpecial = null,
            string tagSkill = null)
        {
            int nextLevel = Level + 1;
            bool needsPerk = nextLevel % 2 == 0;

            string spendSummary = FormatSkillSpendSummary(SkillIncreasesThisLevel);
            string description;
            if (needsPerk && perk != null)
            {
                string perkLabel = FormatChoicePerkLabel(perk.Name, intenseTrainingSpecial, tagSkill);
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
                newStats.SkillPoints = SkillPointsPerLevel;

                // Recalculate HP for new level
                newStats.Health = 95 + (newStats.Endurance * 20) + (newStats.Level * 5);
                if (GetPerkRank("Life Giver") > 0)
                    newStats.Health += 30;

                _history.Push(newStats);
                _skillIncreaseHistory.Push(new Dictionary<string, int>());
                _implantHistory.Push(new HashSet<string>(InstalledImplantNames));
                _questPerkHistory.Push(CloneQuestPerks(TakenQuestPerks));
                _skillBookCountHistory.Push(new Dictionary<string, int>(SkillBookCounts));
                _skillBookLocationHistory.Push(new HashSet<string>(FoundSkillBookLocations));
                _workbenchCrateHistory.Push(new Dictionary<int, int>(WorkbenchCrates));

                if (needsPerk && perk != null)
                {
                    ApplyPerk(perk);
                    _perkHistory.Push(perk);

                    if (!string.IsNullOrEmpty(intenseTrainingSpecial))
                        TryIncreaseSpecialFromPerk(intenseTrainingSpecial);

                    if (!string.IsNullOrEmpty(tagSkill))
                        TryAddSkillPointsFromPerk(tagSkill, 15);
                }
                else
                {
                    _perkHistory.Push(null);
                }

                OnPropertyChanged(null);
                OnPropertyChanged(nameof(OwnedPerks));
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
