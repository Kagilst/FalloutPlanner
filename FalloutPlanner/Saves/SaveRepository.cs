using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace FalloutPlanner.Saves
{
    public class SaveRepository
    {
        private readonly string _root;

        public SaveRepository(string? rootDirectory = null)
        {
            _root = rootDirectory
                ?? Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "FalloutPlanner",
                    "saves");
        }

        public string GetGameDirectory(string gameId)
        {
            var dir = Path.Combine(_root, gameId);
            Directory.CreateDirectory(dir);
            return dir;
        }

        public IReadOnlyList<SavedCharacterSummary> List(string gameId)
        {
            var dir = GetGameDirectory(gameId);
            var results = new List<SavedCharacterSummary>();

            foreach (var path in Directory.EnumerateFiles(dir, "*.json"))
            {
                try
                {
                    var json = File.ReadAllText(path);
                    var file = JsonSerializer.Deserialize<SavedCharacterFile>(json, SaveJsonOptions.Options);
                    if (file == null || file.GameId != gameId)
                        continue;

                    results.Add(new SavedCharacterSummary
                    {
                        Id = file.Id,
                        GameId = file.GameId,
                        DisplayName = file.DisplayName,
                        Level = file.Level,
                        UpdatedUtc = file.UpdatedUtc,
                        FilePath = path
                    });
                }
                catch
                {
                    // Skip corrupt files.
                }
            }

            return results
                .OrderByDescending(s => s.UpdatedUtc)
                .ToList();
        }

        public SavedCharacterFile? Load(string gameId, string saveId)
        {
            var path = GetPath(gameId, saveId);
            if (!File.Exists(path))
                return null;

            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<SavedCharacterFile>(json, SaveJsonOptions.Options);
        }

        public void Save(SavedCharacterFile file)
        {
            if (string.IsNullOrWhiteSpace(file.Id))
                file.Id = Guid.NewGuid().ToString("N");
            if (file.CreatedUtc == default)
                file.CreatedUtc = DateTimeOffset.UtcNow;
            file.UpdatedUtc = DateTimeOffset.UtcNow;

            var path = GetPath(file.GameId, file.Id);
            var json = JsonSerializer.Serialize(file, SaveJsonOptions.Options);
            File.WriteAllText(path, json);
        }

        public bool Delete(string gameId, string saveId)
        {
            var path = GetPath(gameId, saveId);
            if (!File.Exists(path))
                return false;
            File.Delete(path);
            return true;
        }

        private string GetPath(string gameId, string saveId) =>
            Path.Combine(GetGameDirectory(gameId), $"{saveId}.json");
    }
}
