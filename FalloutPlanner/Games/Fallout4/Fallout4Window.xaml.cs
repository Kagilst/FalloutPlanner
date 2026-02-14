using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FalloutPlanner;

public partial class Fallout4Window : Page
{
    public Fallout4Window()
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

