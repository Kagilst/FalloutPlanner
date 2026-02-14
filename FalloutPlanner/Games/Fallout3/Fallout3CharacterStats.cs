using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutPlanner.Games.Fallout3
{
    public class Fallout3CharacterStats
    {
        // SPECIAL
        public int Strength { get; set; }
        public int Perception { get; set; }
        public int Endurance { get; set; }
        public int Charisma { get; set; }
        public int Intelligence { get; set; }
        public int Agility { get; set; }
        public int Luck { get; set; }
        public int Points { get; set; }

        // Misc Stats
        public int Level { get; set; }
        public int ActionPoints { get; set; }
        public int CarryWeight { get; set; }
        public double CriticalChance { get; set; }
        public double DamageResist { get; set; }
        public int FireResist { get; set; }
        public int PoisonResist { get; set; }
        public int RadiationResist { get; set; }
        public int Health { get; set; }
        public double MeleeDamage { get; set; }
        public int UnarmedDamage { get; set; }
        public double SkillPoints { get; set; }
        public int TaggedSkills { get; set; }

        // Skills
        public int Barter { get; set; }
        public int BigGuns { get; set; }
        public int EnergyWeapons { get; set; }
        public int Explosives { get; set; }
        public int Lockpick { get; set; }
        public int Medicine { get; set; }
        public int MeleeWeapons { get; set; }
        public int Repair { get; set; }
        public int Science { get; set; }
        public int SmallGuns { get; set; }
        public int Sneak { get; set; }
        public int Speech { get; set; }
        public int Unarmed { get; set; }

        public Fallout3CharacterStats Clone()
        {
            return (Fallout3CharacterStats)this.MemberwiseClone();
        }
    }
}
