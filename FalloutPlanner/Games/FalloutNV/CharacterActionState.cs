using FalloutPlanner.Games.FalloutNV.Perks;
using System.Collections.Generic;

namespace FalloutPlanner.Games.FalloutNV
{
    /// <summary>Full level-page state snapshot for action-log undo.</summary>
    public class CharacterActionState
    {
        public FalloutNVCharacterStats Stats { get; set; }
        public Dictionary<string, int> SkillIncreases { get; set; } = new();
        public Dictionary<string, int> TakenPerks { get; set; } = new();
        public Dictionary<string, QuestPerkState> QuestPerks { get; set; } = new();
        public HashSet<string> Implants { get; set; } = new();
        public Dictionary<string, int> SkillBookCounts { get; set; } = new();
        public HashSet<string> SkillBookLocations { get; set; } = new();
        /// <summary>Maps crate number (1-4) to number of books found in that crate.</summary>
        public Dictionary<int, int> WorkbenchCrates { get; set; } = new();
    }
}
