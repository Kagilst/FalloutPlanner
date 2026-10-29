using System;

namespace FalloutPlanner.Games.FalloutNV.Traits
{
    public class FalloutNVTrait
    {
        public string Name { get; init; } = "";
        public string Benefit { get; init; } = "";
        public string Penalty { get; init; } = "";
        public string Description => $"Benefit: {Benefit}\nPenalty: {Penalty}";

        /// <summary>
        /// Whether this trait has permanent numeric effects that should be applied at creation.
        /// Situational traits (like Claustrophobia, Early Bird) are selectable but don't modify stats.
        /// </summary>
        public bool HasPermanentEffect { get; init; }

        /// <summary>
        /// Applied when trait is selected at character creation.
        /// Only used for permanent numeric effects.
        /// </summary>
        public Action<FalloutNVCharacter>? ApplyEffect { get; init; }

        /// <summary>
        /// Used to remove the trait's effects if needed.
        /// </summary>
        public Action<FalloutNVCharacter>? RemoveEffect { get; init; }
    }
}
