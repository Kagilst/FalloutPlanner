namespace FalloutPlanner.Games.Fallout4.Perks
{
    public class OwnedPerkInfo
    {
        public string Name { get; init; } = "";
        public string DisplayName { get; init; } = "";
        public int Rank { get; init; }
        public int MaxRank { get; init; }
        public string Requirements { get; init; } = "";
        public string Description { get; init; } = "";
        public bool IsMiscPerk { get; init; }
        public Fallout4MiscPerk? MiscPerk { get; init; }
    }
}
