using FalloutPlanner.Games.FalloutNV.Implants;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace FalloutPlanner.Games.FalloutNV
{
    public partial class FalloutNVImplantsWindow : Window, INotifyPropertyChanged
    {
        private readonly FalloutNVCharacter _character;
        private ImplantChoice _selectedImplant;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ImplantChoice> DefaultImplants { get; } = new();
        public ObservableCollection<ImplantChoice> OWBImplants { get; } = new();

        public string ImplantSlotsText =>
            $"Default Implant Slots: {_character.UsedImplantSlots} / {_character.ImplantSlots} (based on Endurance)";

        public ImplantChoice SelectedImplant
        {
            get => _selectedImplant;
            set
            {
                if (_selectedImplant != value)
                {
                    _selectedImplant = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanInstallSelected));
                    OnPropertyChanged(nameof(SelectedStatusText));
                }
            }
        }

        public bool CanInstallSelected =>
            SelectedImplant != null &&
            !SelectedImplant.IsInstalled &&
            (SelectedImplant.Implant.Category == ImplantCategory.OldWorldBlues || _character.CanInstallDefaultImplant);

        public string SelectedStatusText
        {
            get
            {
                if (SelectedImplant == null)
                    return "";
                if (SelectedImplant.IsInstalled)
                    return "This implant is already installed.";
                if (SelectedImplant.Implant.Category == ImplantCategory.Default && !_character.CanInstallDefaultImplant)
                    return "No available implant slots. Increase Endurance for more slots.";
                return "You can install this implant.";
            }
        }

        public FalloutNVImplantsWindow(FalloutNVCharacter character)
        {
            InitializeComponent();
            _character = character;
            DataContext = this;
            RefreshLists();
        }

        private void RefreshLists()
        {
            DefaultImplants.Clear();
            OWBImplants.Clear();

            foreach (var implant in FalloutNVImplantRepository.DefaultImplants)
            {
                DefaultImplants.Add(new ImplantChoice(implant, _character.IsImplantInstalled(implant.Name)));
            }

            foreach (var implant in FalloutNVImplantRepository.OWBImplants)
            {
                OWBImplants.Add(new ImplantChoice(implant, _character.IsImplantInstalled(implant.Name)));
            }

            OnPropertyChanged(nameof(ImplantSlotsText));
            OnPropertyChanged(nameof(CanInstallSelected));
            OnPropertyChanged(nameof(SelectedStatusText));
        }

        private void Install_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedImplant == null || SelectedImplant.IsInstalled)
                return;

            // Capture before RefreshLists — clearing the ListBox nulls SelectedImplant via binding.
            var implant = SelectedImplant.Implant;
            var name = implant.Name;

            if (_character.TryInstallImplant(implant))
            {
                RefreshLists();
                SelectedImplant = DefaultImplants.FirstOrDefault(i => i.Implant.Name == name)
                                  ?? OWBImplants.FirstOrDefault(i => i.Implant.Name == name);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class ImplantChoice : INotifyPropertyChanged
    {
        public FalloutNVImplant Implant { get; }
        public bool IsInstalled { get; }
        public double ListOpacity => IsInstalled ? 0.6 : 1.0;

        public ImplantChoice(FalloutNVImplant implant, bool isInstalled)
        {
            Implant = implant;
            IsInstalled = isInstalled;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
