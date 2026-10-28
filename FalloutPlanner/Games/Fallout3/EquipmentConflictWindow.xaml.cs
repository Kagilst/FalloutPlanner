using System.Windows;

namespace FalloutPlanner.Games.Fallout3
{
    public partial class EquipmentConflictWindow : Window
    {
        public EquipmentConflictWindow(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
