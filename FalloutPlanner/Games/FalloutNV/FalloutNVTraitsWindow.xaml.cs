using FalloutPlanner.Games.FalloutNV.Traits;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace FalloutPlanner.Games.FalloutNV
{
    public partial class FalloutNVTraitsWindow : Window, INotifyPropertyChanged
    {
        private readonly FalloutNVCharacter _character;
        private FalloutNVTrait _selectedTrait;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TraitChoice> TraitChoices { get; } = new();

        public FalloutNVTrait SelectedTrait
        {
            get => _selectedTrait;
            set
            {
                if (_selectedTrait != value)
                {
                    _selectedTrait = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(HasSelection));
                    OnPropertyChanged(nameof(NoSelection));
                }
            }
        }

        public bool HasSelection => SelectedTrait != null;
        public bool NoSelection => SelectedTrait == null;

        public int SelectedCount => TraitChoices.Count(c => c.IsSelected);
        public string SelectedCountText => $"Selected: {SelectedCount} / 2";

        public bool IsMale
        {
            get => _character.IsMale;
            set
            {
                if (_character.IsMale != value)
                {
                    _character.IsMale = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsFemale));
                }
            }
        }

        public bool IsFemale
        {
            get => !_character.IsMale;
            set
            {
                if (_character.IsMale == value)
                {
                    _character.IsMale = !value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsMale));
                }
            }
        }

        public FalloutNVTraitsWindow(FalloutNVCharacter character)
        {
            InitializeComponent();
            _character = character;
            DataContext = this;

            foreach (var trait in FalloutNVTraitRepository.All)
            {
                TraitChoices.Add(new TraitChoice(trait, this));
            }
        }

        private void Trait_CheckChanged(object sender, RoutedEventArgs e)
        {
            // Update all trait CanSelect states
            foreach (var choice in TraitChoices)
            {
                choice.RefreshCanSelect();
            }
            OnPropertyChanged(nameof(SelectedCount));
            OnPropertyChanged(nameof(SelectedCountText));

            // Set selected trait to the one that was just clicked
            if (sender is System.Windows.Controls.CheckBox cb && cb.DataContext is TraitChoice tc)
            {
                SelectedTrait = tc.Trait;
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            // Apply selected traits to character
            _character.Traits.Clear();
            foreach (var choice in TraitChoices.Where(c => c.IsSelected))
            {
                _character.Traits.Add(choice.Trait.Name);

                // Apply permanent effects
                if (choice.Trait.HasPermanentEffect)
                {
                    choice.Trait.ApplyEffect?.Invoke(_character);
                }
            }

            // Recalculate stats with traits applied
            _character.RecalculateDerivedStats();

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class TraitChoice : INotifyPropertyChanged
    {
        private readonly FalloutNVTraitsWindow _window;
        private bool _isSelected;

        public FalloutNVTrait Trait { get; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanSelect => IsSelected || _window.SelectedCount < 2;

        public TraitChoice(FalloutNVTrait trait, FalloutNVTraitsWindow window)
        {
            Trait = trait;
            _window = window;
        }

        public void RefreshCanSelect()
        {
            OnPropertyChanged(nameof(CanSelect));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
