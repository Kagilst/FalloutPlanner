using FalloutPlanner.Games.Fallout3.SkillBooks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace FalloutPlanner.Games.Fallout3
{
    public partial class Fallout3SkillBooksWindow : Window, INotifyPropertyChanged
    {
        private readonly Fallout3Character _character;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<SkillBookRow> Books { get; } = new();

        public string ComprehensionNote =>
            _character.GetPerkRank("Comprehension") > 0
                ? "Comprehension owned: each book grants +2 skill."
                : "Each book grants +1 skill (+2 with Comprehension).";

        public Fallout3SkillBooksWindow(Fallout3Character character)
        {
            InitializeComponent();
            _character = character;
            DataContext = this;
            Reload();
        }

        private void Reload()
        {
            Books.Clear();
            foreach (var book in Fallout3SkillBookRepository.BookTypes)
                Books.Add(new SkillBookRow(_character, book));
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

        private void Checklist_Click(object sender, RoutedEventArgs e)
        {
            var checklist = new Fallout3SkillBookChecklistWindow(_character) { Owner = this };
            checklist.ShowDialog();
            Reload();
        }

        private void Back_Click(object sender, RoutedEventArgs e) => Close();

        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class SkillBookRow : INotifyPropertyChanged
    {
        private readonly Fallout3Character _character;
        private readonly Fallout3SkillBookType _book;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name => _book.Name;
        public string SkillLabel => _book.SkillDisplayName;
        public string CountDisplay => _character.GetSkillBookCount(_book.Name).ToString();
        public string MaxDisplay => _book.MaxCount < 0 ? "/ ∞" : $"/ {_book.MaxCount}";
        public bool CanIncrease =>
            _book.MaxCount < 0 || _character.GetSkillBookCount(_book.Name) < _book.MaxCount;

        public SkillBookRow(Fallout3Character character, Fallout3SkillBookType book)
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
}
