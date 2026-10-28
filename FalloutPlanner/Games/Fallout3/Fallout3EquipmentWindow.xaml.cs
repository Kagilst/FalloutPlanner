using FalloutPlanner.Games.Fallout3.Equipment;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace FalloutPlanner.Games.Fallout3
{
    public partial class Fallout3EquipmentWindow : Window, INotifyPropertyChanged
    {
        private readonly Fallout3Character _character;
        private EquipmentDisplayStats _stats;

        public event PropertyChangedEventHandler PropertyChanged;

        public string HeadwearName => SlotName(_stats.Headwear);
        public string EyewearName => SlotName(_stats.Eyewear);
        public string BodyName => SlotName(_stats.Body);
        public string HeadwearDetails => SlotDetails(_stats.Headwear);
        public string EyewearDetails => SlotDetails(_stats.Eyewear);
        public string BodyDetails => SlotDetails(_stats.Body);

        public int Strength => _stats.Strength;
        public int Perception => _stats.Perception;
        public int Endurance => _stats.Endurance;
        public int Charisma => _stats.Charisma;
        public int Intelligence => _stats.Intelligence;
        public int Agility => _stats.Agility;
        public int Luck => _stats.Luck;

        public int Health => _stats.Health;
        public int ActionPoints => _stats.ActionPoints;
        public double CriticalChance => _stats.CriticalChance;
        public int DamageResist => _stats.DamageResist;
        public int FireResist => _stats.FireResist;
        public int PoisonResist => _stats.PoisonResist;
        public int RadiationResist => _stats.RadiationResist;
        public double MeleeDamage => _stats.MeleeDamage;
        public int CarryWeight => _stats.CarryWeight;
        public string MovementDebuff => $"{_stats.MovementDebuffPercent:0.##}%";
        public string LoudnessDebuff => $"{_stats.LoudnessDebuffPercent:0.##}%";

        public int Barter => _stats.Barter;
        public int BigGuns => _stats.BigGuns;
        public int EnergyWeapons => _stats.EnergyWeapons;
        public int Explosives => _stats.Explosives;
        public int Lockpick => _stats.Lockpick;
        public int Medicine => _stats.Medicine;
        public int MeleeWeapons => _stats.MeleeWeapons;
        public int Repair => _stats.Repair;
        public int Science => _stats.Science;
        public int SmallGuns => _stats.SmallGuns;
        public int Sneak => _stats.Sneak;
        public int Speech => _stats.Speech;
        public int Unarmed => _stats.Unarmed;

        public Fallout3EquipmentWindow(Fallout3Character character)
        {
            InitializeComponent();
            _character = character;
            _stats = new EquipmentDisplayStats(_character);
            DataContext = this;
        }

        private static string SlotName(Fallout3EquipmentItem item) =>
            item?.Name ?? "None";

        private string SlotDetails(Fallout3EquipmentItem item)
        {
            if (item == null)
                return "";
            double wt = item.GetEffectiveWeight(_stats.HasPowerArmorTraining);
            string wtText = item.WeightWithPowerArmorTraining.HasValue
                ? $"{item.Weight} ({item.WeightWithPowerArmorTraining})"
                : wt.ToString();
            return $"DR {item.DR}  |  Wt {wtText}\n{item.EffectsDisplay}";
        }

        private void EquipHeadwear_Click(object sender, RoutedEventArgs e) =>
            OpenPicker(EquipmentSlot.Headwear);

        private void EquipEyewear_Click(object sender, RoutedEventArgs e) =>
            OpenPicker(EquipmentSlot.Eyewear);

        private void EquipBody_Click(object sender, RoutedEventArgs e) =>
            OpenPicker(EquipmentSlot.Body);

        private void OpenPicker(EquipmentSlot slot)
        {
            var picker = new Fallout3EquipmentPickerWindow(_character, slot) { Owner = this };
            if (picker.ShowDialog() != true)
                return;

            if (picker.ConfirmedUnequip)
            {
                ClearSlot(slot);
            }
            else if (picker.ConfirmedEquip && picker.SelectedItem != null)
            {
                ApplyEquip(picker.SelectedItem);
            }

            Refresh();
        }

        private void ClearSlot(EquipmentSlot slot)
        {
            switch (slot)
            {
                case EquipmentSlot.Eyewear:
                    _character.EquippedEyewearName = null;
                    break;
                case EquipmentSlot.Headwear:
                    _character.EquippedHeadwearName = null;
                    break;
                case EquipmentSlot.Body:
                    _character.EquippedBodyName = null;
                    break;
            }
        }

        private void ApplyEquip(Fallout3EquipmentItem item)
        {
            var eye = Fallout3EquipmentRepository.Find(_character.EquippedEyewearName);
            var head = Fallout3EquipmentRepository.Find(_character.EquippedHeadwearName);
            var body = Fallout3EquipmentRepository.Find(_character.EquippedBodyName);

            if (item.Slot == EquipmentSlot.Headwear && item.BlocksEyewear)
                _character.EquippedEyewearName = null;

            if (item.Slot == EquipmentSlot.Eyewear && head != null && head.BlocksEyewear)
                _character.EquippedHeadwearName = null;

            if (item.Slot == EquipmentSlot.Body && item.BlocksHeadwear)
            {
                _character.EquippedHeadwearName = null;
                _character.EquippedEyewearName = null;
            }

            if ((item.Slot == EquipmentSlot.Headwear || item.Slot == EquipmentSlot.Eyewear) &&
                body != null && body.BlocksHeadwear)
            {
                _character.EquippedBodyName = null;
            }

            switch (item.Slot)
            {
                case EquipmentSlot.Eyewear:
                    _character.EquippedEyewearName = item.Name;
                    break;
                case EquipmentSlot.Headwear:
                    _character.EquippedHeadwearName = item.Name;
                    break;
                case EquipmentSlot.Body:
                    _character.EquippedBodyName = item.Name;
                    break;
            }
        }

        private void Refresh()
        {
            _stats = new EquipmentDisplayStats(_character);
            OnPropertyChanged(null);
        }

        private void Back_Click(object sender, RoutedEventArgs e) => Close();

        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
