using System.Collections.Generic;

namespace FalloutPlanner.Games.FalloutNV.Traits
{
    public static class FalloutNVTraitRepository
    {
        public const string BuiltToDestroy = "Built to Destroy";
        public const string FastShot = "Fast Shot";
        public const string FourEyes = "Four Eyes";
        public const string GoodNatured = "Good Natured";
        public const string HeavyHanded = "Heavy Handed";
        public const string Kamikaze = "Kamikaze";
        public const string LooseCannon = "Loose Cannon";
        public const string SmallFrame = "Small Frame";
        public const string TriggerDiscipline = "Trigger Discipline";
        public const string WildWasteland = "Wild Wasteland";
        public const string Claustrophobia = "Claustrophobia";
        public const string EarlyBird = "Early Bird";
        public const string Hoarder = "Hoarder";
        public const string HotBlooded = "Hot Blooded";
        public const string LogansLoophole = "Logan's Loophole";
        public const string Skilled = "Skilled";

        public static IReadOnlyList<FalloutNVTrait> All { get; } = new List<FalloutNVTrait>
        {
            new()
            {
                Name = BuiltToDestroy,
                Benefit = "+3% critical hit chance",
                Penalty = "Weapon condition decays 15% faster",
                HasPermanentEffect = true,
                ApplyEffect = c => c.CriticalChance += 3,
                RemoveEffect = c => c.CriticalChance -= 3
            },
            new()
            {
                Name = FastShot,
                Benefit = "Guns and energy weapons fire 20% quicker, AP cost 20% lower",
                Penalty = "Guns and energy weapons are 20% less accurate",
                HasPermanentEffect = false
            },
            new()
            {
                Name = FourEyes,
                Benefit = "+2 Perception when wearing glasses",
                Penalty = "-1 Perception permanent (affects perk requirements)",
                HasPermanentEffect = true,
                ApplyEffect = c =>
                {
                    c.Perception = System.Math.Max(1, c.Perception - 1);
                    c.RecalculateDerivedStats();
                },
                RemoveEffect = c =>
                {
                    c.Perception = System.Math.Min(10, c.Perception + 1);
                    c.RecalculateDerivedStats();
                }
            },
            new()
            {
                Name = GoodNatured,
                Benefit = "+5 Speech, Medicine, Repair, Science, and Barter",
                Penalty = "-5 Energy Weapons, Explosives, Guns, Melee Weapons, and Unarmed",
                HasPermanentEffect = true,
                ApplyEffect = c =>
                {
                    // Applied via RecalculateDerivedStats when trait is detected
                },
                RemoveEffect = c =>
                {
                    // Removed via RecalculateDerivedStats when trait is removed
                }
            },
            new()
            {
                Name = HeavyHanded,
                Benefit = "Melee and unarmed do 20% more damage",
                Penalty = "Melee and unarmed do 60% less critical hit damage",
                HasPermanentEffect = false
            },
            new()
            {
                Name = Kamikaze,
                Benefit = "+10 Action Points",
                Penalty = "-2 Damage Threshold and -15% Damage Resistance",
                HasPermanentEffect = true,
                ApplyEffect = c =>
                {
                    c.ActionPoints += 10;
                    c.DamageThreshold -= 2;
                    c.DamageResist -= 15;
                },
                RemoveEffect = c =>
                {
                    c.ActionPoints -= 10;
                    c.DamageThreshold += 2;
                    c.DamageResist += 15;
                }
            },
            new()
            {
                Name = LooseCannon,
                Benefit = "Attack speed with thrown weapons increased by 30%",
                Penalty = "Thrown weapons have 25% less range",
                HasPermanentEffect = false
            },
            new()
            {
                Name = SmallFrame,
                Benefit = "+1 Agility",
                Penalty = "Fragile limbs (25% extra limb damage)",
                HasPermanentEffect = true,
                ApplyEffect = c =>
                {
                    c.Agility = System.Math.Min(10, c.Agility + 1);
                    c.RecalculateDerivedStats();
                },
                RemoveEffect = c =>
                {
                    c.Agility = System.Math.Max(1, c.Agility - 1);
                    c.RecalculateDerivedStats();
                }
            },
            new()
            {
                Name = TriggerDiscipline,
                Benefit = "Guns and energy weapons are 20% more accurate",
                Penalty = "Guns and energy weapons fire 20% slower, AP cost 20% higher",
                HasPermanentEffect = false
            },
            new()
            {
                Name = WildWasteland,
                Benefit = "Adds additional 'wacky' content and special encounters",
                Penalty = "Removes access to some 'sane' content",
                HasPermanentEffect = false
            },
            new()
            {
                Name = Claustrophobia,
                Benefit = "+1 to SPECIAL attributes while outside",
                Penalty = "-1 to SPECIAL attributes while indoors",
                HasPermanentEffect = false
            },
            new()
            {
                Name = EarlyBird,
                Benefit = "+2 to SPECIAL attributes from 6am to 12pm",
                Penalty = "-1 to SPECIAL attributes from 6pm to 6am",
                HasPermanentEffect = false
            },
            new()
            {
                Name = Hoarder,
                Benefit = "+25 lbs to total Carry Weight",
                Penalty = "-1 to SPECIAL attributes while current equipment weight is below 160 lbs",
                HasPermanentEffect = true,
                ApplyEffect = c => c.CarryWeight += 25,
                RemoveEffect = c => c.CarryWeight -= 25
            },
            new()
            {
                Name = HotBlooded,
                Benefit = "+15% damage increase while below 50% health",
                Penalty = "-2 to Perception and Agility while below 50% health",
                HasPermanentEffect = false
            },
            new()
            {
                Name = LogansLoophole,
                Benefit = "Chems last twice as long and addiction rates are set to zero",
                Penalty = "Locks the level cap at 30",
                HasPermanentEffect = true
                // MaxLevel is handled in character logic
            },
            new()
            {
                Name = Skilled,
                Benefit = "+5 points to every skill",
                Penalty = "-10% from experience gained",
                HasPermanentEffect = true,
                ApplyEffect = c =>
                {
                    // Applied via RecalculateDerivedStats when trait is detected
                },
                RemoveEffect = c =>
                {
                    // Removed via RecalculateDerivedStats when trait is removed
                }
            }
        };

        public static FalloutNVTrait? GetTrait(string name)
        {
            foreach (var trait in All)
            {
                if (trait.Name == name)
                    return trait;
            }
            return null;
        }
    }
}
