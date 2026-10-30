using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using FalloutPlanner.Games.Fallout2;

namespace FalloutPlanner;

public partial class Fallout2Window : Page
{
    public Fallout2Character Character { get; set; }

    public Fallout2Window()
    {
        InitializeComponent();

        var startingStats = new Fallout2CharacterStats
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
            Age = 25,
            TaggedSkills = 0,
            PerkRate = 3
        };

        Character = new Fallout2Character(startingStats);
        DataContext = Character;
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (NavigationService?.CanGoBack == true)
        {
            NavigationService.GoBack();
        }
    }

    private void Characters_Click(object sender, RoutedEventArgs e)
    {
        var browser = new FalloutPlanner.Saves.SavedCharactersWindow(FalloutPlanner.Saves.GameIds.Fallout2)
        {
            Owner = Window.GetWindow(this)
        };
        if (browser.ShowDialog() != true || browser.SelectedSave == null)
            return;

        var file = FalloutPlanner.Saves.CharacterSaveService.SharedRepository
            .Load(FalloutPlanner.Saves.GameIds.Fallout2, browser.SelectedSave.Id);
        if (file == null)
        {
            MessageBox.Show(Window.GetWindow(this), "Could not load that save.", "Saved Characters",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var loaded = FalloutPlanner.Saves.Adapters.Fallout2SaveAdapter.FromFile(file);
        NavigationService?.Navigate(new Fallout2LevelPage(loaded));
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
            if (child is StackPanel sp)
            {
                foreach (var item in sp.Children)
                {
                    if (item is Grid g)
                    {
                        foreach (var gc in g.Children)
                        {
                            if (gc is ToggleButton tb && tb.IsChecked == false)
                            {
                                tb.IsEnabled = tagged < 3;
                            }
                        }
                    }
                }
            }
        }
    }

    private void CreateChar_Click(object sender, RoutedEventArgs e)
    {
        // Step 1: Get character name
        var nameWindow = new Fallout2CharacterNameWindow();
        nameWindow.Owner = Window.GetWindow(this);

        bool? nameResult = nameWindow.ShowDialog();

        if (nameResult != true)
            return;

        Character.Name = nameWindow.CharacterName;
        Character.Age = nameWindow.CharacterAge;
        Character.IsMale = nameWindow.IsMale;

        // Step 2: Choose traits (0-2)
        var traitsWindow = new Fallout2TraitsWindow(Character);
        traitsWindow.Owner = Window.GetWindow(this);

        bool? traitsResult = traitsWindow.ShowDialog();

        if (traitsResult != true)
            return;

        // Step 3: Save initial state and navigate to level page
        Character.SaveInitialState();
        NavigationService.Navigate(new Fallout2LevelPage(Character));
    }
}

