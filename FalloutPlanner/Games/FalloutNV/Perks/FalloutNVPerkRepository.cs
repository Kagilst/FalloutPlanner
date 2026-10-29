using System.Collections.Generic;

namespace FalloutPlanner.Games.FalloutNV.Perks
{
    /// <summary>
    /// FNV Leveling Perks - characters choose 1 perk on every EVEN level.
    /// Max level is 50 (30 with Logan's Loophole trait).
    /// </summary>
    public static class FalloutNVPerkRepository
    {
        public static List<FalloutNVPerk> AllPerks { get; } = new()
        {
            // =====================================================
            // Level 2 Perks
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Black Widow",
                RequiredLevel = 2,
                MaxRank = 1,
                Description = "+10% damage to the opposite sex and unique dialogue options with certain characters.",
                Requirement = c => !c.IsMale
            },
            new FalloutNVPerk
            {
                Name = "Lady Killer",
                RequiredLevel = 2,
                MaxRank = 1,
                Description = "+10% damage to the opposite sex and unique dialogue options with certain characters.",
                Requirement = c => c.IsMale
            },
            new FalloutNVPerk
            {
                Name = "Cherchez La Femme",
                RequiredLevel = 2,
                MaxRank = 1,
                Description = "+10% damage to the same sex and unique dialogue options with certain characters.",
                Requirement = c => !c.IsMale
            },
            new FalloutNVPerk
            {
                Name = "Confirmed Bachelor",
                RequiredLevel = 2,
                MaxRank = 1,
                Description = "+10% damage to the same sex and unique dialogue options with certain characters.",
                Requirement = c => c.IsMale
            },
            new FalloutNVPerk
            {
                Name = "Friend of the Night",
                RequiredLevel = 2,
                RequiredPerception = 6,
                RequiredSneak = 30,
                MaxRank = 1,
                Description = "Your eyes adapt quickly to low-light conditions."
            },
            new FalloutNVPerk
            {
                Name = "Heave, Ho!",
                RequiredLevel = 2,
                RequiredStrength = 5,
                RequiredExplosives = 30,
                MaxRank = 1,
                Description = "+50% thrown weapon velocity and range."
            },
            new FalloutNVPerk
            {
                Name = "Hunter",
                RequiredLevel = 2,
                RequiredSurvival = 30,
                MaxRank = 1,
                Description = "In combat, you do 75% more critical damage against animals and mutated animals."
            },
            new FalloutNVPerk
            {
                Name = "Intense Training",
                RequiredLevel = 2,
                MaxRank = 10,
                Description = "You can put a single point into any of your SPECIAL attributes."
            },
            new FalloutNVPerk
            {
                Name = "Rapid Reload",
                RequiredLevel = 2,
                RequiredAgility = 5,
                RequiredGuns = 30,
                MaxRank = 1,
                Description = "All of your weapon reloads are 25% faster than normal."
            },
            new FalloutNVPerk
            {
                Name = "Retention",
                RequiredLevel = 2,
                RequiredIntelligence = 5,
                MaxRank = 1,
                Description = "Skill magazines last for 3 real-time minutes."
            },
            new FalloutNVPerk
            {
                Name = "Swift Learner",
                RequiredLevel = 2,
                RequiredIntelligence = 4,
                MaxRank = 3,
                Description = "You gain an additional 10% whenever experience points are earned."
            },
            // DLC Level 2 Perks
            new FalloutNVPerk
            {
                Name = "In Shining Armor",
                RequiredLevel = 2,
                RequiredRepair = 20,
                RequiredScience = 70,
                MaxRank = 1,
                Description = "+5 DT against energy weapons while wearing metal armor, +2 DT with reflective eyewear/helmets. (Note: Perk is bugged and does not function.)"
            },
            new FalloutNVPerk
            {
                Name = "Junk Rounds",
                RequiredLevel = 2,
                RequiredLuck = 6,
                RequiredRepair = 45,
                MaxRank = 1,
                Description = "You can craft ammunition using scrap metal and tin cans."
            },
            new FalloutNVPerk
            {
                Name = "Light Touch",
                RequiredLevel = 2,
                RequiredAgility = 6,
                RequiredRepair = 45,
                MaxRank = 1,
                Description = "While wearing light armor you gain +5% critical hit chance and your enemies suffer a -25% critical hit chance."
            },
            new FalloutNVPerk
            {
                Name = "Old World Gourmet",
                RequiredLevel = 2,
                RequiredEndurance = 6,
                RequiredSurvival = 45,
                MaxRank = 1,
                Description = "+25% addiction resistance. +50% health bonus from snack foods. Scotch, vodka and wine now give you health in addition to their normal effects."
            },

