using System.Collections.Generic;
using System.Linq;

namespace FalloutPlanner.Games.Fallout4.Perks
{
    public static class Fallout4PerkRepository
    {
        public static IReadOnlyList<Fallout4Perk> AllPerks { get; } = CreatePerks();

        public static IEnumerable<Fallout4Perk> GetByAttribute(Fallout4SpecialAttribute attribute) =>
            AllPerks.Where(p => p.Attribute == attribute).OrderBy(p => p.AttributeRequirement);

        public static Fallout4Perk? GetPerk(string name) =>
            AllPerks.FirstOrDefault(p => p.Name == name);

        private static List<Fallout4Perk> CreatePerks()
        {
            return new List<Fallout4Perk>
            {
                new()
                {
                    Name = "Iron Fist",
                    Attribute = Fallout4SpecialAttribute.Strength,
                    AttributeRequirement = 1,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Channel your chi to unleash devastating fury! Punching attacks do 20% more damage to your opponent."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 9,
                            Description = "Punching attacks now do 40% more damage and can disarm your opponent."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 18,
                            Description = "Punching attacks now do 60% more damage. Unarmed Power Attacks have a chance to cripple one of your opponent's limbs."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 31,
                            Description = "Punching attacks now do 80% more damage. Unarmed Power Attacks have an increased chance to cripple one of your opponent's limbs."
                        },
                        new()
                        {
                            Rank = 5,
                            LevelRequirement = 46,
                            Description = "Punching attacks now do double damage. Criticals in V.A.T.S. will paralyze your opponent."
                        },
                    ]
                },
                new()
                {
                    Name = "Big Leagues",
                    Attribute = Fallout4SpecialAttribute.Strength,
                    AttributeRequirement = 2,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Swing for the fences! Do 20% more melee weapon damage."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 7,
                            Description = "You now do 40% more melee weapon damage and gain a chance to disarm your opponent."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 15,
                            Description = "You now do 60% more melee weapon damage and gain an increased chance to disarm your opponent."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 27,
                            Description = "You now do 80% more melee weapon damage and hit all targets in front of you."
                        },
                        new()
                        {
                            Rank = 5,
                            LevelRequirement = 42,
                            Description = "You now do double damage with a melee weapon, and gain a chance to cripple your opponent, or grand slam their head clean off!"
                        },
                    ]
                },
                new()
                {
                    Name = "Armorer",
                    Attribute = Fallout4SpecialAttribute.Strength,
                    AttributeRequirement = 3,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Protect yourself from the dangers of the Wasteland with access to base level and Rank 1 armor mods."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 13,
                            Description = "You gain access to Rank 2 armor mods."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 25,
                            Description = "You gain access to Rank 3 armor mods."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 39,
                            Description = "You gain access to Rank 4 armor mods."
                        },
                    ]
                },
                new()
                {
                    Name = "Blacksmith",
                    Attribute = Fallout4SpecialAttribute.Strength,
                    AttributeRequirement = 4,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Fire up the forge and gain access to base level and Rank 1 melee weapon mods."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 16,
                            Description = "You gain access to Rank 2 melee weapon mods."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 29,
                            Description = "You gain access to Rank 3 melee weapon mods."
                        },
                    ]
                },
                new()
                {
                    Name = "Heavy Gunner",
                    Attribute = Fallout4SpecialAttribute.Strength,
                    AttributeRequirement = 5,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Thanks to practice and conditioning, heavy guns do 20% more damage."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 11,
                            Description = "Heavy guns now do 40% more damage, and have improved hip fire accuracy."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 21,
                            Description = "Heavy guns now do 60% more damage. Hip fire accuracy is increased even more."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 35,
                            Description = "Heavy guns now do 80% more damage and have a chance to stagger your opponent."
                        },
                        new()
                        {
                            Rank = 5,
                            LevelRequirement = 47,
                            Description = "Heavy guns now do double damage."
                        },
                    ]
                },
                new()
                {
                    Name = "Strong Back",
                    Attribute = Fallout4SpecialAttribute.Strength,
                    AttributeRequirement = 6,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "What are you, part pack mule? Gain +25 to carry weight."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 10,
                            Description = "You now have +50 to carry weight."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 20,
                            Description = "When overencumbered, you can use Action Points to run."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 30,
                            Description = "When overencumbered, you can fast travel."
                        },
                        new()
                        {
                            Rank = 5,
                            LevelRequirement = 40,
                            Description = "When overencumbered, running costs 50% less action points."
                        },
                    ]
                },
                new()
                {
                    Name = "Steady Aim",
                    Attribute = Fallout4SpecialAttribute.Strength,
                    AttributeRequirement = 7,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Stay on target! Hip-fire accuracy is improved when firing any gun."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 28,
                            Description = "Hip-fire accuracy is improved even more when firing any gun."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 49,
                            Description = "Hip-fire damage is improved when firing any gun."
                        },
                    ]
                },
                new()
                {
                    Name = "Basher",
                    Attribute = Fallout4SpecialAttribute.Strength,
                    AttributeRequirement = 8,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Get up close and personal! Gun bashing does 25% more damage."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 5,
                            Description = "Gun bashing now does 50% more damage and possibly cripples your opponent."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 14,
                            Description = "Gun bashing now does 75% more damage and has an increased chance to cripple your opponent."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 26,
                            Description = "Gun bashing does double damage and has an increased chance to cripple your opponent. It may also inflict a Critical Hit."
                        },
                    ]
                },
                new()
                {
                    Name = "Rooted",
                    Attribute = Fallout4SpecialAttribute.Strength,
                    AttributeRequirement = 9,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "You're part tree! While standing still, you gain +25 Damage Resistance and your melee and unarmed attacks deal 25% more damage."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 22,
                            Description = "While standing still, you now gain +50 Damage Resistance and your melee and unarmed attacks deal 50% more damage."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 43,
                            Description = "While standing still, you may automatically disarm enemies that use melee weapons against you."
                        },
                    ]
                },
                new()
                {
                    Name = "Pain Train",
                    Attribute = Fallout4SpecialAttribute.Strength,
                    AttributeRequirement = 10,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Choo Choo! All aboard! While wearing Power Armor, sprinting into enemies hurts and staggers them. (Robots and oversized enemies are immune to the stagger.)"
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 24,
                            Description = "Sprinting into enemies while wearing Power Armor now causes severe damage and a more powerful stagger. (Robots and oversized enemies are immune to the stagger.)"
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 50,
                            Description = "Sprinting into enemies while wearing Power Armor now causes massive damage and knocks them down. Impact landing near enemies inflicts even more damage."
                        },
                    ]
                },
                new()
                {
                    Name = "Pickpocket",
                    Attribute = Fallout4SpecialAttribute.Perception,
                    AttributeRequirement = 1,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Your quick hands and sticky fingers make picking pockets 25% easier."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 6,
                            Description = "Picking pockets is now 50% easier. You can place a live grenade in a person's inventory."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 17,
                            Description = "Picking pockets is now 75% easier, and you can steal equipped weapons."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 30,
                            Description = "Picking pockets is now twice as easy, and you can steal equipped items."
                        },
                    ]
                },
                new()
                {
                    Name = "Rifleman",
                    Attribute = Fallout4SpecialAttribute.Perception,
                    AttributeRequirement = 2,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Keep your distance long and your kill-count high. Attacks with non-automatic rifles do 20% more damage."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 9,
                            Description = "Attacks with non-automatic rifles do 40% more damage and ignore 15% of a target's armor."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 18,
                            Description = "Attacks with non-automatic rifles do 60% more damage and ignore 20% of a target's armor."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 31,
                            Description = "Attacks with non-automatic rifles do 80% more damage and ignore 25% of a target's armor. They also have a slight chance of crippling a limb."
                        },
                        new()
                        {
                            Rank = 5,
                            LevelRequirement = 46,
                            Description = "Attacks with non-automatic rifles do double damage and ignore 30% of a target's armor. They also have a slightly higher chance of crippling a limb."
                        },
                    ]
                },
                new()
                {
                    Name = "Awareness",
                    Attribute = Fallout4SpecialAttribute.Perception,
                    AttributeRequirement = 3,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "To defeat your enemies, know their weaknesses! You can view a target's specific damage resistances in V.A.T.S."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 14,
                            Description = "Knowing their weaknesses lets you attack more efficiently. 5% increase to hit chance and damage dealt to VATS targets."
                        },
                    ]
                },
                new()
                {
                    Name = "Locksmith",
                    Attribute = Fallout4SpecialAttribute.Perception,
                    AttributeRequirement = 4,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Your nimble fingers allow you to pick Advanced locks."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 7,
                            Description = "You can pick Expert locks."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 18,
                            Description = "You can pick Master locks."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 41,
                            Description = "Your bobby pins never break during lockpicking."
                        },
                    ]
                },
                new()
                {
                    Name = "Demolition Expert",
                    Attribute = Fallout4SpecialAttribute.Perception,
                    AttributeRequirement = 5,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "The bigger the boom, the better! Your explosives do 25% more damage, and you can craft explosives at any chemistry station."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 10,
                            Description = "Your explosives do 50% more damage, and grenades gain a throwing arc."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 22,
                            Description = "Your explosives do 75% more damage and affect a larger area."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 34,
                            Description = "Your explosives now do double damage. Mines and grenades shot in V.A.T.S explode for double damage, too."
                        },
                    ]
                },
                new()
                {
                    Name = "Night Person",
                    Attribute = Fallout4SpecialAttribute.Perception,
                    AttributeRequirement = 6,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "You are a creature of the night! Gain +2 to Intelligence and Perception between the hours of 6:00 p.m. and 6:00 a.m."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 25,
                            Description = "You now have +3 to Intelligence and Perception between the hours of 6:00 p.m. and 6:00 a.m., and night vision when sneaking."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 37,
                            Description = "You have 30 extra health between the hours of 6:00 PM and 6:00 AM."
                        },
                    ]
                },
                new()
                {
                    Name = "Refractor",
                    Attribute = Fallout4SpecialAttribute.Perception,
                    AttributeRequirement = 7,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "You must be part mirror! Instantly gain +10 Energy Resistance."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 11,
                            Description = "You now have +20 Energy Resistance."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 21,
                            Description = "You now have +30 Energy Resistance."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 35,
                            Description = "You now have +40 Energy Resistance."
                        },
                        new()
                        {
                            Rank = 5,
                            LevelRequirement = 42,
                            Description = "You now have +50 Energy Resistance."
                        },
                    ]
                },
                new()
                {
                    Name = "Sniper",
                    Attribute = Fallout4SpecialAttribute.Perception,
                    AttributeRequirement = 8,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "It's all about focus. You have improved control and can hold your breath longer when aiming with scopes."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 13,
                            Description = "Non-automatic, scoped rifles have a chance of knocking down your target."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 26,
                            Description = "Non-automatic, scoped rifles gain +25% accuracy to head shot in V.A.T.S."
                        },
                    ]
                },
                new()
                {
                    Name = "Penetrator",
                    Attribute = Fallout4SpecialAttribute.Perception,
                    AttributeRequirement = 9,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "There's no place to hide! In V.A.T.S you can target an enemy's body parts that are blocked by cover, with a decrease in accuracy."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 28,
                            Description = "In V.A.T.S when you target an enemy's body parts that are blocked by cover, there is no decrease in accuracy."
                        },
                    ]
                },
                new()
                {
                    Name = "Concentrated Fire",
                    Attribute = Fallout4SpecialAttribute.Perception,
                    AttributeRequirement = 10,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Stay Focused! In V.A.T.S every attack on the same body part gains +10% accuracy."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 26,
                            Description = "In V.A.T.S every attack on the same body part gains +15% accuracy."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 50,
                            Description = "In V.A.T.S every attack on the same body part gains +20% accuracy and does 20% more damage."
                        },
                    ]
                },
                new()
                {
                    Name = "Toughness",
                    Attribute = Fallout4SpecialAttribute.Endurance,
                    AttributeRequirement = 1,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "If nothing else, you can take a beating! Instantly gain +10 Damage Resistance."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 9,
                            Description = "You now have +20 damage resistance."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 18,
                            Description = "You now have +30 damage resistance."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 31,
                            Description = "You now have +40 damage resistance."
                        },
                        new()
                        {
                            Rank = 5,
                            LevelRequirement = 46,
                            Description = "You now have +50 damage resistance."
                        },
                    ]
                },
                new()
                {
                    Name = "Lead Belly",
                    Attribute = Fallout4SpecialAttribute.Endurance,
                    AttributeRequirement = 2,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Your digestive tract has adjusted to the weirdness of the Wasteland! Take less radiation from eating or drinking."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 6,
                            Description = "You take even less radiation from eating or drinking."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 17,
                            Description = "You take no radiation from eating or drinking."
                        },
                    ]
                },
                new()
                {
                    Name = "Lifegiver",
                    Attribute = Fallout4SpecialAttribute.Endurance,
                    AttributeRequirement = 3,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "You embody wellness! Instantly gain +20 maximum Health."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 8,
                            Description = "You instantly gain another +20 maximum Health."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 20,
                            Description = "You instantly gain another +20 maximum Health, and slowly regenerate lost Health."
                        },
                    ]
                },
                new()
                {
                    Name = "Chem Resistant",
                    Attribute = Fallout4SpecialAttribute.Endurance,
                    AttributeRequirement = 4,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "All the rush without the hassle! You're 50% less likely to get addicted when consuming Chems."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 22,
                            Description = "You gain complete immunity to chem addiction."
                        },
                    ]
                },
                new()
                {
                    Name = "Aquaboy / Aquagirl",
                    Attribute = Fallout4SpecialAttribute.Endurance,
                    AttributeRequirement = 5,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Water is your ally. You no longer take radiation damage from swimming, and can breathe underwater."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 21,
                            Description = "You become totally undetectable while submerged."
                        },
                    ]
                },
                new()
                {
                    Name = "Rad Resistant",
                    Attribute = Fallout4SpecialAttribute.Endurance,
                    AttributeRequirement = 6,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Exposure to the Wasteland has made you more resilient, instantly granting +10 Radiation Resistance."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 13,
                            Description = "You now have +20 Radiation Resistance."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 26,
                            Description = "You now have +30 Radiation Resistance."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 35,
                            Description = "You now have +40 Radiation Resistance."
                        },
                    ]
                },
                new()
                {
                    Name = "Adamantium Skeleton",
                    Attribute = Fallout4SpecialAttribute.Endurance,
                    AttributeRequirement = 7,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Your skeleton has been infused with indestructible metal, reducing limb damage by 30%."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 13,
                            Description = "Your limb damage is now reduced by 60%."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 26,
                            Description = "Your limb damage is eliminated."
                        },
                    ]
                },
                new()
                {
                    Name = "Cannibal",
                    Attribute = Fallout4SpecialAttribute.Endurance,
                    AttributeRequirement = 8,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Feast on mortal flesh to heal your wounds! Eating human corpses restores Health."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 19,
                            Description = "Eating Ghoul or Super Mutant corpses restores Health."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 38,
                            Description = "Eating human, Ghoul or Super Mutant corpses now restores a significant amount of Health."
                        },
                    ]
                },
                new()
                {
                    Name = "Ghoulish",
                    Attribute = Fallout4SpecialAttribute.Endurance,
                    AttributeRequirement = 9,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Sure, you're still human - on the outside! Radiation now regenerates your lost Health."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 24,
                            Description = "Radiation now regenerates even more of your lost Health."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 48,
                            Description = "Radiation now regenerates even more of your lost Health, and some Feral Ghouls will randomly become friendly."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 50,
                            Description = "Rad damage will now begin to slowly heal, restoring health in the process."
                        },
                    ]
                },
                new()
                {
                    Name = "Solar Powered",
                    Attribute = Fallout4SpecialAttribute.Endurance,
                    AttributeRequirement = 10,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Catch some rays! Gain +2 to Strength and Endurance between the hours of 6:00 a.m. and 6:00 p.m."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 27,
                            Description = "Sunlight slowly heals your radiation damage."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 50,
                            Description = "Sunlight slowly regenerates your lost Health."
                        },
                    ]
                },
                new()
                {
                    Name = "Cap Collector",
                    Attribute = Fallout4SpecialAttribute.Charisma,
                    AttributeRequirement = 1,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "You've mastered the art of the deal! Buying and selling prices at vendors are better."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 20,
                            Description = "Buying and selling prices of vendors are now much better."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 41,
                            Description = "You can now invest a total of 500 caps to raise a store's buying capacity."
                        },
                    ]
                },
                new()
                {
                    Name = "Lady Killer / Black Widow",
                    Attribute = Fallout4SpecialAttribute.Charisma,
                    AttributeRequirement = 2,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "You're charming... and dangerous. Opposite gender suffers +5% damage in combat, and are easier to persuade in dialogue."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 7,
                            Description = "Opposite gender now suffer +10% damage in combat, and are even easier to persuade in dialogue. They are also easier to pacify with the Intimidation perk."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 22,
                            Description = "Opposite gender now suffer +15% damage in combat, and are much easier to persuade in dialogue. They are now even easier to pacify with the Intimidation perk."
                        },
                    ]
                },
                new()
                {
                    Name = "Lone Wanderer",
                    Attribute = Fallout4SpecialAttribute.Charisma,
                    AttributeRequirement = 3,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Who needs friends, anyway? When adventuring without a companion, you take 15% less damage and carry weight increases by 50."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 17,
                            Description = "When adventuring without a companion, you take 30% less damage and carry weight increases by 100."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 40,
                            Description = "When adventuring without a companion, you do 25% more damage."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 50,
                            Description = "When adventuring without a companion, you have 25 more action points."
                        },
                    ]
                },
                new()
                {
                    Name = "Attack Dog",
                    Attribute = Fallout4SpecialAttribute.Charisma,
                    AttributeRequirement = 4,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Your faithful canine companion can hold an enemy, giving you a greater chance to hit them in V.A.T.S."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 9,
                            Description = "When your dog holds an enemy, there's a chance he'll cripple the limb he's biting."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 25,
                            Description = "When your dog holds an enemy, there's a chance he'll cause them to bleed."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 31,
                            Description = "When adventuring with your dog, you take 10% less damage."
                        },
                    ]
                },
                new()
                {
                    Name = "Animal Friend",
                    Attribute = Fallout4SpecialAttribute.Charisma,
                    AttributeRequirement = 5,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Commune with beasts! With your gun, aim at any animal below your level and gain a chance to pacify it."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 12,
                            Description = "When you successfully pacify an animal, you can incite it to attack."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 28,
                            Description = "When you successfully pacify an animal, you can give it specific commands."
                        },
                    ]
                },
                new()
                {
                    Name = "Local Leader",
                    Attribute = Fallout4SpecialAttribute.Charisma,
                    AttributeRequirement = 6,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "As the ruler everyone turns to, you are able to establish supply lines between your workshop settlements."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 14,
                            Description = "You can build stores and workstations at workshop settlements."
                        },
                    ]
                },
                new()
                {
                    Name = "Party Boy / Party Girl",
                    Attribute = Fallout4SpecialAttribute.Charisma,
                    AttributeRequirement = 7,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Nobody has a good time like you! There's no chance you'll get addicted to alcohol."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 15,
                            Description = "The effects of alcohol are doubled."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 37,
                            Description = "Your Luck is increased by 3 while you're under the influence of alcohol."
                        },
                    ]
                },
                new()
                {
                    Name = "Inspirational",
                    Attribute = Fallout4SpecialAttribute.Charisma,
                    AttributeRequirement = 8,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Because you lead by example, your companion does more damage in combat, and cannot hurt you."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 19,
                            Description = "Your companion resists more damage in combat, and can't be harmed by your attacks."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 43,
                            Description = "Your companion can carry more items."
                        },
                    ]
                },
                new()
                {
                    Name = "Wasteland Whisperer",
                    Attribute = Fallout4SpecialAttribute.Charisma,
                    AttributeRequirement = 9,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Master the post-apocalypse! With your gun, aim at any Wasteland creature below your level and gain a chance to pacify it."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 21,
                            Description = "When you successfully pacify a creature, you can incite it to attack."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 49,
                            Description = "When you successfully pacify a creature, you can give it specific commands."
                        },
                    ]
                },
                new()
                {
                    Name = "Intimidation",
                    Attribute = Fallout4SpecialAttribute.Charisma,
                    AttributeRequirement = 10,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Time to show everyone who's boss! With your gun, aim at any human opponent below your level and gain a chance to pacify them."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 23,
                            Description = "When you successfully pacify someone, you can incite them to attack."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 50,
                            Description = "When you successfully pacify someone, you can give them specific commands."
                        },
                    ]
                },
                new()
                {
                    Name = "V.A.N.S.",
                    Attribute = Fallout4SpecialAttribute.Intelligence,
                    AttributeRequirement = 1,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Let Vault-Tec guide you! The path to your closest quest target is displayed in V.A.T.S."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 36,
                            Description = "Gain +2 to Perception."
                        },
                    ]
                },
                new()
                {
                    Name = "Medic",
                    Attribute = Fallout4SpecialAttribute.Intelligence,
                    AttributeRequirement = 2,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Is there a doctor in the house? Stimpaks restore 40% of lost Health, and RadAway removes 40% of radiation."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 18,
                            Description = "Stimpaks restore 60% of lost Health, and RadAway removes 60% of radiation."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 30,
                            Description = "Stimpaks restore 80% of lost Health, and RadAway removes 80% of radiation."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 49,
                            Description = "Stimpaks and RadAway restore all lost health and radiation, and work much more quickly."
                        },
                    ]
                },
                new()
                {
                    Name = "Gun Nut",
                    Attribute = Fallout4SpecialAttribute.Intelligence,
                    AttributeRequirement = 3,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Shoot first, kill first, with access to base level and Rank 1 gun mods."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 13,
                            Description = "You gain access to Rank 2 gun mods."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 25,
                            Description = "You gain access to Rank 3 gun mods."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 39,
                            Description = "You gain access to Rank 4 gun mods."
                        },
                    ]
                },
                new()
                {
                    Name = "Hacker",
                    Attribute = Fallout4SpecialAttribute.Intelligence,
                    AttributeRequirement = 4,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Knowledge of cutting-edge computer encryption allows you to hack Advanced terminals."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 9,
                            Description = "You can hack Expert terminals."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 21,
                            Description = "You can hack Master terminals."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 33,
                            Description = "When hacking, you never get locked out of a terminal when things go wrong."
                        },
                    ]
                },
                new()
                {
                    Name = "Scrapper",
                    Attribute = Fallout4SpecialAttribute.Intelligence,
                    AttributeRequirement = 5,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Waste not, want not! You can salvage uncommon components like screws, aluminum, and copper when scrapping weapons and armor."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 23,
                            Description = "You can salvage rare components like circuitry, nuclear material, and fiber optics when scrapping weapons and armor. Items with favorited components are highlighted."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 40,
                            Description = "You get more from salvaging."
                        },
                    ]
                },
                new()
                {
                    Name = "Science!",
                    Attribute = Fallout4SpecialAttribute.Intelligence,
                    AttributeRequirement = 6,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Take full advantage of advanced technology with access to base level and Rank 1 high-tech mods."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 17,
                            Description = "You gain access to Rank 2 high-tech mods."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 28,
                            Description = "You gain access to Rank 3 high-tech mods."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 41,
                            Description = "You gain access to Rank 4 high-tech mods."
                        },
                    ]
                },
                new()
                {
                    Name = "Chemist",
                    Attribute = Fallout4SpecialAttribute.Intelligence,
                    AttributeRequirement = 7,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Any chems you take last 50% longer. Far out."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 16,
                            Description = "Any chems you take now last twice as long."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 32,
                            Description = "Any chems you take now last an additional 150% longer."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 45,
                            Description = "Any chems you take now last an additional 200% longer."
                        },
                    ]
                },
                new()
                {
                    Name = "Robotics Expert",
                    Attribute = Fallout4SpecialAttribute.Intelligence,
                    AttributeRequirement = 8,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Machines will always serve humans, if you have anything to say about it. Hack a robot, and gain a chance to power it on or off, or initiate a self-destruct."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 19,
                            Description = "When you successfully hack a robot, you can incite it to attack."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 44,
                            Description = "When you successfully hack a robot, you can give it specific commands."
                        },
                    ]
                },
                new()
                {
                    Name = "Nuclear Physicist",
                    Attribute = Fallout4SpecialAttribute.Intelligence,
                    AttributeRequirement = 9,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "You've learned to split the atom... and command it. Radiation weapons do 50% more damage and Fusion Cores last an extra 25% longer."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 14,
                            Description = "Radiation weapons now do double damage and Fusion Cores last an extra 50% longer."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 26,
                            Description = "Fusion Cores can be ejected from Power Armor like devastating grenades and Fusion Cores last twice as long."
                        },
                    ]
                },
                new()
                {
                    Name = "Nerd Rage!",
                    Attribute = Fallout4SpecialAttribute.Intelligence,
                    AttributeRequirement = 10,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Genius. Is. ANGRY! When your Health drops below 20%, time slows and you gain +20 Damage resistance and do 20% more damage while the effect lasts."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 31,
                            Description = "You now gain 30 more Damage Resistance and do 30% more damage while Nerd Rage is in effect."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 50,
                            Description = "You now gain 40 more Damage Resistance and do 40% more damage while Nerd Rage is in effect. Kills you make while enraged restore some lost Health."
                        },
                    ]
                },
                new()
                {
                    Name = "Gunslinger",
                    Attribute = Fallout4SpecialAttribute.Agility,
                    AttributeRequirement = 1,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Channel the spirit of the old west! Non-automatic pistols do 20% more damage."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 7,
                            Description = "Non-automatic pistols now do 40% more damage and have increased range."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 15,
                            Description = "Non-automatic pistols now do 60% more damage and range is increased even further."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 27,
                            Description = "Non-automatic pistols now do 80% more damage and their attacks can disarm opponents."
                        },
                        new()
                        {
                            Rank = 5,
                            LevelRequirement = 42,
                            Description = "Non-automatic pistols now do double damage. Their attacks have a much better chance to disarm opponents, and may even cripple a limb."
                        },
                    ]
                },
                new()
                {
                    Name = "Commando",
                    Attribute = Fallout4SpecialAttribute.Agility,
                    AttributeRequirement = 2,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Rigorous combat training means automatic weapons do 20% more damage."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 11,
                            Description = "Attacks with automatic weapons do 40% more damage, with improved hip fire accuracy."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 21,
                            Description = "Attacks with automatic weapons do 60% more damage. Hip fire accuracy is improved even more."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 35,
                            Description = "Attacks with automatic weapons do 80% more damage and gain a chance to stagger opponents."
                        },
                        new()
                        {
                            Rank = 5,
                            LevelRequirement = 49,
                            Description = "Your automatic weapons now do double damage and have a greater chance to stagger opponents."
                        },
                    ]
                },
                new()
                {
                    Name = "Sneak",
                    Attribute = Fallout4SpecialAttribute.Agility,
                    AttributeRequirement = 3,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Become whisper, become shadow. You are 20% harder to detect while sneaking."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 5,
                            Description = "You are now 30% harder to detect while sneaking, and no longer trigger floor-based traps."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 12,
                            Description = "You are now 40% harder to detect while sneaking, and no longer trigger enemy mines."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 23,
                            Description = "You are now 50% harder to detect while sneaking, and running no longer adversely affects stealth."
                        },
                        new()
                        {
                            Rank = 5,
                            LevelRequirement = 38,
                            Description = "Engaging stealth causes distant enemies to lose you."
                        },
                    ]
                },
                new()
                {
                    Name = "Mister Sandman",
                    Attribute = Fallout4SpecialAttribute.Agility,
                    AttributeRequirement = 4,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "As an agent of death, you can instantly kill a sleeping person. Your silenced weapons do an additional 15% sneak attack damage."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 17,
                            Description = "Your silenced weapons do an additional 30% sneak attack damage."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 30,
                            Description = "Your silenced weapons now do 50% more sneak attack damage."
                        },
                    ]
                },
                new()
                {
                    Name = "Action Boy / Action Girl",
                    Attribute = Fallout4SpecialAttribute.Agility,
                    AttributeRequirement = 5,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "There's no time to waste! Action Points regenerate 25% faster."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 18,
                            Description = "Your Action Points now regenerate 50% faster."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 38,
                            Description = "Your Action Points now regenerate 75% faster."
                        },
                    ]
                },
                new()
                {
                    Name = "Moving Target",
                    Attribute = Fallout4SpecialAttribute.Agility,
                    AttributeRequirement = 6,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "They can't hurt what they can't hit! Get 25+ Damage Resistance and 25+ Energy Resistance when you're sprinting."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 24,
                            Description = "You now get +50 Damage Resistance and +50 Energy Resistance when you're sprinting."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 44,
                            Description = "Sprinting costs 50% fewer Action Points."
                        },
                    ]
                },
                new()
                {
                    Name = "Ninja",
                    Attribute = Fallout4SpecialAttribute.Agility,
                    AttributeRequirement = 7,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Trained as a shadow warrior, your ranged sneak attack do 2.5x normal damage and your melee attacks do 4x normal damage."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 16,
                            Description = "Your ranged sneak attacks do 3x normal damage and your melee sneak attacks do 5x normal damage."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 33,
                            Description = "Your ranged sneak attacks do 3.5x normal damage and your melee sneak attacks do 10x normal damage."
                        },
                    ]
                },
                new()
                {
                    Name = "Quick Hands",
                    Attribute = Fallout4SpecialAttribute.Agility,
                    AttributeRequirement = 8,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "In combat, there's no time to hesitate. You can reload all guns faster."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 28,
                            Description = "Reloading guns costs no Action Points in V.A.T.S."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 40,
                            Description = "Quick and efficient. You gain 10 additional Action Points."
                        },
                    ]
                },
                new()
                {
                    Name = "Blitz",
                    Attribute = Fallout4SpecialAttribute.Agility,
                    AttributeRequirement = 9,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Find the gap and make the tackle! V.A.T.S. melee distance is increased significantly."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 29,
                            Description = "V.A.T.S. melee distance is increased even more, and the farther the Blitz distance, the greater the damage."
                        },
                    ]
                },
                new()
                {
                    Name = "Gun Fu",
                    Attribute = Fallout4SpecialAttribute.Agility,
                    AttributeRequirement = 10,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "You've learned to apply ancient martial arts to gunplay! Do 25% more damage to your second V.A.T.S. target and beyond."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 26,
                            Description = "In V.A.T.S. you do 50% more damage to your third target and beyond."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 50,
                            Description = "In V.A.T.S. you instantly do a Critical Hit against your fourth target and beyond."
                        },
                    ]
                },
                new()
                {
                    Name = "Fortune Finder",
                    Attribute = Fallout4SpecialAttribute.Luck,
                    AttributeRequirement = 1,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "You've learned to discover the Wasteland's hidden wealth, and discover more bottle caps in containers."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 5,
                            Description = "You find even more bottle caps in containers."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 25,
                            Description = "You find even more bottle caps in containers."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 40,
                            Description = "You find even more bottle caps in containers, and there is a chance of enemies exploding into a shower of caps when you kill them."
                        },
                    ]
                },
                new()
                {
                    Name = "Scrounger",
                    Attribute = Fallout4SpecialAttribute.Luck,
                    AttributeRequirement = 2,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "You know just how to scavenge to keep the fight going, and find more ammunition in containers."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 7,
                            Description = "You find even more ammunition in containers."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 24,
                            Description = "You find even more ammunition in containers."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 37,
                            Description = "There is a chance to gain ammo when firing the last round in your magazine."
                        },
                    ]
                },
                new()
                {
                    Name = "Bloody Mess",
                    Attribute = Fallout4SpecialAttribute.Luck,
                    AttributeRequirement = 3,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "+5% bonus damage means enemies will sometimes explode into a gory red paste. Watch out for flying eyeballs!"
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 9,
                            Description = "You now inflict +10% damage in combat."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 31,
                            Description = "You now inflict +15% damage in combat."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 47,
                            Description = "When an enemy explodes, nearby enemies may suffer the same fate."
                        },
                    ]
                },
                new()
                {
                    Name = "Mysterious Stranger",
                    Attribute = Fallout4SpecialAttribute.Luck,
                    AttributeRequirement = 4,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Who is he? Why does he help? Who cares! The Mysterious Stranger will appear occasionally in V.A.T.S. to lend a hand, with deadly efficiency..."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 22,
                            Description = "The Mysterious Stranger appears more often in V.A.T.S."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 41,
                            Description = "The Mysterious Stranger appears more often in V.A.T.S. When he kills an opponent, there is a chance your Critical meter gets filled."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 49,
                            Description = "The Mysterious Stranger appears more often in V.A.T.S. When he kills an opponent, there is a high chance your Critical meter gets filled."
                        },
                    ]
                },
                new()
                {
                    Name = "Idiot Savant",
                    Attribute = Fallout4SpecialAttribute.Luck,
                    AttributeRequirement = 5,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "You're not stupid! Just... different. Randomly receive 3x XP from any action, and the lower your Intelligence, the greater the chance."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 11,
                            Description = "You now randomly receive 5x XP from any action. The lower your Intelligence, the greater the chance."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 34,
                            Description = "Randomly receiving bonus XP from any action may trigger 3x XP for all kills for a short period of time. The lower your Intelligence, the greater the chance."
                        },
                    ]
                },
                new()
                {
                    Name = "Better Criticals",
                    Attribute = Fallout4SpecialAttribute.Luck,
                    AttributeRequirement = 6,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Advanced training for enhanced combat effectiveness! Criticals do 50% more extra damage."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 15,
                            Description = "Your criticals now do twice as much extra damage."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 40,
                            Description = "Your criticals now do 2.5x as much extra damage."
                        },
                    ]
                },
                new()
                {
                    Name = "Critical Banker",
                    Attribute = Fallout4SpecialAttribute.Luck,
                    AttributeRequirement = 7,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "You're a patient battlefield tactician, and can save a Critical Hit, to be used in V.A.T.S. when you need it most."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 17,
                            Description = "You can now save 2 Critical Hits, to be used in V.A.T.S. when you need them the most."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 43,
                            Description = "You can now save 3 Critical Hits, to be used in V.A.T.S. when you need them the most. Banking a Critical has a chance to save an additional Critical."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 50,
                            Description = "You can now save 4 Critical Hits, to be used in V.A.T.S. when you need them the most."
                        },
                    ]
                },
                new()
                {
                    Name = "Grim Reaper's Sprint",
                    Attribute = Fallout4SpecialAttribute.Luck,
                    AttributeRequirement = 8,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Death becomes you! Any kill in V.A.T.S. has a 15% chance to restore all Action Points."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 19,
                            Description = "Any kill in V.A.T.S. now has a 25% chance to restore all Action Points."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 46,
                            Description = "Any kill in V.A.T.S. has a 35% chance to restore all Action Points and refill your Critical meter."
                        },
                    ]
                },
                new()
                {
                    Name = "Four Leaf Clover",
                    Attribute = Fallout4SpecialAttribute.Luck,
                    AttributeRequirement = 9,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "Feeling Lucky? You should! Each hit in V.A.T.S. has a chance of filling your Critical meter."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 13,
                            Description = "Each hit in V.A.T.S. now has an even better chance of filling your Critical meter."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 32,
                            Description = "Each hit in V.A.T.S. now has a very good chance of filling your Critical meter."
                        },
                        new()
                        {
                            Rank = 4,
                            LevelRequirement = 48,
                            Description = "Each hit in V.A.T.S. now has an excellent chance of filling your Critical meter."
                        },
                    ]
                },
                new()
                {
                    Name = "Ricochet",
                    Attribute = Fallout4SpecialAttribute.Luck,
                    AttributeRequirement = 10,
                    Ranks =
                    [
                        new()
                        {
                            Rank = 1,
                            LevelRequirement = 0,
                            Description = "What goes around comes around! Any enemy's ranged attacks will sometimes ricochet back and instantly kill them. The closer you are to death, the higher the chance."
                        },
                        new()
                        {
                            Rank = 2,
                            LevelRequirement = 29,
                            Description = "There's an increased chance that an enemy's shot will ricochet back and kill them."
                        },
                        new()
                        {
                            Rank = 3,
                            LevelRequirement = 50,
                            Description = "When an enemy's shot ricochets back and kills them, there is a chance your Critical meter gets filled."
                        },
                    ]
                },
            };
        }
    }
}
