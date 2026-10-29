using System.Collections.Generic;
using System.Linq;

namespace FalloutPlanner.Games.Fallout1.SkillBooks
{
    public static class Fallout1SkillBookRepository
    {
        public static IReadOnlyList<Fallout1SkillBook> AllBooks { get; } = new List<Fallout1SkillBook>
        {
            new()
            {
                Name = "Guns and Bullets",
                SkillName = "Small Guns",
                SkillPropertyName = "SmallGuns",
                Description = "A gun magazine that increases Small Guns skill."
            },
            new()
            {
                Name = "Big Book of Science",
                SkillName = "Science",
                SkillPropertyName = "Science",
                Description = "A comprehensive scientific text that increases Science skill."
            },
            new()
            {
                Name = "Dean's Electronics",
                SkillName = "Repair",
                SkillPropertyName = "Repair",
                Description = "An electronics repair manual that increases Repair skill."
            },
            new()
            {
                Name = "First Aid Book",
                SkillName = "First Aid",
                SkillPropertyName = "FirstAid",
                Description = "A medical reference book that increases First Aid skill."
            },
            new()
            {
                Name = "Scout Handbook",
                SkillName = "Outdoorsman",
                SkillPropertyName = "Outdoorsman",
                Description = "A wilderness survival guide that increases Outdoorsman skill."
            }
        };

        public static Fallout1SkillBook? GetBook(string name) =>
            AllBooks.FirstOrDefault(b => b.Name == name);

        public static Fallout1SkillBook? GetBookBySkill(string skillPropertyName) =>
            AllBooks.FirstOrDefault(b => b.SkillPropertyName == skillPropertyName);
    }
}
