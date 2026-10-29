using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace FalloutPlanner.Games.FalloutNV
{
    public partial class FalloutNVCharacterNameWindow : Window
    {
        private static readonly Regex LettersOnly = new("^[a-zA-Z]+$");

        public FalloutNVCharacterNameWindow()
        {
            InitializeComponent();
        }

        public string CharacterName => NameTextBox.Text;

        private void NameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !LettersOnly.IsMatch(e.Text);
        }

        private void NameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ConfirmButton.IsEnabled = NameTextBox.Text.Length >= 1;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
