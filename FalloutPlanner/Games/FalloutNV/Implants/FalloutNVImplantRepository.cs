using System.Collections.Generic;
using System.Linq;

namespace FalloutPlanner.Games.FalloutNV.Implants
{
    public static class FalloutNVImplantRepository
    {
        public static IReadOnlyList<FalloutNVImplant> All { get; } = new List<FalloutNVImplant>
        {
            // Default Implants (Dr. Usanagi)
            new()
            {
                Name = "StrengthImplant",
                DisplayName = "Hypertrophy Accelerator",
                Description = "+1 Strength",
                UnlockCondition = "Buy from Doctor Usanagi for 4000 caps",
                Cost = 4000,
                Category = ImplantCategory.Default,
                SpecialStatName = "Strength",
                ApplyEffect = c => c.TryIncreaseSpecialFromPerk("Strength"),
                RemoveEffect = c => c.TryDecreaseSpecialFromPerk("Strength")
            },
            new()
            {
                Name = "PerceptionImplant",
                DisplayName = "Optics Enhancer",
                Description = "+1 Perception",
                UnlockCondition = "Buy from Doctor Usanagi for 4000 caps",
                Cost = 4000,
                Category = ImplantCategory.Default,
                SpecialStatName = "Perception",
                ApplyEffect = c => c.TryIncreaseSpecialFromPerk("Perception"),
                RemoveEffect = c => c.TryDecreaseSpecialFromPerk("Perception")
            },
            new()
            {
                Name = "EnduranceImplant",
                DisplayName = "Nociception Regulator",
                Description = "+1 Endurance (does not increase implant slots)",
                UnlockCondition = "Buy from Doctor Usanagi for 4000 caps",
                Cost = 4000,
                Category = ImplantCategory.Default,
                SpecialStatName = "Endurance",
                IsEnduranceImplant = true,
                ApplyEffect = c => c.TryIncreaseSpecialFromPerk("Endurance"),
                RemoveEffect = c => c.TryDecreaseSpecialFromPerk("Endurance")
            },
            new()
            {
                Name = "CharismaImplant",
                DisplayName = "Empathy Synthesizer",
                Description = "+1 Charisma",
                UnlockCondition = "Buy from Doctor Usanagi for 4000 caps",
                Cost = 4000,
                Category = ImplantCategory.Default,
                SpecialStatName = "Charisma",
                ApplyEffect = c => c.TryIncreaseSpecialFromPerk("Charisma"),
                RemoveEffect = c => c.TryDecreaseSpecialFromPerk("Charisma")
            },
            new()
            {
                Name = "IntelligenceImplant",
                DisplayName = "Logic Co-Processor",
                Description = "+1 Intelligence",
                UnlockCondition = "Buy from Doctor Usanagi for 4000 caps",
                Cost = 4000,
                Category = ImplantCategory.Default,
                SpecialStatName = "Intelligence",
                ApplyEffect = c => c.TryIncreaseSpecialFromPerk("Intelligence"),
                RemoveEffect = c => c.TryDecreaseSpecialFromPerk("Intelligence")
            },
            new()
            {
                Name = "AgilityImplant",
                DisplayName = "Reflex Booster",
                Description = "+1 Agility",
                UnlockCondition = "Buy from Doctor Usanagi for 4000 caps",
                Cost = 4000,
                Category = ImplantCategory.Default,
                SpecialStatName = "Agility",
                ApplyEffect = c => c.TryIncreaseSpecialFromPerk("Agility"),
                RemoveEffect = c => c.TryDecreaseSpecialFromPerk("Agility")
            },
            new()
            {
                Name = "LuckImplant",
                DisplayName = "Probability Calculator",
                Description = "+1 Luck",
                UnlockCondition = "Buy from Doctor Usanagi for 4000 caps",
                Cost = 4000,
                Category = ImplantCategory.Default,
                SpecialStatName = "Luck",
                ApplyEffect = c => c.TryIncreaseSpecialFromPerk("Luck"),
                RemoveEffect = c => c.TryDecreaseSpecialFromPerk("Luck")
            },
            new()
            {
                Name = "SubDermalArmor",
                DisplayName = "NEMEAN",
                Description = "Sub-Dermal Armor: +4 Damage Threshold",
                UnlockCondition = "Buy from Doctor Usanagi for 8000 caps",
                Cost = 8000,
                Category = ImplantCategory.Default,
                ApplyEffect = c => c.DamageThreshold += 4,
                RemoveEffect = c => c.DamageThreshold -= 4
            },
            new()
            {
                Name = "MonocyteBreeder",
                DisplayName = "PHOENIX",
                Description = "Monocyte Breeder: Regenerate 1 health per 10 seconds",
                UnlockCondition = "Buy from Doctor Usanagi for 12000 caps",
                Cost = 12000,
                Category = ImplantCategory.Default
                // No stat effect, just passive regen
            },

            // Old World Blues Implants (Sink Auto-Doc)
            new()
            {
                Name = "ImplantC13",
                DisplayName = "Implant C-13",
                Description = "+10% damage against cazadores",
                UnlockCondition = "Surgery from Sink Auto-Doc for 8,000 caps",
                Cost = 8000,
                Category = ImplantCategory.OldWorldBlues
            },
            new()
            {
                Name = "ImplantM5",
                DisplayName = "Implant M-5",
                Description = "Crouched movement speed increased by 20%",
                UnlockCondition = "Surgery from Sink Auto-Doc for 10,000 caps",
                Cost = 10000,
                Category = ImplantCategory.OldWorldBlues
            },
            new()
            {
                Name = "ImplantY3",
                DisplayName = "Implant Y-3",
                Description = "Removes any radiation taken from drinking irradiated water",
                UnlockCondition = "Surgery from Sink Auto-Doc for 12,000 caps",
                Cost = 12000,
                Category = ImplantCategory.OldWorldBlues
            },
            new()
            {
                Name = "ImplantY7",
                DisplayName = "Implant Y-7",
                Description = "+5 health and +2 restored action points through food consumption",
                UnlockCondition = "Surgery from Sink Auto-Doc for 20,000 caps",
                Cost = 20000,
                Category = ImplantCategory.OldWorldBlues
            }
        };

        public static IEnumerable<FalloutNVImplant> DefaultImplants =>
            All.Where(i => i.Category == ImplantCategory.Default);

        public static IEnumerable<FalloutNVImplant> OWBImplants =>
            All.Where(i => i.Category == ImplantCategory.OldWorldBlues);

        public static FalloutNVImplant? GetByName(string name) =>
            All.FirstOrDefault(i => i.Name == name);
    }
}
