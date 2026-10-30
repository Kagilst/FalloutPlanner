using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FalloutPlanner.Saves
{
    public class SavedCharacterFile
    {
        public string Id { get; set; } = "";
        public string GameId { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public int Level { get; set; } = 1;
        public DateTimeOffset CreatedUtc { get; set; }
        public DateTimeOffset UpdatedUtc { get; set; }

        /// <summary>Game-specific JSON payload (current + baseline + action history).</summary>
        public JsonElement GameState { get; set; }

        /// <summary>Oldest-first action log for guides and display rebuild.</summary>
        public List<string> ActionLogChronological { get; set; } = new();
    }

    public class SavedCharacterSummary
    {
        public string Id { get; init; } = "";
        public string GameId { get; init; } = "";
        public string DisplayName { get; init; } = "";
        public int Level { get; init; }
        public DateTimeOffset UpdatedUtc { get; init; }
        public string FilePath { get; init; } = "";

        public string UpdatedDisplay => UpdatedUtc.ToLocalTime().ToString("g");
        public string ListDisplay => $"{DisplayName}  —  Lv {Level}  —  {UpdatedDisplay}";
    }

    public static class SaveJsonOptions
    {
        public static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        };
    }
}
