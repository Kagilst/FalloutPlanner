using System.Collections.Generic;

namespace FalloutPlanner.Games.Fallout1.Traits
{
    public static class Fallout1TraitRepository
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

        public static IReadOnlyList<Fallout1Trait> All { get; } = new List<Fallout1Trait>
        {
            new()
            {
                Name = FastMetabolism,
                Benefit = "Healing Rate is doubled",
                Penalty = "Radiation and Poison Resistance start at 0%",
                HasPermanentEffect = true
                // Effects handled in RecalculateDerivedStats
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
                    c.RecalculateDerivedStats();
                },
                RemoveEffect = c =>
                {
                    c.Strength = System.Math.Max(1, c.Strength - 2);
                    c.RecalculateDerivedStats();
                }
            },
            new()
            {
                Name = SmallFrame,
                Benefit = "+1 Agility",
                Penalty = "Carry Weight uses 15 lbs per Strength instead of 25 lbs",
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
                Name = OneHander,
                Benefit = "+20% chance to hit with one-handed weapons",
                Penalty = "-40% chance to hit with two-handed weapons",
                HasPermanentEffect = false
            },
            new()
            {
                Name = Finesse,
                Benefit = "+10% Critical Chance",
                Penalty = "-30% damage with all weapons",
                HasPermanentEffect = true
                // Effects handled in RecalculateDerivedStats
            },
            new()
            {
                Name = Kamikaze,
                Benefit = "+5 Sequence",
                Penalty = "Armor Class is always 0 (natural AC negated)",
                HasPermanentEffect = true
                // Effects handled in RecalculateDerivedStats
            },
            new()
            {
                Name = HeavyHanded,
                Benefit = "+4 Melee Damage",
                Penalty = "Critical hits do 30% less damage",
                HasPermanentEffect = true
                // Effects handled in RecalculateDerivedStats
            },
            new()
            {
                Name = FastShot,
                Benefit = "-1 AP cost for ranged attacks",
                Penalty = "Cannot make targeted shots",
                HasPermanentEffect = false
            },
            new()
            {
                Name = BloodyMess,
                Benefit = "Violent death animations always play",
                Penalty = "None (purely cosmetic)",
                HasPermanentEffect = false
            },
            new()
            {
                Name = Jinxed,
                Benefit = "Enemies are more likely to suffer critical failures",
                Penalty = "You are also more likely to suffer critical failures",
                HasPermanentEffect = false
            },
            new()
            {
                Name = GoodNatured,
                Benefit = "+15% First Aid, Doctor, Speech, and Barter",
                Penalty = "-10% Small Guns, Big Guns, Energy Weapons, Unarmed, Melee Weapons, and Throwing",
                HasPermanentEffect = true
                // Effects handled in RecalculateDerivedStats
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
                Benefit = "50% less likely to become addicted",
                Penalty = "Chems only last half as long",
                HasPermanentEffect = false
            },
            new()
            {
                Name = SexAppeal,
                Benefit = "Better reactions from opposite sex characters",
                Penalty = "Worse reactions from same sex characters",
                HasPermanentEffect = false
            },
            new()
            {
                Name = Skilled,
                Benefit = "+5 skill points per level",
                Penalty = "Perks every 4 levels instead of 3",
                HasPermanentEffect = true
                // Effects handled in SkillPointsPerLevel and PerkLevel calculations
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

        public static Fallout1Trait? GetTrait(string name)
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
