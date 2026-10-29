using FalloutPlanner.Games.FalloutNV.Perks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace FalloutPlanner.Games.FalloutNV
{
    public partial class FalloutNVLevelUpWindow : Window, INotifyPropertyChanged
    {
        private readonly FalloutNVCharacter _character;
        private PerkChoice _selectedChoice;
        private readonly bool _requiresPerk;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<PerkChoice> PerkChoices { get; }

        public bool RequiresPerk => _requiresPerk;
        public string WindowTitle => _requiresPerk 
            ? $"Level {_character.Level + 1} - Choose a Perk"
            : $"Level {_character.Level + 1} - Level Up";

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

        public bool CanConfirmSelected
        {
            get
            {
                if (!_requiresPerk)
                    return true; // Odd levels don't require perk selection
                return SelectedChoice != null && SelectedChoice.IsAvailable;
            }
        }

        public string AvailabilityNote
        {
            get
            {
                if (!_requiresPerk)
                    return "This is an odd level - no perk required. Click Confirm to level up.";
                if (SelectedChoice == null)
                    return "";
                return SelectedChoice.IsAvailable
                    ? "You meet the requirements for this perk."
                    : "You do not meet the requirements for this perk (still viewable, but cannot confirm).";
            }
        }

        public FalloutNVLevelUpWindow(FalloutNVCharacter character)
        {
            InitializeComponent();

            _character = character;
            _requiresPerk = character.NextLevelRequiresPerk;
            Title = WindowTitle;

            if (_requiresPerk)
            {
                PerkChoices = FalloutNVPerkRepository.AllPerks
                    .Select(p =>
                    {
                        int rank = character.GetPerkRank(p.Name);
                        return new { Perk = p, Rank = rank };
                    })
                    .Where(x => x.Rank < x.Perk.MaxRank)
                    .Select(x => new PerkChoice(
                        x.Perk,
                        character.CanTakePerkOnLevelUp(x.Perk),
                        x.Rank))
                    .ToList();
            }
            else
            {
                PerkChoices = new List<PerkChoice>();
            }

            DataContext = this;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (_requiresPerk)
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
            }
            else
            {
                // Odd level - no perk required
                _character.LevelUp();
            }

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
            new SolidColorBrush(Color.FromRgb(0x66, 0x44, 0x00));

        public FalloutNVPerk Perk { get; }
        public bool IsAvailable { get; }
        public int CurrentRank { get; }
        public double ListOpacity => IsAvailable ? 1.0 : 0.45;
        public Brush ListForeground => IsAvailable ? AvailableBrush : UnavailableBrush;

        public string DisplayName =>
            CurrentRank > 0
                ? $"{Perk.Name} (Rank: {CurrentRank + 1})"
                : Perk.Name;

        public PerkChoice(FalloutNVPerk perk, bool isAvailable, int currentRank)
        {
            Perk = perk;
            IsAvailable = isAvailable;
            CurrentRank = currentRank;
            UnavailableBrush.Freeze();
        }
    }
}
