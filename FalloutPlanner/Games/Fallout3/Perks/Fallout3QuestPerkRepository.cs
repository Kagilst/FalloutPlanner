using System.Collections.Generic;

namespace FalloutPlanner.Games.Fallout3.Perks
{
    public static class Fallout3QuestPerkRepository
    {
        public const string AntMightSight = "Ant Might/Sight";
        public const string SurvivalExpert = "Survival Expert";
        public const string DreamCrusher = "Dream Crusher";

        public static IReadOnlyList<Fallout3Perk> All { get; } = new List<Fallout3Perk>
        {
            new()
            {
                Name = "Covert Ops",
                Description = "+3 Science, Guns, & Lockpick",
                ApplyEffect = c =>
                {
                    c.TryAddSkillPointsFromPerk(nameof(c.Science), 3);
                    c.TryAddSkillPointsFromPerk(nameof(c.SmallGuns), 3);
                    c.TryAddSkillPointsFromPerk(nameof(c.Lockpick), 3);
                }
            },
            new()
            {
                Name = AntMightSight,
                Description = "+1 to Strength or Perception"
                // Choice applied via AntMightSightWindow
            },
            new()
            {
                Name = DreamCrusher,
                Description = "Reduces enemy's crit chance by 50%, 30% discount at Craterside Supply, +30 points to Moira's repair skill"
            },
            new()
            {
                Name = "Hematophage",
                Description = "Bloodpacks heal 20 HP instead of 1"
            },
            new()
            {
                Name = "Yew's Bear Charm",
                Description = "+10 Speech",
                ApplyEffect = c => c.TryAddSkillPointsFromPerk(nameof(c.Speech), 10)
            },
            new()
            {
                Name = "Power Armor Training",
                Description = "Can now use power armor"
            },
            new()
            {
                Name = "Barkskin",
                Description = "+5 DR Permanently",
                ApplyEffect = c => c.DamageResist += 5
            },
            new()
            {
                Name = "Rad Regeneration",
                Description = "Crippled limbs regenerate if suffering from advanced rad. poisoning"
            },
            new()
            {
                Name = SurvivalExpert,
                Description =
                    "Perk always grants +2-6% poison and rad resistances dependent on rank.\n" +
                    "Based on Rank Perk Also Grants:\n" +
                    "+5-15 HP for Normal Version\n" +
                    "+2-6 to Medicine and Science for Smart Version\n" +
                    "+2-6% Damage resistance for Tough Version\n" +
                    "+2-6 to Sneak and Speech for Sly Version\n" +
                    "+1-3 Critical Chance for Snide Version"
                // Rank/variant applied via SurvivalExpertWindow
            },
            new()
            {
                Name = "Wired Reflexes",
                Description = "+10% V.A.T.S Accuracy"
            },
            new()
            {
                Name = "Auto Axepert",
                Description = "+25% more damage with the auto axe"
            },
            new()
            {
                Name = "Ghoul Ecology",
                Description = "+5 Points of damage to ghouls (To everyone due to glitch)"
            },
            new()
            {
                Name = "Xenotech Expert",
                Description = "+20% damage with alien weapons"
            },
            new()
            {
                Name = "Booster Shot",
                Description = "+10% radiation resistance",
                ApplyEffect = c => c.RadiationResist += 10
            },
            new()
            {
                Name = "Pitt Fighter",
                Description = "+3% radiation resistance and +3% damage resistance",
                ApplyEffect = c =>
                {
                    c.RadiationResist += 3;
                    c.DamageResist += 3;
                }
            },
            new()
            {
                Name = "Punga Power",
                Description = "-10 rads from wild punga fruit and -15 rads from refined punga fruit"
            },
            new()
            {
                Name = "Superior Defender",
                Description = "+5 points of damage and +10 DR when standing still"
            },
        };

        public enum SurvivalRank
        {
            JuniorSurvivor,
            SurvivalExpert,
            SurvivalGuru
        }

        public enum SurvivalVariant
        {
            Normal,
            Smart,
            Tough,
            Sly,
            Snide
        }

        public static string SurvivalRankDisplay(SurvivalRank rank) => rank switch
        {
            SurvivalRank.JuniorSurvivor => "Junior Survivor",
            SurvivalRank.SurvivalExpert => "Survival Expert",
            SurvivalRank.SurvivalGuru => "Survival Guru",
            _ => rank.ToString()
        };

        public static string SurvivalVariantDisplay(SurvivalVariant variant) => variant switch
        {
            SurvivalVariant.Normal => "Normal (None/Top answer)",
            SurvivalVariant.Smart => "Smart (IN/PE)",
            SurvivalVariant.Tough => "Tough (ST/EN)",
            SurvivalVariant.Sly => "Sly (AG/CH)",
            SurvivalVariant.Snide => "Snide (Bottom answer)",
            _ => variant.ToString()
        };

