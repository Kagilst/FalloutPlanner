using System.Collections.Generic;
using System.Linq;

namespace FalloutPlanner.Games.Fallout3.SkillBooks
{
    public static partial class Fallout3SkillBookRepository
    {
        public static IReadOnlyList<Fallout3SkillBookType> BookTypes { get; } = BuildBookTypes();
        public static IReadOnlyList<Fallout3SkillBookLocation> Locations { get; } = BuildLocations();

        public static IEnumerable<Fallout3SkillBookLocation> LocationsFor(string bookName) =>
            Locations.Where(l => l.BookName == bookName);

        public static Fallout3SkillBookType? GetBookType(string name) =>
            BookTypes.FirstOrDefault(b => b.Name == name);

        public const string HouseMegaton = "HouseMegaton";
        public const string HouseTenpenny = "HouseTenpenny";

        public static string? OpposingHouseGroup(string? group) => group switch
        {
            HouseMegaton => HouseTenpenny,
            HouseTenpenny => HouseMegaton,
            _ => null
        };
    }
}
