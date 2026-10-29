using System.Windows;
using System.Windows.Controls;

namespace FalloutPlanner.Games.Fallout4
{
    public partial class Fallout4CharacterNameWindow : Window
    {
        public string CharacterName { get; private set; } = "";

        public Fallout4CharacterNameWindow()
        {
            InitializeComponent();
            NameTextBox.Focus();
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConfirmButton.IsEnabled = !string.IsNullOrWhiteSpace(NameTextBox.Text);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            CharacterName = NameTextBox.Text.Trim();
            DialogResult = true;
            Close();
        }
    }
}