        public static int SurvivalTier(SurvivalRank rank) => rank switch
        {
            SurvivalRank.JuniorSurvivor => 1,
            SurvivalRank.SurvivalExpert => 2,
            SurvivalRank.SurvivalGuru => 3,
            _ => 1
        };

        public static string BuildSurvivalDescription(SurvivalRank rank, SurvivalVariant variant)
        {
            int tier = SurvivalTier(rank);
            int resist = tier * 2;
            string baseLine = $"+{resist}% Poison and Radiation Resistance";
            string extra = variant switch
            {
                SurvivalVariant.Normal => $"+{tier * 5} HP",
                SurvivalVariant.Smart => $"+{tier * 2} Medicine, +{tier * 2} Science",
                SurvivalVariant.Tough => $"+{resist}% Damage Resistance",
                SurvivalVariant.Sly => $"+{tier * 2} Sneak, +{tier * 2} Speech",
                SurvivalVariant.Snide => $"+{tier}% Critical Chance",
                _ => ""
            };

            string rankNote = rank switch
            {
                SurvivalRank.JuniorSurvivor => "(complete fewer than 5 optional objectives)",
                SurvivalRank.SurvivalExpert => "(complete 5 to 8 optional objectives)",
                SurvivalRank.SurvivalGuru => "(complete all optional objectives)",
                _ => ""
            };

            return $"{SurvivalRankDisplay(rank)} {rankNote}\n" +
                   $"{SurvivalVariantDisplay(variant)}\n\n" +
                   $"{baseLine}\n{extra}";
        }

        public static void ApplySurvivalExpert(Fallout3Character c, SurvivalRank rank, SurvivalVariant variant) =>
            ApplySurvivalExpertDelta(c, rank, variant, +1);

        public static void RemoveSurvivalExpert(Fallout3Character c, SurvivalRank rank, SurvivalVariant variant) =>
            ApplySurvivalExpertDelta(c, rank, variant, -1);

        private static void ApplySurvivalExpertDelta(
            Fallout3Character c, SurvivalRank rank, SurvivalVariant variant, int sign)
        {
            int tier = SurvivalTier(rank);
            int resist = tier * 2;
            c.PoisonResist += sign * resist;
            c.RadiationResist += sign * resist;

            switch (variant)
            {
                case SurvivalVariant.Normal:
                    c.Health += sign * tier * 5;
                    break;
                case SurvivalVariant.Smart:
                    c.TryAddSkillPointsFromPerk(nameof(c.Medicine), sign * tier * 2);
                    c.TryAddSkillPointsFromPerk(nameof(c.Science), sign * tier * 2);
                    break;
                case SurvivalVariant.Tough:
                    c.DamageResist += sign * resist;
                    break;
                case SurvivalVariant.Sly:
                    c.TryAddSkillPointsFromPerk(nameof(c.Sneak), sign * tier * 2);
                    c.TryAddSkillPointsFromPerk(nameof(c.Speech), sign * tier * 2);
                    break;
                case SurvivalVariant.Snide:
                    c.CriticalChance += sign * tier;
                    break;
            }
        }

        public static void RemoveMiscPerkEffects(Fallout3Character c, QuestPerkState state)
        {
            if (state?.Perk == null)
                return;

            string name = state.Perk.Name;
            if (name == AntMightSight && !string.IsNullOrEmpty(state.ChoiceDetail))
            {
                c.TryDecreaseSpecialFromPerk(state.ChoiceDetail);
                return;
            }

            if (name == SurvivalExpert && !string.IsNullOrEmpty(state.ChoiceDetail))
            {
                var parts = state.ChoiceDetail.Split('|');
                if (parts.Length == 2 &&
                    System.Enum.TryParse(parts[0], out SurvivalRank rank) &&
                    System.Enum.TryParse(parts[1], out SurvivalVariant variant))
                {
                    RemoveSurvivalExpert(c, rank, variant);
                }
                return;
            }

            // Reverse simple ApplyEffect bonuses by name
            switch (name)
            {
                case "Covert Ops":
                    c.TryAddSkillPointsFromPerk(nameof(c.Science), -3);
                    c.TryAddSkillPointsFromPerk(nameof(c.SmallGuns), -3);
                    c.TryAddSkillPointsFromPerk(nameof(c.Lockpick), -3);
                    break;
                case "Yew's Bear Charm":
                    c.TryAddSkillPointsFromPerk(nameof(c.Speech), -10);
                    break;
                case "Barkskin":
                    c.DamageResist -= 5;
                    break;
                case "Booster Shot":
                    c.RadiationResist -= 10;
                    break;
                case "Pitt Fighter":
                    c.RadiationResist -= 3;
                    c.DamageResist -= 3;
                    break;
            }
        }
    }
}
