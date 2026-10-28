using FalloutPlanner.Games.Fallout3.Equipment;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace FalloutPlanner.Games.Fallout3
{
    public partial class Fallout3EquipmentPickerWindow : Window, INotifyPropertyChanged
    {
        private readonly Fallout3Character _character;
        private Fallout3EquipmentItem _selected;

        public event PropertyChangedEventHandler PropertyChanged;

        public EquipmentSlot Slot { get; }
        public string SlotTitle => $"Equip {Slot}";
        public ObservableCollection<EquipmentSection> Sections { get; } = new();
        public Fallout3EquipmentItem SelectedItem => _selected;
        public bool CanEquip => _selected != null;
        public bool ConfirmedEquip { get; private set; }
        public bool ConfirmedUnequip { get; private set; }

        public string SelectedName => _selected?.Name ?? "Select an item";
        public string SelectedDR => _selected?.DR.ToString() ?? "—";
        public string SelectedWeight
        {
            get
            {
                if (_selected == null)
                    return "—";
                if (_selected.WeightWithPowerArmorTraining.HasValue)
                    return $"{_selected.Weight} ({_selected.WeightWithPowerArmorTraining})";
                return _selected.Weight.ToString();
            }
        }
        public string SelectedEffects => _selected?.EffectsDisplay ?? "—";

        public Fallout3EquipmentPickerWindow(Fallout3Character character, EquipmentSlot slot)
        {
            InitializeComponent();
            _character = character;
            Slot = slot;
            DataContext = this;

            foreach (var group in Fallout3EquipmentRepository.CategoriesForSlot(slot))
            {
                Sections.Add(new EquipmentSection
                {
                    Title = group.First().CategoryDisplayName,
                    Items = group.OrderBy(i => i.Name).ToList()
                });
            }
        }

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button { Tag: Fallout3EquipmentItem item })
            {
                _selected = item;
                NotifySelection();
            }
        }

        private void Equip_Click(object sender, RoutedEventArgs e)
        {
            if (_selected == null)
                return;

            if (!TryResolveConflicts(_selected))
                return;

            ConfirmedEquip = true;
            DialogResult = true;
            Close();
        }

        private void Unequip_Click(object sender, RoutedEventArgs e)
        {
            ConfirmedUnequip = true;
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool TryResolveConflicts(Fallout3EquipmentItem item)
        {
            var eye = Fallout3EquipmentRepository.Find(_character.EquippedEyewearName);
            var head = Fallout3EquipmentRepository.Find(_character.EquippedHeadwearName);
            var body = Fallout3EquipmentRepository.Find(_character.EquippedBodyName);

            var removals = new System.Collections.Generic.List<string>();

            if (item.Slot == EquipmentSlot.Headwear && item.BlocksEyewear && eye != null)
                removals.Add($"Eyewear ({eye.Name})");

            if (item.Slot == EquipmentSlot.Eyewear && head != null && head.BlocksEyewear)
                removals.Add($"Headwear ({head.Name})");

            if (item.Slot == EquipmentSlot.Body && item.BlocksHeadwear)
            {
                if (head != null)
                    removals.Add($"Headwear ({head.Name})");
                if (eye != null)
                    removals.Add($"Eyewear ({eye.Name})");
            }

            if ((item.Slot == EquipmentSlot.Headwear || item.Slot == EquipmentSlot.Eyewear) &&
                body != null && body.BlocksHeadwear)
            {
                removals.Add($"Body ({body.Name})");
            }

            if (removals.Count == 0)
                return true;

            string message =
                "Equipping this item will remove:\n\n" +
                string.Join("\n", removals.Select(r => "• " + r)) +
                "\n\nAccept to continue, or Cancel to keep your current gear.";

            var warn = new EquipmentConflictWindow(message) { Owner = this };
            return warn.ShowDialog() == true;
        }

        private void NotifySelection()
        {
            OnPropertyChanged(nameof(SelectedItem));
            OnPropertyChanged(nameof(CanEquip));
            OnPropertyChanged(nameof(SelectedName));
            OnPropertyChanged(nameof(SelectedDR));
            OnPropertyChanged(nameof(SelectedWeight));
            OnPropertyChanged(nameof(SelectedEffects));
        }

        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class EquipmentSection
    {
        public string Title { get; set; }
        public System.Collections.Generic.List<Fallout3EquipmentItem> Items { get; set; }
    }
}
