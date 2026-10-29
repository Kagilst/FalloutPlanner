using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FalloutPlanner.Games.Fallout1.Perks;

namespace FalloutPlanner.Games.Fallout1;

public partial class Fallout1LevelPage : Page
{
    private readonly Fallout1Character _character;

    public Fallout1LevelPage(Fallout1Character character)
    {
        InitializeComponent();
        _character = character;
        DataContext = character;
        UpdateNextPerkLevel();
    }

    private void UpdateNextPerkLevel()
    {
        int current = _character.Level;
        int interval = _character.PerkInterval;
        int next = ((current / interval) + 1) * interval;
        if (next > _character.MaxLevel)
            NextPerkLevel.Text = "Max";
        else
            NextPerkLevel.Text = next.ToString();
    }

    private void IncreaseSkill_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string skillName)
        {
            _character.ModifySkill(skillName, 1);
        }
    }

    private void DecreaseSkill_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string skillName)
        {
            _character.ModifySkill(skillName, -1);
        }
    }

    private void Undo_Click(object sender, RoutedEventArgs e)
    {
        // Undo the last logged action; if only Created Character remains, return to creation
        if (!_character.TryUndoLastAction())
        {
            _character.RevertToCreationState();
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
            return;
        }

        UpdateNextPerkLevel();
    }

    private void Equipment_Click(object sender, RoutedEventArgs e)
    {
        var equipmentWindow = new Fallout1EquipmentWindow();
        equipmentWindow.Owner = Window.GetWindow(this);
        equipmentWindow.ShowDialog();
    }

    private void SkillBooks_Click(object sender, RoutedEventArgs e)
    {
        var booksWindow = new Fallout1SkillBooksWindow(_character);
        booksWindow.Owner = Window.GetWindow(this);
        booksWindow.ShowDialog();
    }

    private void Perks_Click(object sender, RoutedEventArgs e)
    {
        var perksWindow = new Fallout1OwnedPerksWindow(_character);
        perksWindow.Owner = Window.GetWindow(this);
        perksWindow.ShowDialog();
    }

    private void LevelUp_Click(object sender, RoutedEventArgs e)
    {
        if (!_character.CanLevelUp) return;

        int nextLevel = _character.Level + 1;
        bool needsPerk = _character.NextLevelRequiresPerk;

        if (needsPerk)
        {
            var perkWindow = new Fallout1PerkWindow(_character);
            perkWindow.Owner = Window.GetWindow(this);

            if (perkWindow.ShowDialog() == true && perkWindow.SelectedPerk != null)
            {
                _character.LevelUp(perkWindow.SelectedPerk);
            }
        }
        else
        {
            _character.LevelUp();
        }

        UpdateNextPerkLevel();
    }
}
