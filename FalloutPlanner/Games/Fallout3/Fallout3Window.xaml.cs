using FalloutPlanner.Games.Fallout3;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;

namespace FalloutPlanner;

public partial class Fallout3Window : Page
{
    public Fallout3Character Character { get; set; }

    public Fallout3Window()
    {
        InitializeComponent();

        var startingStats = new Fallout3CharacterStats
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
            DamageResist = 0,
            FireResist = 0,
            TaggedSkills = 0,
            Barter = 15,
            BigGuns = 15,
            EnergyWeapons = 15,
            Explosives = 15,
            Lockpick = 15,
            Medicine = 15,
            MeleeWeapons = 15,
            Repair = 15,
            Science = 15,
            SmallGuns = 15,
            Sneak = 15,
            Speech = 15,
            Unarmed = 15
        };

        Character = new Fallout3Character(startingStats);
        DataContext = Character;

    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (NavigationService?.CanGoBack == true)
        {
            NavigationService.GoBack();
        }
    }

    //Modify SPECIAL

    private void ModifySpecial_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button)
        {
            string statName = button.Tag.ToString();
            int amount = button.Content.ToString() == "+" ? 1 : -1;

            Character.ModifySpecial(statName, amount);
        }
    }

    //TAG button
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

        foreach (var child in SkillsPanel.Children) // assuming all toggles in a panel
        {
            if (child is ToggleButton tb && tb.IsChecked == false)
            {
                tb.IsEnabled = tagged < 3; // disable if already 3 tagged
            }
        }
    }


    private void CreateF3Char_Click(object sender, RoutedEventArgs e)
    {
        CharacterNameWindow nameWindow = new CharacterNameWindow();
        nameWindow.Owner = Window.GetWindow(this);

        bool? result = nameWindow.ShowDialog();

        if (result == true)
        {
            
            Character.Name = nameWindow.CharacterName;

            
            Character.SaveInitialState();

            
            NavigationService.Navigate(new Fallout3LevelPage(Character));
        }
    }
}

