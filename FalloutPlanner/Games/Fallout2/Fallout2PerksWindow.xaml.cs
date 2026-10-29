using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using FalloutPlanner.Games.Fallout2.Perks;

namespace FalloutPlanner.Games.Fallout2
{
    public partial class Fallout2PerksWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly Fallout2Character _character;

        public List<OwnedPerkInfo> OwnedPerks { get; }

        private OwnedPerkInfo _selectedPerk;
        public OwnedPerkInfo SelectedPerk
        {
            get => _selectedPerk;
            set
            {
                _selectedPerk = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelection));
                OnPropertyChanged(nameof(ShowRanks));
                OnPropertyChanged(nameof(RanksText));
            }
        }

        public bool HasSelection => SelectedPerk != null;
        public bool ShowRanks => SelectedPerk?.Perk?.MaxRank > 1;
        public string RanksText => SelectedPerk != null
            ? $"{SelectedPerk.CurrentRank}/{SelectedPerk.Perk?.MaxRank ?? 1}"
            : "";

        public Fallout2PerksWindow(Fallout2Character character)
        {
            _character = character;

            OwnedPerks = character.TakenPerks
                .Select(kvp =>
                {
                    var perk = Fallout2PerkRepository.AllPerks.FirstOrDefault(p => p.Name == kvp.Key);
                    return new OwnedPerkInfo
                    {
                        Perk = perk,
                        CurrentRank = kvp.Value,
                        DisplayName = kvp.Value > 1 && (perk?.MaxRank ?? 1) > 1
                            ? $"{kvp.Key} (Rank {kvp.Value}/{perk.MaxRank})"
                            : kvp.Value > 1
                                ? $"{kvp.Key} (Rank {kvp.Value})"
                                : kvp.Key
                    };
                })
                .Where(info => info.Perk != null)
                .OrderBy(info => info.Perk.Name)
                .ToList();

            InitializeComponent();
            DataContext = this;

            if (OwnedPerks.Count > 0)
                SelectedPerk = OwnedPerks[0];
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class OwnedPerkInfo
    {
        public Fallout2Perk Perk { get; init; }
        public int CurrentRank { get; init; }
        public string DisplayName { get; init; }
    }
}
