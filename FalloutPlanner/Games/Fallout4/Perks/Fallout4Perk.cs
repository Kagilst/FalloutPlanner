using System.Collections.Generic;
using System.Linq;

namespace FalloutPlanner.Games.Fallout4.Perks
{
    public enum Fallout4SpecialAttribute
    {
        Strength,
        Perception,
        Endurance,
        Charisma,
        Intelligence,
        Agility,
        Luck
    }

    public class Fallout4PerkRank
    {
        public int Rank { get; set; }
        /// <summary>Minimum character level. Rank 1 has no level gate (0).</summary>
        public int LevelRequirement { get; set; }
        public string Description { get; set; } = "";
    }

    public class Fallout4Perk
    {
        public string Name { get; set; } = "";
        public Fallout4SpecialAttribute Attribute { get; set; }
        public int AttributeRequirement { get; set; }
        public List<Fallout4PerkRank> Ranks { get; set; } = new();

        public int MaxRank => Ranks.Count;

        public Fallout4PerkRank? GetRank(int rank) =>
            Ranks.FirstOrDefault(r => r.Rank == rank);

        public string AttributeShort => Attribute switch
        {
            Fallout4SpecialAttribute.Strength => "STR",
            Fallout4SpecialAttribute.Perception => "PER",
            Fallout4SpecialAttribute.Endurance => "END",
            Fallout4SpecialAttribute.Charisma => "CHR",
            Fallout4SpecialAttribute.Intelligence => "INT",
            Fallout4SpecialAttribute.Agility => "AGI",
            Fallout4SpecialAttribute.Luck => "LCK",
            _ => "?"
        };
    }
}
