using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace FalloutPlanner
{
    public partial class CharacterNameWindow : Window
    {
        private static readonly Regex _lettersOnly = new Regex("^[a-zA-Z]+$");

        public CharacterNameWindow()
        {
            InitializeComponent();
        }
        public string CharacterName => NameTextBox.Text;

        // Prevent typing anything except letters
        private void NameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_lettersOnly.IsMatch(e.Text);
        }

        // Enable confirm only when at least 1 character exists
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

