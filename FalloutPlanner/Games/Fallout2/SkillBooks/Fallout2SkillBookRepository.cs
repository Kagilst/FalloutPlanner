using System.Collections.Generic;
using System.Linq;

namespace FalloutPlanner.Games.Fallout2.SkillBooks
{
    /// <summary>
    /// Fallout 2 skill books. In FO2, skill books give variable skill points based on current skill level.
    /// The formula is: (10 - current skill level / 10) skill points, minimum 1.
    /// With Comprehension perk: 50% more points.
    /// </summary>
    public static class Fallout2SkillBookRepository
    {
        public static IReadOnlyList<Fallout2SkillBookType> BookTypes { get; } = new List<Fallout2SkillBookType>
        {
            new()
            {
                Name = "Guns and Bullets",
                SkillName = "Small Guns",
                SkillPropertyName = nameof(Fallout2Character.SmallGuns),
                MaxCount = -1
            },
            new()
            {
                Name = "Big Book of Science",
                SkillName = "Science",
                SkillPropertyName = nameof(Fallout2Character.Science),
                MaxCount = -1
            },
            new()
            {
                Name = "Dean's Electronics",
                SkillName = "Repair",
                SkillPropertyName = nameof(Fallout2Character.Repair),
                MaxCount = -1
            },
            new()
            {
                Name = "First Aid Book",
                SkillName = "First Aid",
                SkillPropertyName = nameof(Fallout2Character.FirstAid),
                MaxCount = -1
            },
            new()
            {
                Name = "Scout Handbook",
                SkillName = "Outdoorsman",
                SkillPropertyName = nameof(Fallout2Character.Outdoorsman),
                MaxCount = -1
            }
        };

        public static Fallout2SkillBookType GetBookType(string name) =>
            BookTypes.FirstOrDefault(b => b.Name == name);

        /// <summary>
        /// Calculate skill points gained from reading a book at a given skill level.
        /// In FO2: (10 - current skill level / 10), minimum 1.
        /// With Comprehension: 50% more (rounded down).
        /// </summary>
        public static int CalculateSkillGain(int currentSkillLevel, bool hasComprehension)
        {
            int baseGain = 10 - (currentSkillLevel / 10);
            if (baseGain < 1) baseGain = 1;

            if (hasComprehension)
                baseGain = (int)(baseGain * 1.5);

            return baseGain;
        }
    }
}
