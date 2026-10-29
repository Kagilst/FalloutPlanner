using System.Windows;
using System.Windows.Controls;

namespace FalloutPlanner.Games.FalloutNV
{
    public partial class TagSkillWindow : Window
    {
        private readonly FalloutNVCharacter _character;

        public string SelectedSkill { get; private set; }

        // FNV Skills (13 total - no BigGuns/SmallGuns, has Guns/Survival)
        private static readonly (string Property, string Label)[] Skills =
        {
            ("Barter", "Barter"),
            ("EnergyWeapons", "Energy Weapons"),
            ("Explosives", "Explosives"),
            ("Guns", "Guns"),
            ("Lockpick", "Lockpick"),
            ("Medicine", "Medicine"),
            ("MeleeWeapons", "Melee Weapons"),
            ("Repair", "Repair"),
            ("Science", "Science"),
            ("Sneak", "Sneak"),
            ("Speech", "Speech"),
            ("Survival", "Survival"),
            ("Unarmed", "Unarmed"),
        };

        public TagSkillWindow(FalloutNVCharacter character)
        {
            InitializeComponent();
            _character = character;

            foreach (var (property, label) in Skills)
            {
                var prop = typeof(FalloutNVCharacter).GetProperty(property);
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