            // =====================================================
            // Level 4 Perks
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Cannibal",
                RequiredLevel = 4,
                MaxRank = 1,
                Description = "When you're in Sneak mode, you gain the option to eat a human corpse to regain hit points, but lose Karma."
            },
            new FalloutNVPerk
            {
                Name = "Comprehension",
                RequiredLevel = 4,
                RequiredIntelligence = 4,
                MaxRank = 1,
                Description = "You gain one additional skill point for reading books and double the skill points for reading magazines."
            },
            new FalloutNVPerk
            {
                Name = "Educated",
                RequiredLevel = 4,
                RequiredIntelligence = 4,
                MaxRank = 1,
                Description = "You gain two more skill points every time you advance in level."
            },
            new FalloutNVPerk
            {
                Name = "Entomologist",
                RequiredLevel = 4,
                RequiredIntelligence = 4,
                RequiredSurvival = 45,
                MaxRank = 1,
                Description = "You do an additional 50% damage every time you attack a mutated insect."
            },
            new FalloutNVPerk
            {
                Name = "Rad Child",
                RequiredLevel = 4,
                RequiredSurvival = 70,
                MaxRank = 1,
                Description = "Regenerate 2 HP per second per 200 rads accumulated."
            },
            new FalloutNVPerk
            {
                Name = "Run 'n Gun",
                RequiredLevel = 4,
                MaxRank = 1,
                Description = "Halved spread with one-handed ranged weapons while walking or running.",
                Requirement = c => c.Guns >= 45 || c.EnergyWeapons >= 45
            },
            new FalloutNVPerk
            {
                Name = "Travel Light",
                RequiredLevel = 4,
                RequiredSurvival = 45,
                MaxRank = 1,
                Description = "While wearing light armor or no armor, you run 10% faster."
            },

