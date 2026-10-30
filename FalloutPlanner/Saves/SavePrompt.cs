using System.Windows;

namespace FalloutPlanner.Saves
{
    public enum SaveDecision
    {
        Cancel,
        UpdateExisting,
        CreateNew
    }

    public static class SavePrompt
    {
        public static SaveDecision AskUpdateOrNew(Window? owner)
        {
            var result = MessageBox.Show(
                owner,
                "You are continuing an existing save.\n\n" +
                "Click Yes to update that save.\n" +
                "Click No to create a new save.\n" +
                "Click Cancel to abort.",
                "Save Character",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            return result switch
            {
                MessageBoxResult.Yes => SaveDecision.UpdateExisting,
                MessageBoxResult.No => SaveDecision.CreateNew,
                _ => SaveDecision.Cancel
            };
        }

        public static string? AskSaveName(Window? owner, string defaultName, string? gameId = null)
        {
            var dialog = new SaveNameWindow(defaultName, gameId) { Owner = owner };
            return dialog.ShowDialog() == true ? dialog.SaveName : null;
        }
    }
}
