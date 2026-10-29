using System.Windows;

namespace FalloutPlanner.Games.Fallout1;

public partial class Fallout1EquipmentWindow : Window
{
    public Fallout1EquipmentWindow()
    {
        InitializeComponent();
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
