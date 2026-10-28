using System.Collections.Generic;
using System.Linq;

namespace FalloutPlanner.Games.Fallout3.Equipment
{
    public static partial class Fallout3EquipmentRepository
    {
        public static IReadOnlyList<Fallout3EquipmentItem> All { get; } = BuildItems();

        public static Fallout3EquipmentItem? Find(string? name) =>
            string.IsNullOrWhiteSpace(name) ? null : All.FirstOrDefault(i => i.Name == name);

        public static IEnumerable<Fallout3EquipmentItem> ForSlot(EquipmentSlot slot) =>
            All.Where(i => i.Slot == slot);

        public static IEnumerable<IGrouping<EquipmentCategory, Fallout3EquipmentItem>> CategoriesForSlot(
            EquipmentSlot slot) =>
            ForSlot(slot).GroupBy(i => i.Category).OrderBy(g => CategoryOrder(g.Key));

        private static int CategoryOrder(EquipmentCategory c) => c switch
        {
            EquipmentCategory.Eyewear => 0,
            EquipmentCategory.Hats => 1,
            EquipmentCategory.FullHeadwear => 2,
            EquipmentCategory.Clothing => 3,
            EquipmentCategory.Armor => 4,
            EquipmentCategory.FullBody => 5,
            _ => 99
        };
    }
}
