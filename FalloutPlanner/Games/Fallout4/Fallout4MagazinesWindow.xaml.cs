using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using FalloutPlanner.Games.Fallout4.Magazines;

namespace FalloutPlanner.Games.Fallout4
{
    public partial class Fallout4MagazinesWindow : Window, INotifyPropertyChanged
    {
        private readonly Fallout4Character _character;
        private CollectedMagazineEntry? _selectedCollected;
        private SeriesRow? _selectedSeries;
        private Fallout4MagazineIssue? _selectedIssue;
        private bool _selecting;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<CollectedMagazineEntry> Collected { get; } = new();
        public ObservableCollection<SeriesRow> SeriesRows { get; } = new();
        public ObservableCollection<Fallout4MagazineIssue> AvailableIssues { get; } = new();

        public bool ShowCollectedEmpty => Collected.Count == 0;

        public CollectedMagazineEntry? SelectedCollected
        {
            get => _selectedCollected;
            set { if (_selectedCollected != value) { _selectedCollected = value; OnPropertyChanged(); } }
        }

        public SeriesRow? SelectedSeries
        {
            get => _selectedSeries;
            set
            {
                if (_selectedSeries == value) return;
                _selectedSeries = value;
                OnPropertyChanged();
                if (!_selecting)
                    RefreshSeriesControls(updateDetails: true);
            }
        }

