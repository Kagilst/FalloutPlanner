using System.Windows;

namespace FalloutPlanner.Games.FalloutNV
{
    public partial class FalloutNVEquipmentWindow : Window
    {
        public FalloutNVEquipmentWindow(FalloutNVCharacter character)
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
