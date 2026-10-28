using System;
using System.Collections.Generic;
using System.Linq;

namespace FalloutPlanner.Games.Fallout3.Equipment
{
    /// <summary>
    /// Computes equipment-only display stats for the Equipment page.
    /// Does not mutate the underlying character used by the leveling page.
    /// </summary>
    public class EquipmentDisplayStats
    {
        public const double LoudnessPercentPerWeight = 4.17;
        public const double MovementPercentPerWeight = 0.23;
        public const double MovementDebuffMaxPercent = 40.0;

        private readonly Fallout3Character _character;

        public EquipmentDisplayStats(Fallout3Character character)
        {
            _character = character;
        }

        public Fallout3EquipmentItem? Eyewear =>
            Fallout3EquipmentRepository.Find(_character.EquippedEyewearName);

        public Fallout3EquipmentItem? Headwear =>
            Fallout3EquipmentRepository.Find(_character.EquippedHeadwearName);

        public Fallout3EquipmentItem? Body =>
            Fallout3EquipmentRepository.Find(_character.EquippedBodyName);

        public IEnumerable<Fallout3EquipmentItem> EquippedItems =>
            new[] { Eyewear, Headwear, Body }.Where(i => i != null)!;

        public bool HasPowerArmorTraining =>
            _character.HasQuestPerk("Power Armor Training") ||
            _character.GetPerkRank("Power Armor Training") > 0;

        public double TotalWeight =>
            EquippedItems.Sum(i => i!.GetEffectiveWeight(HasPowerArmorTraining));

        public int TotalDR =>
            EquippedItems.Sum(i => i!.DR);

        public double LoudnessDebuffPercent =>
            Math.Round(TotalWeight * LoudnessPercentPerWeight, 2);

        public double MovementDebuffPercent =>
            Math.Round(Math.Min(TotalWeight * MovementPercentPerWeight, MovementDebuffMaxPercent), 2);

        private int ModSum(string stat) =>
            EquippedItems.SelectMany(i => i!.Modifiers)
                .Where(m => m.Stat == stat)
                .Sum(m => m.Amount);

        public int Strength => _character.Strength + ModSum(nameof(Fallout3Character.Strength));
        public int Perception => _character.Perception + ModSum(nameof(Fallout3Character.Perception));
        public int Endurance => _character.Endurance + ModSum(nameof(Fallout3Character.Endurance));
        public int Charisma => _character.Charisma + ModSum(nameof(Fallout3Character.Charisma));
        public int Intelligence => _character.Intelligence + ModSum(nameof(Fallout3Character.Intelligence));
        public int Agility => _character.Agility + ModSum(nameof(Fallout3Character.Agility));
        public int Luck => _character.Luck + ModSum(nameof(Fallout3Character.Luck));

        public int DamageResist => _character.DamageResist + TotalDR;
        public int RadiationResist => _character.RadiationResist + ModSum("RadiationResist");
        public int FireResist => _character.FireResist + ModSum("FireResist");
        public int ActionPoints => _character.ActionPoints + ModSum("ActionPoints");
        public double MeleeDamage => _character.MeleeDamage + ModSum("MeleeDamage")
            + (ModSum(nameof(Fallout3Character.Strength)) * 0.5);
        public int Health => _character.Health + ModSum(nameof(Fallout3Character.Endurance)) * 20;
        public int CarryWeight => _character.CarryWeight + ModSum(nameof(Fallout3Character.Strength)) * 10;
        public double CriticalChance => _character.CriticalChance + ModSum(nameof(Fallout3Character.Luck));
        public int PoisonResist => _character.PoisonResist + ModSum(nameof(Fallout3Character.Endurance)) * 5;

        public int Barter => _character.Barter + ModSum(nameof(Fallout3Character.Barter));
        public int BigGuns => _character.BigGuns + ModSum(nameof(Fallout3Character.BigGuns));
        public int EnergyWeapons => _character.EnergyWeapons + ModSum(nameof(Fallout3Character.EnergyWeapons));
        public int Explosives => _character.Explosives + ModSum(nameof(Fallout3Character.Explosives));
        public int Lockpick => _character.Lockpick + ModSum(nameof(Fallout3Character.Lockpick));
        public int Medicine => _character.Medicine + ModSum(nameof(Fallout3Character.Medicine));
        public int MeleeWeapons => _character.MeleeWeapons + ModSum(nameof(Fallout3Character.MeleeWeapons));
        public int Repair => _character.Repair + ModSum(nameof(Fallout3Character.Repair));
        public int Science => _character.Science + ModSum(nameof(Fallout3Character.Science));
        public int SmallGuns => _character.SmallGuns + ModSum(nameof(Fallout3Character.SmallGuns));
        public int Sneak => _character.Sneak + ModSum(nameof(Fallout3Character.Sneak));
        public int Speech => _character.Speech + ModSum(nameof(Fallout3Character.Speech));
        public int Unarmed => _character.Unarmed + ModSum(nameof(Fallout3Character.Unarmed));
    }
}
