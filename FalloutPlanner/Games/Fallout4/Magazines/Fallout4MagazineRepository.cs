using System.Collections.Generic;
using System.Linq;

namespace FalloutPlanner.Games.Fallout4.Magazines
{
    public static class Fallout4MagazineRepository
    {
        public static IReadOnlyList<Fallout4MagazineSeries> All { get; } = CreateAll();

        public static Fallout4MagazineSeries? GetSeries(string name) =>
            All.FirstOrDefault(s => s.Name == name);

        public static Fallout4MagazineIssue? GetIssue(string issueId) =>
            All.SelectMany(s => s.Issues).FirstOrDefault(i => i.Id == issueId);

        private static List<Fallout4MagazineSeries> CreateAll() => new()
        {
            // --- Unique-effect series (dropdown per issue) ---
            Unique("Astoundingly Awesome Tales", "Astoundingly Awesome",
            [
                Issue("aat-1", "My Brain and I...", "Regenerate 1 point of health per minute.", "Boston Mayoral Shelter"),
                Issue("aat-2", "The Starlet Sniper", "Do +5% damage with scoped weapons.", "Coast Guard Pier"),
                Issue("aat-3", "Attack of the Metal Men", "Take 5% less damage from robots.", "Dunwich Borers"),
                Issue("aat-4", "Invasion of the Zetans", "Do +5% damage with the Alien Blaster.", "Hubris Comics"),
                Issue("aat-5", "The Mad Russian's Revenge", "Gain +5 Poison Resistance.", "Pickman Gallery"),
                Issue("aat-6", "Attack Of The Fishmen", "Do +5% damage against Mirelurks.", "Skylanes Flight 1981"),
                Issue("aat-7", "Deep Sea Terror!", "Do +5% damage at night.", "Outpost Zimonja"),
                Issue("aat-8", "The Man Who Could Stop Time", "Gain +5 Action Points.", "Vault 114"),
                Issue("aat-9", "When Apes Go Bananas!!!", "Do +5% damage with the Cryolator.", "Railroad HQ"),
                Issue("aat-10", "Giant Insects Invade", "Gain +5 Radiation Resistance.", "Sentinel Site"),
                Issue("aat-11", "Rise of the Radiated", "RadAway heals +5% radiation damage.", "East Boston Preparatory School"),
                Issue("aat-12", "Have Dog, Will Travel!", "Your canine companion permanently takes 10% less damage.", "The Institute"),
                Issue("aat-13", "Curse of the Burned!", "Do +5% damage against Ghouls.", "Crater of Atom"),
                Issue("aat-14", "Rise of the Mutants!", "Do +5% damage against Super Mutants.", "Trinity Church"),
            ]),

            Unique("Live & Love", "Live & Love",
            [
                Issue("ll-1", "Life Long Best Friends!", "Companions permanently gain +10 health.", "Faneuil Hall"),
                Issue("ll-2", "Nuke-The-Man!", "Companions permanently inflict +5% damage.", "College Square Station"),
                Issue("ll-3", "Trim the Fat", "Companions permanently gain +10 carry weight.", "WRVR Broadcast Station"),
                Issue("ll-4", "The Secretary Charmer", "Permanently gain +25% XP from persuading women.", "Fiddler's Green Trailer Estates"),
                Issue("ll-5", "Talk Yourself Sober", "Permanently gain +1 Luck from alcohol when adventuring with a companion.", "Revere Beach Station"),
                Issue("ll-6", "Advice From Married Men", "Companions permanently gain +5 Damage Resistance and Energy Resistance.", "The Third Rail"),
                Issue("ll-7", "Beware The Man Handler", "Permanently gain +25% XP from persuading men.", "Bunker Hill"),
                Issue("ll-8", "An Experience to Remember", "Permanently gain +5% XP while adventuring with a companion.", "Hotel Rexford"),
                Issue("ll-9", "I Married a Robot!", "Robot companions permanently inflict +5% damage.", "Diamond City schoolhouse"),
            ]),

            Unique("Wasteland Survival Guide", "Wasteland Survival",
            [
                Issue("wsg-1", "Farming The Wastes", "Heal +50% more from fruits and vegetables.", "Wreck of the USS Riptide"),
                Issue("wsg-2", "Insect Repellent Special", "Permanently take 5% less damage from insects.", "Crater House"),
                Issue("wsg-3", "The Bright Side of Radiation Poisoning", "Permanently heal +50% from irradiated packaged food and drink.", "Egret Tours Marina"),
                Issue("wsg-4", "The Scrapyard Home Decorators Guide", "Unlock new decoration items in workshop settlements.", "Lynn Woods"),
                Issue("wsg-5", "Guide To Diamond City", "Diamond City is now permanently marked on your map.", "Gorski Cabin"),
                Issue("wsg-6", "Commonwealth Coupon Spectacular", "Permanently gain +10% discount from food and drink vendors.", "Nahant Oceanological Society"),
                Issue("wsg-7", "Water Aerobics for Ghouls", "Permanently swim 25% faster.", "Old Gullet Sinkhole"),
                Issue("wsg-8", "Self-Defense Secrets", "Permanently take 5% less damage from melee attacks.", "Ranger Cabin"),
                Issue("wsg-9", "Hunting in the Wastes", "Permanently collect more meat from animal kills.", "Sunshine Tidings Co-op"),
            ]),

            Unique("Hot Rodder", "Hot Rodder",
            [
                Issue("hr-1", "Flame Job!", "Unlocks Hot Rod Flames power armor paint.", "Robotics Disposal Ground"),
                Issue("hr-2", "Hot Pink!", "Unlocks Hot Pink power armor paint.", "Milton Parking Garage"),
                Issue("hr-3", "Racing Stripes!", "Unlocks Racing Stripes power armor paint.", "Schoelt Propane Store"),
            ]),

            Unique("La Coiffe", "La Coiffe",
            [
                Issue("lc-1", "Megaton Hair", "Unlocks additional hairstyles (Megaton Hair).", "Fallon's Department Store"),
                Issue("lc-2", "The Hornet's Nest", "Unlocks additional hairstyles (Hornet's Nest / Anchorage).", "Charlestown Laundry"),
            ]),

            Unique("Picket Fences", "Picket Fences",
            [
                Issue("pf-1", "The House of Tomorrow...Today!", "Build picket fencing at settlement workshops.", "Beantown Brewery"),
                Issue("pf-2", "Modern Hearth!", "Build high tech lights at settlement workshops.", "Hardware Town"),
                Issue("pf-3", "Essential Upgrades!", "Build new statues at settlement workshops.", "Saugus Ironworks"),
                Issue("pf-4", "Modern Lawn Care!", "Build potted plants at settlement workshops.", "Combat Zone"),
                Issue("pf-5", "Welcome Home!", "Build patio furniture at settlement workshops.", "Weston Water Treatment Plant"),
            ]),

            Unique("RobCo Fun", "RobCo Fun",
            [
                Issue("rf-1", "Atomic Command", "Adds Atomic Command holotape game.", "Museum of Freedom"),
                Issue("rf-2", "Grognak & the Ruby Ruins", "Adds Grognak & The Ruby Ruins holotape game.", "Goodneighbor (Memory Den)"),
                Issue("rf-3", "Pipfall", "Adds Pipfall holotape game.", "Fort Hagen Command Center"),
                Issue("rf-4", "Zeta Invaders", "Adds Zeta Invaders holotape game.", "Valentine Detective Agency"),
            ]),

            Unique("Taboo Tattoos", "Taboo Tattoos",
            [
                Issue("tt-1", "13", "Facial tattoo: four-leaf clover with wishbone and '13'.", "Mass Pike Tunnel"),
                Issue("tt-2", "Anchor", "Facial tattoo: anchor and stars on the forehead.", "Thicket Excavations"),
                Issue("tt-3", "Eagle's Nest", "Facial tattoo: cartoon eagle on throat.", "Concord Civic Access"),
                Issue("tt-4", "Sailor", "Facial tattoo: upside-down horseshoe, 'bad luck'.", "Vault 81"),
                Issue("tt-5", "Skulls Are Hip!!", "Facial tattoo: radioactive skull on forehead.", "Irish Pride Industries Shipyard"),
            ]),

            Unique("Total Hack", "Total Hack",
            [
                Issue("th-1", "Control Robots", "Adds Protectron Override Program.", "Wattz Consumer Electronics"),
                Issue("th-2", "Control Spotlights", "Adds Spotlight Override Program.", "The Shamrock Taphouse"),
                Issue("th-3", "Control Turrets", "Adds Turret Override Program.", "Wildwood Cemetery"),
            ]),

            // Far Harbor / Nuka-World magazines
            Unique("Islander's Almanac", "Islander's Almanac",
            [
                Issue("ia-1", "Pincer Dodge", "Take 5% less damage from Mirelurk melee attacks.", "Northwood Ridge Quarry"),
                Issue("ia-2", "Precision Hunting", "5% higher VATS chance against animals you are in combat with.", "Brooke's Head Lighthouse"),
                Issue("ia-3", "Far Harbor Sightseer's Guide", "Marks multiple locations on the map.", "The Last Plank"),
                Issue("ia-4", "Children of Atom Exposé", "Receive 10% less damage from radiation-based attacks.", "Acadia"),
                Issue("ia-5", "Recipe Roundup", "Unlocks sludge-based recipes at chemistry stations.", "National Park Visitor's Center"),
            ]),

            Unique("SCAV!", "SCAV!",
            [
                Issue("scav-1", "#1 The Terrible Truce", "Increases speech challenge success chance by 10%.", "Nuka-World Galactic Zone"),
                Issue("scav-2", "#2 Fear the Knife King", "+25% Combat Knife and Switchblade damage.", "Nuka-World Junkyard"),
                Issue("scav-3", "#3 Mutant Fists of Stephie Knuckles", "+10% hand to hand weapon damage.", "Kiddie Kingdom Fun House"),
                Issue("scav-4", "#4 Nuka Brahmin Stampede!", "+5% explosives damage.", "Grandchester Mystery Mansion"),
                Issue("scav-5", "#5 That No-Caps Rage",
                    "+1 Strength & +1 Endurance if you have <10,000 caps; +2/+2 if <1,000; +3/+3 if <100.",
                    "Dry Rock Gulch Employee Area"),
            ]),

            // --- Stacking series (same bonus per copy) ---
            Stacking("Grognak the Barbarian", "Barbarian", 10, 5,
                "Critical Hits with unarmed and melee attacks do +{0}% damage.",
                "Critical Hits with unarmed and melee attacks do +5% damage per issue."),
            Stacking("Guns and Bullets", "Guns and Bullets", 10, 5,
                "Ballistic weapons do +{0}% critical damage.",
                "Ballistic weapons do +5% critical damage per issue."),
            Stacking("Massachusetts Surgical Journal", "Massachusetts Surgery", 9, 2,
                "Inflict +{0}% limb damage.",
                "Inflict +2% limb damage per issue."),
            Stacking("Tales of a Junktown Jerky Vendor", "Junktown Vendor", 8, 0,
                null,
                "Get better prices when buying from vendors."),
            Stacking("Tesla Science Magazine", "Tesla Science", 9, 5,
                "Energy weapons inflict +{0}% critical damage.",
                "Energy weapons inflict +5% critical damage per issue."),
            Stacking("Tumblers Today", "Tumblers Today", 5, 0,
                null,
                "Gain a bonus to lockpicking."),
            Stacking("U.S. Covert Operations Manual", "Covert Operations", 10, 0,
                null,
                "You are more difficult to detect while sneaking."),
            Stacking("Unstoppables", "Unstoppable", 5, 1,
                "Gain a +{0}% chance of avoiding all damage from an attack.",
                "Gain a +1% chance of avoiding all damage from an attack per issue."),

            new()
            {
                Name = "You're SPECIAL!",
                PerkName = "You're SPECIAL!",
                MaxIssues = 1,
                GrantsSpecialChoice = true,
                IsStacking = true,
                EffectDisplay = "Gain +1 to a SPECIAL attribute of you choice (you can raise a SPECIAL stat past 10 by getting a debuff for a stat already at 10 bringing it below 10 allowing you to choose it in the book prompt)."
            },
        };

