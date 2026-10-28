using FalloutPlanner.Games.Fallout3.SkillBooks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FalloutPlanner.Games.Fallout3
{
    public partial class Fallout3SkillBookChecklistWindow : Window, INotifyPropertyChanged
    {
        private readonly Fallout3Character _character;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<SkillBookSection> Sections { get; } = new();

        public Fallout3SkillBookChecklistWindow(Fallout3Character character)
        {
            InitializeComponent();
            _character = character;
            DataContext = this;
            BuildSections();
        }

        private void BuildSections()
        {
            Sections.Clear();
            foreach (var book in Fallout3SkillBookRepository.BookTypes)
            {
                var section = new SkillBookSection(book.Name);
                foreach (var loc in Fallout3SkillBookRepository.LocationsFor(book.Name))
                    section.Locations.Add(new SkillBookLocationRow(_character, loc, RefreshEnabledStates));
                section.RefreshCounts();
                Sections.Add(section);
            }
            RefreshEnabledStates();
        }

        private void RefreshEnabledStates()
        {
            string? active = _character.ActiveHouseThemeGroup;
            string? opposing = Fallout3SkillBookRepository.OpposingHouseGroup(active);

            foreach (var section in Sections)
            {
                foreach (var row in section.Locations)
                    row.UpdateAvailability(active, opposing);
                section.RefreshCounts();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e) => Close();
    }

    public class SkillBookSection : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string BookName { get; }
        public bool IsExpanded { get; set; }
        public ObservableCollection<SkillBookLocationRow> Locations { get; } = new();

        public int FoundCount => Locations.Count(l => l.IsFound);
        public int TotalCount => Locations.Count;

        public SkillBookSection(string bookName)
        {
            BookName = bookName;
        }

        public void RefreshCounts()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FoundCount)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalCount)));
        }
    }

    public class SkillBookLocationRow : INotifyPropertyChanged
    {
        private readonly Fallout3Character _character;
        private readonly Fallout3SkillBookLocation _location;
        private readonly System.Action _onChanged;
        private bool _isFound;
        private bool _isEnabled = true;
        private Brush _foreground = Brushes.LimeGreen;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Id => _location.Id;
        public string MapMarker => _location.MapMarker;
        public string Description => _location.Description;
        public string? ExclusiveGroup => _location.ExclusiveGroup;

        public bool IsFound
        {
            get => _isFound;
            set
            {
                if (_isFound == value)
                    return;

                if (!_character.TrySetSkillBookLocationFound(_location.Id, value))
                {
                    // Revert UI if blocked (exclusive / max)
                    OnPropertyChanged();
                    return;
                }

                _isFound = value;
                OnPropertyChanged();
                _onChanged?.Invoke();
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            private set
            {
                if (_isEnabled == value)
                    return;
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public Brush ForegroundBrush
        {
            get => _foreground;
            private set
            {
                _foreground = value;
                OnPropertyChanged();
            }
        }

        public SkillBookLocationRow(
            Fallout3Character character,
            Fallout3SkillBookLocation location,
            System.Action onChanged)
        {
            _character = character;
            _location = location;
            _onChanged = onChanged;
            _isFound = character.IsSkillBookLocationFound(location.Id);
            // Fallout theme green text
            _foreground = (Brush)Application.Current.TryFindResource("FalloutTextBrush")
                          ?? Brushes.LimeGreen;
        }

        public void UpdateAvailability(string? activeHouseGroup, string? opposingGroup)
        {
            bool blockedByHouse =
                ExclusiveGroup != null &&
                opposingGroup != null &&
                ExclusiveGroup == opposingGroup &&
                !_isFound;

            // Checklist is independent of skill-book counts; only house exclusivity blocks
            IsEnabled = !blockedByHouse;

            if (blockedByHouse && !_isFound)
                ForegroundBrush = Brushes.IndianRed;
            else
                ForegroundBrush = (Brush)Application.Current.TryFindResource("FalloutTextBrush")
                                 ?? Brushes.LimeGreen;

            // Sync found flag from character without re-applying
            bool actual = _character.IsSkillBookLocationFound(_location.Id);
            if (actual != _isFound)
            {
                _isFound = actual;
                OnPropertyChanged(nameof(IsFound));
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
