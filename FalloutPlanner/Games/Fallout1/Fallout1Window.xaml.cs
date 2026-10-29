using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using FalloutPlanner.Games.Fallout1;

namespace FalloutPlanner;

public partial class Fallout1Window : Page
{
    public Fallout1Character Character { get; set; }

    public Fallout1Window()
    {
        InitializeComponent();

        var startingStats = new Fallout1CharacterStats
        {
            Strength = 5,
            Perception = 5,
            Endurance = 5,
            Charisma = 5,
            Intelligence = 5,
            Agility = 5,
            Luck = 5,
            CharPoints = 5,
            Level = 1,
            IsMale = true,
            Age = 25
        };

        Character = new Fallout1Character(startingStats);
        DataContext = Character;
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (NavigationService?.CanGoBack == true)
            NavigationService.GoBack();
    }

    private void ModifySpecial_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            string? statName = button.Tag?.ToString();
            if (string.IsNullOrEmpty(statName))
                return;

            int amount = button.Content?.ToString() == "+" ? 1 : -1;
            Character.ModifySpecial(statName, amount);
        }
    }

    private void TagSkill_Toggled(object sender, RoutedEventArgs e)
    {
        if (sender is ToggleButton toggle && toggle.Tag != null)
        {
            string skillName = toggle.Tag.ToString()!;
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
        int tagged = Character.TaggedSkillCount;

        foreach (var child in SkillsPanel.Children)
        {
            if (child is StackPanel sp)
            {
                foreach (var item in sp.Children)
                {
                    if (item is Grid g)
                    {
                        foreach (var gc in g.Children)
                        {
                            if (gc is ToggleButton tb && tb.IsChecked == false)
                                tb.IsEnabled = tagged < 3;
                        }
                    }
                }
            }
        }
    }

    private void CreateChar_Click(object sender, RoutedEventArgs e)
    {
        if (!Character.CanConfirm)
            return;

        // Step 1: Name / Age / Gender
        var nameWindow = new Fallout1CharacterNameWindow
        {
            Owner = Window.GetWindow(this)
        };

        if (nameWindow.ShowDialog() != true)
            return;

        Character.Name = nameWindow.CharacterName;
        Character.Age = nameWindow.CharacterAge;
        Character.IsMale = nameWindow.IsMale;

        // Step 2: Traits (0-2)
        var traitsWindow = new Fallout1TraitsWindow(Character)
        {
            Owner = Window.GetWindow(this)
        };

        if (traitsWindow.ShowDialog() != true)
            return;

        // Step 3: Level page
        Character.SaveInitialState();
        NavigationService?.Navigate(new Fallout1LevelPage(Character));
    }
}
