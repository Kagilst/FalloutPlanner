using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FalloutPlanner.Games.Fallout1;
using FalloutPlanner.Games.Fallout1.Traits;

namespace FalloutPlanner;

public partial class Fallout1TraitsWindow : Window
{
    private readonly Fallout1Character _character;
    private readonly List<TraitDisplayItem> _traitItems;
    private readonly HashSet<string> _originalTraits;

    public Fallout1TraitsWindow(Fallout1Character character)
    {
        InitializeComponent();
        _character = character;
        _originalTraits = new HashSet<string>(character.Traits);

        _traitItems = Fallout1TraitRepository.All
            .Select(t => new TraitDisplayItem(t, character.HasTrait(t.Name)))
            .ToList();

        TraitsList.ItemsSource = _traitItems;
        UpdateSelectedCount();
    }

    private void TraitsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (TraitsList.SelectedItem is TraitDisplayItem item)
        {
            TraitTitle.Text = item.Name;
            TraitDescription.Text = item.Trait.Description;
        }
    }

    private void Trait_CheckChanged(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.Tag is string traitName)
        {
            var item = _traitItems.FirstOrDefault(t => t.Name == traitName);
            if (item == null) return;

            int selectedCount = _traitItems.Count(t => t.IsSelected);

            if (item.IsSelected && selectedCount > 2)
            {
                // Can't select more than 2
                item.IsSelected = false;
                checkBox.IsChecked = false;
            }
        }

        UpdateSelectedCount();
    }

    private void UpdateSelectedCount()
    {
        int count = _traitItems.Count(t => t.IsSelected);
        SelectedCount.Text = count.ToString();
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        // Restore original traits
        _character.Traits.Clear();
        foreach (var t in _originalTraits)
        {
            _character.Traits.Add(t);
        }
        _character.RecalculateDerivedStats();
        DialogResult = false;
        Close();
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
        // Apply selected traits
        var selectedTraits = _traitItems.Where(t => t.IsSelected).Select(t => t.Name).ToList();

        // Remove traits that are no longer selected
        var toRemove = _character.Traits.Where(t => !selectedTraits.Contains(t)).ToList();
        foreach (var t in toRemove)
        {
            _character.TryRemoveTrait(t);
        }

        // Add newly selected traits
        foreach (var t in selectedTraits)
        {
            if (!_character.HasTrait(t))
            {
                _character.TryAddTrait(t);
            }
        }

        DialogResult = true;
        Close();
    }
}

public class TraitDisplayItem : INotifyPropertyChanged
{
    public Fallout1Trait Trait { get; }
    public string Name => Trait.Name;

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }

    public TraitDisplayItem(Fallout1Trait trait, bool isSelected)
    {
        Trait = trait;
        _isSelected = isSelected;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
