using System.Windows;

namespace FalloutPlanner.Games.Fallout4
{
    public partial class Fallout4EquipmentWindow : Window
    {
        public Fallout4EquipmentWindow(Fallout4Character character)
        {
            InitializeComponent();
            DataContext = character;
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}
