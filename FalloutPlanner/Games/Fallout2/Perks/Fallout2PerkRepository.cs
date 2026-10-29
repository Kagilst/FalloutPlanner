using System.Collections.Generic;

namespace FalloutPlanner.Games.Fallout2.Perks
{
    /// <summary>
    /// Fallout 2 Leveling Perks - characters choose 1 perk every 3 levels (or 4 with Skilled trait).
    /// Max level is 99.
    /// </summary>
    public static class Fallout2PerkRepository
    {
        public static List<Fallout2Perk> AllPerks { get; } = new()
        {
            // =====================================================
            // Level 3 Perks
            // =====================================================
            new Fallout2Perk
            {
                Name = "Awareness",
                RequiredLevel = 3,
                RequiredPerception = 5,
                MaxRank = 1,
                Description = "With Awareness, you can see the Hit Points and current weapon of any character you examine."
            },
            new Fallout2Perk
            {
                Name = "Bonus HtH Attacks",
                RequiredLevel = 3,
                RequiredAgility = 6,
                MaxRank = 1,
                Description = "Each successful attack costs 1 less AP."
            },
            new Fallout2Perk
            {
                Name = "Bonus HtH Damage",
                RequiredLevel = 3,
                RequiredAgility = 6,
                RequiredStrength = 6,
                MaxRank = 3,
                Description = "You cause +2 points of damage with hand-to-hand and melee attacks for each rank.",
                ApplyEffect = c => c.MeleeDamage += 2
            },
            new Fallout2Perk
            {
                Name = "Bonus Move",
                RequiredLevel = 3,
                RequiredAgility = 5,
                MaxRank = 2,
                Description = "For each rank, you get 2 free action points usable only for movement."
            },
            new Fallout2Perk
            {
                Name = "Bonus Ranged Damage",
                RequiredLevel = 3,
                RequiredAgility = 6,
                RequiredLuck = 6,
                MaxRank = 2,
                Description = "You cause +2 points of damage with ranged weapons for each rank."
            },
            new Fallout2Perk
            {
                Name = "Bonus Rate of Fire",
                RequiredLevel = 3,
                RequiredAgility = 7,
                RequiredIntelligence = 6,
                RequiredPerception = 6,
                MaxRank = 1,
                Description = "Firing ranged weapons costs 1 less AP."
            },
            new Fallout2Perk
            {
                Name = "Earlier Sequence",
                RequiredLevel = 3,
                RequiredPerception = 6,
                MaxRank = 3,
                Description = "+2 to your Sequence per rank.",
                ApplyEffect = c => c.Sequence += 2
            },
            new Fallout2Perk
            {
                Name = "Faster Healing",
                RequiredLevel = 3,
                RequiredEndurance = 6,
                MaxRank = 3,
                Description = "+2 to your Healing Rate.",
                ApplyEffect = c => c.HealingRate += 2
            },
            new Fallout2Perk
            {
                Name = "More Criticals",
                RequiredLevel = 3,
                RequiredLuck = 6,
                MaxRank = 3,
                Description = "+5% Critical Chance per rank.",
                ApplyEffect = c => c.CriticalChance += 5
            },
            new Fallout2Perk
            {
                Name = "Night Vision",
                RequiredLevel = 3,
                RequiredPerception = 6,
                MaxRank = 1,
                Description = "Your perception is increased +3 in low light conditions."
            },
            new Fallout2Perk
            {
                Name = "Presence",
                RequiredLevel = 3,
                RequiredCharisma = 6,
                MaxRank = 3,
                Description = "NPCs have better reactions to you."
            },
            new Fallout2Perk
            {
                Name = "Quick Pockets",
                RequiredLevel = 3,
                RequiredAgility = 5,
                MaxRank = 1,
                Description = "Using inventory costs only 2 AP instead of 4."
            },
            new Fallout2Perk
            {
                Name = "Scout",
                RequiredLevel = 3,
                RequiredPerception = 7,
                MaxRank = 1,
                Description = "+15% to Outdoorsman skill for the purpose of finding special encounters."
            },
            new Fallout2Perk
            {
                Name = "Smooth Talker",
                RequiredLevel = 3,
                RequiredIntelligence = 4,
                MaxRank = 3,
                Description = "Each rank adds +1 to Intelligence for dialogue purposes."
            },
            new Fallout2Perk
            {
                Name = "Strong Back",
                RequiredLevel = 3,
                RequiredStrength = 6,
                RequiredEndurance = 6,
                MaxRank = 3,
                Description = "+50 lbs to Carry Weight per rank.",
                ApplyEffect = c => c.CarryWeight += 50
            },
            new Fallout2Perk
            {
                Name = "Survivalist",
                RequiredLevel = 3,
                RequiredEndurance = 6,
                RequiredIntelligence = 6,
                RequiredOutdoorsman = 40,
                MaxRank = 3,
                Description = "+25% to Outdoorsman skill per rank.",
                ApplyEffect = c => c.Outdoorsman += 25
            },
            new Fallout2Perk
            {
                Name = "Swift Learner",
                RequiredLevel = 3,
                RequiredIntelligence = 4,
                MaxRank = 3,
                Description = "+5% to experience gained per rank."
            },
            new Fallout2Perk
            {
                Name = "Thief",
                RequiredLevel = 3,
                MaxRank = 1,
                Description = "+10% to Sneak, Lockpick, Steal, and Traps.",
                ApplyEffect = c =>
                {
                    c.Sneak += 10;
                    c.Lockpick += 10;
                    c.Steal += 10;
                    c.Traps += 10;
                }
            },
            new Fallout2Perk
            {
                Name = "Toughness",
                RequiredLevel = 3,
                RequiredEndurance = 6,
                RequiredLuck = 6,
                MaxRank = 3,
                Description = "+10% Damage Resistance per rank.",
                ApplyEffect = c => c.DamageResist += 10
            },

            // =====================================================
            // Level 6 Perks
            // =====================================================
            new Fallout2Perk
            {
                Name = "Adrenaline Rush",
                RequiredLevel = 6,
                RequiredStrength = 10,
                MaxRank = 1,
                Description = "When below 50% HP, you gain +1 Strength.",
                Requirement = c => c.Strength < 10
            },
            new Fallout2Perk
            {
                Name = "Cautious Nature",
                RequiredLevel = 6,
                RequiredPerception = 6,
                MaxRank = 1,
                Description = "+3 to Perception when determining chance of random encounters."
            },
            new Fallout2Perk
            {
                Name = "Comprehension",
                RequiredLevel = 6,
                RequiredIntelligence = 6,
                MaxRank = 1,
                Description = "You gain 50% more skill points from reading books."
            },
            new Fallout2Perk
            {
                Name = "Demolition Expert",
                RequiredLevel = 6,
                RequiredAgility = 4,
                RequiredTraps = 75,
                MaxRank = 1,
                Description = "+10% damage with explosives."
            },
            new Fallout2Perk
            {
                Name = "Educated",
                RequiredLevel = 6,
                RequiredIntelligence = 6,
                MaxRank = 3,
                Description = "+2 skill points per level gained per rank."
            },
            new Fallout2Perk
            {
                Name = "Empathy",
                RequiredLevel = 6,
                RequiredPerception = 7,
                RequiredIntelligence = 5,
                MaxRank = 1,
                Description = "You can tell better how NPCs feel about you."
            },
            new Fallout2Perk
            {
                Name = "Flower Child",
                RequiredLevel = 6,
                RequiredEndurance = 5,
                MaxRank = 1,
                Description = "Half the withdrawal time from chem addiction."
            },
            new Fallout2Perk
            {
                Name = "Fortune Finder",
                RequiredLevel = 6,
                RequiredLuck = 8,
                MaxRank = 1,
                Description = "Find more money in random encounters."
            },
            new Fallout2Perk
            {
                Name = "Gambler",
                RequiredLevel = 6,
                RequiredGambling = 50,
                MaxRank = 1,
                Description = "+20% to Gambling skill.",
                ApplyEffect = c => c.Gambling += 20
            },
            new Fallout2Perk
            {
                Name = "Ghost",
                RequiredLevel = 6,
                RequiredSneak = 60,
                MaxRank = 1,
                Description = "+20% to Sneak in low light conditions."
            },
            new Fallout2Perk
            {
                Name = "Harmless",
                RequiredLevel = 6,
                RequiredSteal = 50,
                RequiredCharisma = 6,
                MaxRank = 1,
                Description = "NPCs are more likely to remain unaware after failed steal attempts."
            },
            new Fallout2Perk
            {
                Name = "Healer",
                RequiredLevel = 6,
                RequiredPerception = 7,
                RequiredIntelligence = 5,
                RequiredAgility = 6,
                RequiredFirstAid = 40,
                MaxRank = 2,
                Description = "+10-30 HP healed with First Aid and Doctor skills per rank."
            },
            new Fallout2Perk
            {
                Name = "Heave Ho!",
                RequiredLevel = 6,
                RequiredStrength = 9,
                MaxRank = 3,
                Description = "+2 to Strength for determining throwing range per rank.",
                Requirement = c => c.Strength < 10
            },
            new Fallout2Perk
            {
                Name = "Kama Sutra Master",
                RequiredLevel = 6,
                RequiredEndurance = 5,
                RequiredAgility = 5,
                MaxRank = 1,
                Description = "You are an expert in the art of love."
            },
            new Fallout2Perk
            {
                Name = "Light Step",
                RequiredLevel = 6,
                RequiredAgility = 5,
                RequiredLuck = 5,
                MaxRank = 1,
                Description = "You are less likely to set off traps."
            },
            new Fallout2Perk
            {
                Name = "Living Anatomy",
                RequiredLevel = 6,
                RequiredDoctor = 60,
                MaxRank = 1,
                Description = "+10% to Doctor skill and +5 damage to living creatures.",
                ApplyEffect = c => c.Doctor += 10
            },
            new Fallout2Perk
            {
                Name = "Magnetic Personality",
                RequiredLevel = 6,
                RequiredCharisma = 10,
                MaxRank = 1,
                Description = "+1 to Charisma for determining party size.",
                Requirement = c => c.Charisma < 10
            },
            new Fallout2Perk
            {
                Name = "Master Thief",
                RequiredLevel = 6,
                RequiredLockpick = 50,
                RequiredSteal = 50,
                MaxRank = 1,
                Description = "+15% to Lockpick and Steal.",
                ApplyEffect = c =>
                {
                    c.Lockpick += 15;
                    c.Steal += 15;
                }
            },
            new Fallout2Perk
            {
                Name = "Medic",
                RequiredLevel = 6,
                RequiredFirstAid = 40,
                RequiredDoctor = 40,
                MaxRank = 1,
                Description = "+10% to First Aid and Doctor skills.",
                ApplyEffect = c =>
                {
                    c.FirstAid += 10;
                    c.Doctor += 10;
                }
            },
            new Fallout2Perk
            {
                Name = "Negotiator",
                RequiredLevel = 6,
                RequiredSpeech = 50,
                RequiredBarter = 50,
                MaxRank = 1,
                Description = "+10% to Speech and Barter skills.",
                ApplyEffect = c =>
                {
                    c.Speech += 10;
                    c.Barter += 10;
                }
            },
            new Fallout2Perk
            {
                Name = "Pack Rat",
                RequiredLevel = 6,
                MaxRank = 1,
                Description = "Items that weigh 2 lbs or less now weigh half."
            },
            new Fallout2Perk
            {
                Name = "Pathfinder",
                RequiredLevel = 6,
                RequiredEndurance = 6,
                RequiredOutdoorsman = 40,
                MaxRank = 2,
                Description = "25% less travel time on the world map per rank."
            },
            new Fallout2Perk
            {
                Name = "Rad Resistance",
                RequiredLevel = 6,
                RequiredEndurance = 6,
                RequiredIntelligence = 4,
                MaxRank = 2,
                Description = "+15% Radiation Resistance per rank.",
                ApplyEffect = c => c.RadiationResist += 15
            },
            new Fallout2Perk
            {
                Name = "Ranger",
                RequiredLevel = 6,
                RequiredPerception = 6,
                MaxRank = 1,
                Description = "+15% to Outdoorsman skill.",
                ApplyEffect = c => c.Outdoorsman += 15
            },
            new Fallout2Perk
            {
                Name = "Salesman",
                RequiredLevel = 6,
                RequiredBarter = 50,
                MaxRank = 1,
                Description = "+20% to Barter skill.",
                ApplyEffect = c => c.Barter += 20
            },
            new Fallout2Perk
            {
                Name = "Scrounger",
                RequiredLevel = 6,
                RequiredLuck = 8,
                MaxRank = 1,
                Description = "Find more ammunition in random encounters."
            },
            new Fallout2Perk
            {
                Name = "Sharpshooter",
                RequiredLevel = 6,
                RequiredPerception = 7,
                RequiredIntelligence = 6,
                MaxRank = 2,
                Description = "+2 to Perception for ranged attack calculations per rank."
            },
            new Fallout2Perk
            {
                Name = "Silent Death",
                RequiredLevel = 6,
                RequiredAgility = 10,
                RequiredSneak = 80,
                RequiredUnarmed = 80,
                MaxRank = 1,
                Description = "Double damage with unarmed and melee attacks from behind while sneaking."
            },
            new Fallout2Perk
            {
                Name = "Silent Running",
                RequiredLevel = 6,
                RequiredAgility = 6,
                RequiredSneak = 50,
                MaxRank = 1,
                Description = "You can run while sneaking without penalty."
            },
            new Fallout2Perk
            {
                Name = "Sniper",
                RequiredLevel = 6,
                RequiredPerception = 8,
                RequiredAgility = 8,
                RequiredSmallGuns = 80,
                MaxRank = 1,
                Description = "Better chance for critical hits with ranged weapons based on Luck."
            },
            new Fallout2Perk
            {
                Name = "Speaker",
                RequiredLevel = 6,
                RequiredSpeech = 50,
                MaxRank = 1,
                Description = "+20% to Speech skill.",
                ApplyEffect = c => c.Speech += 20
            },
            new Fallout2Perk
            {
                Name = "Stonewall",
                RequiredLevel = 6,
                RequiredStrength = 6,
                MaxRank = 1,
                Description = "You are less likely to be knocked down in combat."
            },

            // =====================================================
            // Level 9 Perks
            // =====================================================
            new Fallout2Perk
            {
                Name = "Action Boy",
                RequiredLevel = 9,
                RequiredAgility = 5,
                MaxRank = 2,
                Description = "+1 Action Point per rank.",
                ApplyEffect = c => c.ActionPoints += 1
            },
            new Fallout2Perk
            {
                Name = "Better Criticals",
                RequiredLevel = 9,
                RequiredPerception = 6,
                RequiredLuck = 6,
                RequiredAgility = 4,
                MaxRank = 1,
                Description = "+20% to critical hit damage."
            },
            new Fallout2Perk
            {
                Name = "Dodger",
                RequiredLevel = 9,
                RequiredAgility = 4,
                MaxRank = 1,
                Description = "+5 to Armor Class.",
                ApplyEffect = c => c.ArmorClass += 5
            },
            new Fallout2Perk
            {
                Name = "Explorer",
                RequiredLevel = 9,
                MaxRank = 1,
                Description = "Better chance of finding special encounters."
            },
            new Fallout2Perk
            {
                Name = "Mutate!",
                RequiredLevel = 9,
                MaxRank = 1,
                Description = "Change one of your traits for a new one."
            },
            new Fallout2Perk
            {
                Name = "Mysterious Stranger",
                RequiredLevel = 9,
                RequiredLuck = 7,
                MaxRank = 1,
                Description = "A mysterious stranger may help you in combat."
            },
            new Fallout2Perk
            {
                Name = "Pyromaniac",
                RequiredLevel = 9,
                RequiredBigGuns = 75,
                MaxRank = 1,
                Description = "+5 damage with fire-based weapons."
            },
            new Fallout2Perk
            {
                Name = "Tag!",
                RequiredLevel = 9,
                MaxRank = 1,
                Description = "Pick a fourth tag skill. +20% to that skill immediately."
            },
            new Fallout2Perk
            {
                Name = "Weapon Handling",
                RequiredLevel = 9,
                RequiredAgility = 5,
                RequiredStrength = 7,
                MaxRank = 1,
                Description = "-3 to Strength requirements for weapons.",
                Requirement = c => c.Strength < 10
            },

            // =====================================================
            // Level 12 Perks
            // =====================================================
            new Fallout2Perk
            {
                Name = "Bonus HtH Attacks (12)",
                RequiredLevel = 12,
                RequiredAgility = 6,
                MaxRank = 1,
                Description = "Each successful HtH attack costs 1 less AP."
            },
            new Fallout2Perk
            {
                Name = "Gain Strength",
                RequiredLevel = 12,
                RequiredStrength = 10,
                MaxRank = 1,
                Description = "+1 to Strength.",
                Requirement = c => c.Strength < 10,
                ApplyEffect = c => c.Strength = System.Math.Min(10, c.Strength + 1)
            },
            new Fallout2Perk
            {
                Name = "Gain Perception",
                RequiredLevel = 12,
                RequiredPerception = 10,
                MaxRank = 1,
                Description = "+1 to Perception.",
                Requirement = c => c.Perception < 10,
                ApplyEffect = c => c.Perception = System.Math.Min(10, c.Perception + 1)
            },
            new Fallout2Perk
            {
                Name = "Gain Endurance",
                RequiredLevel = 12,
                RequiredEndurance = 10,
                MaxRank = 1,
                Description = "+1 to Endurance.",
                Requirement = c => c.Endurance < 10,
                ApplyEffect = c => c.Endurance = System.Math.Min(10, c.Endurance + 1)
            },
            new Fallout2Perk
            {
                Name = "Gain Charisma",
                RequiredLevel = 12,
                RequiredCharisma = 10,
                MaxRank = 1,
                Description = "+1 to Charisma.",
                Requirement = c => c.Charisma < 10,
                ApplyEffect = c => c.Charisma = System.Math.Min(10, c.Charisma + 1)
            },
            new Fallout2Perk
            {
                Name = "Gain Intelligence",
                RequiredLevel = 12,
                RequiredIntelligence = 10,
                MaxRank = 1,
                Description = "+1 to Intelligence.",
                Requirement = c => c.Intelligence < 10,
                ApplyEffect = c => c.Intelligence = System.Math.Min(10, c.Intelligence + 1)
            },
            new Fallout2Perk
            {
                Name = "Gain Agility",
                RequiredLevel = 12,
                RequiredAgility = 10,
                MaxRank = 1,
                Description = "+1 to Agility.",
                Requirement = c => c.Agility < 10,
                ApplyEffect = c => c.Agility = System.Math.Min(10, c.Agility + 1)
            },
            new Fallout2Perk
            {
                Name = "Gain Luck",
                RequiredLevel = 12,
                RequiredLuck = 10,
                MaxRank = 1,
                Description = "+1 to Luck.",
                Requirement = c => c.Luck < 10,
                ApplyEffect = c => c.Luck = System.Math.Min(10, c.Luck + 1)
            },
            new Fallout2Perk
            {
                Name = "HtH Evade",
                RequiredLevel = 12,
                RequiredUnarmed = 75,
                MaxRank = 1,
                Description = "Your AC increases based on Unarmed skill when no weapon is equipped."
            },
            new Fallout2Perk
            {
                Name = "Lifegiver",
                RequiredLevel = 12,
                RequiredEndurance = 4,
                MaxRank = 2,
                Description = "+4 HP per level gained per rank."
            },

            // =====================================================
            // Level 15 Perks
            // =====================================================
            new Fallout2Perk
            {
                Name = "Slayer",
                RequiredLevel = 15,
                RequiredAgility = 8,
                RequiredStrength = 8,
                RequiredUnarmed = 80,
                MaxRank = 1,
                Description = "All HtH attacks are automatic critical hits."
            },
            new Fallout2Perk
            {
                Name = "Sniper (15)",
                RequiredLevel = 15,
                RequiredPerception = 8,
                RequiredAgility = 8,
                RequiredSmallGuns = 80,
                MaxRank = 1,
                Description = "Your Luck determines critical hit chance with ranged attacks."
            },

            // =====================================================
            // Level 18 Perks
            // =====================================================
            new Fallout2Perk
            {
                Name = "Living Anatomy (18)",
                RequiredLevel = 18,
                RequiredDoctor = 60,
                MaxRank = 1,
                Description = "+10% to Doctor skill and +5 damage to living creatures."
            },

            // =====================================================
            // Level 24 Perks
            // =====================================================
            new Fallout2Perk
            {
                Name = "Slayer (24)",
                RequiredLevel = 24,
                RequiredAgility = 8,
                RequiredStrength = 8,
                RequiredUnarmed = 80,
                MaxRank = 1,
                Description = "All unarmed attacks are critical hits."
            },
        };
    }
}