        public Fallout4MagazineIssue? SelectedIssue
        {
            get => _selectedIssue;
            set
            {
                if (_selectedIssue == value) return;
                _selectedIssue = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanCollectIssue));
                if (value != null && !_selecting && SelectedSeries != null)
                    ShowIssuePreview(value);
            }
        }

        public bool ShowIssuePicker { get; private set; }
        public bool ShowStackControls { get; private set; }
        public bool ShowYoureSpecial { get; private set; }
        public bool CanCollectIssue => SelectedSeries != null && SelectedIssue != null;
        public bool CanCollectStack { get; private set; }
        public bool CanCollectYoureSpecial { get; private set; }
        public string StackProgressText { get; private set; } = "";
        public string DetailTitle { get; private set; } = "";
        public string DetailDescription { get; private set; } = "Select a collected magazine or series.";

        public Fallout4MagazinesWindow(Fallout4Character character)
        {
            InitializeComponent();
            _character = character;
            DataContext = this;
            RefreshAll();
        }

        private void RefreshAll(string? preferCollectedKey = null, string? preferSeriesName = null)
        {
            _selecting = true;

            Collected.Clear();
            foreach (var entry in _character.GetCollectedMagazineEntries())
                Collected.Add(entry);

            SeriesRows.Clear();
            foreach (var series in Fallout4MagazineRepository.All)
                SeriesRows.Add(new SeriesRow(series, _character));

            OnPropertyChanged(nameof(ShowCollectedEmpty));

            CollectedMagazineEntry? collectedToSelect = null;
            SeriesRow? seriesToSelect = null;

            if (!string.IsNullOrEmpty(preferCollectedKey))
                collectedToSelect = Collected.FirstOrDefault(c => c.Key == preferCollectedKey);

            if (!string.IsNullOrEmpty(preferSeriesName))
                seriesToSelect = SeriesRows.FirstOrDefault(s => s.Series.Name == preferSeriesName);

            // Prefer showing the newly collected item; otherwise restore series for finding more
            if (collectedToSelect != null)
            {
                _selectedCollected = collectedToSelect;
                _selectedSeries = null;
                OnPropertyChanged(nameof(SelectedCollected));
                OnPropertyChanged(nameof(SelectedSeries));
                ClearSeriesControls();
                ShowCollectedDetails(collectedToSelect);
            }
            else if (seriesToSelect != null)
            {
                _selectedCollected = null;
                _selectedSeries = seriesToSelect;
                OnPropertyChanged(nameof(SelectedCollected));
                OnPropertyChanged(nameof(SelectedSeries));
                RefreshSeriesControls(updateDetails: true);
            }
            else
            {
                _selectedCollected = null;
                _selectedSeries = SeriesRows.FirstOrDefault();
                OnPropertyChanged(nameof(SelectedCollected));
                OnPropertyChanged(nameof(SelectedSeries));
                RefreshSeriesControls(updateDetails: true);
            }

            _selecting = false;
        }

        private void RefreshSeriesControls(bool updateDetails)
        {
            AvailableIssues.Clear();
            ShowIssuePicker = false;
            ShowStackControls = false;
            ShowYoureSpecial = false;
            CanCollectStack = false;
            CanCollectYoureSpecial = false;
            StackProgressText = "";
            _selectedIssue = null;
            OnPropertyChanged(nameof(SelectedIssue));

            if (SelectedSeries == null)
            {
                NotifyControls();
                return;
            }

            var series = SelectedSeries.Series;

            if (series.GrantsSpecialChoice)
            {
                ShowYoureSpecial = true;
                CanCollectYoureSpecial = _character.CanCollectYoureSpecial();
                if (updateDetails)
                {
                    DetailTitle = series.Name;
                    DetailDescription = series.EffectDisplay;
                }
            }
            else if (series.HasUniqueIssues)
            {
                ShowIssuePicker = true;
                foreach (var issue in _character.GetUncollectedIssues(series))
                    AvailableIssues.Add(issue);

                _selectedIssue = AvailableIssues.FirstOrDefault();
                OnPropertyChanged(nameof(SelectedIssue));
                OnPropertyChanged(nameof(CanCollectIssue));

                if (updateDetails)
                {
                    if (SelectedIssue != null)
                        ShowIssuePreview(SelectedIssue);
                    else
                    {
                        DetailTitle = series.Name;
                        DetailDescription = "All issues collected.";
                    }
                }
            }
            else if (series.IsStacking)
            {
                ShowStackControls = true;
                int count = _character.GetMagazineStackCount(series.Name);
                CanCollectStack = _character.CanCollectStackingMagazine(series);
                StackProgressText = $"{count}/{series.MaxIssues} collected";
                if (updateDetails)
                {
                    DetailTitle = $"{series.Name} ({count}/{series.MaxIssues})";
                    DetailDescription = count > 0
                        ? Fallout4MagazineRepository.GetStackedDescription(series, count)
                        : series.EffectDisplay;
                }
            }

            NotifyControls();
            if (updateDetails)
                NotifyDetails();
        }

        private void ClearSeriesControls()
        {
            AvailableIssues.Clear();
            ShowIssuePicker = false;
            ShowStackControls = false;
            ShowYoureSpecial = false;
            CanCollectStack = false;
            CanCollectYoureSpecial = false;
            StackProgressText = "";
            _selectedIssue = null;
            OnPropertyChanged(nameof(SelectedIssue));
            OnPropertyChanged(nameof(CanCollectIssue));
            NotifyControls();
        }

        private void CollectedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selecting || SelectedCollected == null)
                return;

            _selecting = true;

            // Unhighlight Find Magazines side so re-selecting collected always works
            if (_selectedSeries != null)
            {
                _selectedSeries = null;
                OnPropertyChanged(nameof(SelectedSeries));
                ClearSeriesControls();
            }

            ShowCollectedDetails(SelectedCollected);
            _selecting = false;
        }

        private void SeriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selecting || SelectedSeries == null)
                return;

            _selecting = true;

            // Unhighlight Collected side
            if (_selectedCollected != null)
            {
                _selectedCollected = null;
                OnPropertyChanged(nameof(SelectedCollected));
            }

            RefreshSeriesControls(updateDetails: true);
            _selecting = false;
        }

        private void ShowCollectedDetails(CollectedMagazineEntry entry)
        {
            DetailTitle = entry.DisplayName;
            DetailDescription = entry.Description;
            NotifyDetails();
        }

        private void ShowIssuePreview(Fallout4MagazineIssue issue)
        {
            DetailTitle = $"{SelectedSeries?.Series.Name}: {issue.DisplayName}";
            DetailDescription = $"{issue.Effect}\n\nLocation: {issue.Location}";
            NotifyDetails();
        }

        private void CollectIssue_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedSeries == null || SelectedIssue == null) return;
            var seriesName = SelectedSeries.Series.Name;
            var issueId = SelectedIssue.Id;
            if (_character.TryCollectMagazineIssue(SelectedSeries.Series, SelectedIssue))
                RefreshAll(issueId, seriesName);
        }

        private void CollectStack_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedSeries == null) return;
            var seriesName = SelectedSeries.Series.Name;
            if (_character.TryCollectStackingMagazine(SelectedSeries.Series))
                RefreshAll($"stack:{seriesName}", seriesName);
        }

        private void CollectYoureSpecial_Click(object sender, RoutedEventArgs e)
        {
            var choice = new Fallout4SpecialChoiceWindow(_character) { Owner = this };
            if (choice.ShowDialog() != true || string.IsNullOrEmpty(choice.SelectedSpecial))
                return;

            if (_character.TryCollectYoureSpecial(choice.SelectedSpecial))
                RefreshAll("youre-special", "You're SPECIAL!");
        }

        private void Back_Click(object sender, RoutedEventArgs e) => Close();

        private void NotifyControls()
        {
            OnPropertyChanged(nameof(ShowIssuePicker));
            OnPropertyChanged(nameof(ShowStackControls));
            OnPropertyChanged(nameof(ShowYoureSpecial));
            OnPropertyChanged(nameof(CanCollectIssue));
            OnPropertyChanged(nameof(CanCollectStack));
            OnPropertyChanged(nameof(CanCollectYoureSpecial));
            OnPropertyChanged(nameof(StackProgressText));
        }

        private void NotifyDetails()
        {
            OnPropertyChanged(nameof(DetailTitle));
            OnPropertyChanged(nameof(DetailDescription));
        }

        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class SeriesRow
    {
        public Fallout4MagazineSeries Series { get; }
        public string DisplayName { get; }

        public SeriesRow(Fallout4MagazineSeries series, Fallout4Character character)
        {
            Series = series;
            if (series.GrantsSpecialChoice)
            {
                DisplayName = character.YoureSpecialCollected
                    ? $"{series.Name} (collected)"
                    : series.Name;
            }
            else if (series.IsStacking)
            {
                int count = character.GetMagazineStackCount(series.Name);
                DisplayName = $"{series.Name}  [{count}/{series.MaxIssues}]";
            }
            else
            {
                int collected = series.Issues.Count(i => character.IsMagazineIssueCollected(i.Id));
                DisplayName = $"{series.Name}  [{collected}/{series.MaxIssues}]";
            }
        }
    }
}
