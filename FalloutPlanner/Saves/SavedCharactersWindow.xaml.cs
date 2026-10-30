using System.Windows;
using System.Windows.Input;

namespace FalloutPlanner.Saves
{
    public partial class SavedCharactersWindow : Window
    {
        private readonly string _gameId;
        private readonly SaveRepository _repository = CharacterSaveService.SharedRepository;

        public SavedCharacterSummary? SelectedSave { get; private set; }

        public SavedCharactersWindow(string gameId)
        {
            InitializeComponent();
            _gameId = gameId;
            GameTheme.Apply(this, gameId);
            HeaderText.Text = $"{GameIds.DisplayName(gameId)} — Saved Characters";
            Refresh();
        }

        private void Refresh()
        {
            var list = _repository.List(_gameId);
            SaveList.ItemsSource = list;
            EmptyText.Visibility = list.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            SaveList.Visibility = list.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
            if (list.Count > 0)
                SaveList.SelectedIndex = 0;
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            if (SaveList.SelectedItem is not SavedCharacterSummary summary)
            {
                MessageBox.Show(this, "Select a character to load.", "Saved Characters",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            SelectedSave = summary;
            DialogResult = true;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SaveList.SelectedItem is not SavedCharacterSummary summary)
                return;

            var confirm = MessageBox.Show(this,
                $"Delete save \"{summary.DisplayName}\"?\nThis cannot be undone.",
                "Delete Save",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (confirm != MessageBoxResult.Yes)
                return;

            _repository.Delete(_gameId, summary.Id);
            Refresh();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) =>
            DialogResult = false;
    }
}
