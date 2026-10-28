using FalloutPlanner.Games.Fallout3.Perks;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace FalloutPlanner.Games.Fallout3
{
    public partial class SurvivalExpertWindow : Window, INotifyPropertyChanged
    {
        private Fallout3QuestPerkRepository.SurvivalRank _rank =
            Fallout3QuestPerkRepository.SurvivalRank.JuniorSurvivor;
        private Fallout3QuestPerkRepository.SurvivalVariant _variant =
            Fallout3QuestPerkRepository.SurvivalVariant.Normal;

        public event PropertyChangedEventHandler PropertyChanged;

        public Fallout3QuestPerkRepository.SurvivalRank SelectedRank => _rank;
        public Fallout3QuestPerkRepository.SurvivalVariant SelectedVariant => _variant;

        public string PreviewDescription =>
            Fallout3QuestPerkRepository.BuildSurvivalDescription(_rank, _variant);

        public SurvivalExpertWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Choice_Changed(object sender, RoutedEventArgs e)
        {
            if (sender is not RadioButton rb || rb.Tag is not string tag || rb.IsChecked != true)
                return;

            if (Enum.TryParse(tag, out Fallout3QuestPerkRepository.SurvivalRank rank))
                _rank = rank;
            else if (Enum.TryParse(tag, out Fallout3QuestPerkRepository.SurvivalVariant variant))
                _variant = variant;

            OnPropertyChanged(nameof(PreviewDescription));
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
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
}
