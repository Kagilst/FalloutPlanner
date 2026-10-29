using FalloutPlanner.Games.FalloutNV.SkillBooks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FalloutPlanner.Games.FalloutNV
{
    public partial class FalloutNVSkillBookChecklistWindow : Window, INotifyPropertyChanged
    {
        private readonly FalloutNVCharacter _character;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<SkillBookSection> Sections { get; } = new();

        public FalloutNVSkillBookChecklistWindow(FalloutNVCharacter character)
        {
            InitializeComponent();
            _character = character;
            DataContext = this;
            BuildSections();
        }

        private void BuildSections()
        {
            Sections.Clear();
            foreach (var book in FalloutNVSkillBookRepository.BookTypes)
            {
                var section = new SkillBookSection(book.Name);
                foreach (var loc in FalloutNVSkillBookRepository.LocationsFor(book.Name))
                    section.Locations.Add(new SkillBookLocationRow(_character, loc, RefreshEnabledStates));
                section.RefreshCounts();
                Sections.Add(section);
            }
            RefreshEnabledStates();
        }

        private void RefreshEnabledStates()
        {
            // FNV doesn't have mutually exclusive house groups like FO3
            foreach (var section in Sections)
            {
                foreach (var row in section.Locations)
                    row.UpdateAvailability();
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
        private readonly FalloutNVCharacter _character;
        private readonly FalloutNVSkillBookLocation _location;
        private readonly System.Action _onChanged;
        private bool _isFound;
        private bool _isEnabled = true;
        private Brush _foreground = Brushes.LimeGreen;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Id => _location.Id;
        public string MapMarker => _location.MapMarker;
        public string Description => _location.Description;

        public bool IsFound
        {
            get => _isFound;
            set
            {
                if (_isFound == value)
                    return;

                if (!_character.TrySetSkillBookLocationFound(_location.Id, value))
                {
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
            FalloutNVCharacter character,
            FalloutNVSkillBookLocation location,
            System.Action onChanged)
        {
            _character = character;
            _location = location;
            _onChanged = onChanged;
            _isFound = character.IsSkillBookLocationFound(location.Id);
            _foreground = (Brush)Application.Current.TryFindResource("FalloutTextBrush")
                          ?? Brushes.LimeGreen;
        }

        public void UpdateAvailability()
        {
            // FNV doesn't have mutually exclusive book locations
            IsEnabled = true;
            ForegroundBrush = (Brush)Application.Current.TryFindResource("FalloutTextBrush")
                             ?? Brushes.LimeGreen;

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
