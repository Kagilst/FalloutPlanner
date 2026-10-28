using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FalloutPlanner.Games.Fallout3
{
    /// <summary>
    /// Interaction logic for Fallout3LevelPage.xaml
    /// </summary>
    public partial class Fallout3LevelPage : Page
    {
        public Fallout3Character Character { get; }
        public Fallout3LevelPage(Fallout3Character character)
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

            var window = new Fallout3LevelUpWindow(Character);
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }

        private void Perks_Click(object sender, RoutedEventArgs e)
        {
            var window = new Fallout3OwnedPerksWindow(Character);
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }

        private void Equipment_Click(object sender, RoutedEventArgs e)
        {
            var window = new Fallout3EquipmentWindow(Character);
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }

        private void Bobbleheads_Click(object sender, RoutedEventArgs e)
        {
            var window = new Fallout3BobbleheadsWindow(Character);
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }

        private void SkillBooks_Click(object sender, RoutedEventArgs e)
        {
            var window = new Fallout3SkillBooksWindow(Character);
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            // Undo the last logged action; if only Created Character remains, return to creation
            if (!Character.TryUndoLastAction())
            {
                Character.RevertToCreationState();
                if (NavigationService?.CanGoBack == true)
                    NavigationService.GoBack();
            }
        }
    }

}
