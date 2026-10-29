using FalloutPlanner.Games.FalloutNV.Perks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace FalloutPlanner.Games.FalloutNV
{
    public partial class FalloutNVOwnedPerksWindow : Window, INotifyPropertyChanged
    {
        private readonly FalloutNVCharacter _character;
        private OwnedPerkInfo _selectedOwnedPerk;
        private FalloutNVPerk _selectedMiscPerk;
        private bool _selecting;
        private bool _showMiscOnly;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<OwnedPerkInfo> OwnedPerks { get; } = new();
        public ObservableCollection<FalloutNVPerk> AvailableMiscPerks { get; } = new();

        public bool ShowOwnedEmpty => OwnedPerks.Count == 0;
        public bool ShowMiscEmpty => AvailableMiscPerks.Count == 0;

        public bool ShowMiscOnly
        {
            get => _showMiscOnly;
            set
            {
                if (_showMiscOnly == value)
                    return;
                _showMiscOnly = value;
                OnPropertyChanged();
                RefreshOwned();
            }
        }

        public OwnedPerkInfo SelectedOwnedPerk
        {
            get => _selectedOwnedPerk;
            set
            {
                if (_selectedOwnedPerk == value)
                    return;
                _selectedOwnedPerk = value;
                OnPropertyChanged();
                NotifyDetailProperties();
            }
        }

        public FalloutNVPerk SelectedMiscPerk
        {
            get => _selectedMiscPerk;
            set
            {
                if (_selectedMiscPerk == value)
                    return;
                _selectedMiscPerk = value;
                OnPropertyChanged();
                NotifyDetailProperties();
            }
        }

        public bool ShowOwnedDetails => SelectedOwnedPerk != null && SelectedMiscPerk == null;

        public string DetailTitle =>
            SelectedMiscPerk?.Name
            ?? SelectedOwnedPerk?.DisplayName
            ?? "";

        public string SelectedRequirements =>
            SelectedOwnedPerk?.Perk?.GetRequirementsDisplayText() ?? "";

        public string SelectedDescription
        {
            get
            {
                if (SelectedMiscPerk != null)
                    return SelectedMiscPerk.Description ?? "";
                if (SelectedOwnedPerk == null)
                    return "";
                return !string.IsNullOrWhiteSpace(SelectedOwnedPerk.DescriptionOverride)
                    ? SelectedOwnedPerk.DescriptionOverride
                    : SelectedOwnedPerk.Perk?.Description ?? "";
            }
        }

        public string SelectedRanks =>
            SelectedOwnedPerk == null || SelectedOwnedPerk.Perk == null
                ? ""
                : $"{SelectedOwnedPerk.CurrentRank} / {SelectedOwnedPerk.Perk.MaxRank}";

        public bool ShowRanks =>
            ShowOwnedDetails && !SelectedOwnedPerk.IsMiscPerk && (SelectedOwnedPerk?.Perk?.MaxRank ?? 1) > 1;

        public bool CanTakeSelectedMisc =>
            SelectedMiscPerk != null &&
            !_character.HasQuestPerk(SelectedMiscPerk.Name) &&
            !_character.IsMiscPerkBlocked(SelectedMiscPerk.Name);

        public string BlockedByText
        {
            get
            {
                if (SelectedMiscPerk == null)
                    return "";
                var blocker = _character.GetBlockingPerk(SelectedMiscPerk.Name);
                return string.IsNullOrEmpty(blocker) ? "" : $"Blocked by: {blocker}";
            }
        }

        public bool ShowBlockedBy => !string.IsNullOrEmpty(BlockedByText);

        public FalloutNVOwnedPerksWindow(FalloutNVCharacter character)
        {
            InitializeComponent();
            _character = character;
            DataContext = this;
            RefreshLists();

            if (OwnedPerks.Count > 0)
                SelectedOwnedPerk = OwnedPerks[0];
            else if (AvailableMiscPerks.Count > 0)
                SelectedMiscPerk = AvailableMiscPerks[0];
        }

        private void RefreshLists(string preferSelect = null)
        {
            RefreshOwned(preferSelect);
            RefreshMisc();
        }

        private void RefreshOwned(string preferSelect = null)
        {
            OwnedPerks.Clear();
            foreach (var info in _character.OwnedPerks)
            {
                if (_showMiscOnly && !info.IsMiscPerk)
                    continue;
                OwnedPerks.Add(info);
            }

            OnPropertyChanged(nameof(ShowOwnedEmpty));
            NotifyDetailProperties();

            if (!string.IsNullOrEmpty(preferSelect))
            {
                var match = OwnedPerks.FirstOrDefault(p =>
                    p.DisplayName == preferSelect || p.Perk?.Name == preferSelect);
                if (match != null)
                {
                    SelectedMiscPerk = null;
                    SelectedOwnedPerk = match;
                }
            }
        }

        private void RefreshMisc()
        {
            AvailableMiscPerks.Clear();
            foreach (var perk in FalloutNVQuestPerkRepository.All)
            {
                if (_character.HasQuestPerk(perk.Name))
                    continue;
                AvailableMiscPerks.Add(perk);
            }

            OnPropertyChanged(nameof(ShowMiscEmpty));
            NotifyDetailProperties();
        }

        private void OwnedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selecting || SelectedOwnedPerk == null)
                return;

            _selecting = true;
            SelectedMiscPerk = null;
            NotifyDetailProperties();
            _selecting = false;
        }

        private void MiscList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selecting || SelectedMiscPerk == null)
                return;

            _selecting = true;
            SelectedOwnedPerk = null;
            NotifyDetailProperties();
            _selecting = false;
        }

        private void TakeMiscPerk_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedMiscPerk == null || _character.HasQuestPerk(SelectedMiscPerk.Name))
                return;

            var perk = SelectedMiscPerk;

            // FNV quest perks are simpler - just take them directly
            _character.TryTakeQuestPerk(perk);
            RefreshLists(perk.Name);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NotifyDetailProperties()
        {
            OnPropertyChanged(nameof(DetailTitle));
            OnPropertyChanged(nameof(SelectedRequirements));
            OnPropertyChanged(nameof(SelectedDescription));
            OnPropertyChanged(nameof(SelectedRanks));
            OnPropertyChanged(nameof(ShowRanks));
            OnPropertyChanged(nameof(ShowOwnedDetails));
            OnPropertyChanged(nameof(CanTakeSelectedMisc));
            OnPropertyChanged(nameof(BlockedByText));
            OnPropertyChanged(nameof(ShowBlockedBy));
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
