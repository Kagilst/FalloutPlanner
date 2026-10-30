using System.Windows;

namespace FalloutPlanner.Saves
{
    public partial class SaveNameWindow : Window
    {
        public string SaveName { get; private set; } = "";

        public SaveNameWindow(string defaultName, string? gameId = null)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(gameId))
                GameTheme.Apply(this, gameId);
            NameBox.Text = string.IsNullOrWhiteSpace(defaultName) ? "New Character" : defaultName;
            NameBox.SelectAll();
            NameBox.Focus();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var name = NameBox.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(this, "Enter a save name.", "Save Character",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            SaveName = name;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) =>
            DialogResult = false;
    }
}
