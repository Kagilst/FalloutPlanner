using System;

namespace FalloutPlanner.Games.FalloutNV.Implants
{
    public enum ImplantCategory
    {
        /// <summary>
        /// Default implants purchased from Dr. Usanagi.
        /// These consume implant slots (based on Endurance).
        /// </summary>
        Default,

        /// <summary>
        /// Old World Blues implants from the Sink Auto-Doc.
        /// These do NOT consume implant slots.
        /// </summary>
        OldWorldBlues
    }

    public class FalloutNVImplant
    {
        public string Name { get; init; } = "";
        public string DisplayName { get; init; } = "";
        public string Description { get; init; } = "";
        public string UnlockCondition { get; init; } = "";
        public int Cost { get; init; }
        public ImplantCategory Category { get; init; }

        /// <summary>
        /// For SPECIAL implants, the stat property name (e.g., "Strength").
        /// Null for non-SPECIAL implants.
        /// </summary>
        public string? SpecialStatName { get; init; }

        /// <summary>
        /// Whether this is the Endurance implant (which doesn't increase slots).
        /// </summary>
        public bool IsEnduranceImplant { get; init; }

        /// <summary>
        /// Apply the implant's effect to the character.
        /// </summary>
        public Action<FalloutNVCharacter>? ApplyEffect { get; init; }

        /// <summary>
        /// Remove the implant's effect from the character.
        /// </summary>
        public Action<FalloutNVCharacter>? RemoveEffect { get; init; }
    }
}