        private static Fallout4MagazineSeries Unique(string name, string perk, Fallout4MagazineIssue[] issues) =>
            new()
            {
                Name = name,
                PerkName = perk,
                MaxIssues = issues.Length,
                IsStacking = false,
                Issues = issues.ToList(),
                EffectDisplay = "Each issue grants a unique bonus."
            };

        private static Fallout4MagazineSeries Stacking(
            string name, string perk, int max, int percentPerRank, string? format, string fallback) =>
            new()
            {
                Name = name,
                PerkName = perk,
                MaxIssues = max,
                IsStacking = true,
                PercentPerRank = percentPerRank,
                StackedEffectFormat = format,
                EffectDisplay = fallback
            };

        private static Fallout4MagazineIssue Issue(string id, string title, string effect, string location) =>
            new()
            {
                Id = id,
                DisplayName = title,
                Effect = effect,
                Location = location
            };

        public static string GetStackedDescription(Fallout4MagazineSeries series, int count)
        {
            if (count <= 0)
                return series.EffectDisplay;

            if (series.PercentPerRank > 0 && !string.IsNullOrEmpty(series.StackedEffectFormat))
                return string.Format(series.StackedEffectFormat, series.PercentPerRank * count);

            return $"{series.EffectDisplay} (Rank {count}/{series.MaxIssues})";
        }
    }
}
