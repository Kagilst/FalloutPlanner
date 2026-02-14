using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FalloutPlanner;

public partial class Fallout1Window : Page
{
    public Fallout1Window()
    {
        InitializeComponent();
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        if (NavigationService?.CanGoBack == true)
        {
            NavigationService.GoBack();
        }
    }
}

