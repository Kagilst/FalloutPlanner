using System.Collections.Generic;

namespace FalloutPlanner.Games.Fallout2.Traits
{
    public static class Fallout2TraitRepository
    {
        public const string FastMetabolism = "Fast Metabolism";
        public const string Bruiser = "Bruiser";
        public const string SmallFrame = "Small Frame";
        public const string OneHander = "One Hander";
        public const string Finesse = "Finesse";
        public const string Kamikaze = "Kamikaze";
        public const string HeavyHanded = "Heavy Handed";
        public const string FastShot = "Fast Shot";
        public const string BloodyMess = "Bloody Mess";
        public const string Jinxed = "Jinxed";
        public const string GoodNatured = "Good Natured";
        public const string ChemReliant = "Chem Reliant";
        public const string ChemResistant = "Chem Resistant";
        public const string SexAppeal = "Sex Appeal";
        public const string Skilled = "Skilled";
        public const string Gifted = "Gifted";

        public static IReadOnlyList<Fallout2Trait> All { get; } = new List<Fallout2Trait>
        {
            new()
            {
                Name = FastMetabolism,
                Benefit = "Your Healing Rate is +2",
                Penalty = "Poison and Radiation Resistances are 0",
                HasPermanentEffect = true,
                ApplyEffect = c =>
                {
                    c.HealingRate += 2;
                    c.PoisonResist = 0;
                    c.RadiationResist = 0;
                },
                RemoveEffect = c =>
                {
                    c.HealingRate -= 2;
                    c.RecalculateDerivedStats();
                }
            },
            new()
            {
                Name = Bruiser,
                Benefit = "+2 Strength",
                Penalty = "-2 Action Points",
                HasPermanentEffect = true,
                ApplyEffect = c =>
                {
                    c.Strength = System.Math.Min(10, c.Strength + 2);
                    c.ActionPoints -= 2;
                    c.RecalculateDerivedStats();
                },
                RemoveEffect = c =>
                {
                    c.Strength = System.Math.Max(1, c.Strength - 2);
                    c.ActionPoints += 2;
                    c.RecalculateDerivedStats();
                }
            },
            new()
            {
                Name = SmallFrame,
                Benefit = "+1 Agility",
                Penalty = "Carry Weight is reduced (25 lbs less)",
                HasPermanentEffect = true,
                ApplyEffect = c =>
                {
                    c.Agility = System.Math.Min(10, c.Agility + 1);
                    c.CarryWeight -= 25;
                    c.RecalculateDerivedStats();
                },
                RemoveEffect = c =>
                {
                    c.Agility = System.Math.Max(1, c.Agility - 1);
                    c.CarryWeight += 25;
                    c.RecalculateDerivedStats();
                }
            },
            new()
            {
                Name = OneHander,
                Benefit = "+20% chance to hit with one-handed weapons",
                Penalty = "-40% chance to hit with two-handed weapons",
                HasPermanentEffect = false
            },
            new()
            {
                Name = Finesse,
                Benefit = "+10% Critical Chance",
                Penalty = "-30% base damage with all weapons",
                HasPermanentEffect = true,
                ApplyEffect = c => c.CriticalChance += 10,
                RemoveEffect = c => c.CriticalChance -= 10
            },
            new()
            {
                Name = Kamikaze,
                Benefit = "+5 Sequence",
                Penalty = "Armor Class is 0 (ignores Agility bonus)",
                HasPermanentEffect = true,
                ApplyEffect = c =>
                {
                    c.Sequence += 5;
                    c.ArmorClass = 0;
                },
                RemoveEffect = c =>
                {
                    c.Sequence -= 5;
                    c.RecalculateDerivedStats();
                }
            },
            new()
            {
                Name = HeavyHanded,
                Benefit = "+4 Melee Damage",
                Penalty = "-30% critical hit damage",
                HasPermanentEffect = true,
                ApplyEffect = c => c.MeleeDamage += 4,
                RemoveEffect = c => c.MeleeDamage -= 4
            },
            new()
            {
                Name = FastShot,
                Benefit = "1 less AP per ranged attack",
                Penalty = "Cannot make targeted shots",
                HasPermanentEffect = false
            },
            new()
            {
                Name = BloodyMess,
                Benefit = "More violent death animations",
                Penalty = "Disturbing to others (no real game penalty)",
                HasPermanentEffect = false
            },
            new()
            {
                Name = Jinxed,
                Benefit = "Enemies suffer more critical failures",
                Penalty = "You also suffer more critical failures",
                HasPermanentEffect = false
            },
            new()
            {
                Name = GoodNatured,
                Benefit = "+15% First Aid, Doctor, Speech, Barter",
                Penalty = "-10% Small Guns, Big Guns, Energy, Unarmed, Melee, Throwing",
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
                Name = ChemReliant,
                Benefit = "Chems last twice as long",
                Penalty = "Twice as likely to become addicted",
                HasPermanentEffect = false
            },
            new()
            {
                Name = ChemResistant,
                Benefit = "Half as likely to become addicted",
                Penalty = "Chems only last half as long",
                HasPermanentEffect = false
            },
            new()
            {
                Name = SexAppeal,
                Benefit = "Better reactions from opposite sex",
                Penalty = "Worse reactions from same sex",
                HasPermanentEffect = false
            },
            new()
            {
                Name = Skilled,
                Benefit = "+10% to all skills, +5 skill points per level",
                Penalty = "Perks are gained every 4 levels instead of 3",
                HasPermanentEffect = true,
                ApplyEffect = c =>
                {
                    c.PerkRate = 4;
                    // Skill bonus applied via RecalculateDerivedStats
                },
                RemoveEffect = c =>
                {
                    c.PerkRate = 3;
                    // Skill bonus removed via RecalculateDerivedStats
                }
            },
            new()
            {
                Name = Gifted,
                Benefit = "+1 to all SPECIAL stats",
                Penalty = "-10% to all skills, -5 skill points per level",
                HasPermanentEffect = true,
                ApplyEffect = c =>
                {
                    c.Strength = System.Math.Min(10, c.Strength + 1);
                    c.Perception = System.Math.Min(10, c.Perception + 1);
                    c.Endurance = System.Math.Min(10, c.Endurance + 1);
                    c.Charisma = System.Math.Min(10, c.Charisma + 1);
                    c.Intelligence = System.Math.Min(10, c.Intelligence + 1);
                    c.Agility = System.Math.Min(10, c.Agility + 1);
                    c.Luck = System.Math.Min(10, c.Luck + 1);
                    c.RecalculateDerivedStats();
                },
                RemoveEffect = c =>
                {
                    c.Strength = System.Math.Max(1, c.Strength - 1);
                    c.Perception = System.Math.Max(1, c.Perception - 1);
                    c.Endurance = System.Math.Max(1, c.Endurance - 1);
                    c.Charisma = System.Math.Max(1, c.Charisma - 1);
                    c.Intelligence = System.Math.Max(1, c.Intelligence - 1);
                    c.Agility = System.Math.Max(1, c.Agility - 1);
                    c.Luck = System.Math.Max(1, c.Luck - 1);
                    c.RecalculateDerivedStats();
                }
            }
        };

        public static Fallout2Trait? GetTrait(string name)
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
