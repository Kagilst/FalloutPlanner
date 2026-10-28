namespace FalloutPlanner.Games.Fallout3.SkillBooks
{
    public class Fallout3SkillBookType
    {
        public string Name { get; init; } = "";
        public string SkillDisplayName { get; init; } = "";
        public string SkillPropertyName { get; init; } = "";
        /// <summary>-1 means unlimited.</summary>
        public int MaxCount { get; init; }
    }

    public class Fallout3SkillBookLocation
    {
        public string Id { get; init; } = "";
        public string BookName { get; init; } = "";
        public string MapMarker { get; init; } = "";
        public string Description { get; init; } = "";
        /// <summary>HouseMegaton / HouseTenpenny — mutually exclusive groups.</summary>
        public string? ExclusiveGroup { get; init; }
    }
}
