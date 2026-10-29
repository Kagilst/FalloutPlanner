using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;

using FalloutPlanner.Games.FalloutNV;

namespace FalloutPlanner;

public partial class FalloutNVWindow : Page
{
    public FalloutNVCharacter Character { get; set; }

    public FalloutNVWindow()
    {
        InitializeComponent();

        var startingStats = new FalloutNVCharacterStats
        {
            Strength = 5,
            Perception = 5,
            Endurance = 5,
            Charisma = 5,
            Intelligence = 5,
            Agility = 5,
            Luck = 5,
            Points = 5,
            Level = 1,
            IsMale = true,
            DamageResist = 0,
            DamageThreshold = 0,
            FireResist = 0,
            EnergyResist = 0,
            TaggedSkills = 0,
            // FNV skills initialized via RecalculateDerivedStats
            Barter = 15,
            EnergyWeapons = 15,
            Explosives = 15,
            Guns = 15,
            Lockpick = 15,
            Medicine = 15,
            MeleeWeapons = 15,
            Repair = 15,
            Science = 15,
            Sneak = 15,
            Speech = 15,
            Survival = 15,
            Unarmed = 15
        };

        Character = new FalloutNVCharacter(startingStats);
        DataContext = Character;
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (NavigationService?.CanGoBack == true)
        {
            NavigationService.GoBack();
        }
    }

    // Modify SPECIAL
    private void ModifySpecial_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            string statName = button.Tag.ToString();
            int amount = button.Content.ToString() == "+" ? 1 : -1;

            Character.ModifySpecial(statName, amount);
        }
    }

    // TAG button
    private void TagSkill_Toggled(object sender, RoutedEventArgs e)
    {
        if (sender is ToggleButton toggle && toggle.Tag != null)
        {
            string skillName = toggle.Tag.ToString();
            bool isChecked = toggle.IsChecked == true;

            bool allowed = Character.ModifyTaggedSkill(skillName, isChecked);

            if (!allowed)
            {
                toggle.IsChecked = false;
                return;
            }

            UpdateToggleButtons();
        }
    }

    private void UpdateToggleButtons()
    {
        int tagged = Character.TaggedSkills;

        foreach (var child in SkillsPanel.Children)
        {
            if (child is ToggleButton tb && tb.IsChecked == false)
            {
                tb.IsEnabled = tagged < 3;
            }
        }
    }

    private void CreateNVChar_Click(object sender, RoutedEventArgs e)
    {
        // Step 1: Get character name (NV amber theme)
        var nameWindow = new FalloutNVCharacterNameWindow();
        nameWindow.Owner = Window.GetWindow(this);

        bool? nameResult = nameWindow.ShowDialog();

        if (nameResult != true)
            return;

        Character.Name = nameWindow.CharacterName;

        // Step 2: Choose traits (0-2)
        var traitsWindow = new FalloutNVTraitsWindow(Character);
        traitsWindow.Owner = Window.GetWindow(this);

        bool? traitsResult = traitsWindow.ShowDialog();

        if (traitsResult != true)
            return;

        // Step 3: Save initial state and navigate to level page
        Character.SaveInitialState();
        NavigationService.Navigate(new FalloutNVLevelPage(Character));
    }
}
