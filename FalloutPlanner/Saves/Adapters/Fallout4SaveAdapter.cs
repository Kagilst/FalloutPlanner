using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using FalloutPlanner.Games.Fallout4;

namespace FalloutPlanner.Saves.Adapters
{
    public static class Fallout4SaveAdapter
    {
        public static SavedCharacterFile ToFile(Fallout4Character character, string displayName, string? existingId = null)
        {
            var payload = character.CreateSavePayload();
            var chronological = payload.ActionLogNewestFirst.AsEnumerable().Reverse().ToList();

            return new SavedCharacterFile
            {
                Id = existingId ?? Guid.NewGuid().ToString("N"),
                GameId = GameIds.Fallout4,
                DisplayName = displayName,
                Level = character.Level,
                GameState = JsonSerializer.SerializeToElement(payload, SaveJsonOptions.Options),
                ActionLogChronological = chronological
            };
        }

        public static Fallout4Character FromFile(SavedCharacterFile file)
        {
            var payload = file.GameState.Deserialize<Fallout4SavePayload>(SaveJsonOptions.Options)
                ?? new Fallout4SavePayload();

            var starting = payload.Current?.Stats?.Clone()
                ?? new Fallout4CharacterStats { Name = file.DisplayName, Level = file.Level };

            var character = new Fallout4Character(starting);
            character.LoadFromSavePayload(payload, file.Id);
            if (string.IsNullOrWhiteSpace(character.Name))
                character.Name = file.DisplayName;
            return character;
        }
    }
}
