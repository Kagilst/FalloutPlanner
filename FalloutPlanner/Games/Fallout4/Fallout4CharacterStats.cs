namespace FalloutPlanner.Games.Fallout4
{
    public class Fallout4CharacterStats
    {
        public string Name { get; set; } = "";
        public int Level { get; set; } = 1;

        // Creation pool (21 points; each SPECIAL starts at 1)
        public int Points { get; set; } = 21;

        // Unspent perk points from leveling
        public int PerkPoints { get; set; }

        public int Strength { get; set; } = 1;
        public int Perception { get; set; } = 1;
        public int Endurance { get; set; } = 1;
        public int Charisma { get; set; } = 1;
        public int Intelligence { get; set; } = 1;
        public int Agility { get; set; } = 1;
        public int Luck { get; set; } = 1;

        public int HitPoints { get; set; }
        public int ActionPoints { get; set; }
        public int CarryWeight { get; set; }

        public Fallout4CharacterStats Clone()
        {
            return (Fallout4CharacterStats)MemberwiseClone();
        }
    }
}
