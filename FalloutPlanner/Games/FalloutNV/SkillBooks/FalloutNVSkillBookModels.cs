namespace FalloutPlanner.Games.FalloutNV.SkillBooks
{
    public class FalloutNVSkillBookType
    {
        public string Name { get; init; } = "";
        public string SkillDisplayName { get; init; } = "";
        public string SkillPropertyName { get; init; } = "";
        /// <summary>-1 means unlimited. Max count based on FNV skill book totals.</summary>
        public int MaxCount { get; init; }
    }

    public class FalloutNVSkillBookLocation
    {
        public string Id { get; init; } = "";
        public string BookName { get; init; } = "";
        public string MapMarker { get; init; } = "";
        public string Description { get; init; } = "";
        /// <summary>Exclusive groups for mutually exclusive locations (not used in FNV base).</summary>
        public string? ExclusiveGroup { get; init; }
    }
}
