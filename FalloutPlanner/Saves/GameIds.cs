namespace FalloutPlanner.Saves
{
    public static class GameIds
    {
        public const string Fallout1 = "fallout1";
        public const string Fallout2 = "fallout2";
        public const string Fallout3 = "fallout3";
        public const string FalloutNV = "falloutnv";
        public const string Fallout4 = "fallout4";

        public static string DisplayName(string gameId) => gameId switch
        {
            Fallout1 => "Fallout 1",
            Fallout2 => "Fallout 2",
            Fallout3 => "Fallout 3",
            FalloutNV => "Fallout New Vegas",
            Fallout4 => "Fallout 4",
            _ => gameId
        };

        /// <summary>Short label safe for default export filenames (no spaces).</summary>
        public static string FileNameLabel(string gameId) => gameId switch
        {
            Fallout1 => "Fallout1",
            Fallout2 => "Fallout2",
            Fallout3 => "Fallout3",
            FalloutNV => "FalloutNV",
            Fallout4 => "Fallout4",
            _ => gameId
        };

        public static string FileNameLabelFromDisplayName(string gameDisplayName) => gameDisplayName switch
        {
            "Fallout 1" => "Fallout1",
            "Fallout 2" => "Fallout2",
            "Fallout 3" => "Fallout3",
            "Fallout New Vegas" => "FalloutNV",
            "Fallout 4" => "Fallout4",
            _ => gameDisplayName.Replace(" ", "")
        };
    }
}
