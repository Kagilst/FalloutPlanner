using FalloutPlanner.Games.Fallout3.Perks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace FalloutPlanner.Games.Fallout3
{
    public partial class Fallout3LevelUpWindow : Window, INotifyPropertyChanged
    {
        private readonly Fallout3Character _character;
        private PerkChoice _selectedChoice;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<PerkChoice> PerkChoices { get; }

        public PerkChoice SelectedChoice
        {
            get => _selectedChoice;
            set
            {
                if (_selectedChoice == value)
                    return;
                _selectedChoice = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedRequirementsText));
                OnPropertyChanged(nameof(SelectedDescription));
                OnPropertyChanged(nameof(SelectedRanksText));
                OnPropertyChanged(nameof(ShowRanks));
                OnPropertyChanged(nameof(CanConfirmSelected));
                OnPropertyChanged(nameof(AvailabilityNote));
            }
        }

        public string SelectedRequirementsText =>
            SelectedChoice?.Perk.GetRequirementsDisplayText() ?? "";

        public string SelectedDescription =>
            SelectedChoice?.Perk.Description ?? "";

        public string SelectedRanksText =>
            SelectedChoice == null
                ? ""
                : $"{SelectedChoice.CurrentRank} / {SelectedChoice.Perk.MaxRank}";

        public bool ShowRanks =>
            SelectedChoice?.Perk.MaxRank > 1;

        public bool CanConfirmSelected =>
            SelectedChoice != null && SelectedChoice.IsAvailable;

        public string AvailabilityNote
        {
            get
            {
                if (SelectedChoice == null)
                    return "";
                return SelectedChoice.IsAvailable
                    ? "You meet the requirements for this perk."
                    : "You do not meet the requirements for this perk (still viewable, but cannot confirm).";
            }
        }

        public Fallout3LevelUpWindow(Fallout3Character character)
        {
            InitializeComponent();

            _character = character;
            PerkChoices = Fallout3PerkRepository.AllPerks
                .Select(p =>
                {
                    int rank = character.GetPerkRank(p.Name);
                    return new { Perk = p, Rank = rank };
                })
                // Hide perks that are already at max rank
                .Where(x => x.Rank < x.Perk.MaxRank)
                .Select(x => new PerkChoice(
                    x.Perk,
                    character.CanTakePerkOnLevelUp(x.Perk),
                    x.Rank))
                .ToList();

            DataContext = this;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedChoice == null || !SelectedChoice.IsAvailable)
                return;

            if (!_character.CanTakePerkOnLevelUp(SelectedChoice.Perk))
            {
                MessageBox.Show("You do not meet the requirements.");
                return;
            }

            string intenseSpecial = null;
            string tagSkill = null;

            if (SelectedChoice.Perk.Name == "Intense Training")
            {
                var picker = new IntenseTrainingWindow(_character) { Owner = this };
                if (picker.ShowDialog() != true)
                    return;
                intenseSpecial = picker.SelectedSpecial;
            }
            else if (SelectedChoice.Perk.Name == "Tag!")
            {
                var picker = new TagSkillWindow(_character) { Owner = this };
                if (picker.ShowDialog() != true)
                    return;
                tagSkill = picker.SelectedSkill;
            }

            _character.LevelUp(SelectedChoice.Perk, intenseSpecial, tagSkill);

            DialogResult = true;
            Close();
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class PerkChoice
    {
        private static readonly Brush AvailableBrush =
            (Brush)Application.Current.FindResource("FalloutAccentBrush");
        private static readonly Brush UnavailableBrush =
            new SolidColorBrush(Color.FromRgb(0x33, 0x55, 0x00));

        public Fallout3Perk Perk { get; }
        public bool IsAvailable { get; }
        public int CurrentRank { get; }
        public double ListOpacity => IsAvailable ? 1.0 : 0.45;
        public Brush ListForeground => IsAvailable ? AvailableBrush : UnavailableBrush;

        // Only show next rank if the player already owns at least one rank
        public string DisplayName =>
            CurrentRank > 0
                ? $"{Perk.Name} (Rank: {CurrentRank + 1})"
                : Perk.Name;

        public PerkChoice(Fallout3Perk perk, bool isAvailable, int currentRank)
        {
            Perk = perk;
            IsAvailable = isAvailable;
            CurrentRank = currentRank;
            UnavailableBrush.Freeze();
        }
    }
}
