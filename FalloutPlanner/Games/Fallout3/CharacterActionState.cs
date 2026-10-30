using FalloutPlanner.Games.Fallout3.Perks;
using System.Collections.Generic;

namespace FalloutPlanner.Games.Fallout3
{
    /// <summary>Full level-page state snapshot for action-log undo.</summary>
    public class CharacterActionState
    {
        public Fallout3CharacterStats Stats { get; set; }
        public Dictionary<string, int> SkillIncreases { get; set; } = new();
        public Dictionary<string, int> TakenPerks { get; set; } = new();
        public Dictionary<string, QuestPerkState> QuestPerks { get; set; } = new();
        public HashSet<string> Bobbleheads { get; set; } = new();
        public Dictionary<string, int> SkillBookCounts { get; set; } = new();
        public HashSet<string> SkillBookLocations { get; set; } = new();
        public HashSet<string> TaggedSkillNames { get; set; } = new();
    }
}
