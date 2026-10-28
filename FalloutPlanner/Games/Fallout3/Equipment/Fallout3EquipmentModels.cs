using System.Collections.Generic;

namespace FalloutPlanner.Games.Fallout3.Equipment
{
    public enum EquipmentSlot
    {
        Eyewear,
        Headwear,
        Body
    }

    public enum EquipmentCategory
    {
        Eyewear,
        Hats,
        FullHeadwear,
        FullBody,
        Clothing,
        Armor
    }

    public class EquipmentStatModifier
    {
        public string Stat { get; init; } = "";
        public int Amount { get; init; }
    }

    public class Fallout3EquipmentItem
    {
        public string Name { get; init; } = "";
        public EquipmentSlot Slot { get; init; }
        public EquipmentCategory Category { get; init; }
        public int DR { get; init; }
        public double Weight { get; init; }
        public double? WeightWithPowerArmorTraining { get; init; }
        public string EffectsDisplay { get; init; } = "—";
        public bool BlocksEyewear { get; init; }
        public bool BlocksHeadwear { get; init; }
        public List<EquipmentStatModifier> Modifiers { get; init; } = new();

        public double GetEffectiveWeight(bool hasPowerArmorTraining)
        {
            if (hasPowerArmorTraining && WeightWithPowerArmorTraining.HasValue)
                return WeightWithPowerArmorTraining.Value;
            return Weight;
        }

        public string CategoryDisplayName => Category switch
        {
            EquipmentCategory.Eyewear => "Eyewear",
            EquipmentCategory.Hats => "Hats",
            EquipmentCategory.FullHeadwear => "Full Headwear",
            EquipmentCategory.FullBody => "Full Body",
            EquipmentCategory.Clothing => "Clothing",
            EquipmentCategory.Armor => "Armor",
            _ => Category.ToString()
        };

        public string SummaryLine =>
            $"DR {DR}  |  Wt {Weight}  |  {EffectsDisplay}";
    }
}
