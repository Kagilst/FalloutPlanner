using System;
using System.Windows;
using FalloutPlanner.Games.Fallout1;
using FalloutPlanner.Games.Fallout2;
using FalloutPlanner.Games.Fallout3;
using FalloutPlanner.Games.Fallout4;
using FalloutPlanner.Games.FalloutNV;
using FalloutPlanner.Saves.Adapters;

namespace FalloutPlanner.Saves
{
    /// <summary>Shared save/update flow used by leveling pages.</summary>
    public static class CharacterSaveService
    {
        private static readonly SaveRepository Repository = new();

        public static SaveRepository SharedRepository => Repository;

        public static bool TrySaveFallout4(Fallout4Character character, Window? owner) =>
            TrySave(
                owner,
                character.ActiveSaveId,
                character.Name,
                GameIds.Fallout4,
                (name, id) => Fallout4SaveAdapter.ToFile(character, name, id),
                id => character.ActiveSaveId = id,
                character.MarkClean);

        public static bool TrySaveFallout1(Fallout1Character character, Window? owner) =>
            TrySave(
                owner,
                character.ActiveSaveId,
                character.Name,
                GameIds.Fallout1,
                (name, id) => Fallout1SaveAdapter.ToFile(character, name, id),
                id => character.ActiveSaveId = id,
                character.MarkClean);

        public static bool TrySaveFallout2(Fallout2Character character, Window? owner) =>
            TrySave(
                owner,
                character.ActiveSaveId,
                character.Name,
                GameIds.Fallout2,
                (name, id) => Fallout2SaveAdapter.ToFile(character, name, id),
                id => character.ActiveSaveId = id,
                character.MarkClean);

        public static bool TrySaveFallout3(Fallout3Character character, Window? owner) =>
            TrySave(
                owner,
                character.ActiveSaveId,
                character.Name,
                GameIds.Fallout3,
                (name, id) => Fallout3SaveAdapter.ToFile(character, name, id),
                id => character.ActiveSaveId = id,
                character.MarkClean);

        public static bool TrySaveFalloutNV(FalloutNVCharacter character, Window? owner) =>
            TrySave(
                owner,
                character.ActiveSaveId,
                character.Name,
                GameIds.FalloutNV,
                (name, id) => FalloutNVSaveAdapter.ToFile(character, name, id),
                id => character.ActiveSaveId = id,
                character.MarkClean);

        private static bool TrySave(
            Window? owner,
            string? existingId,
            string characterName,
            string gameId,
            Func<string, string?, SavedCharacterFile> buildFile,
            Action<string> setActiveId,
            Action markClean)
        {
            SaveDecision decision = SaveDecision.CreateNew;

            if (!string.IsNullOrEmpty(existingId))
            {
                decision = SavePrompt.AskUpdateOrNew(owner);
                if (decision == SaveDecision.Cancel)
                    return false;
            }

            if (decision == SaveDecision.UpdateExisting && !string.IsNullOrEmpty(existingId))
            {
                var existing = Repository.Load(gameId, existingId);
                var displayName = existing?.DisplayName ?? characterName;
                var file = buildFile(displayName, existingId);
                if (existing != null)
                    file.CreatedUtc = existing.CreatedUtc;
                Repository.Save(file);
                setActiveId(file.Id);
                markClean();
                MessageBox.Show(owner, "Character save updated.", "Save",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }

            var name = SavePrompt.AskSaveName(owner, characterName, gameId);
            if (string.IsNullOrWhiteSpace(name))
                return false;

            var newFile = buildFile(name, null);
            Repository.Save(newFile);
            setActiveId(newFile.Id);
            markClean();
            MessageBox.Show(owner, "Character saved.", "Save",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }
    }
}
