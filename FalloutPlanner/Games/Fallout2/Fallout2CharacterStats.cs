using System;
using System.Collections.Generic;

namespace FalloutPlanner.Games.Fallout2
{
    public class Fallout2CharacterStats
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
        public int Age { get; set; } = 25;

        // Traits (0-2 selected at creation)
        public List<string> Traits { get; set; } = new();

        // Derived Stats
        public int ActionPoints { get; set; }
        public int HitPoints { get; set; }
        public int MaxHitPoints { get; set; }
        public int CarryWeight { get; set; }
        public int CriticalChance { get; set; }
        public int ArmorClass { get; set; }
        public int MeleeDamage { get; set; }
        public int DamageResist { get; set; }
        public int PoisonResist { get; set; }
        public int RadiationResist { get; set; }
        public int Sequence { get; set; }
        public int HealingRate { get; set; }
        public int PerkRate { get; set; }
        public int SkillPoints { get; set; }
        public int TaggedSkills { get; set; }

        // Skills (18 total for FO1/FO2)
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

        public List<string> Perks { get; set; } = new();

        public Fallout2CharacterStats Clone()
        {
            var clone = (Fallout2CharacterStats)MemberwiseClone();
            clone.Traits = new List<string>(Traits);
            clone.Perks = new List<string>(Perks);
            return clone;
        }
    }
}
