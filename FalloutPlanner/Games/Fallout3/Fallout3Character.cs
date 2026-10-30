using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using FalloutPlanner.Games.Fallout3.Bobbleheads;
using FalloutPlanner.Games.Fallout3.Perks;
using FalloutPlanner.Games.Fallout3.SkillBooks;

namespace FalloutPlanner.Games.Fallout3
{
    public class Fallout3Character :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private Stack<Fallout3CharacterStats> _history = new();
        // Parallel stacks so undo restores spendable skill tracking and perk ownership
        private readonly Stack<Dictionary<string, int>> _skillIncreaseHistory = new();
        private readonly Stack<Fallout3Perk?> _perkHistory = new();
        private readonly Stack<HashSet<string>> _bobbleheadHistory = new();
        private readonly Stack<Dictionary<string, int>> _skillBookCountHistory = new();
        private readonly Stack<HashSet<string>> _skillBookLocationHistory = new();

        /// <summary>
        /// Clean named-character snapshot from the last Confirm (before bobbleheads / books / misc).
        /// Used when undoing from level 1 back to creation so mutated stats are discarded.
        /// </summary>
        private Fallout3CharacterStats _namedBaseline;

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

        public Fallout3CharacterStats Current => _history.Peek();
        private Dictionary<string, int> SkillIncreasesThisLevel => _skillIncreaseHistory.Peek();
        private HashSet<string> FoundBobbleheadNames => _bobbleheadHistory.Peek();
        private Dictionary<string, int> SkillBookCounts => _skillBookCountHistory.Peek();
        private HashSet<string> FoundSkillBookLocations => _skillBookLocationHistory.Peek();

        public bool IsBobbleheadFound(string name) =>
            FoundBobbleheadNames.Contains(name);

        public IReadOnlyCollection<string> FoundBobbleheads => FoundBobbleheadNames;

        /// <summary>
        /// Marks a bobblehead as found and applies its effect (SPECIAL capped at 10, skills at 100).
        /// </summary>
        public bool TryCollectBobblehead(Fallout3Bobblehead bobblehead)
        {
            if (bobblehead == null || FoundBobbleheadNames.Contains(bobblehead.Name))
                return false;

            RecordLoggedAction($"Found Bobblehead: {bobblehead.Name}", () =>
            {
                FoundBobbleheadNames.Add(bobblehead.Name);
                bobblehead.ApplyEffect?.Invoke(this);
                OnPropertyChanged(null);
            });
            return true;
        }

        /// <summary>
        /// Removes a found bobblehead and reverses its effect (SPECIAL min 1, skills clamp 0–100).
        /// </summary>
        public bool TryRemoveBobblehead(Fallout3Bobblehead bobblehead)
        {
            if (bobblehead == null || !FoundBobbleheadNames.Remove(bobblehead.Name))
                return false;

            if (bobblehead.EffectType == BobbleheadEffectType.Special)
                TryDecreaseSpecialFromPerk(bobblehead.StatPropertyName);
            else
                TryAddSkillPointsFromPerk(bobblehead.StatPropertyName, -bobblehead.Amount);

            OnPropertyChanged(null);
            return true;
        }

        //Perk Logic
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
                        var perk = Fallout3PerkRepository.AllPerks.FirstOrDefault(p => p.Name == kvp.Key);
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

        public bool IsMiscPerkBlocked(string perkName)
        {
            if (perkName == Fallout3QuestPerkRepository.DreamCrusher &&
                HasQuestPerk(Fallout3QuestPerkRepository.SurvivalExpert))
                return true;
            if (perkName == Fallout3QuestPerkRepository.SurvivalExpert &&
                HasQuestPerk(Fallout3QuestPerkRepository.DreamCrusher))
                return true;
            return false;
        }

        /// <param name="effectsAlreadyApplied">
        /// True when the caller already applied choice-based effects (Ant Might/Sight, Survival Expert).
        /// </param>
        public bool TryTakeQuestPerk(
            Fallout3Perk perk, QuestPerkState? state = null, bool effectsAlreadyApplied = false)
        {
            if (perk == null || TakenQuestPerks.ContainsKey(perk.Name))
                return false;
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
                // Choice-based misc perks apply their effects here so undo reverts them
                if (perk.Name == Fallout3QuestPerkRepository.AntMightSight &&
                    !string.IsNullOrEmpty(resolved.ChoiceDetail))
                {
                    TryIncreaseSpecialFromPerk(resolved.ChoiceDetail);
                }
                else if (perk.Name == Fallout3QuestPerkRepository.SurvivalExpert &&
                         !string.IsNullOrEmpty(resolved.ChoiceDetail))
                {
                    var parts = resolved.ChoiceDetail.Split('|');
                    if (parts.Length == 2 &&
                        Enum.TryParse(parts[0], out Fallout3QuestPerkRepository.SurvivalRank rank) &&
                        Enum.TryParse(parts[1], out Fallout3QuestPerkRepository.SurvivalVariant variant))
                    {
                        Fallout3QuestPerkRepository.ApplySurvivalExpert(this, rank, variant);
                    }
                }
                else if (!effectsAlreadyApplied)
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

            Fallout3QuestPerkRepository.RemoveMiscPerkEffects(this, state);
            TakenQuestPerks.Remove(perkName);
            OnPropertyChanged(null);
            OnPropertyChanged(nameof(OwnedPerks));
            return true;
        }

