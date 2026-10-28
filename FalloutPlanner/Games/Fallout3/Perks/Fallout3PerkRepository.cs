using System.Collections.Generic;

namespace FalloutPlanner.Games.Fallout3.Perks
{
    public static class Fallout3PerkRepository
    {
        public static List<Fallout3Perk> AllPerks { get; } = new()
        {
            new Fallout3Perk
            {
                Name = "Black Widow/Lady Killer",
                RequiredLevel = 2,
                MaxRank = 1,
                Description = "+10% damage to the opposite sex, and unique dialogue options with certain characters"
            },
            new Fallout3Perk
            {
                Name = "Daddy's Boy/Girl",
                RequiredLevel = 2,
                MaxRank = 3,
                Description = "+5 Science, +5 Medicine",
                RequiredIntelligence = 4,
                ApplyEffect = c =>
                {
                    c.TryAddSkillPointsFromPerk(nameof(c.Science), 5);
                    c.TryAddSkillPointsFromPerk(nameof(c.Medicine), 5);
                }
            },
            new Fallout3Perk
            {
                Name = "Gun Nut",
                RequiredLevel = 2,
                MaxRank = 3,
                Description = "+5 Small Guns, +5 Repair",
                RequiredIntelligence = 4,
                RequiredAgility = 4,
                ApplyEffect = c =>
                {
                    c.TryAddSkillPointsFromPerk(nameof(c.SmallGuns), 5);
                    c.TryAddSkillPointsFromPerk(nameof(c.Repair), 5);
                }
            },
            new Fallout3Perk
            {
                Name = "Little Leaguer",
                RequiredLevel = 2,
                MaxRank = 3,
                Description = "+5 Melee Weapons, +5 Explosives",
                RequiredStrength = 4,
                ApplyEffect = c =>
                {
                    c.TryAddSkillPointsFromPerk(nameof(c.MeleeWeapons), 5);
                    c.TryAddSkillPointsFromPerk(nameof(c.Explosives), 5);
                }
            },
            new Fallout3Perk
            {
                Name = "Thief",
                RequiredLevel = 2,
                MaxRank = 3,
                Description = "+5 Lockpick, +5 Sneak",
                RequiredPerception = 4,
                RequiredAgility = 4,
                ApplyEffect = c =>
                {
                    c.TryAddSkillPointsFromPerk(nameof(c.Lockpick), 5);
                    c.TryAddSkillPointsFromPerk(nameof(c.Sneak), 5);
                }
            },
            new Fallout3Perk
            {
                Name = "Swift Learner",
                RequiredLevel = 2,
                MaxRank = 3,
                Description = "+10% XP whenever XP is earned",
                RequiredIntelligence = 4
            },
            new Fallout3Perk
            {
                Name = "Intense Training",
                RequiredLevel = 2,
                MaxRank = 10,
                Description = "+1 to any SPECIAL stat"
            },
            new Fallout3Perk
            {
                Name = "Child at Heart",
                RequiredLevel = 4,
                MaxRank = 1,
                Description = "Unlocks many unique dialogue options with children",
                RequiredCharisma = 4
            },
            new Fallout3Perk
            {
                Name = "Comprehension",
                RequiredLevel = 4,
                MaxRank = 1,
                Description = "+1 additional skill point whenever a skill book is read",
                RequiredIntelligence = 4
            },
            new Fallout3Perk
            {
                Name = "Educated",
                RequiredLevel = 4,
                MaxRank = 1,
                Description = "+3 skill points per level",
                RequiredIntelligence = 4,
                ApplyEffect = c =>
                {
                    c.SkillPoints += 3;
                }
            },
            new Fallout3Perk
            {
                Name = "Entomologist",
                RequiredLevel = 4,
                MaxRank = 1,
                Description = "+50% damage to insects",
                RequiredIntelligence = 4,
                RequiredScience = 40
            },
            new Fallout3Perk
            {
                Name = "Scoundrel",
                RequiredLevel = 4,
                MaxRank = 3,
                Description = "+5 Speech, +5 Barter",
                RequiredCharisma = 4,
                ApplyEffect = c =>
                {
                    c.TryAddSkillPointsFromPerk(nameof(c.Speech), 5);
                    c.TryAddSkillPointsFromPerk(nameof(c.Barter), 5);
                }
            },
            new Fallout3Perk
            {
                Name = "Iron Fist",
                RequiredLevel = 4,
                MaxRank = 3,
                Description = "+5 points of Unarmed Damage",
                RequiredStrength = 4,
                ApplyEffect = c =>
                {
                    c.UnarmedDamage = Math.Min(c.UnarmedDamage + 5, 100);
                }
            },
            new Fallout3Perk
            {
                Name = "Bloody Mess",
                RequiredLevel = 6,
                MaxRank = 1,
                Description = "+5% overall damage, more violent death animations"
            },
            new Fallout3Perk
            {
                Name = "Lead Belly",
                RequiredLevel = 6,
                MaxRank = 1,
                Description = "-50% radiation taken whenever drinking from a water source",
                RequiredEndurance = 5
            },
            new Fallout3Perk
            {
                Name = "Toughness",
                RequiredLevel = 6,
                MaxRank = 1,
                Description = "+10% DR permanently",
                RequiredEndurance = 5,
                ApplyEffect = c =>
                {
                    c.DamageResist += 10;
                }
            },
            new Fallout3Perk
            {
                Name = "Fortune Finder",
                RequiredLevel = 6,
                MaxRank = 1,
                Description = "Considerably more bottle caps will be found in containers",
                RequiredLuck = 5
            },
            new Fallout3Perk
            {
                Name = "Gunslinger",
                RequiredLevel = 6,
                MaxRank = 1,
                Description = "+25% accuracy in V.A.T.S. with one-handed weapons"
            },
            new Fallout3Perk
            {
                Name = "Demolition Expert",
                RequiredLevel = 6,
                MaxRank = 3,
                Description = "+20% damage with explosives",
                RequiredExplosives = 50
            },
            new Fallout3Perk
            {
                Name = "Commando",
                RequiredLevel = 8,
                MaxRank = 1,
                Description = "+25% accuracy in V.A.T.S. with two-handed weapons"
            },
            new Fallout3Perk
            {
                Name = "Rad Resistance",
                RequiredLevel = 8,
                MaxRank = 1,
                Description = "+25% Radiation Resistance permanently",
                RequiredEndurance = 5,
                ApplyEffect = c =>
                {
                    c.RadiationResist += 25;
                }
            },
            new Fallout3Perk
            {
                Name = "Scrounger",
                RequiredLevel = 8,
                MaxRank = 1,
                Description = "Considerably more ammunition in containers",
                RequiredLuck = 5
            },
            new Fallout3Perk
            {
                Name = "Size Matters",
                RequiredLevel = 8,
                MaxRank = 3,
                Description = "+15 Big Guns",
                RequiredEndurance = 5,
                ApplyEffect = c =>
                {
                    c.TryAddSkillPointsFromPerk(nameof(c.BigGuns), 15);
                }
            },
            new Fallout3Perk
            {
                Name = "Strong Back",
                RequiredLevel = 8,
                MaxRank = 1,
                Description = "+50 Carry Weight",
                RequiredStrength = 5,
                RequiredEndurance = 5,
                ApplyEffect = c =>
                {
                    c.CarryWeight += 50;
                }
            },
            new Fallout3Perk
            {
                Name = "Impartial Mediation",
                RequiredLevel = 8,
                MaxRank = 1,
                Description = "+30 Speech when you have neutral Karma",
                RequiredCharisma = 5
            },
            new Fallout3Perk
            {
                Name = "Animal Friend",
                RequiredLevel = 10,
                MaxRank = 2,
                Description = "Hostile animals become friendly (one rank), and come to your aid except against other animals (two ranks).",
                RequiredCharisma = 6
            },
            new Fallout3Perk
            {
                Name = "Finesse",
                RequiredLevel = 10,
                MaxRank = 1,
                Description = "+5% Critical Chance",
                ApplyEffect = c =>
                {
                    c.CriticalChance += 5;
                }
            },
            new Fallout3Perk
            {
                Name = "Mister Sandman",
                RequiredLevel = 10,
                MaxRank = 1,
                Description = "Can instantly kill a sleeping non-player character and earns bonus XP",
                RequiredSneak = 60
            },
            new Fallout3Perk
            {
                Name = "Mysterious Stranger",
                RequiredLevel = 10,
                MaxRank = 1,
                Description = "Chance that the Stranger will finish off a target in V.A.T.S.",
                RequiredLuck = 6
            },
            new Fallout3Perk
            {
                Name = "Nerd Rage!",
                RequiredLevel = 10,
                MaxRank = 1,
                Description = "+50% DR and Strength increased to 10 whenever health is 20% or lower",
                RequiredIntelligence = 5,
                RequiredScience = 50
            },
            new Fallout3Perk
            {
                Name = "Night Person",
                RequiredLevel = 10,
                MaxRank = 1,
                Description = "+2 Intelligence and +2 Perception between 6:00 p.m. and 6:00 a.m."
            },
            new Fallout3Perk
            {
                Name = "Here and Now",
                RequiredLevel = 10,
                MaxRank = 1,
                Description = "You instantly level up again"
            },
            new Fallout3Perk
            {
                Name = "Pyromaniac",
                RequiredLevel = 12,
                MaxRank = 1,
                Description = "+50% damage with fire-based weapons",
                RequiredExplosives = 60
            },
            new Fallout3Perk
            {
                Name = "Cannibal",
                RequiredLevel = 12,
                MaxRank = 1,
                Description = "Can eat corpses to restore 25 points of health and lose 1 Karma"
            },
            new Fallout3Perk
            {
                Name = "Life Giver",
                RequiredLevel = 12,
                MaxRank = 1,
                Description = "+30 Hit Points",
                RequiredEndurance = 6,
                ApplyEffect = c =>
                {
                    c.Health += 30;
                }
            },
            new Fallout3Perk
            {
                Name = "Robotics Expert",
                RequiredLevel = 12,
                MaxRank = 1,
                Description = "+25% damage to robots, can shut down robots by sneaking up on them",
                RequiredScience = 50
            },
            new Fallout3Perk
            {
                Name = "Sniper",
                RequiredLevel = 12,
                MaxRank = 1,
                Description = "+25% to hit the target's head in V.A.T.S.",
                RequiredPerception = 6,
                RequiredAgility = 6
            },
            new Fallout3Perk
            {
                Name = "Silent Running",
                RequiredLevel = 12,
                MaxRank = 1,
                Description = "+10 Sneak, foot speed and armor weight no longer affect sneaking",
                RequiredAgility = 6,
                RequiredSneak = 50,
                ApplyEffect = c =>
                {
                    c.TryAddSkillPointsFromPerk(nameof(c.Sneak), 10);
                }
            },
            new Fallout3Perk
            {
                Name = "Fast Metabolism",
                RequiredLevel = 12,
                MaxRank = 1,
                Description = "+20% Hit Points restored with stimpaks"
            },
            new Fallout3Perk
            {
                Name = "Lawbringer",
                RequiredLevel = 14,
                MaxRank = 1,
                Description = "Can sell fingers to Sonora Cruz"
            },
            new Fallout3Perk
            {
                Name = "Chemist",
                RequiredLevel = 14,
                MaxRank = 1,
                Description = "Chems last twice as long",
                RequiredMedicine = 60
            },
            new Fallout3Perk
            {
                Name = "Contract Killer",
                RequiredLevel = 14,
                MaxRank = 1,
                Description = "Can sell ears to Daniel Littlehorn"
            },
            new Fallout3Perk
            {
                Name = "Cyborg",
                RequiredLevel = 14,
                MaxRank = 1,
                Description = "+10 Energy Weapons, +10% to Radiation Resistance, Poison Resistance and Damage Resistance",
                RequiredMedicine = 60,
                RequiredScience = 60,
                ApplyEffect = c =>
                {
                    c.TryAddSkillPointsFromPerk(nameof(c.EnergyWeapons), 10);
                    c.RadiationResist += 10;
                    c.PoisonResist += 10;
                    c.DamageResist += 10;
                }
            },
            new Fallout3Perk
            {
                Name = "Light Step",
                RequiredLevel = 14,
                MaxRank = 1,
                Description = "Floor traps or mines will not be set off",
                RequiredPerception = 6,
                RequiredAgility = 6
            },
            new Fallout3Perk
            {
                Name = "Master Trader",
                RequiredLevel = 14,
                MaxRank = 1,
                Description = "All items cost 25% less",
                RequiredCharisma = 6,
                RequiredBarter = 60
            },
            new Fallout3Perk
            {
                Name = "Adamantium Skeleton",
                RequiredLevel = 14,
                MaxRank = 1,
                Description = "Damage taken by limbs reduced by 50%"
            },
            new Fallout3Perk
            {
                Name = "Tag!",
                RequiredLevel = 16,
                MaxRank = 1,
                Description = "Fourth \"tag\" skill: +15 points to that skill"
            },
            new Fallout3Perk
            {
                Name = "Better Criticals",
                RequiredLevel = 16,
                MaxRank = 1,
                Description = "+50% damage with critical hits",
                RequiredPerception = 6,
                RequiredLuck = 6
            },
            new Fallout3Perk
            {
                Name = "Action Boy/Action Girl",
                RequiredLevel = 16,
                MaxRank = 1,
                Description = "+25 Action Points",
                RequiredAgility = 6,
                ApplyEffect = c =>
                {
                    // Action Boy/Girl: flat +25 Action Points
                    c.ActionPoints += 25;
                }
            },
            new Fallout3Perk
            {
                Name = "Chem Resistant",
                RequiredLevel = 16,
                MaxRank = 1,
                Description = "Half as likely to get addicted",
                RequiredMedicine = 60
            },
            new Fallout3Perk
            {
                Name = "Infiltrator",
                RequiredLevel = 18,
                MaxRank = 1,
                Description = "Can pick a broken lock with one more bobby pin",
                RequiredPerception = 7,
                RequiredLockpick = 70
            },
            new Fallout3Perk
            {
                Name = "Computer Whiz",
                RequiredLevel = 18,
                MaxRank = 1,
                Description = "Can hack a locked down terminal with four more chances",
                RequiredIntelligence = 7,
                RequiredScience = 70
            },
            new Fallout3Perk
            {
                Name = "Concentrated Fire",
                RequiredLevel = 18,
                MaxRank = 1,
                Description = "+5% accuracy in V.A.T.S. with every attack queued",
                RequiredEnergyWeapons = 60,
                RequiredSmallGuns = 60
            },
            new Fallout3Perk
            {
                Name = "Paralyzing Palm",
                RequiredLevel = 18,
                MaxRank = 1,
                Description = "30% to paralyze an enemy for 30 seconds with an unarmed attack in V.A.T.S.",
                RequiredUnarmed = 70
            },
            new Fallout3Perk
            {
                Name = "Solar Powered",
                RequiredLevel = 20,
                MaxRank = 1,
                Description = "+2 Strength and +1 HP every 10 seconds while outside, from 6:00 a.m. to 6:00 p.m.",
                RequiredEndurance = 7
            },
            new Fallout3Perk
            {
                Name = "Ninja",
                RequiredLevel = 20,
                MaxRank = 1,
                Description = "+15% critical chance with melee and unarmed weapons, +25% damage with sneak attack criticals",
                RequiredMeleeWeapons = 80,
                RequiredSneak = 80
            },
            new Fallout3Perk
            {
                Name = "Grim Reaper's Sprint",
                RequiredLevel = 20,
                MaxRank = 1,
                Description = "A kill in V.A.T.S. restores all your AP immediately"
            },
            new Fallout3Perk
            {
                Name = "Explorer",
                RequiredLevel = 20,
                MaxRank = 1,
                Description = "All locations are marked on your map"
            },
            new Fallout3Perk
            {
                Name = "Deep Sleep",
                RequiredLevel = 22,
                MaxRank = 1,
                Description = "Sleeping in any bed gives the Well Rested effect, which increases your XP by 10%, for 8 hours."
            },
            new Fallout3Perk
            {
                Name = "Puppies!",
                RequiredLevel = 22,
                MaxRank = 1,
                Description = "If Dogmeat dies, a puppy will spawn outside of Vault 101"
            },
            new Fallout3Perk
            {
                Name = "Quantum Chemist",
                RequiredLevel = 22,
                MaxRank = 1,
                Description = "Every 10 Nuka-Colas you acquire are automatically converted into a Nuka-Cola Quantum",
                RequiredScience = 70
            },
            new Fallout3Perk
            {
                Name = "Devil's Highway",
                RequiredLevel = 24,
                MaxRank = 1,
                Description = "Karma is instantly set to Very Evil",
                KarmaRequirementNote = "Karma higher than Very Evil"
            },
            new Fallout3Perk
            {
                Name = "Escalator to Heaven",
                RequiredLevel = 24,
                MaxRank = 1,
                Description = "Karma is instantly set to Very Good",
                KarmaRequirementNote = "Karma lower than Very Good"
            },
            new Fallout3Perk
            {
                Name = "Karmic Rebalance",
                RequiredLevel = 24,
                MaxRank = 1,
                Description = "Karma is instantly set to Neutral",
                KarmaRequirementNote = "Karma not neutral"
            },
            new Fallout3Perk
            {
                Name = "No Weaknesses",
                RequiredLevel = 24,
                MaxRank = 1,
                Description = "All SPECIAL stats lower than 5 become 5",
                ApplyEffect = c => c.ApplySpecialFloor(5)
            },
            new Fallout3Perk
            {
                Name = "Rad Tolerance",
                RequiredLevel = 26,
                MaxRank = 1,
                Description = "No effects from minor radiation poisoning (still gives alert)",
                RequiredEndurance = 7
            },
            new Fallout3Perk
            {
                Name = "Warmonger",
                RequiredLevel = 26,
                MaxRank = 1,
                Description = "All schematics are added at V3",
                RequiredIntelligence = 7
            },
            new Fallout3Perk
            {
                Name = "Nerves of Steel",
                RequiredLevel = 26,
                MaxRank = 1,
                Description = "+1 AP every 10 seconds",
                RequiredAgility = 7
            },
            new Fallout3Perk
            {
                Name = "Party Girl/Party Boy",
                RequiredLevel = 28,
                MaxRank = 1,
                Description = "No chance for alcohol addiction"
            },
            new Fallout3Perk
            {
                Name = "Rad Absorption",
                RequiredLevel = 28,
                MaxRank = 1,
                Description = "-1 Rad every 20 seconds",
                RequiredEndurance = 7
            },
            new Fallout3Perk
            {
                Name = "Nuclear Anomaly",
                RequiredLevel = 30,
                MaxRank = 1,
                Description = "Whenever your health is reduced to 20 or less, you will erupt into a devastating nuclear explosion"
            },
            new Fallout3Perk
            {
                Name = "Almost Perfect",
                RequiredLevel = 30,
                MaxRank = 1,
                Description = "All SPECIAL stats are raised to 9",
                ApplyEffect = c => c.ApplySpecialCap(9)
            },
        };
    }
}
