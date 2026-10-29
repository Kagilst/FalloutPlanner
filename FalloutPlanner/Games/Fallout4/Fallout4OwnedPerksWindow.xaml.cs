using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using FalloutPlanner.Games.Fallout4.Perks;

namespace FalloutPlanner.Games.Fallout4
{
    public partial class Fallout4OwnedPerksWindow : Window, INotifyPropertyChanged
    {
        private readonly Fallout4Character _character;
        private OwnedPerkInfo? _selectedOwned;
        private OwnedPerkInfo? _selectedMisc;
        private bool _selecting;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<OwnedPerkInfo> OwnedPerks { get; } = new();
        public ObservableCollection<OwnedPerkInfo> MiscPerks { get; } = new();

        public bool ShowOwnedEmpty => OwnedPerks.Count == 0;
        public bool ShowMiscEmpty => MiscPerks.Count == 0;

        public OwnedPerkInfo? SelectedOwnedPerk
        {
            get => _selectedOwned;
            set { if (_selectedOwned != value) { _selectedOwned = value; OnPropertyChanged(); } }
        }

        public OwnedPerkInfo? SelectedMiscPerk
        {
            get => _selectedMisc;
            set
            {
                if (_selectedMisc == value) return;
                _selectedMisc = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanTakeSelectedMisc));
                OnPropertyChanged(nameof(ExclusionNote));
                OnPropertyChanged(nameof(ShowExclusionNote));
            }
        }

        public string DetailTitle { get; private set; } = "";
        public string SelectedRequirements { get; private set; } = "";
        public string SelectedRanks { get; private set; } = "";
        public string SelectedDescription { get; private set; } = "Select a perk to view details.";
        public bool ShowRequirements { get; private set; }
        public bool ShowRanks { get; private set; }

        public bool CanTakeSelectedMisc =>
            SelectedMiscPerk?.MiscPerk != null &&
            _character.CanTakeMiscPerk(SelectedMiscPerk.MiscPerk);

        public string ExclusionNote
        {
            get
            {
                var misc = SelectedMiscPerk?.MiscPerk ?? SelectedOwnedPerk?.MiscPerk;
                if (misc?.ExclusionGroup == null)
                    return "";

                return misc.ExclusionGroup switch
                {
                    Fallout4MiscPerkRepository.AtomExclusiveGroup =>
                        "Exclusive: only one of Crusader of Atom, Inquisitor of Atom, or Far Harbor Survivalist.",
                    Fallout4MiscPerkRepository.AcadiaExclusiveGroup =>
                        "Exclusive: only one of Destroyer of Acadia or Protector of Acadia.",
                    Fallout4MiscPerkRepository.NukaRaiderGroup =>
                        "Limit: choose at most 2 of Ace Operator, Chosen Disciple, and Pack Alpha.",
                    _ => ""
                };
            }
        }

        public bool ShowExclusionNote => !string.IsNullOrEmpty(ExclusionNote);

        public Fallout4OwnedPerksWindow(Fallout4Character character)
        {
            InitializeComponent();
            _character = character;
            DataContext = this;
            Refresh();
        }

        private void Refresh(string? preferOwned = null, string? preferMisc = null)
        {
            OwnedPerks.Clear();
            foreach (var perk in _character.GetOwnedPerks())
                OwnedPerks.Add(perk);

            MiscPerks.Clear();
            foreach (var perk in _character.GetAvailableMiscPerks())
                MiscPerks.Add(perk);

            OnPropertyChanged(nameof(ShowOwnedEmpty));
            OnPropertyChanged(nameof(ShowMiscEmpty));

            OwnedPerkInfo? ownedSel = null;
            OwnedPerkInfo? miscSel = null;
            if (!string.IsNullOrEmpty(preferOwned))
                ownedSel = OwnedPerks.FirstOrDefault(p => p.Name == preferOwned);
            if (!string.IsNullOrEmpty(preferMisc))
                miscSel = MiscPerks.FirstOrDefault(p => p.Name == preferMisc);

            if (ownedSel != null)
            {
                SelectedOwnedPerk = ownedSel;
                SelectedMiscPerk = null;
                ShowPerkDetails(ownedSel);
            }
            else if (miscSel != null)
            {
                SelectedOwnedPerk = null;
                SelectedMiscPerk = miscSel;
                ShowPerkDetails(miscSel);
            }
            else if (OwnedPerks.Count > 0)
            {
                SelectedOwnedPerk = OwnedPerks[0];
                SelectedMiscPerk = null;
                ShowPerkDetails(OwnedPerks[0]);
            }
            else if (MiscPerks.Count > 0)
            {
                SelectedOwnedPerk = null;
                SelectedMiscPerk = MiscPerks[0];
                ShowPerkDetails(MiscPerks[0]);
            }
            else
            {
                SelectedOwnedPerk = null;
                SelectedMiscPerk = null;
                ClearDetails();
            }

            OnPropertyChanged(nameof(CanTakeSelectedMisc));
            OnPropertyChanged(nameof(ExclusionNote));
            OnPropertyChanged(nameof(ShowExclusionNote));
        }

        private void OwnedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selecting || SelectedOwnedPerk == null) return;
            _selecting = true;
            SelectedMiscPerk = null;
            ShowPerkDetails(SelectedOwnedPerk);
            _selecting = false;
        }

        private void MiscList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selecting || SelectedMiscPerk == null) return;
            _selecting = true;
            SelectedOwnedPerk = null;
            ShowPerkDetails(SelectedMiscPerk);
            _selecting = false;
        }

        private void ShowPerkDetails(OwnedPerkInfo perk)
        {
            DetailTitle = perk.DisplayName;
            SelectedRequirements = perk.Requirements;
            SelectedRanks = perk.MaxRank > 1 ? $"{perk.Rank}/{perk.MaxRank}" : "";
            SelectedDescription = string.IsNullOrWhiteSpace(perk.Description)
                ? "No description available."
                : perk.Description;
            ShowRequirements = !string.IsNullOrWhiteSpace(perk.Requirements);
            ShowRanks = perk.MaxRank > 1;
            NotifyDetails();
            OnPropertyChanged(nameof(CanTakeSelectedMisc));
            OnPropertyChanged(nameof(ExclusionNote));
            OnPropertyChanged(nameof(ShowExclusionNote));
        }

        private void ClearDetails()
        {
            DetailTitle = "";
            SelectedRequirements = "";
            SelectedRanks = "";
            SelectedDescription = "Select a perk to view details.";
            ShowRequirements = false;
            ShowRanks = false;
            NotifyDetails();
        }

        private void NotifyDetails()
        {
            OnPropertyChanged(nameof(DetailTitle));
            OnPropertyChanged(nameof(SelectedRequirements));
            OnPropertyChanged(nameof(SelectedRanks));
            OnPropertyChanged(nameof(SelectedDescription));
            OnPropertyChanged(nameof(ShowRequirements));
            OnPropertyChanged(nameof(ShowRanks));
        }

        private void TakeMiscPerk_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedMiscPerk?.MiscPerk == null)
                return;

            var name = SelectedMiscPerk.MiscPerk.Name;
            if (_character.TryTakeMiscPerk(SelectedMiscPerk.MiscPerk))
                Refresh(preferOwned: name);
        }

        private void Back_Click(object sender, RoutedEventArgs e) => Close();

        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
