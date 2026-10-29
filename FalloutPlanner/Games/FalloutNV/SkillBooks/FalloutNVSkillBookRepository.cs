using System.Collections.Generic;
using System.Linq;

namespace FalloutPlanner.Games.FalloutNV.SkillBooks
{
    public static partial class FalloutNVSkillBookRepository
    {
        public static IReadOnlyList<FalloutNVSkillBookType> BookTypes { get; } = BuildBookTypes();
        public static IReadOnlyList<FalloutNVSkillBookLocation> Locations { get; } = BuildLocations();

        public static IEnumerable<FalloutNVSkillBookLocation> LocationsFor(string bookName) =>
            Locations.Where(l => l.BookName == bookName);

        public static FalloutNVSkillBookType? GetBookType(string name) =>
            BookTypes.FirstOrDefault(b => b.Name == name);

        public static string? OpposingHouseGroup(string? group) => null; // Not used in FNV

        private static List<FalloutNVSkillBookType> BuildBookTypes()
        {
            return new List<FalloutNVSkillBookType>
            {
                new() { Name = "Tales of a Junktown Jerky Vendor", SkillDisplayName = "Barter", SkillPropertyName = "Barter", MaxCount = 7 },
                new() { Name = "Nikola Tesla and You", SkillDisplayName = "Energy Weapons", SkillPropertyName = "EnergyWeapons", MaxCount = 7 },
                new() { Name = "Duck and Cover!", SkillDisplayName = "Explosives", SkillPropertyName = "Explosives", MaxCount = 7 },
                new() { Name = "Guns and Bullets", SkillDisplayName = "Guns", SkillPropertyName = "Guns", MaxCount = 7 },
                new() { Name = "Tumblers Today", SkillDisplayName = "Lockpick", SkillPropertyName = "Lockpick", MaxCount = 7 },
                new() { Name = "D.C. Journal of Internal Medicine", SkillDisplayName = "Medicine", SkillPropertyName = "Medicine", MaxCount = 7 },
                new() { Name = "Grognak the Barbarian", SkillDisplayName = "Melee Weapons", SkillPropertyName = "MeleeWeapons", MaxCount = 7 },
                new() { Name = "Dean's Electronics", SkillDisplayName = "Repair", SkillPropertyName = "Repair", MaxCount = 7 },
                new() { Name = "Big Book of Science", SkillDisplayName = "Science", SkillPropertyName = "Science", MaxCount = 9 },
                new() { Name = "Chinese Army: Special Ops Training Manual", SkillDisplayName = "Sneak", SkillPropertyName = "Sneak", MaxCount = 9 },
                new() { Name = "Lying, Congressional Style", SkillDisplayName = "Speech", SkillPropertyName = "Speech", MaxCount = 7 },
                new() { Name = "Wasteland Survival Guide", SkillDisplayName = "Survival", SkillPropertyName = "Survival", MaxCount = 6 },
                new() { Name = "Pugilism Illustrated", SkillDisplayName = "Unarmed", SkillPropertyName = "Unarmed", MaxCount = 7 },
            };
        }

