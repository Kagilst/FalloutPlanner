using System;
using System.Collections.Generic;

namespace FalloutPlanner.Games.Fallout1
{
    public class Fallout1CharacterStats
    {
        // SPECIAL (start all at 5, 5 Char Points to distribute)
        public int Strength { get; set; } = 5;
        public int Perception { get; set; } = 5;
        public int Endurance { get; set; } = 5;
        public int Charisma { get; set; } = 5;
        public int Intelligence { get; set; } = 5;
        public int Agility { get; set; } = 5;
        public int Luck { get; set; } = 5;
        public int CharPoints { get; set; } = 5;

        // Character Info
        public string CharacterName { get; set; } = "";
        public int Level { get; set; } = 1;
        public bool IsMale { get; set; } = true;
        public int Age { get; set; } = 25;

        // Traits (0-2 selected at creation)
        public List<string> Traits { get; set; } = new();

        // Tagged Skills (3)
        public HashSet<string> TaggedSkills { get; set; } = new();

        // Derived Stats
        public int HitPoints { get; set; }
        public int ArmorClass { get; set; }
        public int ActionPoints { get; set; }
        public int CarryWeight { get; set; }
        public int MeleeDamage { get; set; }
        public int DamageResistance { get; set; }
        public int PoisonResistance { get; set; }
        public int RadiationResistance { get; set; }
        public int Sequence { get; set; }
        public int HealingRate { get; set; }
        public int CriticalChance { get; set; }
        public int SkillPoints { get; set; }

        // Skills (18 for FO1)
        public int SmallGuns { get; set; }
        public int BigGuns { get; set; }
        public int EnergyWeapons { get; set; }
        public int Unarmed { get; set; }
        public int MeleeWeapons { get; set; }
        public int Throwing { get; set; }
        public int FirstAid { get; set; }
        public int Doctor { get; set; }
        public int Sneak { get; set; }
        public int Lockpick { get; set; }
        public int Steal { get; set; }
        public int Traps { get; set; }
        public int Science { get; set; }
        public int Repair { get; set; }
        public int Speech { get; set; }
        public int Barter { get; set; }
        public int Gambling { get; set; }
        public int Outdoorsman { get; set; }

        public Fallout1CharacterStats Clone()
        {
            var clone = (Fallout1CharacterStats)MemberwiseClone();
            clone.Traits = new List<string>(Traits);
            clone.TaggedSkills = new HashSet<string>(TaggedSkills);
            return clone;
        }
    }
}
