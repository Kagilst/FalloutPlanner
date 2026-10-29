using System.Collections.Generic;

namespace FalloutPlanner.Games.Fallout1.Perks
{
    /// <summary>
    /// Fallout 1 Perks - characters choose 1 perk every 3 levels (or 4 with Skilled trait).
    /// Max level is 21.
    /// </summary>
    public static class Fallout1PerkRepository
    {
        public static List<Fallout1Perk> AllPerks { get; } = new()
        {
            // =====================================================
            // Level 3 Perks
            // =====================================================
            new Fallout1Perk
            {
                Name = "Awareness",
                RequiredLevel = 3,
                RequiredPerception = 5,
                Description = "Examining a target shows their current hit points, weapon and ammunition count."
            },
            new Fallout1Perk
            {
                Name = "Bonus HtH Attacks",
                RequiredLevel = 3,
                RequiredAgility = 6,
                Description = "You get an extra hand-to-hand attack per round."
            },
            new Fallout1Perk
            {
                Name = "Bonus HtH Damage",
                RequiredLevel = 3,
                RequiredAgility = 6,
                RequiredStrength = 6,
                MaxRank = 3,
                Description = "You do +2 points of damage with hand-to-hand and melee attacks.",
                ApplyEffect = c => c.MeleeDamage += 2
            },
            new Fallout1Perk
            {
                Name = "Bonus Move",
                RequiredLevel = 3,
                RequiredAgility = 5,
                MaxRank = 2,
                Description = "You get 2 free movement Action Points per turn that can only be used for movement."
            },
            new Fallout1Perk
            {
                Name = "Bonus Ranged Damage",
                RequiredLevel = 6,
                RequiredAgility = 6,
                RequiredLuck = 6,
                MaxRank = 2,
                Description = "You do +2 points of damage with ranged weapons per rank."
            },
            new Fallout1Perk
            {
                Name = "Earlier Sequence",
                RequiredLevel = 3,
                RequiredPerception = 6,
                MaxRank = 3,
                Description = "+2 to your Sequence stat per rank.",
                ApplyEffect = c => c.Sequence += 2
            },
            new Fallout1Perk
            {
                Name = "Faster Healing",
                RequiredLevel = 3,
                RequiredEndurance = 6,
                MaxRank = 3,
                Description = "Your Healing Rate is improved by +2 per rank.",
                ApplyEffect = c => c.HealingRate += 2
            },
            new Fallout1Perk
            {
                Name = "More Criticals",
                RequiredLevel = 6,
                RequiredLuck = 6,
                MaxRank = 3,
                Description = "+5% to your critical chance per rank.",
                ApplyEffect = c => c.CriticalChance += 5
            },
            new Fallout1Perk
            {
                Name = "Night Vision",
                RequiredLevel = 3,
                RequiredPerception = 6,
                Description = "You suffer only -1 penalty to hit in the dark instead of -2."
            },
            new Fallout1Perk
            {
                Name = "Presence",
                RequiredLevel = 3,
                RequiredCharisma = 6,
                MaxRank = 3,
                Description = "Initial reaction of NPCs is improved."
            },
            new Fallout1Perk
            {
                Name = "Quick Pockets",
                RequiredLevel = 3,
                RequiredAgility = 5,
                Description = "Accessing inventory only costs 2 AP instead of 4."
            },
            new Fallout1Perk
            {
                Name = "Smooth Talker",
                RequiredLevel = 3,
                RequiredIntelligence = 4,
                MaxRank = 3,
                Description = "+1 Intelligence for dialogue purposes per rank."
            },
            new Fallout1Perk
            {
                Name = "Strong Back",
                RequiredLevel = 3,
                RequiredStrength = 6,
                RequiredEndurance = 6,
                MaxRank = 3,
                Description = "+50 lbs to your carry weight per rank.",
                ApplyEffect = c => c.CarryWeight += 50
            },
            new Fallout1Perk
            {
                Name = "Stonewall",
                RequiredLevel = 3,
                RequiredStrength = 6,
                Description = "50% less chance of being knocked down."
            },
            new Fallout1Perk
            {
                Name = "Toughness",
                RequiredLevel = 3,
                RequiredEndurance = 6,
                RequiredLuck = 6,
                MaxRank = 3,
                Description = "+10% to your Damage Resistance per rank.",
                ApplyEffect = c => c.DamageResistance += 10
            },
            new Fallout1Perk
            {
                Name = "Rad Resistance",
                RequiredLevel = 6,
                RequiredEndurance = 6,
                RequiredIntelligence = 4,
                MaxRank = 2,
                Description = "+15% to your Radiation Resistance per rank.",
                ApplyEffect = c => c.RadiationResistance += 15
            },

            // =====================================================
            // Level 6 Perks
            // =====================================================
            new Fallout1Perk
            {
                Name = "Healer",
                RequiredLevel = 6,
                RequiredPerception = 7,
                RequiredIntelligence = 5,
                RequiredAgility = 6,
                RequiredFirstAid = 40,
                MaxRank = 2,
                Description = "Healing skills restore 4-10 more HP per rank."
            },
            new Fallout1Perk
            {
                Name = "Fortune Finder",
                RequiredLevel = 6,
                RequiredLuck = 8,
                Description = "You find more bottle caps in random encounters."
            },
            new Fallout1Perk
            {
                Name = "Sharpshooter",
                RequiredLevel = 6,
                RequiredPerception = 7,
                RequiredIntelligence = 6,
                MaxRank = 2,
                Description = "+2 to Perception for ranged attack purposes per rank."
            },
            new Fallout1Perk
            {
                Name = "Educated",
                RequiredLevel = 6,
                RequiredIntelligence = 6,
                MaxRank = 3,
                Description = "+2 skill points per level per rank."
            },
            new Fallout1Perk
            {
                Name = "Scout",
                RequiredLevel = 3,
                RequiredPerception = 7,
                Description = "Distance for exploration on the world map is increased."
            },
            new Fallout1Perk
            {
                Name = "Mysterious Stranger",
                RequiredLevel = 6,
                RequiredLuck = 7,
                Description = "A stranger may show up to help you in random encounters."
            },
            new Fallout1Perk
            {
                Name = "Ranger",
                RequiredLevel = 6,
                RequiredPerception = 6,
                RequiredOutdoorsman = 40,
                Description = "Chance of encountering hostile groups is reduced."
            },
            new Fallout1Perk
            {
                Name = "Scrounger",
                RequiredLevel = 6,
                RequiredLuck = 8,
                Description = "You find double the ammunition in random encounters."
            },
            new Fallout1Perk
            {
                Name = "Swift Learner",
                RequiredLevel = 3,
                RequiredIntelligence = 4,
                MaxRank = 3,
                Description = "+5% bonus to experience points earned per rank."
            },
            new Fallout1Perk
            {
                Name = "Pathfinder",
                RequiredLevel = 6,
                RequiredEndurance = 6,
                RequiredOutdoorsman = 40,
                MaxRank = 2,
                Description = "Travel time on the world map is reduced by 25% per rank."
            },
            new Fallout1Perk
            {
                Name = "Master Trader",
                RequiredLevel = 9,
                RequiredCharisma = 7,
                RequiredBarter = 60,
                Description = "You get 25% better prices in all trades."
            },

            // =====================================================
            // Level 9 Perks
            // =====================================================
            new Fallout1Perk
            {
                Name = "Sniper",
                RequiredLevel = 18,
                RequiredPerception = 8,
                RequiredAgility = 8,
                RequiredSmallGuns = 80,
                Description = "Increased chance to score a critical hit with ranged weapons."
            },
            new Fallout1Perk
            {
                Name = "Slayer",
                RequiredLevel = 18,
                RequiredStrength = 8,
                RequiredAgility = 8,
                RequiredUnarmed = 80,
                Description = "All your HtH attacks are critical hits."
            },
            new Fallout1Perk
            {
                Name = "Action Boy",
                RequiredLevel = 12,
                RequiredAgility = 5,
                MaxRank = 2,
                Description = "+1 Action Point per rank.",
                ApplyEffect = c => c.ActionPoints += 1
            },
            new Fallout1Perk
            {
                Name = "Lifegiver",
                RequiredLevel = 12,
                RequiredEndurance = 4,
                MaxRank = 2,
                Description = "+4 Hit Points per rank.",
                ApplyEffect = c => c.HitPoints += 4
            },
            new Fallout1Perk
            {
                Name = "Mental Block",
                RequiredLevel = 12,
                Description = "You are more resistant to mental attacks."
            },
            new Fallout1Perk
            {
                Name = "Pyromaniac",
                RequiredLevel = 9,
                RequiredBigGuns = 75,
                Description = "+5 damage with fire-based weapons."
            },
            new Fallout1Perk
            {
                Name = "Survivalist",
                RequiredLevel = 3,
                RequiredEndurance = 6,
                RequiredIntelligence = 6,
                RequiredOutdoorsman = 40,
                MaxRank = 3,
                Description = "+20% to Outdoorsman skill per rank."
            },
            new Fallout1Perk
            {
                Name = "Master Thief",
                RequiredLevel = 12,
                RequiredLockpick = 50,
                RequiredSteal = 50,
                Description = "+10% to Lockpick and Steal skills."
            },
            new Fallout1Perk
            {
                Name = "Medic",
                RequiredLevel = 12,
                RequiredFirstAid = 40,
                RequiredDoctor = 40,
                Description = "+10% to First Aid and Doctor skills."
            },
            new Fallout1Perk
            {
                Name = "Mr. Fixit",
                RequiredLevel = 12,
                RequiredScience = 40,
                RequiredRepair = 40,
                Description = "+10% to Science and Repair skills."
            },
            new Fallout1Perk
            {
                Name = "Speaker",
                RequiredLevel = 9,
                RequiredSpeech = 50,
                MaxRank = 1,
                Description = "+20% to Speech skill."
            },
            new Fallout1Perk
            {
                Name = "Ghost",
                RequiredLevel = 6,
                RequiredSneak = 60,
                Description = "+20% to Sneak skill in darkness."
            },
            new Fallout1Perk
            {
                Name = "Pickpocket",
                RequiredLevel = 9,
                RequiredAgility = 8,
                RequiredSteal = 80,
                Description = "Steal success is not affected by item size."
            },
            new Fallout1Perk
            {
                Name = "Silent Running",
                RequiredLevel = 6,
                RequiredAgility = 6,
                RequiredSneak = 50,
                Description = "You can sneak and run at the same time."
            },
            new Fallout1Perk
            {
                Name = "Silent Death",
                RequiredLevel = 18,
                RequiredAgility = 10,
                RequiredSneak = 80,
                RequiredUnarmed = 80,
                Description = "HtH attacks from behind cause double damage while sneaking."
            },
            new Fallout1Perk
            {
                Name = "Bonus Rate of Fire",
                RequiredLevel = 9,
                RequiredPerception = 6,
                RequiredIntelligence = 6,
                RequiredAgility = 7,
                Description = "Ranged attacks cost 1 less AP to perform."
            },
            new Fallout1Perk
            {
                Name = "Cautious Nature",
                RequiredLevel = 3,
                RequiredPerception = 6,
                Description = "+3 to Perception for determining placement at the start of combat."
            },
            new Fallout1Perk
            {
                Name = "Comprehension",
                RequiredLevel = 3,
                RequiredIntelligence = 6,
                Description = "Skill books give +2 skill points instead of +1."
            },
            new Fallout1Perk
            {
                Name = "Cult of Personality",
                RequiredLevel = 12,
                RequiredCharisma = 10,
                Description = "NPCs react to you based on your reputation regardless of alignment."
            },
            new Fallout1Perk
            {
                Name = "Dodger",
                RequiredLevel = 9,
                RequiredAgility = 4,
                Description = "+5 to Armor Class."
            },
            new Fallout1Perk
            {
                Name = "Explorer",
                RequiredLevel = 9,
                Description = "Higher chance of finding special encounters."
            },
            new Fallout1Perk
            {
                Name = "Flower Child",
                RequiredLevel = 6,
                RequiredEndurance = 5,
                Description = "50% less likely to become addicted, withdrawal time is halved."
            },
            new Fallout1Perk
            {
                Name = "Gambler",
                RequiredLevel = 6,
                RequiredGambling = 50,
                Description = "+20% to Gambling skill."
            },
            new Fallout1Perk
            {
                Name = "Gain Strength",
                RequiredLevel = 12,
                RequiredStrength = 9,
                Description = "+1 to Strength (maximum 10).",
                ApplyEffect = c => { if (c.Strength < 10) c.Strength++; }
            },
            new Fallout1Perk
            {
                Name = "Gain Perception",
                RequiredLevel = 12,
                RequiredPerception = 9,
                Description = "+1 to Perception (maximum 10).",
                ApplyEffect = c => { if (c.Perception < 10) c.Perception++; }
            },
            new Fallout1Perk
            {
                Name = "Gain Endurance",
                RequiredLevel = 12,
                RequiredEndurance = 9,
                Description = "+1 to Endurance (maximum 10).",
                ApplyEffect = c => { if (c.Endurance < 10) c.Endurance++; }
            },
            new Fallout1Perk
            {
                Name = "Gain Charisma",
                RequiredLevel = 12,
                RequiredCharisma = 9,
                Description = "+1 to Charisma (maximum 10).",
                ApplyEffect = c => { if (c.Charisma < 10) c.Charisma++; }
            },
            new Fallout1Perk
            {
                Name = "Gain Intelligence",
                RequiredLevel = 12,
                RequiredIntelligence = 9,
                Description = "+1 to Intelligence (maximum 10).",
                ApplyEffect = c => { if (c.Intelligence < 10) c.Intelligence++; }
            },
            new Fallout1Perk
            {
                Name = "Gain Agility",
                RequiredLevel = 12,
                RequiredAgility = 9,
                Description = "+1 to Agility (maximum 10).",
                ApplyEffect = c => { if (c.Agility < 10) c.Agility++; }
            },
            new Fallout1Perk
            {
                Name = "Gain Luck",
                RequiredLevel = 12,
                RequiredLuck = 9,
                Description = "+1 to Luck (maximum 10).",
                ApplyEffect = c => { if (c.Luck < 10) c.Luck++; }
            },
            new Fallout1Perk
            {
                Name = "Here and Now",
                RequiredLevel = 3,
                MaxRank = 2,
                Description = "Immediately gain one level."
                // Apply effect is handled specially in level-up logic
            },
            new Fallout1Perk
            {
                Name = "Kama Sutra Master",
                RequiredLevel = 3,
                RequiredEndurance = 5,
                RequiredAgility = 5,
                Description = "Better romantic... dialogue options."
            },
            new Fallout1Perk
            {
                Name = "Light Step",
                RequiredLevel = 9,
                RequiredAgility = 5,
                RequiredLuck = 5,
                Description = "50% less chance of triggering floor traps."
            },
            new Fallout1Perk
            {
                Name = "Magnetic Personality",
                RequiredLevel = 6,
                RequiredCharisma = 5,
                Description = "+1 to maximum party size."
            },
            new Fallout1Perk
            {
                Name = "Mutate!",
                RequiredLevel = 9,
                Description = "Change one of your traits."
            },
            new Fallout1Perk
            {
                Name = "Negotiator",
                RequiredLevel = 6,
                RequiredSpeech = 50,
                RequiredBarter = 50,
                Description = "+10% to Speech and Barter skills."
            },
            new Fallout1Perk
            {
                Name = "Pack Rat",
                RequiredLevel = 6,
                Description = "+50 lbs to Carry Weight.",
                ApplyEffect = c => c.CarryWeight += 50
            },
            new Fallout1Perk
            {
                Name = "Animal Friend",
                RequiredLevel = 9,
                RequiredIntelligence = 5,
                RequiredOutdoorsman = 25,
                Description = "Animals will not attack you."
            },
            new Fallout1Perk
            {
                Name = "Living Anatomy",
                RequiredLevel = 12,
                RequiredDoctor = 60,
                Description = "+10 damage to living creatures, can see their HP."
            },
            new Fallout1Perk
            {
                Name = "Tag!",
                RequiredLevel = 12,
                Description = "Choose another skill to tag (+20% bonus)."
            },
            new Fallout1Perk
            {
                Name = "Better Criticals",
                RequiredLevel = 9,
                RequiredPerception = 6,
                RequiredLuck = 6,
                RequiredAgility = 4,
                Description = "+20% bonus to critical hit rolls."
            },
            new Fallout1Perk
            {
                Name = "Quick Recovery",
                RequiredLevel = 6,
                RequiredAgility = 5,
                Description = "Standing from prone costs only 1 AP instead of 2."
            },
            new Fallout1Perk
            {
                Name = "Weapon Handling",
                RequiredLevel = 12,
                RequiredAgility = 5,
                RequiredStrength = 6,
                Description = "Strength requirement for weapons is reduced by 3."
            },
            new Fallout1Perk
            {
                Name = "Demolition Expert",
                RequiredLevel = 9,
                RequiredAgility = 4,
                RequiredTraps = 75,
                Description = "Explosives do more damage and are easier to set."
            },
        };
    }
}
