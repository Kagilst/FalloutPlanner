using System.Collections.Generic;

namespace FalloutPlanner.Saves.Adapters
{
    /// <summary>Shared shape for FO1–NV save payloads (game-specific CharacterActionState).</summary>
    public class ClassicGameSavePayload<TState>
    {
        public TState? Current { get; set; }
        public TState? Baseline { get; set; }
        public List<TState> ActionStates { get; set; } = new();
        public List<string> ActionLogNewestFirst { get; set; } = new();
    }
}
