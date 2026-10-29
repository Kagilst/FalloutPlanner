using FalloutPlanner.Games.FalloutNV.SkillBooks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace FalloutPlanner.Games.FalloutNV
{
    public partial class FalloutNVSkillBooksWindow : Window, INotifyPropertyChanged
    {
        private readonly FalloutNVCharacter _character;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<SkillBookRow> Books { get; } = new();
        public ObservableCollection<WorkbenchCrateRow> WorkbenchCrates { get; } = new();

        public string ComprehensionNote =>
            _character.GetPerkRank("Comprehension") > 0
                ? "Comprehension owned: each book grants +4 skill."
                : "Each book grants +3 skill (+4 with Comprehension).";

        public FalloutNVSkillBooksWindow(FalloutNVCharacter character)
        {
            InitializeComponent();
            _character = character;
            DataContext = this;
            Reload();
        }

        private void Reload()
        {
            Books.Clear();
            foreach (var book in FalloutNVSkillBookRepository.BookTypes)
            {
                // Skip workbench crate pseudo-type if it exists
                if (book.Name == "Workbench Crate")
                    continue;
                Books.Add(new SkillBookRow(_character, book));
            }

            WorkbenchCrates.Clear();
            for (int i = 1; i <= 4; i++)
            {
                WorkbenchCrates.Add(new WorkbenchCrateRow(_character, i));
            }

            OnPropertyChanged(nameof(ComprehensionNote));
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: SkillBookRow row })
            {
                row.Increase();
                OnPropertyChanged(nameof(ComprehensionNote));
            }
        }

        private void OpenCrate_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: WorkbenchCrateRow crateRow })
            {
                var dialog = new WorkbenchCrateWindow(_character, crateRow.CrateNumber)
                {
                    Owner = this
                };

                if (dialog.ShowDialog() == true && dialog.SelectedBooks.Count > 0)
                {
                    // Apply the selected books as skill bonuses
                    int pointsPerBook = _character.SkillBookPointsPerBook;
                    foreach (var skillPropertyName in dialog.SelectedBooks)
                    {
                        _character.TryAddSkillPointsFromPerk(skillPropertyName, pointsPerBook);
                    }

                    // Mark the crate as opened with the count of books found
                    _character.SetWorkbenchCrateOpened(crateRow.CrateNumber, dialog.SelectedBooks.Count);
                    crateRow.Notify();

                    // Refresh the regular book rows in case we want to show totals
                    foreach (var bookRow in Books)
                    {
                        bookRow.Notify();
                    }
                }
            }
        }

        private void Checklist_Click(object sender, RoutedEventArgs e)
        {
            var checklist = new FalloutNVSkillBookChecklistWindow(_character) { Owner = this };
            checklist.ShowDialog();
            Reload();
        }

        private void Back_Click(object sender, RoutedEventArgs e) => Close();

        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class SkillBookRow : INotifyPropertyChanged
    {
        private readonly FalloutNVCharacter _character;
        private readonly FalloutNVSkillBookType _book;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name => _book.Name;
        public string SkillLabel => _book.SkillDisplayName;
        public string CountDisplay => _character.GetSkillBookCount(_book.Name).ToString();
        public string MaxDisplay => _book.MaxCount < 0 ? "/ ∞" : $"/ {_book.MaxCount}";
        public bool CanIncrease =>
            _book.MaxCount < 0 || _character.GetSkillBookCount(_book.Name) < _book.MaxCount;

        public SkillBookRow(FalloutNVCharacter character, FalloutNVSkillBookType book)
        {
            _character = character;
            _book = book;
        }

        public void Increase()
        {
            int n = _character.GetSkillBookCount(_book.Name);
            if (_character.TrySetSkillBookCount(_book.Name, n + 1))
                Notify();
        }

        public void Notify()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CountDisplay)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanIncrease)));
        }
    }

    public class WorkbenchCrateRow : INotifyPropertyChanged
    {
        private readonly FalloutNVCharacter _character;

        public event PropertyChangedEventHandler PropertyChanged;

        public int CrateNumber { get; }

        public string CrateName => $"Workbench Crate {CrateNumber}";

        public bool IsOpened => _character.IsWorkbenchCrateOpened(CrateNumber);

        public int BooksFound => _character.GetWorkbenchCrateBooksCount(CrateNumber);

        public string StatusText => IsOpened
            ? $"{BooksFound} book{(BooksFound == 1 ? "" : "s")} found"
            : "Not opened";

        public string ButtonText => IsOpened ? "Opened" : "Open";

        public bool CanOpen => !IsOpened;

        public WorkbenchCrateRow(FalloutNVCharacter character, int crateNumber)
        {
            _character = character;
            CrateNumber = crateNumber;
        }

        public void Notify()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsOpened)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BooksFound)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanOpen)));
        }
    }
}
