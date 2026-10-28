using System.Collections.Generic;

namespace FalloutPlanner.Games.Fallout3.Bobbleheads
{
    public static class Fallout3BobbleheadRepository
    {
        public static IReadOnlyList<Fallout3Bobblehead> All { get; } = new List<Fallout3Bobblehead>
        {
            Special("Strength", "+1 Strength",
                "Megaton: On a desk upstairs in Lucas Sims House."),
            Special("Perception", "+1 Perception",
                "Republic of Dave: On a shelf in the Museum of Dave."),
            Special("Endurance", "+1 Endurance",
                "Deathclaw Sanctuary: Down the ramp on an overturned barrel next to a brahmin corpse."),
            Special("Charisma", "+1 Charisma",
                "Vault 108: In the center of the cloning lab on a table."),
            Special("Intelligence", "+1 Intelligence",
                "Rivet City: In the science lab on a table."),
            Special("Agility", "+1 Agility",
                "Greener Pastures Disposal Site: On a desk inside the office."),
            Special("Luck", "+1 Luck",
                "Arlington Cemetery: Inside the basement of the lone house on the hill."),

            Skill("Barter", "+10 Barter",
                "Evergreen Mills: Inside the room with Smiling Jack."),
            Skill("Big Guns", "+10 Big Guns",
                "Fort Constantine: Inside the safe in the basement of the small house.", "BigGuns"),
            Skill("Energy Weapons", "+10 Energy Weapons",
                "Raven Rock: Inside Colonel Autumn's bedroom.", "EnergyWeapons"),
            Skill("Explosives", "+10 Explosives",
                "WKML Broadcast Station: In the sealed cistern near the station."),
            Skill("Lockpick", "+10 Lockpick",
                "Bethesda Ruins: On a desk in Bethesda Offices East."),
            Skill("Medicine", "+10 Medicine",
                "Vault 101: Found on a table in the medical area."),
            Skill("Melee Weapons", "+10 Melee Weapons",
                "Dunwich Building: Found on the floor heading to the exit at the end of the dungeon.", "MeleeWeapons"),
            Skill("Repair", "+10 Repair",
                "Arefu: Inside Evan Kings house on a table."),
            Skill("Science", "+10 Science",
                "Vault 106: Inside the living quarters."),
            Skill("Small Guns", "+10 Small Guns",
                "National Guard Depot: On a shelf next to the sealed door.", "SmallGuns"),
            Skill("Sneak", "+10 Sneak",
                "Yaoi Guai Tunnels: On a metal box next to a pool of water."),
            Skill("Speech", "+10 Speech",
                "Paradise Falls: Inside Eulogy's Pad on a table."),
            Skill("Unarmed", "+10 Unarmed",
                "Rockopolis: On the floor next to a body."),
        };

        private static Fallout3Bobblehead Special(string name, string effect, string location) =>
            new()
            {
                Name = name,
                EffectDisplay = effect,
                Location = location,
                EffectType = BobbleheadEffectType.Special,
                StatPropertyName = name,
                Amount = 1,
                ApplyEffect = c => c.TryIncreaseSpecialFromPerk(name)
            };

        private static Fallout3Bobblehead Skill(
            string displayName, string effect, string location, string? propertyName = null) =>
            new()
            {
                Name = displayName,
                EffectDisplay = effect,
                Location = location,
                EffectType = BobbleheadEffectType.Skill,
                StatPropertyName = propertyName ?? displayName,
                Amount = 10,
                ApplyEffect = c => c.TryAddSkillPointsFromPerk(propertyName ?? displayName, 10)
            };
    }
}
