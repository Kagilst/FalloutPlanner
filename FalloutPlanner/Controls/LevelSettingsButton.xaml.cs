using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FalloutPlanner.Controls
{
    public partial class LevelSettingsButton : UserControl
    {
        public event RoutedEventHandler? RestartRequested;
        public event RoutedEventHandler? HomeRequested;
        public event RoutedEventHandler? SaveRequested;
        public event RoutedEventHandler? ExportGuideRequested;

        public LevelSettingsButton()
        {
            InitializeComponent();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MenuPopup.IsOpen = !MenuPopup.IsOpen;
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            MenuPopup.IsOpen = false;
            if (!ConfirmLeave("Restart"))
                return;
            RestartRequested?.Invoke(this, e);
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MenuPopup.IsOpen = false;
            if (!ConfirmLeave("return Home"))
                return;

            if (HomeRequested != null)
            {
                HomeRequested.Invoke(this, e);
                return;
            }

            NavigationService.GetNavigationService(this)?.Navigate(new HomePage());
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MenuPopup.IsOpen = false;
            SaveRequested?.Invoke(this, e);
        }

        private void ExportGuide_Click(object sender, RoutedEventArgs e)
        {
            MenuPopup.IsOpen = false;
            ExportGuideRequested?.Invoke(this, e);
        }

        private static bool ConfirmLeave(string action)
        {
            var result = MessageBox.Show(
                $"Are you sure you want to {action}?\n\nYou will lose your current progress if you have not saved first.",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            return result == MessageBoxResult.Yes;
        }
    }
}
