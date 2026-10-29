using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using FalloutPlanner.Games.Fallout2.Traits;

namespace FalloutPlanner.Games.Fallout2
{
    public partial class Fallout2TraitsWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly Fallout2Character _character;

        public List<TraitChoice> TraitChoices { get; }

        private Fallout2Trait _selectedTrait;
        public Fallout2Trait SelectedTrait
        {
            get => _selectedTrait;
            set { _selectedTrait = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasSelection)); OnPropertyChanged(nameof(NoSelection)); }
        }

        public bool HasSelection => SelectedTrait != null;
        public bool NoSelection => SelectedTrait == null;

        public string SelectedCountText => $"Selected: {TraitChoices.Count(c => c.IsSelected)}/2";

        public Fallout2TraitsWindow(Fallout2Character character)
        {
            _character = character;
            TraitChoices = Fallout2TraitRepository.All
                .Select(t => new TraitChoice { Trait = t })
                .ToList();

            InitializeComponent();
            DataContext = this;
        }

        private void Trait_CheckChanged(object sender, RoutedEventArgs e)
        {
            int selectedCount = TraitChoices.Count(c => c.IsSelected);

            foreach (var choice in TraitChoices)
            {
                choice.CanSelect = choice.IsSelected || selectedCount < 2;
            }

            // Update preview to show last checked/interacted trait
            if (sender is System.Windows.Controls.CheckBox cb && cb.DataContext is TraitChoice tc)
            {
                SelectedTrait = tc.Trait;
            }

            OnPropertyChanged(nameof(SelectedCountText));
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            // Apply selected traits
            foreach (var choice in TraitChoices.Where(c => c.IsSelected))
            {
                _character.Traits.Add(choice.Trait.Name);

                if (choice.Trait.HasPermanentEffect)
                    choice.Trait.ApplyEffect?.Invoke(_character);
            }

            _character.RecalculateDerivedStats();

            DialogResult = true;
            Close();
        }
    }

    public class TraitChoice : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public Fallout2Trait Trait { get; init; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        private bool _canSelect = true;
        public bool CanSelect
        {
            get => _canSelect;
            set { _canSelect = value; OnPropertyChanged(); }
        }
    }
}
