using System;
using System.Linq;
using System.Text.Json;
using FalloutPlanner.Games.Fallout1;
using FalloutPlanner.Games.Fallout2;
using FalloutPlanner.Games.Fallout3;
using FalloutPlanner.Games.FalloutNV;

namespace FalloutPlanner.Saves.Adapters
{
    public static class Fallout1SaveAdapter
    {
        public static SavedCharacterFile ToFile(Fallout1Character character, string displayName, string? existingId = null)
        {
            var payload = character.CreateSavePayload();
            return new SavedCharacterFile
            {
                Id = existingId ?? Guid.NewGuid().ToString("N"),
                GameId = GameIds.Fallout1,
                DisplayName = displayName,
                Level = character.Level,
                GameState = JsonSerializer.SerializeToElement(payload, SaveJsonOptions.Options),
                ActionLogChronological = payload.ActionLogNewestFirst.AsEnumerable().Reverse().ToList()
            };
        }

        public static Fallout1Character FromFile(SavedCharacterFile file)
        {
            var payload = file.GameState.Deserialize<ClassicGameSavePayload<Games.Fallout1.CharacterActionState>>(SaveJsonOptions.Options)
                ?? new ClassicGameSavePayload<Games.Fallout1.CharacterActionState>();
            var starting = payload.Current?.Stats?.Clone()
                ?? new Fallout1CharacterStats { CharacterName = file.DisplayName, Level = Math.Max(1, file.Level) };
            var character = new Fallout1Character(starting);
            character.LoadFromSavePayload(payload, file.Id);
            if (string.IsNullOrWhiteSpace(character.Name))
                character.Name = file.DisplayName;
            return character;
        }
    }

    public static class Fallout2SaveAdapter
    {
        public static SavedCharacterFile ToFile(Fallout2Character character, string displayName, string? existingId = null)
        {
            var payload = character.CreateSavePayload();
            return new SavedCharacterFile
            {
                Id = existingId ?? Guid.NewGuid().ToString("N"),
                GameId = GameIds.Fallout2,
                DisplayName = displayName,
                Level = character.Level,
                GameState = JsonSerializer.SerializeToElement(payload, SaveJsonOptions.Options),
                ActionLogChronological = payload.ActionLogNewestFirst.AsEnumerable().Reverse().ToList()
            };
        }

        public static Fallout2Character FromFile(SavedCharacterFile file)
        {
            var payload = file.GameState.Deserialize<ClassicGameSavePayload<Games.Fallout2.CharacterActionState>>(SaveJsonOptions.Options)
                ?? new ClassicGameSavePayload<Games.Fallout2.CharacterActionState>();
            var starting = payload.Current?.Stats?.Clone()
                ?? new Fallout2CharacterStats { CharacterName = file.DisplayName, Level = Math.Max(1, file.Level) };
            var character = new Fallout2Character(starting);
            character.LoadFromSavePayload(payload, file.Id);
            if (string.IsNullOrWhiteSpace(character.Name))
                character.Name = file.DisplayName;
            return character;
        }
    }

    public static class Fallout3SaveAdapter
    {
        public static SavedCharacterFile ToFile(Fallout3Character character, string displayName, string? existingId = null)
        {
            var payload = character.CreateSavePayload();
            return new SavedCharacterFile
            {
                Id = existingId ?? Guid.NewGuid().ToString("N"),
                GameId = GameIds.Fallout3,
                DisplayName = displayName,
                Level = character.Level,
                GameState = JsonSerializer.SerializeToElement(payload, SaveJsonOptions.Options),
                ActionLogChronological = payload.ActionLogNewestFirst.AsEnumerable().Reverse().ToList()
            };
        }

        public static Fallout3Character FromFile(SavedCharacterFile file)
        {
            var payload = file.GameState.Deserialize<ClassicGameSavePayload<Games.Fallout3.CharacterActionState>>(SaveJsonOptions.Options)
                ?? new ClassicGameSavePayload<Games.Fallout3.CharacterActionState>();
            var starting = payload.Current?.Stats?.Clone()
                ?? new Fallout3CharacterStats { CharacterName = file.DisplayName, Level = Math.Max(1, file.Level) };
            var character = new Fallout3Character(starting);
            character.LoadFromSavePayload(payload, file.Id);
            if (string.IsNullOrWhiteSpace(character.Name))
                character.Name = file.DisplayName;
            return character;
        }
    }

    public static class FalloutNVSaveAdapter
    {
        public static SavedCharacterFile ToFile(FalloutNVCharacter character, string displayName, string? existingId = null)
        {
            var payload = character.CreateSavePayload();
            return new SavedCharacterFile
            {
                Id = existingId ?? Guid.NewGuid().ToString("N"),
                GameId = GameIds.FalloutNV,
                DisplayName = displayName,
                Level = character.Level,
                GameState = JsonSerializer.SerializeToElement(payload, SaveJsonOptions.Options),
                ActionLogChronological = payload.ActionLogNewestFirst.AsEnumerable().Reverse().ToList()
            };
        }

        public static FalloutNVCharacter FromFile(SavedCharacterFile file)
        {
            var payload = file.GameState.Deserialize<ClassicGameSavePayload<Games.FalloutNV.CharacterActionState>>(SaveJsonOptions.Options)
                ?? new ClassicGameSavePayload<Games.FalloutNV.CharacterActionState>();
            var starting = payload.Current?.Stats?.Clone()
                ?? new FalloutNVCharacterStats { CharacterName = file.DisplayName, Level = Math.Max(1, file.Level) };
            var character = new FalloutNVCharacter(starting);
            character.LoadFromSavePayload(payload, file.Id);
            if (string.IsNullOrWhiteSpace(character.Name))
                character.Name = file.DisplayName;
            return character;
        }
    }
}
