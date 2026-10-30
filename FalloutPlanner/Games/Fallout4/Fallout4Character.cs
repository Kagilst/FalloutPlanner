using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FalloutPlanner.Games.Fallout4.Bobbleheads;
using FalloutPlanner.Games.Fallout4.Magazines;
using FalloutPlanner.Games.Fallout4.Perks;

namespace FalloutPlanner.Games.Fallout4
{
    public class Fallout4Character : INotifyPropertyChanged
    {
        public const int MaxLevel = 300;
        public const int CreationPoints = 21;
        public const int MaxSpecialFromPoints = 10;
        public const int MaxSpecialAbsolute = 11;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly Stack<Fallout4CharacterStats> _history = new();
        private Fallout4CharacterStats? _namedBaseline;
        private readonly List<CharacterActionState> _actionStates = new();

        public ObservableCollection<string> ActionLog { get; } = new();
        public Dictionary<string, int> PerkRanks { get; } = new();
        public Dictionary<string, int> MiscPerkRanks { get; } = new();
        public HashSet<string> FoundBobbleheadNames { get; } = new();
        public Dictionary<string, int> MagazineStackCounts { get; } = new();
        public HashSet<string> CollectedMagazineIssueIds { get; } = new();
        public bool YoureSpecialCollected { get; private set; }
        public string? YoureSpecialChoice { get; private set; }

        /// <summary>Id of the disk save this session is linked to, if any.</summary>
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

        public Fallout4CharacterStats Current => _history.Peek();

        public Fallout4Character(Fallout4CharacterStats startingStats)
        {
            _history.Push(startingStats);
            RecalculateDerivedStats();
        }

        public string Name
        {
            get => Current.Name;
            set { if (Current.Name != value) { Current.Name = value; OnPropertyChanged(); } }
        }

