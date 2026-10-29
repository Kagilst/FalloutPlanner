using System.Collections.Generic;

namespace FalloutPlanner.Games.Fallout4.Bobbleheads
{
    public static class Fallout4BobbleheadRepository
    {
        public static IReadOnlyList<Fallout4Bobblehead> All { get; } = new List<Fallout4Bobblehead>
        {
            // SPECIAL — permanently +1 (can raise a 10 to 11)
            Special("Strength Bobblehead", "Strength", "+1 Strength",
                "Mass Fusion Building: On the head of the metal wall statue five levels above the lobby desk."),
            Special("Perception Bobblehead", "Perception", "+1 Perception",
                "Museum of Freedom: On a desk next to a broken terminal where you first meet Preston Garvey."),
            Special("Endurance Bobblehead", "Endurance", "+1 Endurance",
                "Poseidon Energy: On the metal desk with a magazine, near steamer trunk, central metal catwalk."),
            Special("Charisma Bobblehead", "Charisma", "+1 Charisma",
                "Parsons State Insane Asylum: On Jack Cabot's office desk, close to the elevator, administration area."),
            Special("Intelligence Bobblehead", "Intelligence", "+1 Intelligence",
                "Boston Public Library: On the computer bank, mechanical room, northwest corner of the library."),
            Special("Agility Bobblehead", "Agility", "+1 Agility",
                "Wreck of the FMS Northern Star: On the edge of the bow of the ship, wooden platform."),
            Special("Luck Bobblehead", "Luck", "+1 Luck",
                "Spectacle Island: Second deck pilot house of a green tugboat at the southern end, on a locker shelf."),

            // Utility
            Perk("Barter Bobblehead", "+5% better prices",
                "Longneck Lukowski's Cannery: Inside the metal catwalk hut, northwest upper area of the main cannery room."),
            Perk("Lock Picking Bobblehead", "Easier lockpicking",
                "Pickman Gallery: Last tunnel chamber; on the ground between brick pillars and a bin fireplace."),
            Perk("Medicine Bobblehead", "+10% Stimpak healing",
                "Vault 81: In Curie's office, southeast corner of the Vault."),
            Perk("Repair Bobblehead", "+10% Fusion Core duration",
                "Corvega Assembly Plant: End of the top exterior gantry (blue balloon), southwest roof, on a wooden box."),
            Perk("Science Bobblehead", "+1 guess when hacking",
                "Malden Middle School (Vault 75): On the desk overlooking the subterranean training area, science labs."),
            Perk("Sneak Bobblehead", "+10% harder to detect",
                "Dunwich Borers: On a small metal table by a lantern, next to metal post terminal area #4."),
            Perk("Speech Bobblehead", "+100 vendor caps",
                "Park Street Station (Vault 114): In the Overseer's office on the desk."),

            // Weapons
            Perk("Big Guns Bobblehead", "+25% critical damage with heavy weapons",
                "Vault 95: Living quarters, northmost room, on a radio."),
            Perk("Energy Weapons Bobblehead", "+25% critical damage with energy weapons",
                "Fort Hagen Command Center: Southwest kitchens, on a small table between two fridges."),
            Perk("Explosive Bobblehead", "+15% explosive damage",
                "Saugus Ironworks: Blast furnace, second level catwalk behind Slag, on a control panel."),
            Perk("Melee Bobblehead", "+25% critical damage with melee weapons",
                "Trinity Tower: On a table in the cage where Rex Goodman and Strong are held."),
            Perk("Small Guns Bobblehead", "+25% critical damage with ballistic guns",
                "Gunners Plaza: On the broadcast desk in the on-air room, first floor, west side."),
            Perk("Unarmed Bobblehead", "+25% critical damage with unarmed attacks",
                "Atom Cats Garage: On the hood of the rusty car in the main warehouse."),
        };

        private static Fallout4Bobblehead Special(string name, string special, string effect, string location) =>
            new()
            {
                Name = name,
                SpecialPropertyName = special,
                EffectDisplay = effect,
                Location = location,
                Kind = Fallout4BobbleheadKind.Special
            };

        private static Fallout4Bobblehead Perk(string name, string effect, string location) =>
            new()
            {
                Name = name,
                EffectDisplay = effect,
                Location = location,
                Kind = Fallout4BobbleheadKind.Perk
            };
    }
}
