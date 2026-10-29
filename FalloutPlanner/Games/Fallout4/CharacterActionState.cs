using System.Collections.Generic;

namespace FalloutPlanner.Games.Fallout4
{
    public class CharacterActionState
    {
        public Fallout4CharacterStats Stats { get; set; }
        public Dictionary<string, int> PerkRanks { get; set; } = new();
        public HashSet<string> FoundBobbleheads { get; set; } = new();
        /// <summary>Stacking magazine series name → count.</summary>
        public Dictionary<string, int> MagazineStackCounts { get; set; } = new();
        /// <summary>Collected unique magazine issue IDs.</summary>
        public HashSet<string> CollectedMagazineIssues { get; set; } = new();
        /// <summary>You're SPECIAL! collected.</summary>
        public bool YoureSpecialCollected { get; set; }
        public string? YoureSpecialChoice { get; set; }
        /// <summary>Companion / quest misc perk ranks.</summary>
        public Dictionary<string, int> MiscPerkRanks { get; set; } = new();
    }
}
