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
    }

}
