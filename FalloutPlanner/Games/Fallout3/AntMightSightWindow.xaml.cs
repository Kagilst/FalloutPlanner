using System.Windows;
using System.Windows.Controls;

namespace FalloutPlanner.Games.Fallout3
{
    public partial class AntMightSightWindow : Window
    {
        private readonly Fallout3Character _character;

        public string SelectedSpecial { get; private set; }

        public AntMightSightWindow(Fallout3Character character)
        {
            InitializeComponent();
            _character = character;

            StrengthButton.Content = $"Strength ({_character.Strength})";
            PerceptionButton.Content = $"Perception ({_character.Perception})";
            StrengthButton.IsEnabled = _character.Strength < 10;
            PerceptionButton.IsEnabled = _character.Perception < 10;
        }

        private void Special_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.Tag is not string special)
                return;

            SelectedSpecial = special;
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