            // =====================================================
            // Level 6 Perks
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Bloody Mess",
                RequiredLevel = 6,
                MaxRank = 1,
                Description = "+5% overall damage; more violent death animations."
            },
            new FalloutNVPerk
            {
                Name = "Demolition Expert",
                RequiredLevel = 6,
                RequiredExplosives = 50,
                MaxRank = 3,
                Description = "+20% damage with explosives."
            },
            new FalloutNVPerk
            {
                Name = "Ferocious Loyalty",
                RequiredLevel = 6,
                RequiredCharisma = 6,
                MaxRank = 1,
                Description = "When you drop below 50% HP, companions gain +50 DR."
            },
            new FalloutNVPerk
            {
                Name = "Fortune Finder",
                RequiredLevel = 6,
                RequiredLuck = 5,
                MaxRank = 1,
                Description = "Considerably more bottle caps will be found in stockpiles."
            },
            new FalloutNVPerk
            {
                Name = "Gunslinger",
                RequiredLevel = 6,
                MaxRank = 1,
                Description = "+25% accuracy in V.A.T.S. with one-handed weapons."
            },
            new FalloutNVPerk
            {
                Name = "Hand Loader",
                RequiredLevel = 6,
                RequiredRepair = 70,
                MaxRank = 1,
                Description = "When using Guns, you are twice as likely to recover cases and hulls. You also have all hand load recipes unlocked."
            },
            new FalloutNVPerk
            {
                Name = "Lead Belly",
                RequiredLevel = 6,
                MaxRank = 1,
                Description = "-50% radiation taken from food and water sources.",
                Requirement = c => c.Endurance >= 5 || c.Survival >= 40
            },
            new FalloutNVPerk
            {
                Name = "Shotgun Surgeon",
                RequiredLevel = 6,
                RequiredGuns = 45,
                MaxRank = 1,
                Description = "When using shotguns, regardless of ammunition used, you ignore an additional 10 points of a target's Damage Threshold."
            },
            new FalloutNVPerk
            {
                Name = "The Professional",
                RequiredLevel = 6,
                RequiredSneak = 70,
                MaxRank = 1,
                Description = "Your Sneak Attack Criticals with revolvers, pistols, and submachine guns inflict an additional 20% damage."
            },
            new FalloutNVPerk
            {
                Name = "Toughness",
                RequiredLevel = 6,
                RequiredEndurance = 5,
                MaxRank = 2,
                Description = "+3 DT permanently.",
                ApplyEffect = c => c.DamageThreshold += 3
            },
            new FalloutNVPerk
            {
                Name = "Vigilant Recycler",
                RequiredLevel = 6,
                RequiredScience = 70,
                MaxRank = 1,
                Description = "When using Energy Weapons, you are twice as likely to recover drained ammunition."
            },
            // DLC Level 6 Perks
            new FalloutNVPerk
            {
                Name = "Mad Bomber",
                RequiredLevel = 6,
                RequiredRepair = 45,
                RequiredExplosives = 45,
                MaxRank = 1,
                Description = "Enables you to create special explosive recipes at any workbench."
            },

            // =====================================================
            // Level 8 Perks
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Commando",
                RequiredLevel = 8,
                MaxRank = 1,
                Description = "+25% accuracy in V.A.T.S. with two-handed weapons."
            },
            new FalloutNVPerk
            {
                Name = "Cowboy",
                RequiredLevel = 8,
                RequiredGuns = 45,
                RequiredMeleeWeapons = 45,
                MaxRank = 1,
                Description = "+25% damage done by dynamite, hatchets, knives, revolvers, and lever-action guns."
            },
            new FalloutNVPerk
            {
                Name = "Living Anatomy",
                RequiredLevel = 8,
                RequiredMedicine = 70,
                MaxRank = 1,
                Description = "Shows health and Damage Threshold of any target. +5% bonus to damage against humans and non-feral ghouls."
            },
            new FalloutNVPerk
            {
                Name = "Pack Rat",
                RequiredLevel = 8,
                RequiredIntelligence = 5,
                RequiredBarter = 70,
                MaxRank = 1,
                Description = "Items with a weight of two pounds or less now weigh half as much."
            },
            new FalloutNVPerk
            {
                Name = "Quick Draw",
                RequiredLevel = 8,
                RequiredAgility = 5,
                MaxRank = 1,
                Description = "Makes weapon equipping and holstering 50% faster."
            },
            new FalloutNVPerk
            {
                Name = "Rad Resistance",
                RequiredLevel = 8,
                RequiredEndurance = 5,
                RequiredSurvival = 40,
                MaxRank = 1,
                Description = "+25% radiation resistance permanently.",
                ApplyEffect = c => c.RadiationResist += 25
            },
            new FalloutNVPerk
            {
                Name = "Scrounger",
                RequiredLevel = 8,
                RequiredLuck = 5,
                MaxRank = 1,
                Description = "Considerably more ammunition in stockpiles."
            },
            new FalloutNVPerk
            {
                Name = "Stonewall",
                RequiredLevel = 8,
                RequiredStrength = 6,
                RequiredEndurance = 6,
                MaxRank = 1,
                Description = "+5 DT against melee and unarmed attacks and cannot be knocked down during combat.",
                ApplyEffect = c => c.DamageThreshold += 5
            },
            new FalloutNVPerk
            {
                Name = "Strong Back",
                RequiredLevel = 8,
                RequiredStrength = 5,
                RequiredEndurance = 5,
                MaxRank = 1,
                Description = "+50 Carry Weight.",
                ApplyEffect = c => c.CarryWeight += 50
            },
            new FalloutNVPerk
            {
                Name = "Super Slam!",
                RequiredLevel = 8,
                RequiredStrength = 6,
                RequiredMeleeWeapons = 45,
                MaxRank = 1,
                Description = "All melee and unarmed attacks have a chance of knocking your target down. 15% for Unarmed or one-handed melee, 30% for two-handed melee."
            },
            new FalloutNVPerk
            {
                Name = "Terrifying Presence",
                RequiredLevel = 8,
                RequiredSpeech = 70,
                MaxRank = 1,
                Description = "Can intimidate foes through dialogue; closing dialogue results in the foe fleeing for 5 seconds."
            },
            // DLC Level 8 Perks
            new FalloutNVPerk
            {
                Name = "Grunt",
                RequiredLevel = 8,
                RequiredGuns = 45,
                RequiredExplosives = 20,
                MaxRank = 1,
                Description = "25% more damage with 9mm pistols and SMGs, .45 pistols and SMGs, service rifles, assault and Marksman carbines, light machine guns, frag grenades, grenade rifles and launchers and combat knives."
            },
            new FalloutNVPerk
            {
                Name = "Home on the Range",
                RequiredLevel = 8,
                RequiredSurvival = 70,
                MaxRank = 1,
                Description = "Whenever you interact with a campfire, you have the option of sleeping, with all the benefits that sleep brings."
            },
            new FalloutNVPerk
            {
                Name = "Sneering Imperialist",
                RequiredLevel = 8,
                MaxRank = 1,
                Description = "+15% Damage and +25% accuracy in V.A.T.S. against various tribal and raider characters."
            },
            new FalloutNVPerk
            {
                Name = "Tribal Wisdom",
                RequiredLevel = 8,
                RequiredSurvival = 70,
                MaxRank = 1,
                Description = "-50% limb damage from animals, mutated animals, and mutated insects, +25% to Poison Resistance, ability to eat mutated insects in sneak mode."
            },

            // =====================================================
            // Level 10 Perks
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Here and Now",
                RequiredLevel = 10,
                MaxRank = 1,
                Description = "You instantly level up again."
            },
            new FalloutNVPerk
            {
                Name = "Animal Friend",
                RequiredLevel = 10,
                RequiredCharisma = 6,
                RequiredSurvival = 45,
                MaxRank = 2,
                Description = "With the first rank, hostile animals become neutral. With the second rank, they assist in combat."
            },
            new FalloutNVPerk
            {
                Name = "Finesse",
                RequiredLevel = 10,
                MaxRank = 1,
                Description = "+5% Critical Chance.",
                ApplyEffect = c => c.CriticalChance += 5
            },
            new FalloutNVPerk
            {
                Name = "Math Wrath",
                RequiredLevel = 10,
                RequiredScience = 70,
                MaxRank = 1,
                Description = "Reduces all AP costs by 10%."
            },
            new FalloutNVPerk
            {
                Name = "Miss Fortune",
                RequiredLevel = 10,
                RequiredLuck = 6,
                MaxRank = 1,
                Description = "10% chance that Miss Fortune will incapacitate a target in V.A.T.S."
            },
            new FalloutNVPerk
            {
                Name = "Mister Sandman",
                RequiredLevel = 10,
                RequiredSneak = 60,
                MaxRank = 1,
                Description = "Can instantly kill a sleeping non-player character and earn bonus XP when doing so."
            },
            new FalloutNVPerk
            {
                Name = "Mysterious Stranger",
                RequiredLevel = 10,
                RequiredLuck = 6,
                MaxRank = 1,
                Description = "10% chance that the Mysterious Stranger will finish off a target in V.A.T.S."
            },
            new FalloutNVPerk
            {
                Name = "Nerd Rage!",
                RequiredLevel = 10,
                RequiredIntelligence = 5,
                RequiredScience = 50,
                MaxRank = 1,
                Description = "+15 DT and Strength increased to 10 whenever health is 20% or lower."
            },
            new FalloutNVPerk
            {
                Name = "Night Person",
                RequiredLevel = 10,
                MaxRank = 1,
                Description = "+2 Intelligence and +2 Perception between 6:00 P.M. and 6:00 A.M."
            },
            new FalloutNVPerk
            {
                Name = "Plasma Spaz",
                RequiredLevel = 10,
                RequiredEnergyWeapons = 70,
                MaxRank = 1,
                Description = "AP costs for all plasma weapons are reduced by 20%."
            },
            // DLC Level 10 Perks
            new FalloutNVPerk
            {
                Name = "And Stay Back",
                RequiredLevel = 10,
                RequiredGuns = 70,
                MaxRank = 1,
                Description = "Shotguns have a 10% chance, per pellet, of knocking an enemy back in a way similar to the Super Slam! perk."
            },
            new FalloutNVPerk
            {
                Name = "Fight the Power!",
                RequiredLevel = 10,
                MaxRank = 1,
                Description = "+2 Damage Threshold and +5% Critical Chance against anyone wearing NCR, Legion, or Brotherhood of Steel armor."
            },

            // =====================================================
            // Level 12 Perks
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Fast Metabolism",
                RequiredLevel = 12,
                MaxRank = 1,
                Description = "+20% Hit Points restored with stimpaks."
            },
            new FalloutNVPerk
            {
                Name = "Ghastly Scavenger",
                RequiredLevel = 12,
                MaxRank = 1,
                Description = "When you're in Sneak mode, you can eat super mutant or feral ghoul corpses to regain hit points.",
                Requirement = c => c.GetPerkRank("Cannibal") > 0
            },
            new FalloutNVPerk
            {
                Name = "Hit the Deck",
                RequiredLevel = 12,
                RequiredExplosives = 70,
                MaxRank = 1,
                Description = "+25 DT against explosives."
            },
            new FalloutNVPerk
            {
                Name = "Life Giver",
                RequiredLevel = 12,
                RequiredEndurance = 6,
                MaxRank = 1,
                Description = "+30 hit points.",
                ApplyEffect = c => c.Health += 30
            },
            new FalloutNVPerk
            {
                Name = "Long Haul",
                RequiredLevel = 12,
                RequiredEndurance = 6,
                RequiredBarter = 70,
                MaxRank = 1,
                Description = "Being over-encumbered no longer prevents you from using fast travel."
            },
            new FalloutNVPerk
            {
                Name = "Piercing Strike",
                RequiredLevel = 12,
                RequiredUnarmed = 70,
                MaxRank = 1,
                Description = "All your unarmed and melee attacks negate 15 points of DT."
            },
            new FalloutNVPerk
            {
                Name = "Pyromaniac",
                RequiredLevel = 12,
                RequiredExplosives = 60,
                MaxRank = 1,
                Description = "+50% damage with fire-based weapons."
            },
            new FalloutNVPerk
            {
                Name = "Robotics Expert",
                RequiredLevel = 12,
                RequiredScience = 50,
                MaxRank = 1,
                Description = "+25% damage to robots; can shut down robots by sneaking up on them."
            },
            new FalloutNVPerk
            {
                Name = "Silent Running",
                RequiredLevel = 12,
                RequiredAgility = 6,
                RequiredSneak = 50,
                MaxRank = 1,
                Description = "Running no longer factors into a successful sneak attempt."
            },
            new FalloutNVPerk
            {
                Name = "Sniper",
                RequiredLevel = 12,
                RequiredPerception = 6,
                RequiredAgility = 6,
                MaxRank = 1,
                Description = "25% more likely to hit the target's head in V.A.T.S."
            },
            new FalloutNVPerk
            {
                Name = "Splash Damage",
                RequiredLevel = 12,
                RequiredExplosives = 70,
                MaxRank = 1,
                Description = "Explosives have a 25% larger area of effect."
            },
            new FalloutNVPerk
            {
                Name = "Unstoppable Force",
                RequiredLevel = 12,
                RequiredStrength = 7,
                RequiredMeleeWeapons = 90,
                MaxRank = 1,
                Description = "x4 normal damage through enemy Blocks with melee and unarmed attacks."
            },
            // DLC Level 12 Perks
            new FalloutNVPerk
            {
                Name = "Alertness",
                RequiredLevel = 12,
                MaxRank = 1,
                Description = "+2 Perception when crouched and still.",
                Requirement = c => c.Perception >= 6 && c.Perception <= 9
            },
            new FalloutNVPerk
            {
                Name = "Heavyweight",
                RequiredLevel = 12,
                RequiredStrength = 7,
                MaxRank = 1,
                Description = "Weapons with a weight of more than 10 have their weight cut in half. This does not affect weapons modded to be under 10 weight."
            },
            new FalloutNVPerk
            {
                Name = "Hobbler",
                RequiredLevel = 12,
                RequiredPerception = 7,
                MaxRank = 1,
                Description = "Your chance to hit an opponent's legs in V.A.T.S. is increased by 25%."
            },

            // =====================================================
            // Level 14 Perks
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Adamantium Skeleton",
                RequiredLevel = 14,
                MaxRank = 1,
                Description = "Damage taken by limbs reduced by 50%."
            },
            new FalloutNVPerk
            {
                Name = "Center of Mass",
                RequiredLevel = 14,
                RequiredGuns = 70,
                MaxRank = 1,
                Description = "In V.A.T.S., you do an additional 15% damage when targeting the torso."
            },
            new FalloutNVPerk
            {
                Name = "Chemist",
                RequiredLevel = 14,
                RequiredMedicine = 60,
                MaxRank = 1,
                Description = "Chems and (in Hardcore) stimpaks last twice as long."
            },
            new FalloutNVPerk
            {
                Name = "Jury Rigging",
                RequiredLevel = 14,
                RequiredRepair = 90,
                MaxRank = 1,
                Description = "Repair any item using a roughly similar item."
            },
            new FalloutNVPerk
            {
                Name = "Light Step",
                RequiredLevel = 14,
                RequiredPerception = 6,
                RequiredAgility = 6,
                MaxRank = 1,
                Description = "Floor traps or mines will not be set off."
            },
            new FalloutNVPerk
            {
                Name = "Purifier",
                RequiredLevel = 14,
                MaxRank = 1,
                Description = "You do 50% extra damage with melee and Unarmed weapons against centaurs, night stalkers, spore plants, spore carriers, deathclaws and super mutants."
            },

            // =====================================================
            // Level 16 Perks
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Action Boy",
                RequiredLevel = 16,
                RequiredAgility = 6,
                MaxRank = 2,
                Description = "+15 Action Points.",
                ApplyEffect = c => c.ActionPoints += 15,
                Requirement = c => c.IsMale
            },
            new FalloutNVPerk
            {
                Name = "Action Girl",
                RequiredLevel = 16,
                RequiredAgility = 6,
                MaxRank = 2,
                Description = "+15 Action Points.",
                ApplyEffect = c => c.ActionPoints += 15,
                Requirement = c => !c.IsMale
            },
            new FalloutNVPerk
            {
                Name = "Better Criticals",
                RequiredLevel = 16,
                RequiredPerception = 6,
                RequiredLuck = 6,
                MaxRank = 1,
                Description = "+50% damage with critical hits."
            },
            new FalloutNVPerk
            {
                Name = "Chem Resistant",
                RequiredLevel = 16,
                RequiredMedicine = 60,
                MaxRank = 1,
                Description = "Half as likely to get addicted."
            },
            new FalloutNVPerk
            {
                Name = "Meltdown",
                RequiredLevel = 16,
                RequiredEnergyWeapons = 90,
                MaxRank = 1,
                Description = "Foes killed by your Energy Weapons emit a corona of harmful energy."
            },
            new FalloutNVPerk
            {
                Name = "Tag!",
                RequiredLevel = 16,
                MaxRank = 1,
                Description = "Fourth 'tag' skill: +15 points to that skill."
            },
            new FalloutNVPerk
            {
                Name = "Weapon Handling",
                RequiredLevel = 16,
                MaxRank = 1,
                Description = "Weapon Strength requirements are now 2 points lower than normal for you.",
                Requirement = c => c.Strength < 10
            },

            // =====================================================
            // Level 18 Perks
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Computer Whiz",
                RequiredLevel = 18,
                RequiredIntelligence = 7,
                RequiredScience = 70,
                MaxRank = 1,
                Description = "Can make one extra attempt to hack a locked-down terminal."
            },
            new FalloutNVPerk
            {
                Name = "Concentrated Fire",
                RequiredLevel = 18,
                RequiredEnergyWeapons = 60,
                RequiredGuns = 60,
                MaxRank = 1,
                Description = "+5% accuracy in V.A.T.S. with every subsequent attack on a given body part queued."
            },
            new FalloutNVPerk
            {
                Name = "Infiltrator",
                RequiredLevel = 18,
                RequiredPerception = 7,
                RequiredLockpick = 70,
                MaxRank = 1,
                Description = "Can make one more attempt to pick a broken lock."
            },
            new FalloutNVPerk
            {
                Name = "Paralyzing Palm",
                RequiredLevel = 18,
                RequiredUnarmed = 70,
                MaxRank = 1,
                Description = "Can paralyze an enemy for 30 seconds with a V.A.T.S. unarmed attack."
            },
            // DLC Level 18 Perks
            new FalloutNVPerk
            {
                Name = "Walker Instinct",
                RequiredLevel = 18,
                RequiredSurvival = 50,
                MaxRank = 1,
                Description = "+1 Perception and Agility when outside."
            },

            // =====================================================
            // Level 20 Perks
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Explorer",
                RequiredLevel = 20,
                MaxRank = 1,
                Description = "All locations are marked on your map."
            },
            new FalloutNVPerk
            {
                Name = "Grim Reaper's Sprint",
                RequiredLevel = 20,
                MaxRank = 1,
                Description = "A kill in V.A.T.S. restores 20 AP immediately."
            },
            new FalloutNVPerk
            {
                Name = "Ninja",
                RequiredLevel = 20,
                RequiredMeleeWeapons = 80,
                RequiredSneak = 80,
                MaxRank = 1,
                Description = "Critical chance multiplied by x1.15 with melee and unarmed weapons, +25% damage with sneak attack criticals."
            },
            new FalloutNVPerk
            {
                Name = "Solar Powered",
                RequiredLevel = 20,
                RequiredEndurance = 7,
                MaxRank = 1,
                Description = "+2 Strength and +1 HP per second while outside, from 6:00 A.M. to 6:00 P.M."
            },
            // DLC Level 20 Perks
            new FalloutNVPerk
            {
                Name = "Eye for Eye",
                RequiredLevel = 20,
                MaxRank = 1,
                Description = "For each crippled limb you have, you do an additional 10% damage."
            },
            new FalloutNVPerk
            {
                Name = "Atomic!",
                RequiredLevel = 20,
                RequiredEndurance = 6,
                MaxRank = 1,
                Description = "In irradiated areas, +25% move and attack speed, +2 DT, +2 ST. With excess rad level, AP regen scales from 1.1 times to 1.5 times normal."
            },
            new FalloutNVPerk
            {
                Name = "Mile in Their Shoes",
                RequiredLevel = 20,
                RequiredSurvival = 25,
                MaxRank = 1,
                Description = "You have come to understand night stalkers. Consuming night stalker squeezin's now grants bonuses to Perception (+1 PER), Poison Resistance (+5), and Stealth (+5 Sneak) in addition to the normal benefits."
            },
            new FalloutNVPerk
            {
                Name = "Them's Good Eatin'",
                RequiredLevel = 20,
                RequiredSurvival = 55,
                MaxRank = 1,
                Description = "Any living creature you kill has a 50% chance to have the potent healing items thin red paste or blood sausage when looted."
            },

            // =====================================================
            // Level 22 Perks
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Laser Commander",
                RequiredLevel = 22,
                RequiredEnergyWeapons = 90,
                MaxRank = 1,
                Description = "You do an extra 15% damage and have a 10% extra chance to critically hit with any laser weapon."
            },
            new FalloutNVPerk
            {
                Name = "Nuka Chemist",
                RequiredLevel = 22,
                RequiredScience = 90,
                MaxRank = 1,
                Description = "Unlocks special Nuka-Cola recipes at the workbench."
            },
            new FalloutNVPerk
            {
                Name = "Spray and Pray",
                RequiredLevel = 22,
                MaxRank = 1,
                Description = "Your attacks do 75% less damage to companions."
            },
            // DLC Level 22 Perks
            new FalloutNVPerk
            {
                Name = "Irradiated Beauty",
                RequiredLevel = 22,
                RequiredEndurance = 8,
                MaxRank = 1,
                Description = "Sleep removes all Rads (Hardcore: only -100 Rads)."
            },
            new FalloutNVPerk
            {
                Name = "Voracious Reader",
                RequiredLevel = 22,
                RequiredIntelligence = 7,
                MaxRank = 1,
                Description = "Damaged books become blank magazines; can copy existing magazines into blank magazines."
            },

            // =====================================================
            // Level 24 Perks
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Slayer",
                RequiredLevel = 24,
                RequiredAgility = 7,
                RequiredUnarmed = 90,
                MaxRank = 1,
                Description = "The speed of all your melee and unarmed attacks is increased by 30%."
            },

            // =====================================================
            // Level 26 Perks
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Nerves of Steel",
                RequiredLevel = 26,
                RequiredAgility = 7,
                MaxRank = 1,
                Description = "20% faster AP regeneration."
            },
            // DLC Level 26 Perks
            new FalloutNVPerk
            {
                Name = "Lessons Learned",
                RequiredLevel = 26,
                RequiredIntelligence = 6,
                MaxRank = 1,
                Description = "+1% XP gain per player level."
            },
            new FalloutNVPerk
            {
                Name = "Tunnel Runner",
                RequiredLevel = 26,
                RequiredAgility = 8,
                MaxRank = 1,
                Description = "+25% sneaking speed when wearing light or no armor."
            },

            // =====================================================
            // Level 28 Perks
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Rad Absorption",
                RequiredLevel = 28,
                RequiredEndurance = 7,
                MaxRank = 1,
                Description = "-1 Rad every 20 seconds."
            },
            // DLC Level 28 Perks
            new FalloutNVPerk
            {
                Name = "Roughin' It",
                RequiredLevel = 28,
                RequiredSurvival = 100,
                MaxRank = 1,
                Description = "Sleeping outside gives Well Rested benefit."
            },

            // =====================================================
            // Level 30 Perks
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Implant GRX",
                RequiredLevel = 30,
                RequiredEndurance = 8,
                MaxRank = 2,
                Description = "You gain a non-addictive subdermal turbo injector. Rank 2 increases effect from 2 to 3 seconds and uses per day from 5 to 10."
            },
            // DLC Level 30 Perks
            new FalloutNVPerk
            {
                Name = "Burden to Bear",
                RequiredLevel = 30,
                RequiredStrength = 6,
                RequiredEndurance = 6,
                MaxRank = 1,
                Description = "+50 carry weight.",
                ApplyEffect = c => c.CarryWeight += 50
            },

            // =====================================================
            // Level 36 Perks (DLC)
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Broad Daylight",
                RequiredLevel = 36,
                MaxRank = 1,
                Description = "No Sneak penalty for using Pip-Boy light."
            },

            // =====================================================
            // Level 40 Perks (DLC)
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Certified Tech",
                RequiredLevel = 40,
                MaxRank = 1,
                Description = "+25% critical hit chance against robots, 85% chance of finding an extra crafting component on destroyed robots."
            },

            // =====================================================
            // Level 50 Perks (DLC)
            // =====================================================
            new FalloutNVPerk
            {
                Name = "Ain't Like That Now",
                RequiredLevel = 50,
                MaxRank = 1,
                Description = "Karma reset to 0, +25% AP regeneration rate, +20% attack speed, immunity to critical hits.",
                KarmaRequirementNote = "Karma < -250 (Evil)"
            },
            new FalloutNVPerk
            {
                Name = "Just Lucky I'm Alive",
                RequiredLevel = 50,
                MaxRank = 1,
                Description = "+4 Luck for 3 minutes upon finishing a battle with less than 25% health; immunity to critical hits, +50% critical damage.",
                KarmaRequirementNote = "Karma -250 to 250 (Neutral)"
            },
            new FalloutNVPerk
            {
                Name = "Thought You Died",
                RequiredLevel = 50,
                MaxRank = 1,
                Description = "+10 Health per 100 Karma; Karma reset to 0, +10% damage, immunity to critical hits.",
                KarmaRequirementNote = "Karma >= 250 (Good)"
            },
        };
    }
}
