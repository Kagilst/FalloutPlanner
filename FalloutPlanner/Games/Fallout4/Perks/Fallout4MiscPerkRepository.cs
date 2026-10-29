using System.Collections.Generic;
using System.Linq;

namespace FalloutPlanner.Games.Fallout4.Perks
{
    public static class Fallout4MiscPerkRepository
    {
        public const string AtomExclusiveGroup = "FarHarborAtom";
        public const string AcadiaExclusiveGroup = "FarHarborAcadia";
        public const string NukaRaiderGroup = "NukaWorldRaider";

        public static IReadOnlyList<Fallout4MiscPerk> All { get; } = CreateAll();

        public static Fallout4MiscPerk? Get(string name) =>
            All.FirstOrDefault(p => p.Name == name);

        public static IEnumerable<Fallout4MiscPerk> GetByGroup(string groupId) =>
            All.Where(p => p.ExclusionGroup == groupId);

        private static List<Fallout4MiscPerk> CreateAll() => new()
        {
            // --- Companion perks ---
            Companion("Trigger Rush", "Cait",
                "Action Points regenerate 25% faster if Hit Points are below 25% of maximum."),
            Companion("Robot Sympathy", "Codsworth",
                "Gain +10 Damage Resistance against robots' energy attacks."),
            Companion("Combat Medic", "Curie",
                "Heal 100 Hit Points if current Hit Points are below 10%."),
            Companion("Know Your Enemy", "Paladin Danse",
                "Deal 20% more damage against feral ghouls, super mutants, and synths."),
            Companion("Cloak & Dagger", "Deacon",
                "+20% sneak attack damage. Stealth Boy effects last 40% longer."),
            Companion("Lessons in Blood", "Porter Gage",
                "+5% XP per kill and +10 Damage Resistance."),
            Companion("Isodoped", "John Hancock",
                "Critical Hit meter fills 20% faster when you have 250 or more radiation."),
            Companion("Killshot", "Robert MacCready",
                "20% greater chance to hit an enemy's head in V.A.T.S."),
            Companion("Close to Metal", "Nick Valentine",
                "1 additional guess when hacking terminals; 50% faster terminal lockout cooldown."),
            Companion("Hunter's Wisdom", "Old Longfellow",
                "Damage Resistance and Energy Resistance of animals and sea creatures reduced by 25%."),
            Companion("Gift of Gab", "Piper",
                "Double XP when persuading people or discovering new places."),
            Companion("United We Stand", "Preston Garvey",
                "+20 Damage Resistance and +20% damage when outnumbered."),
            Companion("Berserk", "Strong",
                "+20% melee damage when Hit Points are below 25% of maximum."),
            Companion("Shield Harmonics", "X6-88",
                "+20 Energy Resistance."),

            // --- Quest perks ---
            // Far Harbor Atom / Survivalist — only one
            Quest("Crusader of Atom",
                "Provides a bonus to your weapon's damage. The higher your rads, the higher the bonus.",
                AtomExclusiveGroup),
            Quest("Inquisitor of Atom",
                "Provides a bonus to your weapon's damage. The higher your rads, the higher the bonus.",
                AtomExclusiveGroup),
            Quest("Far Harbor Survivalist",
                "Gain +5 to all resistances.",
                AtomExclusiveGroup),

            // Acadia — only one
            Quest("Destroyer of Acadia",
                "When your health falls below 20%, do 4x damage for 30 seconds.",
                AcadiaExclusiveGroup),
            Quest("Protector of Acadia",
                "When your health falls below 20%, gain 1000 Damage Resistance and Energy Resistance for 30 seconds.",
                AcadiaExclusiveGroup),

            // Nuka-World raider — at most 2 of 3
            Quest("Ace Operator",
                "While in shadows, stealth visibility is 90% and you deal 25% more damage with silenced weapons.",
                NukaRaiderGroup, limit: 2),
            Quest("Chosen Disciple",
                "Kills made by your melee weapons restore some Action Points.",
                NukaRaiderGroup, limit: 2),
            Quest("Pack Alpha",
                "Take 25% less damage and deal 25% more damage while unarmed or using melee weapons.",
                NukaRaiderGroup, limit: 2),

            new()
            {
                Name = "Wasteland Warlord",
                Kind = Fallout4MiscPerkKind.Quest,
                MaxRank = 3,
                Description =
                    "Rank 1: Unlocks new structures at all Raider outposts.\n" +
                    "Rank 2: Unlocks additional structures at all Raider outposts.\n" +
                    "Rank 3: You rule the wastes! Your outposts can construct all Raider structures!"
            },
        };

        private static Fallout4MiscPerk Companion(string name, string companion, string benefit) =>
            new()
            {
                Name = name,
                CompanionName = companion,
                Kind = Fallout4MiscPerkKind.Companion,
                Description = benefit,
                MaxRank = 1
            };

        private static Fallout4MiscPerk Quest(string name, string benefit, string? group = null, int limit = 1) =>
            new()
            {
                Name = name,
                Kind = Fallout4MiscPerkKind.Quest,
                Description = benefit,
                MaxRank = 1,
                ExclusionGroup = group,
                ExclusionGroupLimit = limit
            };
    }
}
