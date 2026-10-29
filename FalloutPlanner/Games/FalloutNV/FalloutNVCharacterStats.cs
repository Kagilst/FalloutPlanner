using System;
using System.Collections.Generic;

namespace FalloutPlanner.Games.FalloutNV
{
    public class FalloutNVCharacterStats
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

        // Character Info
        public string CharacterName { get; set; } = "";
        public int Level { get; set; }
        public bool IsMale { get; set; } = true;

        // Traits (0-2 selected at creation)
        public List<string> Traits { get; set; } = new();

        // Derived Stats
        public int ActionPoints { get; set; }
        public int Health { get; set; }
        public int CarryWeight { get; set; }
        public double CriticalChance { get; set; }
        public int DamageResist { get; set; }
        public int DamageThreshold { get; set; }
        public int FireResist { get; set; }
        public int EnergyResist { get; set; }
        public int PoisonResist { get; set; }
        public int RadiationResist { get; set; }
        public double MeleeDamage { get; set; }
        public double UnarmedDamage { get; set; }
        public int CompanionNerve { get; set; }
        public int SkillPoints { get; set; }
        public int TaggedSkills { get; set; }

        // Skills (13 for FNV - NO BigGuns/SmallGuns)
        public int Barter { get; set; }
        public int EnergyWeapons { get; set; }
        public int Explosives { get; set; }
        public int Guns { get; set; }
        public int Lockpick { get; set; }
        public int Medicine { get; set; }
        public int MeleeWeapons { get; set; }
        public int Repair { get; set; }
        public int Science { get; set; }
        public int Sneak { get; set; }
        public int Speech { get; set; }
        public int Survival { get; set; }
        public int Unarmed { get; set; }

        public List<string> Perks { get; set; } = new();

        public FalloutNVCharacterStats Clone()
        {
            var clone = (FalloutNVCharacterStats)MemberwiseClone();
            clone.Traits = new List<string>(Traits);
            clone.Perks = new List<string>(Perks);
            return clone;
        }
    }
}
