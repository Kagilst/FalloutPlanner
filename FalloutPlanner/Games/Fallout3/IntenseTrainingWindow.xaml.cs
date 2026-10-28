using System.Windows;
using System.Windows.Controls;

namespace FalloutPlanner.Games.Fallout3
{
    public partial class IntenseTrainingWindow : Window
    {
        private readonly Fallout3Character _character;

        public string SelectedSpecial { get; private set; }

        public IntenseTrainingWindow(Fallout3Character character)
        {
            InitializeComponent();
            _character = character;

            ConfigureButton(StrengthButton, _character.Strength);
            ConfigureButton(PerceptionButton, _character.Perception);
            ConfigureButton(EnduranceButton, _character.Endurance);
            ConfigureButton(CharismaButton, _character.Charisma);
            ConfigureButton(IntelligenceButton, _character.Intelligence);
            ConfigureButton(AgilityButton, _character.Agility);
            ConfigureButton(LuckButton, _character.Luck);
        }

        private static void ConfigureButton(Button button, int value)
        {
            button.Content = $"{button.Tag} ({value})";
            button.IsEnabled = value < 10;
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
