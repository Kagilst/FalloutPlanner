using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FalloutPlanner.Games.Fallout2
{
    public partial class Fallout2LevelPage : Page
    {
        public Fallout2Character Character { get; }

        public Fallout2LevelPage(Fallout2Character character)
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

            // Check if next level grants a perk
            if (!Character.NextLevelRequiresPerk)
            {
                Character.LevelUp();
                return;
            }

            var window = new Fallout2LevelUpWindow(Character);
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }

        private void Perks_Click(object sender, RoutedEventArgs e)
        {
            var window = new Fallout2PerksWindow(Character);
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }

        private void Equipment_Click(object sender, RoutedEventArgs e)
        {
            var window = new Fallout2EquipmentWindow();
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }

        private void SkillBooks_Click(object sender, RoutedEventArgs e)
        {
            var window = new Fallout2SkillBooksWindow(Character);
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

        private void Settings_Restart(object sender, RoutedEventArgs e)
        {
            Character.RevertToCreationState();
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
        }

        private void Settings_Home(object sender, RoutedEventArgs e) =>
            NavigationService?.Navigate(new FalloutPlanner.HomePage());

        private void Settings_Save(object sender, RoutedEventArgs e)
        {
            FalloutPlanner.Saves.CharacterSaveService.TrySaveFallout2(Character, Window.GetWindow(this));
        }

        private void Settings_ExportGuide(object sender, RoutedEventArgs e)
        {
            FalloutPlanner.Saves.MarkdownGuideExporter.TryExportGeneric(
                "Fallout 2",
                Character.Name,
                Character.Level,
                Character.GetSpecialLine(starting: true),
                Character.GetSpecialLine(starting: false),
                Character.GetTaggedSkillsLine(),
                Character.ActionLog,
                $"- Hit Points: {Character.HitPoints}\n- Skill Points: {Character.SkillPoints}",
                Window.GetWindow(this));
        }
    }
}
