using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using FalloutPlanner.Games.Fallout4.Perks;
using FalloutPlanner.Saves;

namespace FalloutPlanner.Games.Fallout4
{
    public partial class Fallout4LevelPage : Page
    {
        public Fallout4Character Character { get; }
        private Fallout4SpecialAttribute _filter = Fallout4SpecialAttribute.Strength;
        private readonly ObservableCollection<PerkListItem> _perkItems = new();

        public Fallout4LevelPage(Fallout4Character character)
        {
            InitializeComponent();
            Character = character;
            DataContext = character;
            Character.PropertyChanged += Character_PropertyChanged;
            RefreshPerkList();
            UpdateAttributeFilterHighlight();
        }

        private void Character_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is null
                or nameof(Fallout4Character.PerkPoints)
                or nameof(Fallout4Character.Level)
                or nameof(Fallout4Character.Strength)
                or nameof(Fallout4Character.Perception)
                or nameof(Fallout4Character.Endurance)
                or nameof(Fallout4Character.Charisma)
                or nameof(Fallout4Character.Intelligence)
                or nameof(Fallout4Character.Agility)
                or nameof(Fallout4Character.Luck))
            {
                RefreshPerkListAvailability();
                UpdateSelectedPerkDetails();
            }
        }

        private void AttributeFilter_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.Tag is null)
                return;

            if (Enum.TryParse<Fallout4SpecialAttribute>(btn.Tag.ToString(), out var attr))
            {
                _filter = attr;
                UpdateAttributeFilterHighlight();
                RefreshPerkList();
            }
        }

        private void UpdateAttributeFilterHighlight()
        {
            foreach (var child in AttributeFilterBar.Children)
            {
                if (child is not Button btn)
                    continue;

                bool selected = btn.Tag?.ToString() == _filter.ToString();
                if (selected)
                {
                    btn.Background = (Brush)FindResource("FalloutAccentBrush");
                    btn.Foreground = Brushes.Black;
                }
                else
                {
                    btn.ClearValue(Control.BackgroundProperty);
                    btn.ClearValue(Control.ForegroundProperty);
                }
            }
        }

        private void Settings_Restart(object sender, RoutedEventArgs e)
        {
            Character.RevertToCreationState();
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
        }

        private void Settings_Home(object sender, RoutedEventArgs e) =>
            NavigationService?.Navigate(new FalloutPlanner.HomePage());

        private void Settings_Save(object sender, RoutedEventArgs e)
        {
            CharacterSaveService.TrySaveFallout4(Character, Window.GetWindow(this));
        }

        private void Settings_ExportGuide(object sender, RoutedEventArgs e)
        {
            MarkdownGuideExporter.TryExportFallout4(Character, Window.GetWindow(this));
        }

        private void RefreshPerkList()
        {
            string? selectedName = (PerkList.SelectedItem as PerkListItem)?.Perk.Name;
            _perkItems.Clear();

            foreach (var perk in Fallout4PerkRepository.GetByAttribute(_filter))
                _perkItems.Add(CreatePerkItem(perk));

            PerkList.ItemsSource = _perkItems;

            var match = _perkItems.FirstOrDefault(p => p.Perk.Name == selectedName);
            PerkList.SelectedItem = match ?? _perkItems.FirstOrDefault();
            UpdateSelectedPerkDetails();
        }

        private void RefreshPerkListAvailability()
        {
            foreach (var item in _perkItems)
                item.Refresh(Character);
            UpdateSelectedPerkDetails();
        }

        private PerkListItem CreatePerkItem(Fallout4Perk perk)
        {
            var item = new PerkListItem(perk);
            item.Refresh(Character);
            return item;
        }

        private void PerkList_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            UpdateSelectedPerkDetails();

        private void UpdateSelectedPerkDetails()
        {
            if (PerkList.SelectedItem is not PerkListItem item)
            {
                SelectedPerkTitle.Text = "";
                SelectedPerkReqs.Text = "";
                SelectedPerkDesc.Text = "";
                TakePerkButton.IsEnabled = false;
                return;
            }

            int owned = Character.GetPerkRank(item.Perk.Name);
            SelectedPerkTitle.Text = $"{item.Perk.Name}  ({owned}/{item.Perk.MaxRank})";
            SelectedPerkReqs.Text = Character.GetPerkRequirementText(item.Perk);
            SelectedPerkDesc.Text = Character.GetNextPerkRankDescription(item.Perk);
            item.Refresh(Character);
            TakePerkButton.IsEnabled = item.CanTake;
        }

        private void TakePerk_Click(object sender, RoutedEventArgs e)
        {
            if (PerkList.SelectedItem is not PerkListItem item)
                return;

            if (Character.TryTakeNextPerkRank(item.Perk))
            {
                RefreshPerkListAvailability();
                UpdateSelectedPerkDetails();
            }
        }

        private void IncreaseSpecial_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.Tag is null)
                return;

            if (Character.TryIncreaseSpecialWithPerkPoint(btn.Tag.ToString()!))
                RefreshPerkListAvailability();
        }

        private void LevelUp_Click(object sender, RoutedEventArgs e)
        {
            Character.LevelUp();
            RefreshPerkListAvailability();
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (!Character.TryUndoLastAction())
            {
                Character.RevertToCreationState();
                if (NavigationService?.CanGoBack == true)
                    NavigationService.GoBack();
                return;
            }

            RefreshPerkList();
        }

        private void Equipment_Click(object sender, RoutedEventArgs e)
        {
            var window = new Fallout4EquipmentWindow(Character) { Owner = Window.GetWindow(this) };
            window.ShowDialog();
        }

        private void Bobbleheads_Click(object sender, RoutedEventArgs e)
        {
            var window = new Fallout4BobbleheadsWindow(Character) { Owner = Window.GetWindow(this) };
            window.ShowDialog();
            RefreshPerkListAvailability();
        }

        private void Magazines_Click(object sender, RoutedEventArgs e)
        {
            var window = new Fallout4MagazinesWindow(Character) { Owner = Window.GetWindow(this) };
            window.ShowDialog();
            RefreshPerkListAvailability();
        }

        private void Perks_Click(object sender, RoutedEventArgs e)
        {
            var window = new Fallout4OwnedPerksWindow(Character) { Owner = Window.GetWindow(this) };
            window.ShowDialog();
        }
    }

    public class PerkListItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public Fallout4Perk Perk { get; }
        public string DisplayName { get; private set; } = "";
        public bool CanTake { get; private set; }
        public bool MeetsAttributeRequirement { get; private set; }
        public double ListOpacity { get; private set; } = 1.0;

        public PerkListItem(Fallout4Perk perk)
        {
            Perk = perk;
        }

        public void Refresh(Fallout4Character character)
        {
            int owned = character.GetPerkRank(Perk.Name);
            DisplayName = $"{Perk.AttributeShort} {Perk.AttributeRequirement}  {Perk.Name}  [{owned}/{Perk.MaxRank}]";
            CanTake = character.CanTakeNextPerkRank(Perk);
            MeetsAttributeRequirement = character.GetSpecial(Perk.Attribute) >= Perk.AttributeRequirement;
            ListOpacity = MeetsAttributeRequirement ? 1.0 : 0.35;
            OnPropertyChanged(nameof(DisplayName));
            OnPropertyChanged(nameof(CanTake));
            OnPropertyChanged(nameof(MeetsAttributeRequirement));
            OnPropertyChanged(nameof(ListOpacity));
        }
    }
}
