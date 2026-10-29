using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using FalloutPlanner.Games.Fallout2.Perks;

namespace FalloutPlanner.Games.Fallout2
{
    public partial class Fallout2LevelUpWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly Fallout2Character _character;

        public List<PerkChoice> PerkChoices { get; }

        private PerkChoice _selectedChoice;
        public PerkChoice SelectedChoice
        {
            get => _selectedChoice;
            set
            {
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

        public string SelectedRequirementsText => SelectedChoice?.Perk?.GetRequirementsDisplayText() ?? "";
        public string SelectedDescription => SelectedChoice?.Perk?.Description ?? "";
        public string SelectedRanksText => SelectedChoice?.Perk != null
            ? $"{_character.GetPerkRank(SelectedChoice.Perk.Name)}/{SelectedChoice.Perk.MaxRank}"
            : "";
        public bool ShowRanks => SelectedChoice?.Perk?.MaxRank > 1;
        public bool CanConfirmSelected => SelectedChoice?.CanTake == true;

        public string AvailabilityNote
        {
            get
            {
                if (SelectedChoice == null) return "";
                if (SelectedChoice.CanTake) return "You can take this perk.";
                return "You do not meet the requirements for this perk.";
            }
        }

        public Fallout2LevelUpWindow(Fallout2Character character)
        {
            _character = character;

            PerkChoices = Fallout2PerkRepository.AllPerks
                .Select(p => new PerkChoice
                {
                    Perk = p,
                    CanTake = character.CanTakePerkOnLevelUp(p),
                    DisplayName = p.MaxRank > 1
                        ? $"{p.Name} ({character.GetPerkRank(p.Name)}/{p.MaxRank})"
                        : p.Name
                })
                .OrderByDescending(c => c.CanTake)
                .ThenBy(c => c.Perk.Name)
                .ToList();

            InitializeComponent();
            DataContext = this;

            if (PerkChoices.Count > 0)
                SelectedChoice = PerkChoices.FirstOrDefault(c => c.CanTake) ?? PerkChoices[0];
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedChoice?.CanTake != true)
                return;

            string tagSkill = null;

            // Handle Tag! perk
            if (SelectedChoice.Perk.Name == "Tag!")
            {
                // For simplicity, skip tag skill selection for now
                // In a full implementation, you'd show a dialog to pick the skill
            }

            _character.LevelUp(SelectedChoice.Perk, tagSkill);
            DialogResult = true;
            Close();
        }
    }

    public class PerkChoice : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Fallout2Perk Perk { get; init; }
        public bool CanTake { get; init; }
        public string DisplayName { get; init; }

        public double ListOpacity => CanTake ? 1.0 : 0.5;
        public Brush ListForeground => CanTake
            ? new SolidColorBrush(Color.FromRgb(0x33, 0xFF, 0x33))
            : new SolidColorBrush(Color.FromRgb(0x66, 0x99, 0x66));
    }
}
