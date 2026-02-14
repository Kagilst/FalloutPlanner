using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FalloutPlanner;

public partial class HomePage : Page
{
    public HomePage()
    {
        InitializeComponent();
    }

    private void Fallout1Button_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new Fallout1Window());
    }

    private void Fallout2Button_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new Fallout2Window());
    }

    private void Fallout3Button_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new Fallout3Window());
    }

    private void FalloutNVButton_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new FalloutNVWindow());
    }

    private void Fallout4Button_Click(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new Fallout4Window());
    }
}

