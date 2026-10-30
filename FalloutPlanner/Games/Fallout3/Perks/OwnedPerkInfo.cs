namespace FalloutPlanner.Games.Fallout3.Perks
{
    public class OwnedPerkInfo
    {
        public Fallout3Perk Perk { get; set; }
        public int CurrentRank { get; set; }
        public string DisplayName { get; set; }
        /// <summary>When set (quest perks with choices), shown instead of Perk.Description.</summary>
        public string DescriptionOverride { get; set; }
        public bool IsMiscPerk { get; set; }
    }

    public class QuestPerkState
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public Fallout3Perk Perk { get; set; }
        public string ChoiceDetail { get; set; }
    }
}
