using System.Windows;

namespace FalloutPlanner.Games.Fallout2;

public partial class Fallout2SkillBooksWindow : Window
{
    public Fallout2SkillBooksWindow(Fallout2Character character)
    {
        InitializeComponent();
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
