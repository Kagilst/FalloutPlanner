using System.Windows;

namespace FalloutPlanner.Games.Fallout2
{
    public partial class Fallout2EquipmentWindow : Window
    {
        public Fallout2EquipmentWindow()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
