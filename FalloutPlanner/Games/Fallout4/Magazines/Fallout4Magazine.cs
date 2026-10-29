using System.Collections.Generic;

namespace FalloutPlanner.Games.Fallout4.Magazines
{
    public class Fallout4MagazineIssue
    {
        public string Id { get; init; } = "";
        public string DisplayName { get; init; } = "";
        public string Effect { get; init; } = "";
        public string Location { get; init; } = "";
    }

    public class Fallout4MagazineSeries
    {
        public string Name { get; init; } = "";
        public string PerkName { get; init; } = "";
        public int MaxIssues { get; init; }
        public bool GrantsSpecialChoice { get; init; }

        /// <summary>Same bonus stacks with each copy (e.g. Guns and Bullets).</summary>
        public bool IsStacking { get; init; }

        /// <summary>Percent added per rank for stacking series (0 if not percentage-based).</summary>
        public int PercentPerRank { get; init; }

        /// <summary>Format string with {0} for total percent, e.g. "Ballistic weapons do +{0}% critical damage."</summary>
        public string? StackedEffectFormat { get; init; }

        /// <summary>Fallback description when PercentPerRank is 0.</summary>
        public string EffectDisplay { get; init; } = "";

        public List<Fallout4MagazineIssue> Issues { get; init; } = new();

        public bool HasUniqueIssues => !IsStacking && !GrantsSpecialChoice && Issues.Count > 0;
    }

    /// <summary>Entry shown in the collected magazines panel.</summary>
    public class CollectedMagazineEntry
    {
        public string Key { get; init; } = "";
        public string DisplayName { get; init; } = "";
        public string Description { get; init; } = "";
        public bool IsStacking { get; init; }
        public string SeriesName { get; init; } = "";
    }
}
