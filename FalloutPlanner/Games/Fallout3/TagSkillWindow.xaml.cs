using System.Windows;
using System.Windows.Controls;

namespace FalloutPlanner.Games.Fallout3
{
    public partial class TagSkillWindow : Window
    {
        private readonly Fallout3Character _character;

        public string SelectedSkill { get; private set; }

        private static readonly (string Property, string Label)[] Skills =
        {
            ("Barter", "Barter"),
            ("BigGuns", "Big Guns"),
            ("EnergyWeapons", "Energy Weapons"),
            ("Explosives", "Explosives"),
            ("Lockpick", "Lockpick"),
            ("Medicine", "Medicine"),
            ("MeleeWeapons", "Melee Weapons"),
            ("Repair", "Repair"),
            ("Science", "Science"),
            ("SmallGuns", "Small Guns"),
            ("Sneak", "Sneak"),
            ("Speech", "Speech"),
            ("Unarmed", "Unarmed"),
        };

        public TagSkillWindow(Fallout3Character character)
        {
            InitializeComponent();
            _character = character;

            foreach (var (property, label) in Skills)
            {
                var prop = typeof(Fallout3Character).GetProperty(property);
                int value = (int)prop.GetValue(_character);

                var button = new Button
                {
                    Content = $"{label} ({value})",
                    Tag = property,
                    Margin = new Thickness(0, 3, 0, 3),
                    Style = (Style)FindResource("BoxButtons"),
                    IsEnabled = value < 100
                };
                button.Click += Skill_Click;
                SkillButtons.Children.Add(button);
            }
        }

        private void Skill_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button || button.Tag is not string skill)
                return;

            SelectedSkill = skill;
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
