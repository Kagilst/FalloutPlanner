using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FalloutPlanner.Games.Fallout4
{
    public partial class Fallout4SpecialChoiceWindow : Window
    {
        public string? SelectedSpecial { get; private set; }

        public Fallout4SpecialChoiceWindow(Fallout4Character character)
        {
            InitializeComponent();

            var options = new List<string>();
            void Add(string name, int value)
            {
                if (value < Fallout4Character.MaxSpecialAbsolute)
                    options.Add($"{name} ({value})");
            }

            Add("Strength", character.Strength);
            Add("Perception", character.Perception);
            Add("Endurance", character.Endurance);
            Add("Charisma", character.Charisma);
            Add("Intelligence", character.Intelligence);
            Add("Agility", character.Agility);
            Add("Luck", character.Luck);

            SpecialList.ItemsSource = options;
            if (options.Count > 0)
                SpecialList.SelectedIndex = 0;
        }

        private void SpecialList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConfirmButton.IsEnabled = SpecialList.SelectedItem != null;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (SpecialList.SelectedItem is not string text)
                return;

            SelectedSpecial = text.Split(' ')[0];
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
