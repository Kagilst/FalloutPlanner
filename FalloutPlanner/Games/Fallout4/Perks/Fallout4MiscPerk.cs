namespace FalloutPlanner.Games.Fallout4.Perks
{
    public enum Fallout4MiscPerkKind
    {
        Companion,
        Quest
    }

    public class Fallout4MiscPerk
    {
        public string Name { get; init; } = "";
        public string Description { get; init; } = "";
        public Fallout4MiscPerkKind Kind { get; init; }
        public string? CompanionName { get; init; }
        public int MaxRank { get; init; } = 1;
        /// <summary>Shared exclusion group id; null if unrestricted.</summary>
        public string? ExclusionGroup { get; init; }
        /// <summary>Max perks allowed from this exclusion group (default 1).</summary>
        public int ExclusionGroupLimit { get; init; } = 1;

        public string ListDisplayName =>
            Kind == Fallout4MiscPerkKind.Companion && !string.IsNullOrEmpty(CompanionName)
                ? $"{Name} ({CompanionName})"
                : Name;

        public string CategoryLabel =>
            Kind == Fallout4MiscPerkKind.Companion ? "Companion Perk" : "Quest Perk";
    }
}
