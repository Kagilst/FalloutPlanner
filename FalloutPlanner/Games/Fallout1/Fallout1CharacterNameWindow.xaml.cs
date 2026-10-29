using System;
using System.Windows;

namespace FalloutPlanner.Games.Fallout1
{
    public partial class Fallout1CharacterNameWindow : Window
    {
        public string CharacterName { get; private set; } = "";
        public int CharacterAge { get; private set; } = 25;
        public bool IsMale { get; private set; } = true;

        public Fallout1CharacterNameWindow()
        {
            InitializeComponent();
            NameInput.Focus();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            string name = NameInput.Text?.Trim() ?? "";

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a character name.", "Name Required",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(AgeInput.Text, out int age))
                age = 25;
            age = Math.Clamp(age, 16, 35);

            CharacterName = name;
            CharacterAge = age;
            IsMale = MaleRadio.IsChecked == true;

            DialogResult = true;
            Close();
        }
    }
}
