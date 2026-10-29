using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using FalloutPlanner.Games.Fallout4.Bobbleheads;

namespace FalloutPlanner.Games.Fallout4
{
    public partial class Fallout4BobbleheadsWindow : Window, INotifyPropertyChanged
    {
        private readonly Fallout4Character _character;
        private Fallout4Bobblehead? _selectedFound;
        private Fallout4Bobblehead? _selectedNotFound;
        private Fallout4Bobblehead? _selectedBobblehead;
        private bool _selecting;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Fallout4Bobblehead> FoundBobbleheads { get; } = new();
        public ObservableCollection<Fallout4Bobblehead> NotFoundBobbleheads { get; } = new();

        public bool ShowFoundEmpty => FoundBobbleheads.Count == 0;
        public bool ShowNotFoundEmpty => NotFoundBobbleheads.Count == 0;

        public Fallout4Bobblehead? SelectedFound
        {
            get => _selectedFound;
            set { if (_selectedFound != value) { _selectedFound = value; OnPropertyChanged(); } }
        }

        public Fallout4Bobblehead? SelectedNotFound
        {
            get => _selectedNotFound;
            set
            {
                if (_selectedNotFound == value) return;
                _selectedNotFound = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanAddSelected));
            }
        }

        public Fallout4Bobblehead? SelectedBobblehead
        {
            get => _selectedBobblehead;
            private set
            {
                if (_selectedBobblehead == value) return;
                _selectedBobblehead = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedEffect));
                OnPropertyChanged(nameof(SelectedLocation));
                OnPropertyChanged(nameof(CanAddSelected));
            }
        }

        public string SelectedEffect => SelectedBobblehead?.EffectDisplay ?? "";
        public string SelectedLocation => SelectedBobblehead?.Location ?? "";

        public bool CanAddSelected =>
            SelectedBobblehead != null &&
            SelectedNotFound == SelectedBobblehead &&
            !_character.IsBobbleheadFound(SelectedBobblehead.Name);

        public Fallout4BobbleheadsWindow(Fallout4Character character)
        {
            InitializeComponent();
            _character = character;
            DataContext = this;
            RefreshLists();
        }

        private void RefreshLists(string? preferSelectName = null)
        {
            FoundBobbleheads.Clear();
            NotFoundBobbleheads.Clear();

            foreach (var bobble in Fallout4BobbleheadRepository.All)
            {
                if (_character.IsBobbleheadFound(bobble.Name))
                    FoundBobbleheads.Add(bobble);
                else
                    NotFoundBobbleheads.Add(bobble);
            }

            OnPropertyChanged(nameof(ShowFoundEmpty));
            OnPropertyChanged(nameof(ShowNotFoundEmpty));

            Fallout4Bobblehead? toSelect = null;
            if (!string.IsNullOrEmpty(preferSelectName))
            {
                toSelect = FoundBobbleheads.FirstOrDefault(b => b.Name == preferSelectName)
                           ?? NotFoundBobbleheads.FirstOrDefault(b => b.Name == preferSelectName);
            }

            toSelect ??= FoundBobbleheads.FirstOrDefault()
                         ?? NotFoundBobbleheads.FirstOrDefault();

            if (toSelect == null)
            {
                SelectedFound = null;
                SelectedNotFound = null;
                SelectedBobblehead = null;
                return;
            }

            if (FoundBobbleheads.Contains(toSelect))
            {
                SelectedNotFound = null;
                SelectedFound = toSelect;
                SelectedBobblehead = toSelect;
            }
            else
            {
                SelectedFound = null;
                SelectedNotFound = toSelect;
                SelectedBobblehead = toSelect;
            }
        }

        private void FoundList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selecting || SelectedFound == null) return;
            _selecting = true;
            SelectedNotFound = null;
            SelectedBobblehead = SelectedFound;
            _selecting = false;
        }

        private void NotFoundList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selecting || SelectedNotFound == null) return;
            _selecting = true;
            SelectedFound = null;
            SelectedBobblehead = SelectedNotFound;
            _selecting = false;
        }

        private void AddToFound_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedNotFound == null) return;
            var name = SelectedNotFound.Name;
            if (_character.TryCollectBobblehead(SelectedNotFound))
                RefreshLists(name);
        }

        private void Back_Click(object sender, RoutedEventArgs e) => Close();

        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
