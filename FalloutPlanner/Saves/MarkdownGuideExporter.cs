using System.Collections.Generic;
using System.Linq;
using System.Text;
using FalloutPlanner.Games.Fallout4;
using FalloutPlanner.Games.Fallout4.Perks;
using Microsoft.Win32;

namespace FalloutPlanner.Saves
{
    public static class MarkdownGuideExporter
    {
        public static bool TryExportFallout4(Fallout4Character character, System.Windows.Window? owner)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Export Leveling Guide",
                Filter = "Markdown (*.md)|*.md|All files (*.*)|*.*",
                FileName = SanitizeFileName($"{character.Name}-{GameIds.FileNameLabel(GameIds.Fallout4)}-leveling-guide.md"),
                DefaultExt = ".md",
                AddExtension = true
            };

            if (dialog.ShowDialog(owner) != true)
                return false;

            var markdown = BuildFallout4Guide(character);
            System.IO.File.WriteAllText(dialog.FileName, markdown);
            System.Windows.MessageBox.Show(owner,
                $"Guide exported to:\n{dialog.FileName}",
                "Export Guide",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
            return true;
        }

        public static string BuildFallout4Guide(Fallout4Character character)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"# {character.Name} — Fallout 4 Leveling Guide");
            sb.AppendLine();
            sb.AppendLine($"- **Game:** Fallout 4");
            sb.AppendLine($"- **Level:** {character.Level}");
            sb.AppendLine($"- **Perk Points:** {character.PerkPoints}");
            sb.AppendLine();
            AppendSpecialSections(
                sb,
                character.GetSpecialLine(starting: true),
                character.GetSpecialLine(starting: false),
                taggedSkillsLine: null);
            sb.AppendLine("## Steps");
            sb.AppendLine();

            var steps = character.ActionLog.Reverse().ToList();
            if (steps.Count == 0)
                sb.AppendLine("_No steps recorded._");
            else
            {
                int n = 1;
                foreach (var step in steps)
                {
                    sb.AppendLine($"{n}. {EnrichFallout4Step(step)}");
                    n++;
                }
            }

            sb.AppendLine();
            sb.AppendLine("## Current Status");
            sb.AppendLine();
            sb.AppendLine($"- Hit Points: {character.HitPoints}");
            sb.AppendLine($"- Action Points: {character.ActionPoints}");
            sb.AppendLine($"- Carry Weight: {character.CarryWeight}");
            sb.AppendLine();
            sb.AppendLine("### Owned Perks");
            sb.AppendLine();
            var owned = character.GetOwnedPerks();
            if (owned.Count == 0)
                sb.AppendLine("_None._");
            else
            {
                foreach (var perk in owned)
                {
                    sb.AppendLine($"- **{perk.DisplayName}**");
                    if (!string.IsNullOrWhiteSpace(perk.Description))
                        sb.AppendLine($"  - {perk.Description.Replace("\n", "\n  - ")}");
                }
            }

            return sb.ToString();
        }

        public static bool TryExportGeneric(
            string gameDisplayName,
            string characterName,
            int level,
            string startingSpecialLine,
            string completedSpecialLine,
            string? taggedSkillsLine,
            IEnumerable<string> actionLogNewestFirst,
            string statusSection,
            System.Windows.Window? owner)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Export Leveling Guide",
                Filter = "Markdown (*.md)|*.md|All files (*.*)|*.*",
                FileName = SanitizeFileName(
                    $"{characterName}-{GameIds.FileNameLabelFromDisplayName(gameDisplayName)}-leveling-guide.md"),
                DefaultExt = ".md",
                AddExtension = true
            };

            if (dialog.ShowDialog(owner) != true)
                return false;

            var sb = new StringBuilder();
            sb.AppendLine($"# {characterName} — {gameDisplayName} Leveling Guide");
            sb.AppendLine();
            sb.AppendLine($"- **Game:** {gameDisplayName}");
            sb.AppendLine($"- **Level:** {level}");
            sb.AppendLine();
            AppendSpecialSections(sb, startingSpecialLine, completedSpecialLine, taggedSkillsLine);
            sb.AppendLine("## Steps");
            sb.AppendLine();

            var steps = actionLogNewestFirst.Reverse().ToList();
            if (steps.Count == 0)
                sb.AppendLine("_No steps recorded._");
            else
            {
                int n = 1;
                foreach (var step in steps)
                {
                    sb.AppendLine($"{n}. {step}");
                    n++;
                }
            }

            sb.AppendLine();
            sb.AppendLine("## Current Status");
            sb.AppendLine();
            sb.AppendLine(statusSection);

            System.IO.File.WriteAllText(dialog.FileName, sb.ToString());
            System.Windows.MessageBox.Show(owner,
                $"Guide exported to:\n{dialog.FileName}",
                "Export Guide",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
            return true;
        }

        private static void AppendSpecialSections(
            StringBuilder sb,
            string startingSpecialLine,
            string completedSpecialLine,
            string? taggedSkillsLine)
        {
            sb.AppendLine("## Starting SPECIAL");
            sb.AppendLine();
            sb.AppendLine(string.IsNullOrWhiteSpace(startingSpecialLine) ? "_Unavailable._" : startingSpecialLine);
            if (!string.IsNullOrWhiteSpace(taggedSkillsLine))
            {
                sb.AppendLine();
                sb.AppendLine($"**Tagged Skills:** {taggedSkillsLine}");
            }
            sb.AppendLine();
            sb.AppendLine("## Completed SPECIAL");
            sb.AppendLine();
            sb.AppendLine(string.IsNullOrWhiteSpace(completedSpecialLine) ? "_Unavailable._" : completedSpecialLine);
            sb.AppendLine();
        }

        private static string EnrichFallout4Step(string step)
        {
            // "Perk: Gunslinger Rank 1" → append short description when available
            const string prefix = "Perk: ";
            if (!step.StartsWith(prefix))
                return step;

            var rest = step.Substring(prefix.Length);
            var rankIdx = rest.LastIndexOf(" Rank ");
            if (rankIdx <= 0)
                return step;

            var name = rest.Substring(0, rankIdx);
            if (!int.TryParse(rest.Substring(rankIdx + 6), out int rank))
                return step;

            var perk = Fallout4PerkRepository.GetPerk(name);
            var desc = perk?.GetRank(rank)?.Description;
            if (string.IsNullOrWhiteSpace(desc))
                return step;

            return $"{step} — {desc}";
        }

        private static string SanitizeFileName(string name)
        {
            foreach (var c in System.IO.Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }
    }
}
