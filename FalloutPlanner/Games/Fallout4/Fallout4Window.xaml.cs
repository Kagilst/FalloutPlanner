using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FalloutPlanner.Games.Fallout4;

namespace FalloutPlanner;

public partial class Fallout4Window : Page
{
    public Fallout4Character Character { get; }

    public Fallout4Window()
    {
        InitializeComponent();

        Character = new Fallout4Character(new Fallout4CharacterStats
        {
            Strength = 1,
            Perception = 1,
            Endurance = 1,
            Charisma = 1,
            Intelligence = 1,
            Agility = 1,
            Luck = 1,
            Points = Fallout4Character.CreationPoints,
            Level = 1,
            PerkPoints = 0
        });

        DataContext = Character;
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (NavigationService?.CanGoBack == true)
            NavigationService.GoBack();
    }

    private void ModifySpecial_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button || button.Tag is null)
            return;

        string statName = button.Tag.ToString()!;
        int amount = button.Content?.ToString() == "+" ? 1 : -1;
        Character.ModifySpecialCreation(statName, amount);
    }

    private void CreateCharacter_Click(object sender, RoutedEventArgs e)
    {
        if (!Character.CanConfirmCreation)
            return;

        var nameWindow = new Fallout4CharacterNameWindow
        {
            Owner = Window.GetWindow(this)
        };

        if (nameWindow.ShowDialog() == true)
        {
            Character.Name = nameWindow.CharacterName;
            Character.SaveInitialState();
            NavigationService?.Navigate(new Fallout4LevelPage(Character));
        }
    }
}
