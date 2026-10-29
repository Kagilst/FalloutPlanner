using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FalloutPlanner.Games.Fallout1.Perks;

namespace FalloutPlanner.Games.Fallout1;

public partial class Fallout1PerkWindow : Window
{
    private readonly Fallout1Character _character;
    private readonly List<Fallout1Perk> _availablePerks;

    public Fallout1Perk? SelectedPerk { get; private set; }

    public Fallout1PerkWindow(Fallout1Character character)
    {
        InitializeComponent();
        _character = character;

        // Filter to perks that meet requirements at next level
        _availablePerks = Fallout1PerkRepository.AllPerks
            .Where(p => _character.CanTakePerkOnLevelUp(p))
            .OrderBy(p => p.RequiredLevel)
            .ThenBy(p => p.Name)
            .ToList();

        PerksList.ItemsSource = _availablePerks;
    }

    private void PerksList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (PerksList.SelectedItem is Fallout1Perk perk)
        {
            PerkTitle.Text = perk.Name;
            PerkRequirements.Text = $"Requirements: {perk.GetRequirementsDisplayText()}";
            PerkDescription.Text = perk.Description;

            if (perk.MaxRank > 1)
            {
                int current = _character.GetPerkRank(perk.Name);
                PerkTitle.Text = $"{perk.Name} (Rank {current + 1}/{perk.MaxRank})";
            }

            SelectButton.IsEnabled = true;
        }
        else
        {
            SelectButton.IsEnabled = false;
        }
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void Select_Click(object sender, RoutedEventArgs e)
    {
        if (PerksList.SelectedItem is Fallout1Perk perk)
        {
            SelectedPerk = perk;
            DialogResult = true;
            Close();
        }
    }
}