        private static List<FalloutNVSkillBookLocation> BuildLocations()
        {
            return new List<FalloutNVSkillBookLocation>
            {
                // =====================================================
                // Big Book of Science (9 total)
                // =====================================================
                new() { Id = "Science_Ashton", BookName = "Big Book of Science", MapMarker = "Ashton missile silo", Description = "In the north corner of launch deck, level 3 of the silo wreckage just past a Hard locked safe" },
                new() { Id = "Science_BrewersBeer", BookName = "Big Book of Science", MapMarker = "Brewer's beer bootlegging", Description = "In the cellar, in the back room on a table to the left of the chemistry set" },
                new() { Id = "Science_CampForlornHope", BookName = "Big Book of Science", MapMarker = "Camp Forlorn Hope", Description = "In the command center, in the southeast corner, on the desk" },
                new() { Id = "Science_HELIOS", BookName = "Big Book of Science", MapMarker = "HELIOS One", Description = "Reward from Ignacio Rivas for distributing power to entire grid during That Lucky Old Sun" },
                new() { Id = "Science_Nipton", BookName = "Big Book of Science", MapMarker = "Nipton", Description = "On the desk in Mayor Joseph B. Steyn's office on the top floor of the Nipton Town Hall building" },
                new() { Id = "Science_REPCONN", BookName = "Big Book of Science", MapMarker = "REPCONN headquarters", Description = "Located on the first floor through the door next to the radioactive barrel display, located on a shelf past the Hard locked door" },
                new() { Id = "Science_SierraMadre", BookName = "Big Book of Science", MapMarker = "Sierra Madre Casino (DM)", Description = "On a bookshelf in Vera's room, which is found in the Executive suites area of the Sierra Madre casino" },
                new() { Id = "Science_Y0", BookName = "Big Book of Science", MapMarker = "Y-0 research center (OWB)", Description = "In a corner of the canyon south of the Y-0 building, between two xander root plants by the hand of a skeleton" },

                // =====================================================
                // Chinese Army: Special Ops Training Manual (9 total)
                // =====================================================
                new() { Id = "Sneak_CampSearchlight", BookName = "Chinese Army: Special Ops Training Manual", MapMarker = "Camp Searchlight", Description = "In the east church's basement, by the eastern wall, at the foot of the metal shelves, on the floor" },
                new() { Id = "Sneak_Goodsprings", BookName = "Chinese Army: Special Ops Training Manual", MapMarker = "Goodsprings", Description = "In the Goodsprings home inhabited by Trudy and Sunny Smiles, east from the schoolhouse, on a bookshelf" },
                new() { Id = "Sneak_NCRSharecropper", BookName = "Chinese Army: Special Ops Training Manual", MapMarker = "NCR sharecropper farms", Description = "On some papers, by the corner desk inside the sharecropper barracks near the four water towers in the northeast corner of the fenced-in area" },
                new() { Id = "Sneak_PuestaDelSol", BookName = "Chinese Army: Special Ops Training Manual", MapMarker = "Puesta del Sol switching station (DM)", Description = "In the easternmost room of the switching station there is a large machine on the south side of the room, located on the catwalk that runs around the machine" },
                new() { Id = "Sneak_SunstoneTower", BookName = "Chinese Army: Special Ops Training Manual", MapMarker = "Sunstone Tower roof (DM)", Description = "On a bookshelf facing the wall (hidden from view) on the southeast landing up one flight of stairs from the first floor interior entrance, acquired by placing/throwing explosives onto the bookshelf underneath the filing cabinet" },
                new() { Id = "Sneak_Vault3", BookName = "Chinese Army: Special Ops Training Manual", MapMarker = "Vault 3", Description = "In the living quarters on a shelf, down the stairs from the recreation area through an Average locked door" },
                new() { Id = "Sneak_X13_Floor", BookName = "Chinese Army: Special Ops Training Manual", MapMarker = "X-13 research facility (OWB)", Description = "On the floor, opposite the door in the room where the chest plate for the stealth suit Mk II is found" },
                new() { Id = "Sneak_X13_Safe", BookName = "Chinese Army: Special Ops Training Manual", MapMarker = "X-13 research facility (OWB)", Description = "In the testing facility section, in the reward safe upon completion of all the stealth suit tests" },

                // =====================================================
                // D.C. Journal of Internal Medicine (7 total)
                // =====================================================
                new() { Id = "Medicine_CaveAbaddon", BookName = "D.C. Journal of Internal Medicine", MapMarker = "Cave of the Abaddon (LR)", Description = "After exiting the Cave of the Abaddon, in the south corner under a nightstand, in the room with the bed and the exit to The Divide (In the 'Buried Building'), underneath the RALPHIE poster" },
                new() { Id = "Medicine_FollowersSafehouse", BookName = "D.C. Journal of Internal Medicine", MapMarker = "Followers safehouse", Description = "On the bedside table shelf, by the bed on the right, in the second room. Entry requires a key" },
                new() { Id = "Medicine_HELIOS", BookName = "D.C. Journal of Internal Medicine", MapMarker = "HELIOS One", Description = "Upstairs in the main building, lying on a bed in the same room with the east terminal password" },
                new() { Id = "Medicine_HellsMotel", BookName = "D.C. Journal of Internal Medicine", MapMarker = "Hell's Motel", Description = "The shack inside the Mesquite Mountains crater, below the globe with a hat, on the table with the radio and repair kit" },
                new() { Id = "Medicine_Novac", BookName = "D.C. Journal of Internal Medicine", MapMarker = "Novac", Description = "In the middle of the queen-sized bed, inside Ranger Andy's bungalow" },
                new() { Id = "Medicine_VillaClinic", BookName = "D.C. Journal of Internal Medicine", MapMarker = "Villa clinic (DM)", Description = "In the Villa clinic basement, in a tool cabinet next to some fire extinguishers" },

                // =====================================================
                // Dean's Electronics (7 total)
                // =====================================================
                new() { Id = "Repair_AbandonedBoS", BookName = "Dean's Electronics", MapMarker = "Abandoned BoS bunker (LR)", Description = "On the metal sill, opposite the reloading bench, in the area with the office. Though the location exists without the add-on, it is sealed by a keyless lock" },
                new() { Id = "Repair_HopevilleSilo", BookName = "Dean's Electronics", MapMarker = "Hopeville missile silo bunker (LR)", Description = "When walking into the room with the first missile, the book is located to the left of the stairs, amongst some rubble" },
                new() { Id = "Repair_Nellis", BookName = "Dean's Electronics", MapMarker = "Nellis Air Force Base", Description = "Inside Loyal's house, on the long counter next to the ruined terminal" },
                new() { Id = "Repair_PuestaDelSol", BookName = "Dean's Electronics", MapMarker = "Puesta del Sol switching station (DM)", Description = "On the upper level next to the terminal that shuts off power to the protected speakers. Very difficult to reach without Christine" },
                new() { Id = "Repair_Sloan", BookName = "Dean's Electronics", MapMarker = "Sloan", Description = "In the worker barracks, on the top shelf to the left of the entrance" },
                new() { Id = "Repair_WindFarm", BookName = "Dean's Electronics", MapMarker = "Southern Nevada wind farm", Description = "In the shack, on the table with the clipboard and scrap metal" },

                // =====================================================
                // Duck and Cover! (7 total)
                // =====================================================
                new() { Id = "Explosives_CollapsedOverpass", BookName = "Duck and Cover!", MapMarker = "Collapsed overpass tunnel (LR)", Description = "Just south of the exit to The High Road, behind some sandbags, in a crate underneath a teddy bear" },
                new() { Id = "Explosives_MojaveOutpost", BookName = "Duck and Cover!", MapMarker = "Mojave Outpost", Description = "Inside the Mojave Outpost barracks, below the bar counter in the middle of the bottom shelf" },
                new() { Id = "Explosives_Nellis", BookName = "Duck and Cover!", MapMarker = "Nellis Air Force Base", Description = "Among the books on the shelf, above the grenade rifle, inside Pearl's barracks" },
                new() { Id = "Explosives_RangerFoxtrot", BookName = "Duck and Cover!", MapMarker = "Ranger Station Foxtrot", Description = "On the table next to Comm Officer Lenk" },
                new() { Id = "Explosives_ResidentialDistrict", BookName = "Duck and Cover!", MapMarker = "Residential District (DM)", Description = "Stuck underneath the bed in the room Dean Domino is first encountered, only obtainable by using explosives or similar mechanisms to force the book out" },
                new() { Id = "Explosives_SloanBarracks", BookName = "Duck and Cover!", MapMarker = "Sloan", Description = "Inside the worker barracks on the back left shelf under a first aid box" },

                // =====================================================
                // Grognak the Barbarian (7 total)
                // =====================================================
                new() { Id = "Melee_CannibalJohnson", BookName = "Grognak the Barbarian", MapMarker = "Cannibal Johnson's cave", Description = "On the mattress near the radio, close to the campfire" },
                new() { Id = "Melee_CottonwoodCove", BookName = "Grognak the Barbarian", MapMarker = "Cottonwood Cove", Description = "Inside Aurelius of Phoenix's office on the upper floor of the HQ, inside the nightstand next to the bed" },
                new() { Id = "Melee_HiddenSupplyCave", BookName = "Grognak the Barbarian", MapMarker = "Hidden supply cave", Description = "Resting on one of the large metal crates in the cave, sometimes behind the metal crates. Cave is behind an Average locked door" },
                new() { Id = "Melee_HopevilleWomen", BookName = "Grognak the Barbarian", MapMarker = "Hopeville women's barracks (LR)", Description = "At the back of the barracks on a small table under a ruined book" },
                new() { Id = "Melee_Jacobstown", BookName = "Grognak the Barbarian", MapMarker = "Jacobstown", Description = "Lying in front of the oven in the southernmost bungalow" },
                new() { Id = "Melee_VillaPolice", BookName = "Grognak the Barbarian", MapMarker = "Villa police station (DM)", Description = "Inside the police station basement, on the floor in the room in the northwest corner in front of the filing cabinets" },

                // =====================================================
                // Guns and Bullets (7 total)
                // =====================================================
                new() { Id = "Guns_Gomorrah", BookName = "Guns and Bullets", MapMarker = "Gomorrah", Description = "In Big Sal's office past a Hard locked door, upstairs from the main level of Brimstone, on the first low bookshelf on the left" },
                new() { Id = "Guns_HopevilleArmory", BookName = "Guns and Bullets", MapMarker = "Hopeville armory (LR)", Description = "Underneath the desk in northeast corner, with a bed that can be slept on" },
                new() { Id = "Guns_NevadaHighwayPatrol", BookName = "Guns and Bullets", MapMarker = "Nevada Highway Patrol station", Description = "On the desk without the computer in the entrance office room, nearest the door leading to the cells" },
                new() { Id = "Guns_RaulsShack", BookName = "Guns and Bullets", MapMarker = "Raul's shack", Description = "Inside the shack, in the wooden crate next to the door" },
                new() { Id = "Guns_SierraMadre", BookName = "Guns and Bullets", MapMarker = "Sierra Madre casino (DM)", Description = "On the floor behind the cashier's desk, accessible by the ceiling beams" },
                new() { Id = "Guns_Vault34", BookName = "Guns and Bullets", MapMarker = "Vault 34", Description = "On the small metal table between the sofas by the pool table, in the armory common room" },

                // =====================================================
                // Lying, Congressional Style (7 total)
                // =====================================================
                new() { Id = "Speech_BrooksTumbleweed", BookName = "Lying, Congressional Style", MapMarker = "Brooks tumbleweed ranch", Description = "On an open locker shelf, in the upstairs room with the chessboard" },
                new() { Id = "Speech_CeruleanRobotics", BookName = "Lying, Congressional Style", MapMarker = "Cerulean Robotics", Description = "On the floor among the office desks" },
                new() { Id = "Speech_LuckyJimMine", BookName = "Lying, Congressional Style", MapMarker = "Lucky Jim Mine", Description = "Inside the Lucky Jim Mine house, on the bottom shelf below a varmint rifle" },
                new() { Id = "Speech_NCRCorrectional", BookName = "Lying, Congressional Style", MapMarker = "NCR Correctional Facility", Description = "Second floor of the NCR CF administration, on a desk" },
                new() { Id = "Speech_Tampico", BookName = "Lying, Congressional Style", MapMarker = "The Tampico (DM)", Description = "On a shelf, opposite the holotape projector" },
                new() { Id = "Speech_ThirdStreetMunicipal", BookName = "Lying, Congressional Style", MapMarker = "Third Street Municipal Building (LR)", Description = "Top floor (accessed through the municipal sewers), in the easternmost corner, underneath a physician's desk (may fall off building after detonating warhead)" },

                // =====================================================
                // Nikola Tesla and You (7 total)
                // =====================================================
                new() { Id = "Energy_HiddenValley", BookName = "Nikola Tesla and You", MapMarker = "Hidden Valley", Description = "Located on the first floor of the Hidden Valley bunker building, in the white plastic bin by the desk in Schuler's office, west of the central hub corridor junction" },
                new() { Id = "Energy_HopevilleHQ", BookName = "Nikola Tesla and You", MapMarker = "Hopeville Missile Base Headquarters (LR)", Description = "Located in the first room, on the wooden shelf directly on the right upon entering, hidden underneath a burned book" },
                new() { Id = "Energy_OldNuclearTest", BookName = "Nikola Tesla and You", MapMarker = "Old nuclear test site", Description = "Inside the nuclear test shack on the small table opposite to the exit" },
                new() { Id = "Energy_REPCONN_VeryHard", BookName = "Nikola Tesla and You", MapMarker = "REPCONN headquarters", Description = "On the first floor, past the Very Hard locked door, on top of the safe. The key is on the third floor from a suitcase next to a skeleton. The room can also be accessed through a hole in the floor of a western room on the second floor, past a Hard locked door" },
                new() { Id = "Energy_REPCONN_2ndFloor", BookName = "Nikola Tesla and You", MapMarker = "REPCONN headquarters", Description = "A second copy can be found on the second floor, through the first door on the left, on the desk with the very easy locked terminal" },
                new() { Id = "Energy_SierraMadreVault", BookName = "Nikola Tesla and You", MapMarker = "Sierra Madre vault (DM)", Description = "In the pipes below the vault, in a duffle bag" },

                // =====================================================
                // Pugilism Illustrated (7 total)
                // =====================================================
                new() { Id = "Unarmed_FishermansPride", BookName = "Pugilism Illustrated", MapMarker = "Fisherman's Pride shack", Description = "On the bedside table, inside the shack" },
                new() { Id = "Unarmed_NiptonRoad", BookName = "Pugilism Illustrated", MapMarker = "Nipton Road reststop", Description = "Inside the store, lying on the wooden shelf on the left upon entering, close to the counter" },
                new() { Id = "Unarmed_TheTops", BookName = "Pugilism Illustrated", MapMarker = "The Tops", Description = "Inside the presidential suite, inside the pool room, on the tiny table in the corner" },
                new() { Id = "Unarmed_Vault11", BookName = "Pugilism Illustrated", MapMarker = "Vault 11", Description = "Inside the living quarters, in the first female dorm, by the fallen dresser near a small sign on the ceiling that says 'ladies'" },
                new() { Id = "Unarmed_Villa", BookName = "Pugilism Illustrated", MapMarker = "Villa (DM)", Description = "Behind the fountain at the entrance of the Dead Money add-on" },
                new() { Id = "Unarmed_Wastewater", BookName = "Pugilism Illustrated", MapMarker = "Wastewater treatment plant", Description = "In the northeast corner of the third room, behind a set of tipped-over lockers" },

                // =====================================================
                // Tales of a Junktown Jerky Vendor (7 total)
                // =====================================================
                new() { Id = "Barter_AlliedTech", BookName = "Tales of a Junktown Jerky Vendor", MapMarker = "Allied Technologies offices", Description = "In the largest room on the west side of the building, on the floor in front of the Nuka-Cola machine" },
                new() { Id = "Barter_CapCounterfeiting", BookName = "Tales of a Junktown Jerky Vendor", MapMarker = "Cap counterfeiting shack", Description = "On the suitcase by the bed, in the small back cellar room" },
                new() { Id = "Barter_MarkedMenGuard", BookName = "Tales of a Junktown Jerky Vendor", MapMarker = "Marked men guard outpost (LR)", Description = "Located through a cellar door on the eastern wall of the building, downstairs on a table near some gore on the northwest wall" },
                new() { Id = "Barter_Primm", BookName = "Tales of a Junktown Jerky Vendor", MapMarker = "Primm", Description = "Inside the Bison Steve Hotel, on the floor of the gift shop, behind the counter" },
                new() { Id = "Barter_SalidaDelSol", BookName = "Tales of a Junktown Jerky Vendor", MapMarker = "Salida del Sol South (DM)", Description = "In a building opposite the door to Salida del Sol house, on a book shelf" },
                new() { Id = "Barter_Vault22", BookName = "Tales of a Junktown Jerky Vendor", MapMarker = "Vault 22", Description = "In the lab room up the stairs from the elevator in pest control, on the table with the bubbling vials" },

                // =====================================================
                // Tumblers Today (7 total)
                // =====================================================
                new() { Id = "Lockpick_BitterSprings", BookName = "Tumblers Today", MapMarker = "Bitter Springs recreation area", Description = "Inside the office shack to the west, on the desk in the smaller of the two rooms" },
                new() { Id = "Lockpick_HopevilleMen", BookName = "Tumblers Today", MapMarker = "Hopeville Missile Base (LR)", Description = "Inside the Hopeville men's barracks, in the toilets underneath an overturned box" },
                new() { Id = "Lockpick_ProspectorsDen", BookName = "Tumblers Today", MapMarker = "The Prospector's Den", Description = "Inside the caves, in the subterranean den structure, in the large barracks room, on the floor in the far right corner" },
                new() { Id = "Lockpick_SierraMadreCasino", BookName = "Tumblers Today", MapMarker = "Sierra Madre Casino (DM)", Description = "In the casino floor area, in the southeast third floor office on top of the tool cabinet" },
                new() { Id = "Lockpick_SilverPeak", BookName = "Tumblers Today", MapMarker = "Silver Peak mine", Description = "Inside the shack, behind the two small tables, on the ground in an open locker, to the right of the entrance" },
                new() { Id = "Lockpick_WolfhornRanch", BookName = "Tumblers Today", MapMarker = "Wolfhorn Ranch", Description = "Inside the farmhouse, partially under the fridge without a door" },

                // =====================================================
                // Wasteland Survival Guide (6 total)
                // =====================================================
                new() { Id = "Survival_LoneWolfRadio", BookName = "Wasteland Survival Guide", MapMarker = "Lone Wolf Radio", Description = "Inside the radio trailer, near the mattress" },
                new() { Id = "Survival_MatthewsFarm", BookName = "Wasteland Survival Guide", MapMarker = "Matthews Animal Husbandry Farm", Description = "Top balcony of the northern barn, near the barrel crate" },
                new() { Id = "Survival_MesquiteMountains", BookName = "Wasteland Survival Guide", MapMarker = "Mesquite Mountains camp site", Description = "Inside the western tent, behind the bedding and toolbox, may glitch into the ground" },
                new() { Id = "Survival_ScavengerPlatform", BookName = "Wasteland Survival Guide", MapMarker = "Scavenger platform", Description = "Among the books under the fallen metal bookshelf, on the northern platform section" },
                new() { Id = "Survival_WasteDisposal", BookName = "Wasteland Survival Guide", MapMarker = "Waste disposal station (LR)", Description = "In the middle of the fallen radioactive barrels, behind a skeleton draped over one of the barrels, between two lead-lined metal boxes" },

                // =====================================================
                // Workbench Crates (4 total - Honest Hearts DLC)
                // =====================================================
                new() { Id = "Workbench_AngelCave", BookName = "Workbench Crate", MapMarker = "Angel Cave (HH)", Description = "Next to the workbench to the right of the small platform where Joshua Graham sits at his little makeshift desk" },
                new() { Id = "Workbench_CuevaGuarache", BookName = "Workbench Crate", MapMarker = "Cueva Guarache (HH)", Description = "Next to the workbench in the Survivalist's living quarters where the Survival cache is found" },
                new() { Id = "Workbench_FallenRockCave", BookName = "Workbench Crate", MapMarker = "Fallen Rock Cave (HH)", Description = "Next to the workbench in the same room as the unique Compliance Regulator energy weapon" },
                new() { Id = "Workbench_StoneBonesCave", BookName = "Workbench Crate", MapMarker = "Stone Bones Cave (HH)", Description = "Next to the workbench in the same room as the unique Desert Ranger combat armor" },
            };
        }
    }
}
