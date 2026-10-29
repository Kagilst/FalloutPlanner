using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FalloutPlanner.Games.Fallout1.Perks;

namespace FalloutPlanner.Games.Fallout1;

public partial class Fallout1OwnedPerksWindow : Window
{
    private readonly Fallout1Character _character;
    private readonly List<OwnedPerkDisplay> _ownedPerks;

    public Fallout1OwnedPerksWindow(Fallout1Character character)
    {
        InitializeComponent();
        _character = character;

        _ownedPerks = character.TakenPerks
            .Select(kvp =>
            {
                var perk = Fallout1PerkRepository.AllPerks.FirstOrDefault(p => p.Name == kvp.Key);
                return new OwnedPerkDisplay
                {
                    Name = kvp.Key,
                    Rank = kvp.Value,
                    DisplayName = kvp.Value > 1 ? $"{kvp.Key} (Rank {kvp.Value})" : kvp.Key,
                    Description = perk?.Description ?? ""
                };
            })
            .OrderBy(p => p.Name)
            .ToList();

        PerksList.ItemsSource = _ownedPerks.Select(p => p.DisplayName);

        if (_ownedPerks.Count == 0)
        {
            PerkDescription.Text = "No perks acquired yet.";
        }
    }

    private void PerksList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        int index = PerksList.SelectedIndex;
        if (index >= 0 && index < _ownedPerks.Count)
        {
            var perk = _ownedPerks[index];
            PerkTitle.Text = perk.DisplayName;
            PerkDescription.Text = perk.Description;
        }
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private class OwnedPerkDisplay
    {
        public string Name { get; set; } = "";
        public int Rank { get; set; }
        public string DisplayName { get; set; } = "";
        public string Description { get; set; } = "";
    }
}
