using System;

namespace FalloutPlanner.Games.Fallout1.Traits
{
    public class Fallout1Trait
    {
        public string Name { get; init; } = "";
        public string Benefit { get; init; } = "";
        public string Penalty { get; init; } = "";
        public string Description => $"Benefit: {Benefit}\nPenalty: {Penalty}";

        /// <summary>
        /// Whether this trait has permanent numeric effects that should be applied at creation.
        /// </summary>
        public bool HasPermanentEffect { get; init; }

        /// <summary>
        /// Applied when trait is selected at character creation.
        /// </summary>
        public Action<Fallout1Character>? ApplyEffect { get; init; }

        /// <summary>
        /// Used to remove the trait's effects if needed.
        /// </summary>
        public Action<Fallout1Character>? RemoveEffect { get; init; }
    }
}
