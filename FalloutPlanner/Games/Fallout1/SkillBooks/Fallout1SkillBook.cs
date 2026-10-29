namespace FalloutPlanner.Games.Fallout1.SkillBooks
{
    public class Fallout1SkillBook
    {
        public string Name { get; init; } = "";
        public string SkillName { get; init; } = "";
        public string SkillPropertyName { get; init; } = "";
        public int PointsPerBook { get; init; } = 1; // +1 (or +2 with Comprehension)
        public int MaxBooks { get; init; } = -1; // -1 means unlimited for planner purposes
        public string Description { get; init; } = "";
    }
}
