using System;

namespace FalloutPlanner.Games.Fallout3.Bobbleheads
{
    public enum BobbleheadEffectType
    {
        Special,
        Skill
    }

    public class Fallout3Bobblehead
    {
        public string Name { get; init; } = "";
        public string EffectDisplay { get; init; } = "";
        public string Location { get; init; } = "";
        public BobbleheadEffectType EffectType { get; init; }
        /// <summary>Property name on Fallout3Character (SPECIAL or skill).</summary>
        public string StatPropertyName { get; init; } = "";
        public int Amount { get; init; }
        public Action<Fallout3Character>? ApplyEffect { get; init; }
    }
}
