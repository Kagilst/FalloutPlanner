namespace FalloutPlanner.Games.Fallout4.Bobbleheads
{
    public enum Fallout4BobbleheadKind
    {
        Special,
        Perk
    }

    public class Fallout4Bobblehead
    {
        public string Name { get; init; } = "";
        public string EffectDisplay { get; init; } = "";
        public string Location { get; init; } = "";
        public Fallout4BobbleheadKind Kind { get; init; }
        /// <summary>SPECIAL property name when Kind is Special.</summary>
        public string? SpecialPropertyName { get; init; }
    }
}
