using System.Windows;
using System.Windows.Controls;

namespace FalloutPlanner.Games.Fallout1;

public partial class Fallout1SkillBooksWindow : Window
{
    private readonly Fallout1Character _character;

    public Fallout1SkillBooksWindow(Fallout1Character character)
    {
        InitializeComponent();
        _character = character;

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        int points = _character.SkillBookPointsPerBook;
        PointsPerBook.Text = points.ToString();
        ComprehensionNote.Text = points == 2 ? "Comprehension perk active" : "1 normally, 2 with Comprehension";

        GunsAndBulletsCount.Text = _character.GetSkillBookCount("Guns and Bullets").ToString();
        BigBookOfScienceCount.Text = _character.GetSkillBookCount("Big Book of Science").ToString();
        DeansElectronicsCount.Text = _character.GetSkillBookCount("Dean's Electronics").ToString();
        FirstAidBookCount.Text = _character.GetSkillBookCount("First Aid Book").ToString();
        ScoutHandbookCount.Text = _character.GetSkillBookCount("Scout Handbook").ToString();
    }

    private void IncreaseBook_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string bookName)
        {
            _character.TryAddSkillBook(bookName);
            UpdateDisplay();
        }
    }

    private void DecreaseBook_Click(object sender, RoutedEventArgs e)
    {
        // Skill books can't be "unread" - this button is just for UI consistency
        // but won't do anything since we don't support removing read books
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
