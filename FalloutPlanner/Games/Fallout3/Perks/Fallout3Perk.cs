using System;
using System.Collections.Generic;
using System.Linq;

namespace FalloutPlanner.Games.Fallout3.Perks
{
    public class Fallout3Perk
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
        public int RequiredScience { get; set; }
        public int RequiredExplosives { get; set; }
        public int RequiredSneak { get; set; }
        public int RequiredMedicine { get; set; }
        public int RequiredLockpick { get; set; }
        public int RequiredSmallGuns { get; set; }
        public int RequiredEnergyWeapons { get; set; }
        public int RequiredUnarmed { get; set; }
        public int RequiredMeleeWeapons { get; set; }
        public int RequiredBarter { get; set; }
        public int RequiredBigGuns { get; set; }
        public int RequiredRepair { get; set; }
        public int RequiredSpeech { get; set; }

        /// <summary>
        /// Informational only (karma is not tracked). Shown in requirements text.
        /// </summary>
        public string KarmaRequirementNote { get; set; }

        public int MaxRank { get; set; } = 1;

        public Func<Fallout3Character, bool> Requirement { get; set; }

        public Action<Fallout3Character> ApplyEffect { get; set; }

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
                    parts.Add($"{label} {value}");
            }

            AddSkill("Barter", RequiredBarter);
            AddSkill("Big Guns", RequiredBigGuns);
            AddSkill("Energy Weapons", RequiredEnergyWeapons);
            AddSkill("Explosives", RequiredExplosives);
            AddSkill("Lockpick", RequiredLockpick);
            AddSkill("Medicine", RequiredMedicine);
            AddSkill("Melee Weapons", RequiredMeleeWeapons);
            AddSkill("Repair", RequiredRepair);
            AddSkill("Science", RequiredScience);
            AddSkill("Small Guns", RequiredSmallGuns);
            AddSkill("Sneak", RequiredSneak);
            AddSkill("Speech", RequiredSpeech);
            AddSkill("Unarmed", RequiredUnarmed);

            if (!string.IsNullOrWhiteSpace(KarmaRequirementNote))
                parts.Add(KarmaRequirementNote);

            return parts.Count == 0 ? "None" : string.Join(", ", parts);
        }

        public bool MeetsStatRequirements(Fallout3Character c, int? levelOverride = null)
        {
            // During level-up, requirements use the level being leveled into
            int level = levelOverride ?? c.Level;
            if (level < RequiredLevel) return false;
            if (RequiredStrength > 0 && c.Strength < RequiredStrength) return false;
            if (RequiredPerception > 0 && c.Perception < RequiredPerception) return false;
            if (RequiredEndurance > 0 && c.Endurance < RequiredEndurance) return false;
            if (RequiredCharisma > 0 && c.Charisma < RequiredCharisma) return false;
            if (RequiredIntelligence > 0 && c.Intelligence < RequiredIntelligence) return false;
            if (RequiredAgility > 0 && c.Agility < RequiredAgility) return false;
            if (RequiredLuck > 0 && c.Luck < RequiredLuck) return false;

            if (RequiredScience > 0 && c.Science < RequiredScience) return false;
            if (RequiredExplosives > 0 && c.Explosives < RequiredExplosives) return false;
            if (RequiredSneak > 0 && c.Sneak < RequiredSneak) return false;
            if (RequiredMedicine > 0 && c.Medicine < RequiredMedicine) return false;
            if (RequiredLockpick > 0 && c.Lockpick < RequiredLockpick) return false;
            if (RequiredSmallGuns > 0 && c.SmallGuns < RequiredSmallGuns) return false;
            if (RequiredEnergyWeapons > 0 && c.EnergyWeapons < RequiredEnergyWeapons) return false;
            if (RequiredUnarmed > 0 && c.Unarmed < RequiredUnarmed) return false;
            if (RequiredMeleeWeapons > 0 && c.MeleeWeapons < RequiredMeleeWeapons) return false;
            if (RequiredBarter > 0 && c.Barter < RequiredBarter) return false;
            if (RequiredBigGuns > 0 && c.BigGuns < RequiredBigGuns) return false;
            if (RequiredRepair > 0 && c.Repair < RequiredRepair) return false;
            if (RequiredSpeech > 0 && c.Speech < RequiredSpeech) return false;

            // Custom requirement (if set) — karma notes are NOT enforced here
            if (Requirement != null && !Requirement(c))
                return false;

            return true;
        }
    }
}
