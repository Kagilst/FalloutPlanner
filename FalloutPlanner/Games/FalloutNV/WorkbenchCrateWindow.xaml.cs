using FalloutPlanner.Games.FalloutNV.SkillBooks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace FalloutPlanner.Games.FalloutNV
{
    public partial class WorkbenchCrateWindow : Window, INotifyPropertyChanged
    {
        private readonly FalloutNVCharacter _character;
        private readonly int _crateNumber;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<WorkbenchBookChoice> BookChoices { get; } = new();

        public string CrateTitle => $"Workbench Crate {_crateNumber}";

        public int SelectedCount => BookChoices.Count(c => c.IsSelected);

        public string ComprehensionNote =>
            _character.GetPerkRank("Comprehension") > 0
                ? "Comprehension owned: each book grants +4 skill."
                : "Each book grants +3 skill (+4 with Comprehension).";

        /// <summary>
        /// Selected book property names (skill property names) to apply.
        /// </summary>
        public List<string> SelectedBooks { get; private set; } = new();

        public WorkbenchCrateWindow(FalloutNVCharacter character, int crateNumber)
        {
            InitializeComponent();
            _character = character;
            _crateNumber = crateNumber;
            DataContext = this;

            // Add all 13 skill book types as choices
            foreach (var bookType in FalloutNVSkillBookRepository.BookTypes)
            {
                // Skip the workbench crate "book type" if present
                if (bookType.Name == "Workbench Crate")
                    continue;

                BookChoices.Add(new WorkbenchBookChoice(bookType, this));
            }
        }

        private void Book_CheckChanged(object sender, RoutedEventArgs e)
        {
            foreach (var choice in BookChoices)
            {
                choice.RefreshCanSelect();
            }
            OnPropertyChanged(nameof(SelectedCount));
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            SelectedBooks = BookChoices
                .Where(c => c.IsSelected)
                .Select(c => c.BookType.SkillPropertyName)
                .ToList();

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class WorkbenchBookChoice : INotifyPropertyChanged
    {
        private readonly WorkbenchCrateWindow _window;
        private bool _isSelected;

        public FalloutNVSkillBookType BookType { get; }

        public string DisplayName => $"{BookType.Name} ({BookType.SkillDisplayName})";

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool CanSelect => IsSelected || _window.SelectedCount < 4;

        public WorkbenchBookChoice(FalloutNVSkillBookType bookType, WorkbenchCrateWindow window)
        {
            BookType = bookType;
            _window = window;
        }

        public void RefreshCanSelect()
        {
            OnPropertyChanged(nameof(CanSelect));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
