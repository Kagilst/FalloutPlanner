using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FalloutPlanner.Games.FalloutNV
{
    public partial class FalloutNVLevelPage : Page
    {
        public FalloutNVCharacter Character { get; }

        public FalloutNVLevelPage(FalloutNVCharacter character)
        {
            InitializeComponent();
            Character = character;
            DataContext = character;
        }

        private void IncreaseSkill_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string skillName = btn.Tag.ToString();
                Character.ModifySkill(skillName, 1);
            }
        }

        private void DecreaseSkill_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string skillName = btn.Tag.ToString();
                Character.ModifySkill(skillName, -1);
            }
        }

        private void LevelUp_Click(object sender, RoutedEventArgs e)
        {
            if (!Character.CanLevelUp)
                return;

            // Odd levels have no perk pick — level up immediately.
            if (!Character.NextLevelRequiresPerk)
            {
                Character.LevelUp();
                return;
            }

            var window = new FalloutNVLevelUpWindow(Character);
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }

        private void Perks_Click(object sender, RoutedEventArgs e)
        {
            var window = new FalloutNVOwnedPerksWindow(Character);
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }

        private void Equipment_Click(object sender, RoutedEventArgs e)
        {
            var window = new FalloutNVEquipmentWindow(Character);
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }

        private void Implants_Click(object sender, RoutedEventArgs e)
        {
            var window = new FalloutNVImplantsWindow(Character);
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }

        private void SkillBooks_Click(object sender, RoutedEventArgs e)
        {
            var window = new FalloutNVSkillBooksWindow(Character);
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (!Character.TryUndoLastAction())
            {
                Character.RevertToCreationState();
                if (NavigationService?.CanGoBack == true)
                    NavigationService.GoBack();
            }
        }
    }
}
