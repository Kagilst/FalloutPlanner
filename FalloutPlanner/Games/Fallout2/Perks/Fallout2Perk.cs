using System;
using System.Collections.Generic;

namespace FalloutPlanner.Games.Fallout2.Perks
{
    public class Fallout2Perk
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int RequiredLevel { get; set; }
        public int RequiredStrength { get; set; }
        public int RequiredPerception { get; set; }
        public int RequiredEndurance { get; set; }
        public int RequiredCharisma { get; set; }
        public int RequiredIntelligence { get; set; }
        public int RequiredAgility { get; set; }
        public int RequiredLuck { get; set; }

        // FO2 Skills
        public int RequiredSmallGuns { get; set; }
        public int RequiredBigGuns { get; set; }
        public int RequiredEnergyWeapons { get; set; }
        public int RequiredUnarmed { get; set; }
        public int RequiredMeleeWeapons { get; set; }
        public int RequiredThrowing { get; set; }
        public int RequiredFirstAid { get; set; }
        public int RequiredDoctor { get; set; }
        public int RequiredSneak { get; set; }
        public int RequiredLockpick { get; set; }
        public int RequiredSteal { get; set; }
        public int RequiredTraps { get; set; }
        public int RequiredScience { get; set; }
        public int RequiredRepair { get; set; }
        public int RequiredSpeech { get; set; }
        public int RequiredBarter { get; set; }
        public int RequiredGambling { get; set; }
        public int RequiredOutdoorsman { get; set; }

        public int MaxRank { get; set; } = 1;

        public Func<Fallout2Character, bool> Requirement { get; set; }

        public Action<Fallout2Character> ApplyEffect { get; set; }

        public string GetRequirementsDisplayText()
        {
            var parts = new List<string>();

            if (RequiredLevel > 0)
                parts.Add($"Level {RequiredLevel}");

            void AddSpecial(string label, int value)
            {
                if (value > 0)
                    parts.Add($"{label} {value}");
            }

            AddSpecial("ST", RequiredStrength);
            AddSpecial("PE", RequiredPerception);
            AddSpecial("EN", RequiredEndurance);
            AddSpecial("CH", RequiredCharisma);
            AddSpecial("IN", RequiredIntelligence);
            AddSpecial("AG", RequiredAgility);
            AddSpecial("LK", RequiredLuck);

            void AddSkill(string label, int value)
            {
                if (value > 0)
                    parts.Add($"{label} {value}%");
            }

            AddSkill("Small Guns", RequiredSmallGuns);
            AddSkill("Big Guns", RequiredBigGuns);
            AddSkill("Energy Weapons", RequiredEnergyWeapons);
            AddSkill("Unarmed", RequiredUnarmed);
            AddSkill("Melee Weapons", RequiredMeleeWeapons);
            AddSkill("Throwing", RequiredThrowing);
            AddSkill("First Aid", RequiredFirstAid);
            AddSkill("Doctor", RequiredDoctor);
            AddSkill("Sneak", RequiredSneak);
            AddSkill("Lockpick", RequiredLockpick);
            AddSkill("Steal", RequiredSteal);
            AddSkill("Traps", RequiredTraps);
            AddSkill("Science", RequiredScience);
            AddSkill("Repair", RequiredRepair);
            AddSkill("Speech", RequiredSpeech);
            AddSkill("Barter", RequiredBarter);
            AddSkill("Gambling", RequiredGambling);
            AddSkill("Outdoorsman", RequiredOutdoorsman);

            return parts.Count == 0 ? "None" : string.Join(", ", parts);
        }

        public bool MeetsStatRequirements(Fallout2Character c, int? levelOverride = null)
        {
            int level = levelOverride ?? c.Level;
            if (level < RequiredLevel) return false;
            if (RequiredStrength > 0 && c.Strength < RequiredStrength) return false;
            if (RequiredPerception > 0 && c.Perception < RequiredPerception) return false;
            if (RequiredEndurance > 0 && c.Endurance < RequiredEndurance) return false;
            if (RequiredCharisma > 0 && c.Charisma < RequiredCharisma) return false;
            if (RequiredIntelligence > 0 && c.Intelligence < RequiredIntelligence) return false;
            if (RequiredAgility > 0 && c.Agility < RequiredAgility) return false;
            if (RequiredLuck > 0 && c.Luck < RequiredLuck) return false;

            // FO2 Skills
            if (RequiredSmallGuns > 0 && c.SmallGuns < RequiredSmallGuns) return false;
            if (RequiredBigGuns > 0 && c.BigGuns < RequiredBigGuns) return false;
            if (RequiredEnergyWeapons > 0 && c.EnergyWeapons < RequiredEnergyWeapons) return false;
            if (RequiredUnarmed > 0 && c.Unarmed < RequiredUnarmed) return false;
            if (RequiredMeleeWeapons > 0 && c.MeleeWeapons < RequiredMeleeWeapons) return false;
            if (RequiredThrowing > 0 && c.Throwing < RequiredThrowing) return false;
            if (RequiredFirstAid > 0 && c.FirstAid < RequiredFirstAid) return false;
            if (RequiredDoctor > 0 && c.Doctor < RequiredDoctor) return false;
            if (RequiredSneak > 0 && c.Sneak < RequiredSneak) return false;
            if (RequiredLockpick > 0 && c.Lockpick < RequiredLockpick) return false;
            if (RequiredSteal > 0 && c.Steal < RequiredSteal) return false;
            if (RequiredTraps > 0 && c.Traps < RequiredTraps) return false;
            if (RequiredScience > 0 && c.Science < RequiredScience) return false;
            if (RequiredRepair > 0 && c.Repair < RequiredRepair) return false;
            if (RequiredSpeech > 0 && c.Speech < RequiredSpeech) return false;
            if (RequiredBarter > 0 && c.Barter < RequiredBarter) return false;
            if (RequiredGambling > 0 && c.Gambling < RequiredGambling) return false;
            if (RequiredOutdoorsman > 0 && c.Outdoorsman < RequiredOutdoorsman) return false;

            if (Requirement != null && !Requirement(c))
                return false;

            return true;
        }
    }
}