        public int Level
        {
            get => Current.Level;
            set { if (Current.Level != value) { Current.Level = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanLevelUp)); } }
        }

        public int Points
        {
            get => Current.Points;
            set
            {
                if (Current.Points != value)
                {
                    Current.Points = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanConfirmCreation));
                    NotifySpecialButtons();
                }
            }
        }

        public int PerkPoints
        {
            get => Current.PerkPoints;
            set
            {
                if (Current.PerkPoints != value)
                {
                    Current.PerkPoints = value;
                    OnPropertyChanged();
                    NotifySpecialButtons();
                    OnPropertyChanged(nameof(CanTakeSelectedPerkHint));
                }
            }
        }

        public int Strength
        {
            get => Current.Strength;
            set { SetSpecial(nameof(Strength), value, v => Current.Strength = v); }
        }

        public int Perception
        {
            get => Current.Perception;
            set { SetSpecial(nameof(Perception), value, v => Current.Perception = v); }
        }

        public int Endurance
        {
            get => Current.Endurance;
            set { SetSpecial(nameof(Endurance), value, v => Current.Endurance = v); }
        }

        public int Charisma
        {
            get => Current.Charisma;
            set { SetSpecial(nameof(Charisma), value, v => Current.Charisma = v); }
        }

        public int Intelligence
        {
            get => Current.Intelligence;
            set { SetSpecial(nameof(Intelligence), value, v => Current.Intelligence = v); }
        }

        public int Agility
        {
            get => Current.Agility;
            set { SetSpecial(nameof(Agility), value, v => Current.Agility = v); }
        }

        public int Luck
        {
            get => Current.Luck;
            set { SetSpecial(nameof(Luck), value, v => Current.Luck = v); }
        }

        public int HitPoints
        {
            get => Current.HitPoints;
            private set { if (Current.HitPoints != value) { Current.HitPoints = value; OnPropertyChanged(); } }
        }

        public int ActionPoints
        {
            get => Current.ActionPoints;
            private set { if (Current.ActionPoints != value) { Current.ActionPoints = value; OnPropertyChanged(); } }
        }

        public int CarryWeight
        {
            get => Current.CarryWeight;
            private set { if (Current.CarryWeight != value) { Current.CarryWeight = value; OnPropertyChanged(); } }
        }

        public bool CanConfirmCreation => Points == 0;
        public bool CanLevelUp => Level < MaxLevel;
        public bool IsMaxLevel => Level >= MaxLevel;

        public bool CanIncreaseStrengthCreation => Points > 0 && Strength < MaxSpecialFromPoints;
        public bool CanDecreaseStrengthCreation => Strength > 1;
        public bool CanIncreasePerceptionCreation => Points > 0 && Perception < MaxSpecialFromPoints;
        public bool CanDecreasePerceptionCreation => Perception > 1;
        public bool CanIncreaseEnduranceCreation => Points > 0 && Endurance < MaxSpecialFromPoints;
        public bool CanDecreaseEnduranceCreation => Endurance > 1;
        public bool CanIncreaseCharismaCreation => Points > 0 && Charisma < MaxSpecialFromPoints;
        public bool CanDecreaseCharismaCreation => Charisma > 1;
        public bool CanIncreaseIntelligenceCreation => Points > 0 && Intelligence < MaxSpecialFromPoints;
        public bool CanDecreaseIntelligenceCreation => Intelligence > 1;
        public bool CanIncreaseAgilityCreation => Points > 0 && Agility < MaxSpecialFromPoints;
        public bool CanDecreaseAgilityCreation => Agility > 1;
        public bool CanIncreaseLuckCreation => Points > 0 && Luck < MaxSpecialFromPoints;
        public bool CanDecreaseLuckCreation => Luck > 1;

        public bool CanIncreaseStrengthPerk => PerkPoints > 0 && Strength < MaxSpecialFromPoints;
        public bool CanIncreasePerceptionPerk => PerkPoints > 0 && Perception < MaxSpecialFromPoints;
        public bool CanIncreaseEndurancePerk => PerkPoints > 0 && Endurance < MaxSpecialFromPoints;
        public bool CanIncreaseCharismaPerk => PerkPoints > 0 && Charisma < MaxSpecialFromPoints;
        public bool CanIncreaseIntelligencePerk => PerkPoints > 0 && Intelligence < MaxSpecialFromPoints;
        public bool CanIncreaseAgilityPerk => PerkPoints > 0 && Agility < MaxSpecialFromPoints;
        public bool CanIncreaseLuckPerk => PerkPoints > 0 && Luck < MaxSpecialFromPoints;

        /// <summary>Bound for UI refresh hooks; always true as a placeholder.</summary>
        public bool CanTakeSelectedPerkHint => PerkPoints > 0;

        private void SetSpecial(string name, int value, Action<int> assign)
        {
            assign(value);
            RecalculateDerivedStats();
            OnPropertyChanged(name);
            NotifySpecialButtons();
        }

        public int GetSpecial(Fallout4SpecialAttribute attribute) => attribute switch
        {
            Fallout4SpecialAttribute.Strength => Strength,
            Fallout4SpecialAttribute.Perception => Perception,
            Fallout4SpecialAttribute.Endurance => Endurance,
            Fallout4SpecialAttribute.Charisma => Charisma,
            Fallout4SpecialAttribute.Intelligence => Intelligence,
            Fallout4SpecialAttribute.Agility => Agility,
            Fallout4SpecialAttribute.Luck => Luck,
            _ => 1
        };

        public int GetPerkRank(string perkName) =>
            PerkRanks.TryGetValue(perkName, out var r) ? r : 0;

        public int GetMiscPerkRank(string perkName) =>
            MiscPerkRanks.TryGetValue(perkName, out var r) ? r : 0;

        public bool HasMiscPerk(string perkName) => GetMiscPerkRank(perkName) > 0;

        /// <summary>Leveled chart perks + taken companion/quest misc perks.</summary>
        public IReadOnlyList<OwnedPerkInfo> GetOwnedPerks()
        {
            var list = new List<OwnedPerkInfo>();

            foreach (var kvp in PerkRanks.Where(k => k.Value > 0).OrderBy(k => k.Key))
            {
                var perk = Fallout4PerkRepository.GetPerk(kvp.Key);
                if (perk == null) continue;

                list.Add(new OwnedPerkInfo
                {
                    Name = perk.Name,
                    DisplayName = $"{perk.Name} (Rank {kvp.Value}/{perk.MaxRank})",
                    Rank = kvp.Value,
                    MaxRank = perk.MaxRank,
                    Requirements = $"{perk.AttributeShort} {perk.AttributeRequirement}",
                    Description = FormatOwnedRankDescriptions(perk, kvp.Value)
                });
            }

            foreach (var kvp in MiscPerkRanks.Where(k => k.Value > 0).OrderBy(k => k.Key))
            {
                var misc = Fallout4MiscPerkRepository.Get(kvp.Key);
                if (misc == null) continue;

                string display = misc.MaxRank > 1
                    ? $"{misc.ListDisplayName} (Rank {kvp.Value}/{misc.MaxRank})"
                    : misc.ListDisplayName;

                list.Add(new OwnedPerkInfo
                {
                    Name = misc.Name,
                    DisplayName = display,
                    Rank = kvp.Value,
                    MaxRank = misc.MaxRank,
                    Requirements = misc.CategoryLabel,
                    Description = misc.Description,
                    IsMiscPerk = true,
                    MiscPerk = misc
                });
            }

            return list;
        }

        public IReadOnlyList<OwnedPerkInfo> GetAvailableMiscPerks()
        {
            var list = new List<OwnedPerkInfo>();
            foreach (var misc in Fallout4MiscPerkRepository.All)
            {
                int owned = GetMiscPerkRank(misc.Name);
                if (owned >= misc.MaxRank)
                    continue;
                if (IsMiscPerkBlocked(misc))
                    continue;

                string display = misc.MaxRank > 1 && owned > 0
                    ? $"{misc.ListDisplayName} (Rank {owned}/{misc.MaxRank})"
                    : misc.ListDisplayName;

                list.Add(new OwnedPerkInfo
                {
                    Name = misc.Name,
                    DisplayName = display,
                    Rank = owned,
                    MaxRank = misc.MaxRank,
                    Requirements = misc.CategoryLabel,
                    Description = misc.Description,
                    IsMiscPerk = true,
                    MiscPerk = misc
                });
            }

            return list;
        }

        public bool IsMiscPerkBlocked(Fallout4MiscPerk perk)
        {
            if (perk == null || string.IsNullOrEmpty(perk.ExclusionGroup))
                return false;

            int ownedInGroup = Fallout4MiscPerkRepository.GetByGroup(perk.ExclusionGroup)
                .Count(p => HasMiscPerk(p.Name) && p.Name != perk.Name);

            // Already own this perk (rank-up path) — not blocked by group
            if (HasMiscPerk(perk.Name))
                return false;

            return ownedInGroup >= perk.ExclusionGroupLimit;
        }

        public bool CanTakeMiscPerk(Fallout4MiscPerk perk)
        {
            if (perk == null)
                return false;
            int owned = GetMiscPerkRank(perk.Name);
            if (owned >= perk.MaxRank)
                return false;
            return !IsMiscPerkBlocked(perk);
        }

        public bool TryTakeMiscPerk(Fallout4MiscPerk perk)
        {
            if (!CanTakeMiscPerk(perk))
                return false;

            int nextRank = GetMiscPerkRank(perk.Name) + 1;
            string label = perk.MaxRank > 1
                ? $"Misc Perk: {perk.Name} Rank {nextRank}"
                : $"Misc Perk: {perk.Name}";

            RecordLoggedAction(label, () =>
            {
                MiscPerkRanks[perk.Name] = nextRank;
                OnPropertyChanged(null);
            });

            return true;
        }

        public bool IsBobbleheadFound(string name) => FoundBobbleheadNames.Contains(name);

        public int GetMagazineStackCount(string seriesName) =>
            MagazineStackCounts.TryGetValue(seriesName, out var n) ? n : 0;

        public bool IsMagazineIssueCollected(string issueId) =>
            CollectedMagazineIssueIds.Contains(issueId);

        public bool CanCollectStackingMagazine(Fallout4MagazineSeries series) =>
            series != null && series.IsStacking && !series.GrantsSpecialChoice &&
            GetMagazineStackCount(series.Name) < series.MaxIssues;

        public bool CanCollectYoureSpecial() =>
            !YoureSpecialCollected;

        public IEnumerable<Fallout4MagazineIssue> GetUncollectedIssues(Fallout4MagazineSeries series) =>
            series.Issues.Where(i => !CollectedMagazineIssueIds.Contains(i.Id));

        public IReadOnlyList<CollectedMagazineEntry> GetCollectedMagazineEntries()
        {
            var entries = new List<CollectedMagazineEntry>();

            foreach (var series in Fallout4MagazineRepository.All.Where(s => s.IsStacking && !s.GrantsSpecialChoice))
            {
                int count = GetMagazineStackCount(series.Name);
                if (count <= 0) continue;

                entries.Add(new CollectedMagazineEntry
                {
                    Key = $"stack:{series.Name}",
                    SeriesName = series.Name,
                    DisplayName = $"{series.Name} ({count}/{series.MaxIssues})",
                    Description = Fallout4MagazineRepository.GetStackedDescription(series, count),
                    IsStacking = true
                });
            }

            foreach (var series in Fallout4MagazineRepository.All.Where(s => s.HasUniqueIssues))
            {
                foreach (var issue in series.Issues.Where(i => CollectedMagazineIssueIds.Contains(i.Id)))
                {
                    entries.Add(new CollectedMagazineEntry
                    {
                        Key = issue.Id,
                        SeriesName = series.Name,
                        DisplayName = $"{series.Name}: {issue.DisplayName}",
                        Description = issue.Effect + (string.IsNullOrEmpty(issue.Location) ? "" : $"\n\nLocation: {issue.Location}"),
                        IsStacking = false
                    });
                }
            }

            if (YoureSpecialCollected)
            {
                entries.Add(new CollectedMagazineEntry
                {
                    Key = "youre-special",
                    SeriesName = "You're SPECIAL!",
                    DisplayName = $"You're SPECIAL! (+1 {YoureSpecialChoice})",
                    Description =
                        $"Applied: +1 {YoureSpecialChoice}.\n\n" +
                        "Gain +1 to a SPECIAL attribute of you choice (you can raise a SPECIAL stat past 10 by getting a debuff for a stat already at 10 bringing it below 10 allowing you to choose it in the book prompt).",
                    IsStacking = false
                });
            }

            return entries.OrderBy(e => e.DisplayName).ToList();
        }

        public bool TryCollectBobblehead(Fallout4Bobblehead bobblehead)
        {
            if (bobblehead == null || FoundBobbleheadNames.Contains(bobblehead.Name))
                return false;

            RecordLoggedAction($"Found Bobblehead: {bobblehead.Name}", () =>
            {
                FoundBobbleheadNames.Add(bobblehead.Name);
                if (bobblehead.Kind == Fallout4BobbleheadKind.Special &&
                    !string.IsNullOrEmpty(bobblehead.SpecialPropertyName))
                {
                    TryIncreaseSpecialFromCollectible(bobblehead.SpecialPropertyName);
                }

                NotifySpecialButtons();
                OnPropertyChanged(null);
            });

            return true;
        }

        public bool TryCollectStackingMagazine(Fallout4MagazineSeries series)
        {
            if (!CanCollectStackingMagazine(series))
                return false;

            int next = GetMagazineStackCount(series.Name) + 1;
            RecordLoggedAction($"Read: {series.Name} ({next}/{series.MaxIssues})", () =>
            {
                MagazineStackCounts[series.Name] = next;
                OnPropertyChanged(null);
            });

            return true;
        }

        public bool TryCollectMagazineIssue(Fallout4MagazineSeries series, Fallout4MagazineIssue issue)
        {
            if (series == null || issue == null || CollectedMagazineIssueIds.Contains(issue.Id))
                return false;
            if (!series.Issues.Any(i => i.Id == issue.Id))
                return false;

            RecordLoggedAction($"Read: {series.Name} — {issue.DisplayName}", () =>
            {
                CollectedMagazineIssueIds.Add(issue.Id);
                RecalculateDerivedStats();
                OnPropertyChanged(null);
            });

            return true;
        }

        public bool TryCollectYoureSpecial(string specialChoice)
        {
            if (YoureSpecialCollected || string.IsNullOrEmpty(specialChoice))
                return false;

            RecordLoggedAction($"Read: You're SPECIAL! (+1 {specialChoice})", () =>
            {
                YoureSpecialCollected = true;
                YoureSpecialChoice = specialChoice;
                TryIncreaseSpecialFromCollectible(specialChoice);
                NotifySpecialButtons();
                OnPropertyChanged(null);
            });

            return true;
        }

        /// <summary>Bobblehead / You're SPECIAL! — can raise SPECIAL up to 11.</summary>
        public bool TryIncreaseSpecialFromCollectible(string statName)
        {
            var prop = GetType().GetProperty(statName);
            if (prop == null || prop.PropertyType != typeof(int))
                return false;

            int current = (int)prop.GetValue(this)!;
            if (current >= MaxSpecialAbsolute)
                return false;

            prop.SetValue(this, current + 1);
            RecalculateDerivedStats();
            OnPropertyChanged(statName);
            NotifySpecialButtons();
            return true;
        }

        public void ModifySpecialCreation(string statName, int amount)
        {
            var prop = GetType().GetProperty(statName);
            if (prop == null || prop.PropertyType != typeof(int))
                return;

            int current = (int)prop.GetValue(this)!;
            if (amount > 0)
            {
                if (Points <= 0 || current >= MaxSpecialFromPoints)
                    return;
                prop.SetValue(this, current + 1);
                Points--;
            }
            else if (amount < 0)
            {
                if (current <= 1)
                    return;
                prop.SetValue(this, current - 1);
                Points++;
            }

            RecalculateDerivedStats();
            OnPropertyChanged(statName);
            OnPropertyChanged(nameof(Points));
            OnPropertyChanged(nameof(CanConfirmCreation));
            NotifySpecialButtons();
        }

        public bool TryIncreaseSpecialWithPerkPoint(string statName)
        {
            if (PerkPoints <= 0)
                return false;

            var prop = GetType().GetProperty(statName);
            if (prop == null || prop.PropertyType != typeof(int))
                return false;

            int current = (int)prop.GetValue(this)!;
            if (current >= MaxSpecialFromPoints)
                return false;

            RecordLoggedAction($"SPECIAL: +1 {statName}", () =>
            {
                prop.SetValue(this, current + 1);
                PerkPoints--;
                RecalculateDerivedStats();
                OnPropertyChanged(statName);
                OnPropertyChanged(nameof(PerkPoints));
                NotifySpecialButtons();
                OnPropertyChanged(null);
            });

            return true;
        }

        public bool CanTakeNextPerkRank(Fallout4Perk perk)
        {
            if (perk == null || PerkPoints <= 0)
                return false;

            int owned = GetPerkRank(perk.Name);
            if (owned >= perk.MaxRank)
                return false;

            if (GetSpecial(perk.Attribute) < perk.AttributeRequirement)
                return false;

            var next = perk.GetRank(owned + 1);
            if (next == null)
                return false;

            if (next.LevelRequirement > 0 && Level < next.LevelRequirement)
                return false;

            return true;
        }

        public string GetPerkRequirementText(Fallout4Perk perk)
        {
            if (perk == null)
                return "";

            int owned = GetPerkRank(perk.Name);
            if (owned >= perk.MaxRank)
                return $"Max rank ({perk.MaxRank}) reached.";

            var next = perk.GetRank(owned + 1)!;
            string lvl = next.LevelRequirement > 0
                ? $"Level {next.LevelRequirement}"
                : "No level requirement";

            return $"{perk.AttributeShort} {perk.AttributeRequirement}, {lvl} (Rank {next.Rank})";
        }

        public string GetNextPerkRankDescription(Fallout4Perk perk)
        {
            if (perk == null)
                return "";

            int owned = GetPerkRank(perk.Name);
            if (owned >= perk.MaxRank)
                return perk.GetRank(owned)?.Description ?? "";

            return perk.GetRank(owned + 1)?.Description ?? "";
        }

        /// <summary>Descriptions for ranks 1..ownedRank so the owned-perks panel shows every rank taken.</summary>
        private static string FormatOwnedRankDescriptions(Fallout4Perk perk, int ownedRank)
        {
            if (perk == null || ownedRank <= 0)
                return "";

            if (ownedRank == 1 || perk.MaxRank <= 1)
                return perk.GetRank(1)?.Description ?? "";

            var parts = new List<string>();
            for (int r = 1; r <= ownedRank; r++)
            {
                var info = perk.GetRank(r);
                if (info == null || string.IsNullOrWhiteSpace(info.Description))
                    continue;
                parts.Add($"Rank {r}: {info.Description}");
            }

            return string.Join("\n\n", parts);
        }

        public bool TryTakeNextPerkRank(Fallout4Perk perk)
        {
            if (!CanTakeNextPerkRank(perk))
                return false;

            int nextRank = GetPerkRank(perk.Name) + 1;
            RecordLoggedAction($"Perk: {perk.Name} Rank {nextRank}", () =>
            {
                PerkRanks[perk.Name] = nextRank;
                PerkPoints--;
                RecalculateDerivedStats();
                OnPropertyChanged(nameof(PerkPoints));
                NotifySpecialButtons();
                OnPropertyChanged(null);
            });

            return true;
        }

        public void LevelUp()
        {
            if (!CanLevelUp)
                return;

            RecordLoggedAction($"Level Up: Level {Level + 1}", () =>
            {
                Level++;
                PerkPoints++;
                RecalculateDerivedStats();
                OnPropertyChanged(nameof(Level));
                OnPropertyChanged(nameof(PerkPoints));
                OnPropertyChanged(nameof(CanLevelUp));
                NotifySpecialButtons();
                OnPropertyChanged(null);
            });
        }

        public void SaveInitialState()
        {
            Points = 0;
            PerkPoints = 0;
            RecalculateDerivedStats();

            var snapshot = Current.Clone();
            _namedBaseline = snapshot.Clone();

            _history.Clear();
            PerkRanks.Clear();
            MiscPerkRanks.Clear();
            FoundBobbleheadNames.Clear();
            MagazineStackCounts.Clear();
            CollectedMagazineIssueIds.Clear();
            YoureSpecialCollected = false;
            YoureSpecialChoice = null;
            _history.Push(snapshot);

            ResetActionLog($"Created Character: {Name}");
            OnPropertyChanged(null);
        }

        public void RevertToCreationState()
        {
            var working = (_namedBaseline ?? Current).Clone();
            working.PerkPoints = 0;
            working.Level = 1;
            working.Points = 0;
            _history.Clear();
            PerkRanks.Clear();
            MiscPerkRanks.Clear();
            FoundBobbleheadNames.Clear();
            MagazineStackCounts.Clear();
            CollectedMagazineIssueIds.Clear();
            YoureSpecialCollected = false;
            YoureSpecialChoice = null;
            _history.Push(working);
            ActionLog.Clear();
            _actionStates.Clear();
            RecalculateDerivedStats();
            OnPropertyChanged(null);
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

        private void ResetActionLog(string createdEntry)
        {
            ActionLog.Clear();
            _actionStates.Clear();
            ActionLog.Add(createdEntry);
            _actionStates.Add(CaptureActionState());
        }

        public CharacterActionState CaptureActionState() => new()
        {
            Stats = Current.Clone(),
            PerkRanks = new Dictionary<string, int>(PerkRanks),
            MiscPerkRanks = new Dictionary<string, int>(MiscPerkRanks),
            FoundBobbleheads = new HashSet<string>(FoundBobbleheadNames),
            MagazineStackCounts = new Dictionary<string, int>(MagazineStackCounts),
            CollectedMagazineIssues = new HashSet<string>(CollectedMagazineIssueIds),
            YoureSpecialCollected = YoureSpecialCollected,
            YoureSpecialChoice = YoureSpecialChoice
        };

        public void RestoreActionState(CharacterActionState state)
        {
            PerkRanks.Clear();
            foreach (var kvp in state.PerkRanks)
                PerkRanks[kvp.Key] = kvp.Value;

            MiscPerkRanks.Clear();
            foreach (var kvp in state.MiscPerkRanks)
                MiscPerkRanks[kvp.Key] = kvp.Value;

            FoundBobbleheadNames.Clear();
            foreach (var name in state.FoundBobbleheads)
                FoundBobbleheadNames.Add(name);

            MagazineStackCounts.Clear();
            foreach (var kvp in state.MagazineStackCounts)
                MagazineStackCounts[kvp.Key] = kvp.Value;

            CollectedMagazineIssueIds.Clear();
            foreach (var id in state.CollectedMagazineIssues)
                CollectedMagazineIssueIds.Add(id);

            YoureSpecialCollected = state.YoureSpecialCollected;
            YoureSpecialChoice = state.YoureSpecialChoice;

            _history.Clear();
            _history.Push(state.Stats.Clone());
            RecalculateDerivedStats();
            OnPropertyChanged(null);
        }

        public FalloutPlanner.Saves.Adapters.Fallout4SavePayload CreateSavePayload()
        {
            var payload = new FalloutPlanner.Saves.Adapters.Fallout4SavePayload
            {
                Current = CaptureActionState(),
                Baseline = _namedBaseline == null
                    ? null
                    : new CharacterActionState { Stats = _namedBaseline.Clone() },
                ActionLogNewestFirst = ActionLog.ToList()
            };

            foreach (var state in _actionStates)
                payload.ActionStates.Add(CloneActionState(state));

            return payload;
        }

        public void LoadFromSavePayload(FalloutPlanner.Saves.Adapters.Fallout4SavePayload payload, string? saveId)
        {
            if (payload.Baseline?.Stats != null)
                _namedBaseline = payload.Baseline.Stats.Clone();
            else if (payload.Current?.Stats != null)
                _namedBaseline = payload.Current.Stats.Clone();

            ActionLog.Clear();
            _actionStates.Clear();

            if (payload.ActionStates.Count > 0 && payload.ActionLogNewestFirst.Count > 0)
            {
                for (int i = 0; i < payload.ActionLogNewestFirst.Count; i++)
                    ActionLog.Add(payload.ActionLogNewestFirst[i]);

                foreach (var state in payload.ActionStates)
                    _actionStates.Add(CloneActionState(state));

                RestoreActionState(_actionStates[0]);
            }
            else if (payload.Current != null)
            {
                RestoreActionState(CloneActionState(payload.Current));
                ResetActionLog($"Created Character: {Name}");
            }

            ActiveSaveId = saveId;
            MarkClean();
            OnPropertyChanged(null);
        }

        private static CharacterActionState CloneActionState(CharacterActionState state) => new()
        {
            Stats = state.Stats.Clone(),
            PerkRanks = new Dictionary<string, int>(state.PerkRanks),
            MiscPerkRanks = new Dictionary<string, int>(state.MiscPerkRanks),
            FoundBobbleheads = new HashSet<string>(state.FoundBobbleheads),
            MagazineStackCounts = new Dictionary<string, int>(state.MagazineStackCounts),
            CollectedMagazineIssues = new HashSet<string>(state.CollectedMagazineIssues),
            YoureSpecialCollected = state.YoureSpecialCollected,
            YoureSpecialChoice = state.YoureSpecialChoice
        };

        private void RecordLoggedAction(string description, Action apply)
        {
            if (_actionStates.Count > 0)
                _actionStates[0] = CaptureActionState();

            apply();

            ActionLog.Insert(0, description);
            _actionStates.Insert(0, CaptureActionState());
            IsDirty = true;
        }

        public void RecalculateDerivedStats()
        {
            var stats = Current;

            // Approximate FO4 base formulas
            int hitPoints = 80 + (stats.Endurance * 5) + ((stats.Level - 1) * (2 + stats.Endurance / 2));
            int actionPoints = 60 + (stats.Agility * 10);
            int carryWeight = 200 + (stats.Strength * 10);

            // Unconditional perk bonuses that affect tracked pools
            // Strong Back: rank 1 = +25 CW, rank 2+ = +50 CW (replaces, not stacks)
            int strongBack = GetPerkRank("Strong Back");
            if (strongBack >= 2)
                carryWeight += 50;
            else if (strongBack >= 1)
                carryWeight += 25;

            // Lifegiver: +20 max HP per rank (ranks 1-3)
            int lifegiver = GetPerkRank("Lifegiver");
            if (lifegiver > 0)
                hitPoints += Math.Min(lifegiver, 3) * 20;

            // Quick Hands rank 3: +10 Action Points
            if (GetPerkRank("Quick Hands") >= 3)
                actionPoints += 10;

            // Magazine: Astoundingly Awesome Tales — The Man Who Could Stop Time (+5 AP)
            if (CollectedMagazineIssueIds.Contains("aat-8"))
                actionPoints += 5;

            // Situational bonuses (companion / time-of-day / sprinting / regen) are ignored

            stats.HitPoints = hitPoints;
            stats.ActionPoints = actionPoints;
            stats.CarryWeight = carryWeight;

            OnPropertyChanged(nameof(HitPoints));
            OnPropertyChanged(nameof(ActionPoints));
            OnPropertyChanged(nameof(CarryWeight));
        }

        private void NotifySpecialButtons()
        {
            OnPropertyChanged(nameof(CanIncreaseStrengthCreation));
            OnPropertyChanged(nameof(CanDecreaseStrengthCreation));
            OnPropertyChanged(nameof(CanIncreasePerceptionCreation));
            OnPropertyChanged(nameof(CanDecreasePerceptionCreation));
            OnPropertyChanged(nameof(CanIncreaseEnduranceCreation));
            OnPropertyChanged(nameof(CanDecreaseEnduranceCreation));
            OnPropertyChanged(nameof(CanIncreaseCharismaCreation));
            OnPropertyChanged(nameof(CanDecreaseCharismaCreation));
            OnPropertyChanged(nameof(CanIncreaseIntelligenceCreation));
            OnPropertyChanged(nameof(CanDecreaseIntelligenceCreation));
            OnPropertyChanged(nameof(CanIncreaseAgilityCreation));
            OnPropertyChanged(nameof(CanDecreaseAgilityCreation));
            OnPropertyChanged(nameof(CanIncreaseLuckCreation));
            OnPropertyChanged(nameof(CanDecreaseLuckCreation));

            OnPropertyChanged(nameof(CanIncreaseStrengthPerk));
            OnPropertyChanged(nameof(CanIncreasePerceptionPerk));
            OnPropertyChanged(nameof(CanIncreaseEndurancePerk));
            OnPropertyChanged(nameof(CanIncreaseCharismaPerk));
            OnPropertyChanged(nameof(CanIncreaseIntelligencePerk));
            OnPropertyChanged(nameof(CanIncreaseAgilityPerk));
            OnPropertyChanged(nameof(CanIncreaseLuckPerk));
            OnPropertyChanged(nameof(CanTakeSelectedPerkHint));
        }
    }
}