        public int GetSkillBookCount(string bookName) =>
            SkillBookCounts.TryGetValue(bookName, out var n) ? n : 0;

        public bool IsSkillBookLocationFound(string locationId) =>
            FoundSkillBookLocations.Contains(locationId);

        public string? ActiveHouseThemeGroup
        {
            get
            {
                foreach (var loc in Fallout3SkillBookRepository.Locations)
                {
                    if (loc.ExclusiveGroup == null)
                        continue;
                    if (FoundSkillBookLocations.Contains(loc.Id))
                        return loc.ExclusiveGroup;
                }
                return null;
            }
        }

        public int SkillBookPointsPerBook =>
            GetPerkRank("Comprehension") > 0 ? 2 : 1;

        public bool TrySetSkillBookCount(string bookName, int newCount)
        {
            var book = Fallout3SkillBookRepository.GetBookType(bookName);
            if (book == null)
                return false;

            int current = GetSkillBookCount(bookName);
            if (book.MaxCount >= 0)
                newCount = Math.Clamp(newCount, 0, book.MaxCount);
            else
                newCount = Math.Max(0, newCount);

            if (newCount == current)
                return false;

            // Only increases are allowed; decreases must go through Undo.
            // Checklist is never auto-updated from the counter.
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

        private void ApplySkillBookCountDelta(Fallout3SkillBookType book, int deltaBooks)
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

        /// <summary>
        /// Checklist-only tracking. Does not change skill book counts, skills, or the action log.
        /// </summary>
        public bool TrySetSkillBookLocationFound(string locationId, bool found)
        {
            var loc = Fallout3SkillBookRepository.Locations.FirstOrDefault(l => l.Id == locationId);
            if (loc == null)
                return false;

            bool currently = FoundSkillBookLocations.Contains(locationId);
            if (currently == found)
                return true;

            if (found)
            {
                if (loc.ExclusiveGroup != null)
                {
                    var opposing = Fallout3SkillBookRepository.OpposingHouseGroup(loc.ExclusiveGroup);
                    if (opposing != null && ActiveHouseThemeGroup == opposing)
                        return false;
                }

                FoundSkillBookLocations.Add(locationId);
            }
            else
            {
                FoundSkillBookLocations.Remove(locationId);
            }

            return true;
        }

        public bool CanTakePerk(Fallout3Perk perk, int? levelOverride = null)
        {
            if (!perk.MeetsStatRequirements(this, levelOverride))
                return false;

            if (TakenPerks.ContainsKey(perk.Name) &&
                TakenPerks[perk.Name] >= perk.MaxRank)
                return false;

            return true;
        }

        /// <summary>
        /// Perk availability when leveling up (checks the level you are becoming).
        /// </summary>
        public bool CanTakePerkOnLevelUp(Fallout3Perk perk) =>
            CanTakePerk(perk, Level + 1);

        public void TakePerk(Fallout3Perk perk)
        {
            if (!CanTakePerk(perk))
                return;

            ApplyPerk(perk);
            OnPropertyChanged(null);
        }

        private void ApplyPerk(Fallout3Perk perk)
        {
            if (!TakenPerks.ContainsKey(perk.Name))
                TakenPerks[perk.Name] = 0;

            TakenPerks[perk.Name]++;
            perk.ApplyEffect?.Invoke(this);
        }

        private void UntakePerk(Fallout3Perk perk)
        {
            if (!TakenPerks.ContainsKey(perk.Name))
                return;

            TakenPerks[perk.Name]--;
            if (TakenPerks[perk.Name] <= 0)
                TakenPerks.Remove(perk.Name);
        }

        //Skill Logic
        public bool ModifySkill(string skillName, int amount)
        {
            var property = GetType().GetProperty(skillName);

            if (property == null || property.PropertyType != typeof(int))
                return false;

            int currentValue = (int)property.GetValue(this);
            int newValue = currentValue + amount;

            // Increase
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

            // Decrease
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
            OnPropertyChanged(nameof(CanIncreaseBigGuns));
            OnPropertyChanged(nameof(CanIncreaseEnergyWeapons));
            OnPropertyChanged(nameof(CanIncreaseExplosives));
            OnPropertyChanged(nameof(CanIncreaseLockpick));
            OnPropertyChanged(nameof(CanIncreaseMedicine));
            OnPropertyChanged(nameof(CanIncreaseMeleeWeapons));
            OnPropertyChanged(nameof(CanIncreaseRepair));
            OnPropertyChanged(nameof(CanIncreaseScience));
            OnPropertyChanged(nameof(CanIncreaseSmallGuns));
            OnPropertyChanged(nameof(CanIncreaseSneak));
            OnPropertyChanged(nameof(CanIncreaseSpeech));
            OnPropertyChanged(nameof(CanIncreaseUnarmed));

            OnPropertyChanged(nameof(CanDecreaseBarter));
            OnPropertyChanged(nameof(CanDecreaseBigGuns));
            OnPropertyChanged(nameof(CanDecreaseEnergyWeapons));
            OnPropertyChanged(nameof(CanDecreaseExplosives));
            OnPropertyChanged(nameof(CanDecreaseLockpick));
            OnPropertyChanged(nameof(CanDecreaseMedicine));
            OnPropertyChanged(nameof(CanDecreaseMeleeWeapons));
            OnPropertyChanged(nameof(CanDecreaseRepair));
            OnPropertyChanged(nameof(CanDecreaseScience));
            OnPropertyChanged(nameof(CanDecreaseSmallGuns));
            OnPropertyChanged(nameof(CanDecreaseSneak));
            OnPropertyChanged(nameof(CanDecreaseSpeech));
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

        // Barter
        public bool CanIncreaseBarter => CanIncreaseSkill(nameof(Barter));
        public bool CanDecreaseBarter => CanDecreaseSkill(nameof(Barter));

        // Big Guns
        public bool CanIncreaseBigGuns => CanIncreaseSkill(nameof(BigGuns));
        public bool CanDecreaseBigGuns => CanDecreaseSkill(nameof(BigGuns));

        // Energy Weapons
        public bool CanIncreaseEnergyWeapons => CanIncreaseSkill(nameof(EnergyWeapons));
        public bool CanDecreaseEnergyWeapons => CanDecreaseSkill(nameof(EnergyWeapons));

        // Explosives
        public bool CanIncreaseExplosives => CanIncreaseSkill(nameof(Explosives));
        public bool CanDecreaseExplosives => CanDecreaseSkill(nameof(Explosives));

        // Lockpick
        public bool CanIncreaseLockpick => CanIncreaseSkill(nameof(Lockpick));
        public bool CanDecreaseLockpick => CanDecreaseSkill(nameof(Lockpick));

        // Medicine
        public bool CanIncreaseMedicine => CanIncreaseSkill(nameof(Medicine));
        public bool CanDecreaseMedicine => CanDecreaseSkill(nameof(Medicine));

        // Melee Weapons
        public bool CanIncreaseMeleeWeapons => CanIncreaseSkill(nameof(MeleeWeapons));
        public bool CanDecreaseMeleeWeapons => CanDecreaseSkill(nameof(MeleeWeapons));

        // Repair
        public bool CanIncreaseRepair => CanIncreaseSkill(nameof(Repair));
        public bool CanDecreaseRepair => CanDecreaseSkill(nameof(Repair));

        // Science
        public bool CanIncreaseScience => CanIncreaseSkill(nameof(Science));
        public bool CanDecreaseScience => CanDecreaseSkill(nameof(Science));

        // Small Guns
        public bool CanIncreaseSmallGuns => CanIncreaseSkill(nameof(SmallGuns));
        public bool CanDecreaseSmallGuns => CanDecreaseSkill(nameof(SmallGuns));

        // Sneak
        public bool CanIncreaseSneak => CanIncreaseSkill(nameof(Sneak));
        public bool CanDecreaseSneak => CanDecreaseSkill(nameof(Sneak));

        // Speech
        public bool CanIncreaseSpeech => CanIncreaseSkill(nameof(Speech));
        public bool CanDecreaseSpeech => CanDecreaseSkill(nameof(Speech));

        // Unarmed
        public bool CanIncreaseUnarmed => CanIncreaseSkill(nameof(Unarmed));
        public bool CanDecreaseUnarmed => CanDecreaseSkill(nameof(Unarmed));

        //Constructor
        public string Name { get; set; }
        public int Level => Current.Level;

        /// <summary>
        /// Equipment loadout names (equipment page only — leveling stats ignore these).
        /// </summary>
        public string EquippedEyewearName { get; set; }
        public string EquippedHeadwearName { get; set; }
        public string EquippedBodyName { get; set; }

        public Fallout3Character(Fallout3CharacterStats baseStats)
        {
            _history.Push(baseStats.Clone());
            _skillIncreaseHistory.Push(new Dictionary<string, int>());
            _perkHistory.Push(null);
            _bobbleheadHistory.Push(new HashSet<string>());
            _questPerkHistory.Push(new Dictionary<string, QuestPerkState>());
            _skillBookCountHistory.Push(new Dictionary<string, int>());
            _skillBookLocationHistory.Push(new HashSet<string>());
            RecalculateDerivedStats();
            OnPropertyChanged(null);
        }

        public void SaveInitialState()
        {
            // Grant level-1 skill points so the leveling screen shows them immediately
            SkillPoints = SkillPointsPerLevel;

            var snapshot = CreateCurrentSnapshot();
            // Keep a pristine copy for undo-back to creation (collectibles mutate Current in place)
            _namedBaseline = snapshot.Clone();

            // Always reset to [working, named] so undo-back never stacks extra states
            _history.Clear();
            _skillIncreaseHistory.Clear();
            _perkHistory.Clear();
            _bobbleheadHistory.Clear();
            _questPerkHistory.Clear();
            _skillBookCountHistory.Clear();
            _skillBookLocationHistory.Clear();
            TakenPerks.Clear();

            _history.Push(snapshot.Clone());
            _history.Push(snapshot);
            _skillIncreaseHistory.Push(new Dictionary<string, int>());
            _skillIncreaseHistory.Push(new Dictionary<string, int>());
            _perkHistory.Push(null);
            _perkHistory.Push(null);
            _bobbleheadHistory.Push(new HashSet<string>());
            _bobbleheadHistory.Push(new HashSet<string>());
            _questPerkHistory.Push(new Dictionary<string, QuestPerkState>());
            _questPerkHistory.Push(new Dictionary<string, QuestPerkState>());
            _skillBookCountHistory.Push(new Dictionary<string, int>());
            _skillBookCountHistory.Push(new Dictionary<string, int>());
            _skillBookLocationHistory.Push(new HashSet<string>());
            _skillBookLocationHistory.Push(new HashSet<string>());

            ResetActionLog($"Created Character: {Name}");
            OnPropertyChanged(null);
        }

        /// <summary>
        /// Collapses history to a single working SPECIAL state when returning
        /// from the level page back to character creation.
        /// Restores stats from the named baseline so bobblehead / book / misc bonuses are discarded.
        /// </summary>
        public void RevertToCreationState()
        {
            var working = (_namedBaseline ?? Current).Clone();
            working.SkillPoints = 0;
            _history.Clear();
            _skillIncreaseHistory.Clear();
            _perkHistory.Clear();
            _bobbleheadHistory.Clear();
            _questPerkHistory.Clear();
            _skillBookCountHistory.Clear();
            _skillBookLocationHistory.Clear();
            TakenPerks.Clear();
            _history.Push(working);
            _skillIncreaseHistory.Push(new Dictionary<string, int>());
            _perkHistory.Push(null);
            _bobbleheadHistory.Push(new HashSet<string>());
            _questPerkHistory.Push(new Dictionary<string, QuestPerkState>());
            _skillBookCountHistory.Push(new Dictionary<string, int>());
            _skillBookLocationHistory.Push(new HashSet<string>());
            ActionLog.Clear();
            _actionStates.Clear();
            OnPropertyChanged(null);
        }

        private void ResetActionLog(string createdEntry)
        {
            ActionLog.Clear();
            _actionStates.Clear();
            // Oldest entry lives at the bottom; newer actions are inserted at the top
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
                Bobbleheads = new HashSet<string>(FoundBobbleheadNames),
                SkillBookCounts = new Dictionary<string, int>(SkillBookCounts),
                SkillBookLocations = new HashSet<string>(FoundSkillBookLocations),
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
            _bobbleheadHistory.Clear();
            _questPerkHistory.Clear();
            _skillBookCountHistory.Clear();
            _skillBookLocationHistory.Clear();

            // Keep a two-frame stack: named baseline underneath, restored working state on top
            _history.Push((_namedBaseline ?? state.Stats).Clone());
            _history.Push(state.Stats.Clone());
            _skillIncreaseHistory.Push(new Dictionary<string, int>());
            _skillIncreaseHistory.Push(new Dictionary<string, int>(state.SkillIncreases));
            _perkHistory.Push(null);
            _perkHistory.Push(null);
            _bobbleheadHistory.Push(new HashSet<string>());
            _bobbleheadHistory.Push(new HashSet<string>(state.Bobbleheads));
            _questPerkHistory.Push(new Dictionary<string, QuestPerkState>());
            _questPerkHistory.Push(CloneQuestPerks(state.QuestPerks));
            _skillBookCountHistory.Push(new Dictionary<string, int>());
            _skillBookCountHistory.Push(new Dictionary<string, int>(state.SkillBookCounts));
            _skillBookLocationHistory.Push(new HashSet<string>());
            _skillBookLocationHistory.Push(new HashSet<string>(state.SkillBookLocations));

            OnPropertyChanged(null);
            OnPropertyChanged(nameof(OwnedPerks));
        }

        /// <summary>
        /// Records a player action in the history list and snapshots state for undo.
        /// Newest actions are shown at the top. Refreshes the previous snapshot first
        /// so unlogged skill spends are preserved.
        /// </summary>
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
                    ?? Fallout3PerkRepository.AllPerks.FirstOrDefault(p => p.Name == kvp.Value.Name)
                    ?? Fallout3QuestPerkRepository.All.FirstOrDefault(p => p.Name == kvp.Value.Name);
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

        private Fallout3CharacterStats CreateCurrentSnapshot()
        {
            return new Fallout3CharacterStats
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
                TaggedSkills = TaggedSkills,

                Barter = Barter,
                BigGuns = BigGuns,
                EnergyWeapons = EnergyWeapons,
                Explosives = Explosives,
                Lockpick = Lockpick,
                Medicine = Medicine,
                MeleeWeapons = MeleeWeapons,
                Repair = Repair,
                Science = Science,
                SmallGuns = SmallGuns,
                Sneak = Sneak,
                Speech = Speech,
                Unarmed = Unarmed,

                ActionPoints = ActionPoints,
                Health = Health,
                CarryWeight = CarryWeight,
                MeleeDamage = MeleeDamage,
                PoisonResist = PoisonResist,
                RadiationResist = RadiationResist,
                CriticalChance = CriticalChance,
                DamageResist = DamageResist,
                FireResist = FireResist,
                UnarmedDamage = UnarmedDamage,
                SkillPoints = SkillPoints
            };
        }

        //SPECIAL
        public void ModifySpecial(string statName, int amount)
        {
            var property = GetType().GetProperty(statName);

            if (property == null || property.PropertyType != typeof(int))
                return;

            int currentValue = (int)property.GetValue(this);
            int newValue = currentValue + amount;

            // SPECIAL range check
            if (newValue < 1 || newValue > 10)
                return;

            // Remaining points check
            if (amount > 0)
            {
                if (Points <= 0)
                    return;

                Points -= 1;
            }
            else // decreasing logic
            {
                Points += 1;
            }

            property.SetValue(this, newValue);

            RecalculateDerivedStats();
        }

        //TAG logic
        private HashSet<string> _taggedSkillNames = new();
        public bool ModifyTaggedSkill(string skillName, bool isToggled)
        {
            // Prevent toggling if already at 3 skills
            if (isToggled && TaggedSkills >= 3)
                return false; // 4th toggle not allowed

            if (isToggled)
            {
                if (!_taggedSkillNames.Add(skillName))
                    return true; // already tagged

                TaggedSkills++;
            }
            else
            {
                if (!_taggedSkillNames.Remove(skillName))
                    return true; // wasn't tagged

                TaggedSkills--;
            }

            // Recalculate so tagged +15 stays in sync when SPECIAL changes later
            RecalculateDerivedStats();
            OnPropertyChanged(nameof(TaggedSkills));
            OnPropertyChanged(nameof(CanConfirm));

            return true;
        }

        //SPECIAL

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

        //Character STATS

        public int ActionPoints
        {
            get => Current.ActionPoints;
            set
            {
                if (Current.ActionPoints != value)
                {
                    Current.ActionPoints = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Health
        {
            get => Current.Health;
            set
            {
                if (Current.Health != value)
                {
                    Current.Health = value;
                    OnPropertyChanged();
                }
            }
        }

        public int CarryWeight
        {
            get => Current.CarryWeight;
            set
            {
                if (Current.CarryWeight != value)
                {
                    Current.CarryWeight = value;
                    OnPropertyChanged();
                }
            }
        }

        public double CriticalChance
        {
            get => Current.CriticalChance;
            set
            {
                if (Current.CriticalChance != value)
                {
                    Current.CriticalChance = value;
                    OnPropertyChanged();
                }
            }
        }

        public int DamageResist
        {
            get => Current.DamageResist;
            set
            {
                if (Current.DamageResist != value)
                {
                    Current.DamageResist = value;
                    OnPropertyChanged();
                }
            }
        }

        public int FireResist
        {
            get => Current.FireResist;
            set
            {
                if (Current.FireResist != value)
                {
                    Current.FireResist = value;
                    OnPropertyChanged();
                }
            }
        }

        public int PoisonResist
        {
            get => Current.PoisonResist;
            set
            {
                if (Current.PoisonResist != value)
                {
                    Current.PoisonResist = value;
                    OnPropertyChanged();
                }
            }
        }

        public int RadiationResist
        {
            get => Current.RadiationResist;
            set
            {
                if (Current.RadiationResist != value)
                {
                    Current.RadiationResist = value;
                    OnPropertyChanged();
                }
            }
        }

        public double MeleeDamage
        {
            get => Current.MeleeDamage;
            set
            {
                if (Current.MeleeDamage != value)
                {
                    Current.MeleeDamage = value;
                    OnPropertyChanged();
                }
            }
        }

        public int UnarmedDamage
        {
            get => Current.UnarmedDamage;
            set
            {
                if (Current.UnarmedDamage != value)
                {
                    Current.UnarmedDamage = value;
                    OnPropertyChanged();
                }
            }
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


        //Character Skills
        public int Barter
        {
            get => Current.Barter;
            set
            {
                if (Current.Barter != value)
                {
                    Current.Barter = value;
                    OnPropertyChanged();
                }
            }
        }

        public int BigGuns
        {
            get => Current.BigGuns;
            set
            {
                if (Current.BigGuns != value)
                {
                    Current.BigGuns = value;
                    OnPropertyChanged();
                }
            }
        }

        public int EnergyWeapons
        {
            get => Current.EnergyWeapons;
            set
            {
                if (Current.EnergyWeapons != value)
                {
                    Current.EnergyWeapons = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Explosives
        {
            get => Current.Explosives;
            set
            {
                if (Current.Explosives != value)
                {
                    Current.Explosives = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Lockpick
        {
            get => Current.Lockpick;
            set
            {
                if (Current.Lockpick != value)
                {
                    Current.Lockpick = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Medicine
        {
            get => Current.Medicine;
            set
            {
                if (Current.Medicine != value)
                {
                    Current.Medicine = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MeleeWeapons
        {
            get => Current.MeleeWeapons;
            set
            {
                if (Current.MeleeWeapons != value)
                {
                    Current.MeleeWeapons = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Repair
        {
            get => Current.Repair;
            set
            {
                if (Current.Repair != value)
                {
                    Current.Repair = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Science
        {
            get => Current.Science;
            set
            {
                if (Current.Science != value)
                {
                    Current.Science = value;
                    OnPropertyChanged();
                }
            }
        }

        public int SmallGuns
        {
            get => Current.SmallGuns;
            set
            {
                if (Current.SmallGuns != value)
                {
                    Current.SmallGuns = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Sneak
        {
            get => Current.Sneak;
            set
            {
                if (Current.Sneak != value)
                {
                    Current.Sneak = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Speech
        {
            get => Current.Speech;
            set
            {
                if (Current.Speech != value)
                {
                    Current.Speech = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Unarmed
        {
            get => Current.Unarmed;
            set
            {
                if (Current.Unarmed != value)
                {
                    Current.Unarmed = value;
                    OnPropertyChanged();
                }
            }
        }


        //Special change calculations
        private int CalculateSkill(int baseValue, string skillName)
        {
            if (_taggedSkillNames.Contains(skillName))
                return baseValue + 15;

            return baseValue;
        }

        private void RecalculateDerivedStats()
        {
            var stats = Current;

            // Derived stats
            stats.ActionPoints = 65 + (stats.Agility * 2);
            stats.Health = 100 + (stats.Endurance * 20);
            stats.CarryWeight = 150 + (stats.Strength * 10);
            stats.MeleeDamage = stats.Strength * 0.5;
            stats.PoisonResist = (stats.Endurance - 1) * 5;
            stats.RadiationResist = (stats.Endurance - 1) * 2;
            stats.CriticalChance = stats.Luck;

            // Starting Stats Formulas
            stats.Barter = CalculateSkill(2 + (stats.Luck / 2) + (stats.Luck % 2) + (stats.Charisma * 2), nameof(Barter));
            stats.BigGuns = CalculateSkill(2 + (stats.Luck / 2) + (stats.Luck % 2) + (stats.Endurance * 2), nameof(BigGuns));
            stats.EnergyWeapons = CalculateSkill(2 + (stats.Luck / 2) + (stats.Luck % 2) + (stats.Perception * 2), nameof(EnergyWeapons));
            stats.Explosives = CalculateSkill(2 + (stats.Luck / 2) + (stats.Luck % 2) + (stats.Perception * 2), nameof(Explosives));
            stats.Lockpick = CalculateSkill(2 + (stats.Luck / 2) + (stats.Luck % 2) + (stats.Perception * 2), nameof(Lockpick));
            stats.Medicine = CalculateSkill(2 + (stats.Luck / 2) + (stats.Luck % 2) + (stats.Intelligence * 2), nameof(Medicine));
            stats.MeleeWeapons = CalculateSkill(2 + (stats.Luck / 2) + (stats.Luck % 2) + (stats.Strength * 2), nameof(MeleeWeapons));
            stats.Repair = CalculateSkill(2 + (stats.Luck / 2) + (stats.Luck % 2) + (stats.Intelligence * 2), nameof(Repair));
            stats.Science = CalculateSkill(2 + (stats.Luck / 2) + (stats.Luck % 2) + (stats.Intelligence * 2), nameof(Science));
            stats.SmallGuns = CalculateSkill(2 + (stats.Luck / 2) + (stats.Luck % 2) + (stats.Agility * 2), nameof(SmallGuns));
            stats.Sneak = CalculateSkill(2 + (stats.Luck / 2) + (stats.Luck % 2) + (stats.Agility * 2), nameof(Sneak));
            stats.Speech = CalculateSkill(2 + (stats.Luck / 2) + (stats.Luck % 2) + (stats.Charisma * 2), nameof(Speech));
            stats.Unarmed = CalculateSkill(2 + (stats.Luck / 2) + (stats.Luck % 2) + (stats.Endurance * 2), nameof(Unarmed));

            stats.UnarmedDamage = stats.Unarmed switch
            {
                <= 10 => 1,
                <= 30 => 2,
                <= 50 => 3,
                <= 70 => 4,
                <= 90 => 5,
                _ => 6
            };

            // Update ui (includes SkillPointsPerLevel when Intelligence changes)
            OnPropertyChanged(null);
            OnPropertyChanged(nameof(SkillPointsPerLevel));
        }


        //Create Character Confirmation
        public bool CanConfirm => Points == 0 && TaggedSkills == 3;

        public const int MaxLevel = 30;

        public bool IsMaxLevel => Level >= MaxLevel;

        public bool AllSkillsAtMax =>
            Barter >= 100 && BigGuns >= 100 && EnergyWeapons >= 100 && Explosives >= 100 &&
            Lockpick >= 100 && Medicine >= 100 && MeleeWeapons >= 100 && Repair >= 100 &&
            Science >= 100 && SmallGuns >= 100 && Sneak >= 100 && Speech >= 100 && Unarmed >= 100;

        /// <summary>
        /// Level up when all skill points are spent, or when every skill is already at 100
        /// (leftover points cannot be spent) and the character is not max level.
        /// </summary>
        public bool CanLevelUp =>
            !IsMaxLevel && (SkillPoints == 0 || AllSkillsAtMax);

        /// <summary>
        /// Skill points gained each level (10 + Intelligence).
        /// Shown on the creation screen so the player knows what to expect.
        /// </summary>
        public int SkillPointsPerLevel => 10 + Intelligence;

        public int GetPerkRank(string perkName) =>
            TakenPerks.TryGetValue(perkName, out var rank) ? rank : 0;

        /// <summary>
        /// Increases a SPECIAL by 1 (Intense Training / No Weaknesses / Almost Perfect)
        /// and adjusts dependent skills by the formula delta so spent points and perk
        /// bonuses are preserved.
        /// </summary>
        public bool TryIncreaseSpecialFromPerk(string specialName)
        {
            var property = GetType().GetProperty(specialName);
            if (property == null || property.PropertyType != typeof(int))
                return false;

            int current = (int)property.GetValue(this);
            if (current >= 10)
                return false;

            int oldStr = Strength, oldPer = Perception, oldEnd = Endurance,
                oldCha = Charisma, oldInt = Intelligence, oldAgi = Agility, oldLuk = Luck;

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
                    break;
                case nameof(Intelligence):
                    Current.Intelligence++;
                    // Intense Training / bobblehead INT also grants +1 skill point for the current level
                    SkillPoints++;
                    OnPropertyChanged(nameof(SkillPointsPerLevel));
                    OnPropertyChanged(nameof(CanLevelUp));
                    break;
                case nameof(Agility):
                    Current.Agility++;
                    ActionPoints += 2;
                    break;
                case nameof(Luck):
                    Current.Luck++;
                    CriticalChance += 1;
                    break;
                default:
                    return false;
            }

            ApplySkillDeltasForSpecialChange(oldStr, oldPer, oldEnd, oldCha, oldInt, oldAgi, oldLuk);

            OnPropertyChanged(specialName);
            OnPropertyChanged(null);
            OnPropertyChanged(nameof(CanLevelUp));
            return true;
        }

        /// <summary>
        /// Decreases a SPECIAL by 1 (bobblehead remove) and adjusts dependent skills by formula delta.
        /// </summary>
        public bool TryDecreaseSpecialFromPerk(string specialName)
        {
            var property = GetType().GetProperty(specialName);
            if (property == null || property.PropertyType != typeof(int))
                return false;

            int current = (int)property.GetValue(this);
            if (current <= 1)
                return false;

            int oldStr = Strength, oldPer = Perception, oldEnd = Endurance,
                oldCha = Charisma, oldInt = Intelligence, oldAgi = Agility, oldLuk = Luck;

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
                    ActionPoints -= 2;
                    break;
                case nameof(Luck):
                    Current.Luck--;
                    CriticalChance -= 1;
                    break;
                default:
                    return false;
            }

            ApplySkillDeltasForSpecialChange(oldStr, oldPer, oldEnd, oldCha, oldInt, oldAgi, oldLuk);

            OnPropertyChanged(specialName);
            OnPropertyChanged(null);
            return true;
        }

        private static int GetSpecialSkillBase(
            string skillName, int str, int per, int end, int cha, int intel, int agi, int luk)
        {
            int luckPart = luk / 2 + luk % 2;
            return skillName switch
            {
                nameof(Barter) => 2 + luckPart + cha * 2,
                nameof(BigGuns) => 2 + luckPart + end * 2,
                nameof(EnergyWeapons) => 2 + luckPart + per * 2,
                nameof(Explosives) => 2 + luckPart + per * 2,
                nameof(Lockpick) => 2 + luckPart + per * 2,
                nameof(Medicine) => 2 + luckPart + intel * 2,
                nameof(MeleeWeapons) => 2 + luckPart + str * 2,
                nameof(Repair) => 2 + luckPart + intel * 2,
                nameof(Science) => 2 + luckPart + intel * 2,
                nameof(SmallGuns) => 2 + luckPart + agi * 2,
                nameof(Sneak) => 2 + luckPart + agi * 2,
                nameof(Speech) => 2 + luckPart + cha * 2,
                nameof(Unarmed) => 2 + luckPart + end * 2,
                _ => 0
            };
        }

        private void ApplySkillDeltasForSpecialChange(
            int oldStr, int oldPer, int oldEnd, int oldCha, int oldInt, int oldAgi, int oldLuk)
        {
            void Adjust(string skillName)
            {
                int oldBase = CalculateSkill(
                    GetSpecialSkillBase(skillName, oldStr, oldPer, oldEnd, oldCha, oldInt, oldAgi, oldLuk),
                    skillName);
                int newBase = CalculateSkill(
                    GetSpecialSkillBase(skillName, Strength, Perception, Endurance, Charisma, Intelligence, Agility, Luck),
                    skillName);
                int delta = newBase - oldBase;
                if (delta != 0)
                    TryAddSkillPointsFromPerk(skillName, delta);
            }

            Adjust(nameof(Barter));
            Adjust(nameof(BigGuns));
            Adjust(nameof(EnergyWeapons));
            Adjust(nameof(Explosives));
            Adjust(nameof(Lockpick));
            Adjust(nameof(Medicine));
            Adjust(nameof(MeleeWeapons));
            Adjust(nameof(Repair));
            Adjust(nameof(Science));
            Adjust(nameof(SmallGuns));
            Adjust(nameof(Sneak));
            Adjust(nameof(Speech));
            Adjust(nameof(Unarmed));

            RefreshUnarmedDamage();
        }

        private void RefreshUnarmedDamage()
        {
            int fromSkill = Unarmed switch
            {
                <= 10 => 1,
                <= 30 => 2,
                <= 50 => 3,
                <= 70 => 4,
                <= 90 => 5,
                _ => 6
            };
            UnarmedDamage = fromSkill + GetPerkRank("Iron Fist") * 5;
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
                nameof(BigGuns) => BigGuns,
                nameof(EnergyWeapons) => EnergyWeapons,
                nameof(Explosives) => Explosives,
                nameof(Lockpick) => Lockpick,
                nameof(Medicine) => Medicine,
                nameof(MeleeWeapons) => MeleeWeapons,
                nameof(Repair) => Repair,
                nameof(Science) => Science,
                nameof(SmallGuns) => SmallGuns,
                nameof(Sneak) => Sneak,
                nameof(Speech) => Speech,
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
                case nameof(BigGuns): Current.BigGuns = updated; break;
                case nameof(EnergyWeapons): Current.EnergyWeapons = updated; break;
                case nameof(Explosives): Current.Explosives = updated; break;
                case nameof(Lockpick): Current.Lockpick = updated; break;
                case nameof(Medicine): Current.Medicine = updated; break;
                case nameof(MeleeWeapons): Current.MeleeWeapons = updated; break;
                case nameof(Repair): Current.Repair = updated; break;
                case nameof(Science): Current.Science = updated; break;
                case nameof(SmallGuns): Current.SmallGuns = updated; break;
                case nameof(Sneak): Current.Sneak = updated; break;
                case nameof(Speech): Current.Speech = updated; break;
                case nameof(Unarmed): Current.Unarmed = updated; break;
            }

            OnPropertyChanged(skillName);
            OnPropertyChanged(nameof(CanLevelUp));
            if (skillName == nameof(Unarmed))
                RefreshUnarmedDamage();
            return true;
        }

        private static string FormatSkillName(string propertyName) => propertyName switch
        {
            nameof(BigGuns) => "Big Guns",
            nameof(EnergyWeapons) => "Energy Weapons",
            nameof(MeleeWeapons) => "Melee Weapons",
            nameof(SmallGuns) => "Small Guns",
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

        //Character Leveling
        public void LevelUp(
            Fallout3Perk perk,
            string intenseTrainingSpecial = null,
            string tagSkill = null)
        {
            int nextLevel = Level + 1;
            int skillIncrease = 10 + Intelligence;
            if (TakenPerks.ContainsKey("Educated"))
                skillIncrease += 3;

            string spendSummary = FormatSkillSpendSummary(SkillIncreasesThisLevel);
            string perkLabel = FormatChoicePerkLabel(perk.Name, intenseTrainingSpecial, tagSkill);
            string description = string.IsNullOrEmpty(spendSummary)
                ? $"Level Up: Level {nextLevel}, Perk: {perkLabel}"
                : $"Level Up: Level {nextLevel}, {spendSummary}, Perk: {perkLabel}";

            RecordLoggedAction(description, () =>
            {
                var newStats = Current.Clone();

                newStats.Level++;
                newStats.SkillPoints = skillIncrease;

                _history.Push(newStats);
                // New level starts with no skill spends yet; prior level keeps its spend map
                _skillIncreaseHistory.Push(new Dictionary<string, int>());
                // Carry found bobbleheads / quest perks into the new level snapshot
                _bobbleheadHistory.Push(new HashSet<string>(FoundBobbleheadNames));
                _questPerkHistory.Push(CloneQuestPerks(TakenQuestPerks));
                _skillBookCountHistory.Push(new Dictionary<string, int>(SkillBookCounts));
                _skillBookLocationHistory.Push(new HashSet<string>(FoundSkillBookLocations));

                ApplyPerk(perk);
                _perkHistory.Push(perk);

                // Choice perks applied inside the same logged action so stats/skills/SP stay in sync
                if (!string.IsNullOrEmpty(intenseTrainingSpecial))
                    TryIncreaseSpecialFromPerk(intenseTrainingSpecial);

                if (!string.IsNullOrEmpty(tagSkill))
                    TryAddSkillPointsFromPerk(tagSkill, 15);

                OnPropertyChanged(null);
                OnPropertyChanged(nameof(OwnedPerks));
                OnPropertyChanged(nameof(CanLevelUp));
            });
        }

        /// <summary>
        /// Undoes the last logged action (or last level-up). Returns false when
        /// only the created-character entry remains so the UI can navigate back.
        /// </summary>
        public bool TryUndoLevel() => TryUndoLastAction();

        public void UndoLevel()
        {
            TryUndoLastAction();
        }
    }
}
