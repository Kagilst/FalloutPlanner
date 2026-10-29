using System.Collections.Generic;

namespace FalloutPlanner.Games.Fallout2
{
    public class CharacterActionState
    {
        public Fallout2CharacterStats Stats { get; set; }
        public Dictionary<string, int> SkillIncreases { get; set; } = new();
        public Dictionary<string, int> TakenPerks { get; set; } = new();
        public Dictionary<string, int> SkillBookCounts { get; set; } = new();
        /// <summary>Permanent % invested via skill points / books / perks (survives SPECIAL recalc).</summary>
        public Dictionary<string, int> SkillInvestment { get; set; } = new();
    }
}
