using System.Collections.Generic;
using FalloutPlanner.Games.Fallout4;

namespace FalloutPlanner.Saves.Adapters
{
    public class Fallout4SavePayload
    {
        public CharacterActionState Current { get; set; } = new();
        public CharacterActionState? Baseline { get; set; }
        public List<CharacterActionState> ActionStates { get; set; } = new();
        public List<string> ActionLogNewestFirst { get; set; } = new();
    }
}
