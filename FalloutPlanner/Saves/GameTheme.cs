using System;
using System.Windows;

namespace FalloutPlanner.Saves
{
    /// <summary>Applies a game's ResourceDictionary so dialogs match that section's palette.</summary>
    public static class GameTheme
    {
        public static void Apply(FrameworkElement element, string gameId)
        {
            var path = ResourcePath(gameId);
            if (path == null)
                return;

            var dict = new ResourceDictionary
            {
                Source = new Uri(path, UriKind.RelativeOrAbsolute)
            };
            element.Resources.MergedDictionaries.Insert(0, dict);
        }

        public static string? ResourcePath(string gameId) => gameId switch
        {
            GameIds.Fallout1 => "pack://application:,,,/Games/Fallout1/Fallout1Resources.xaml",
            GameIds.Fallout2 => "pack://application:,,,/Games/Fallout2/Fallout2Resources.xaml",
            GameIds.Fallout3 => "pack://application:,,,/Games/Fallout3/Fallout3Resources.xaml",
            GameIds.FalloutNV => "pack://application:,,,/Games/FalloutNV/FalloutNVResources.xaml",
            GameIds.Fallout4 => "pack://application:,,,/Games/Fallout4/Fallout4Resources.xaml",
            _ => null
        };
    }
}
