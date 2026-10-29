namespace FalloutPlanner.Games.Fallout2.SkillBooks
{
    public class Fallout2SkillBookType
    {
        public string Name { get; init; }
        public string SkillName { get; init; }
        public string SkillPropertyName { get; init; }
        public int MaxCount { get; init; } = -1; // -1 means unlimited
    }
}
