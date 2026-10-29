using System.Collections.Generic;
using FalloutPlanner.Games.Fallout1.Perks;

namespace FalloutPlanner.Games.Fallout1
{
    /// <summary>Full level-page state snapshot for action-log undo.</summary>
    public class CharacterActionState
    {
        public Fallout1CharacterStats Stats { get; set; } = null!;
        public Dictionary<string, int> SkillIncreases { get; set; } = new();
        public Dictionary<string, int> TakenPerks { get; set; } = new();
        public Fallout1Perk? LastPerk { get; set; }
        public Dictionary<string, int> SkillBookCounts { get; set; } = new();
    }
}
