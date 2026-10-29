using System.Collections.Generic;

namespace FalloutPlanner.Games.FalloutNV.Perks
{
    /// <summary>
    /// FNV Quest/Misc Perks - these are obtained through quests and gameplay, not leveling.
    /// Includes challenge perks, combat moves, OWB organs, Dead Money, Lonesome Road, etc.
    /// </summary>
    public static class FalloutNVQuestPerkRepository
    {
        // Mutex group names for perks that block each other
        public const string MutexBrainGroup = "OWB_Brain";
        public const string MutexHeartGroup = "OWB_Heart";
        public const string MutexSpineGroup = "OWB_Spine";
        public const string MutexElijahGroup = "Elijah";
        public const string MutexMissileGroup = "LonesomeRoad_Missiles";

        public static IReadOnlyList<FalloutNVPerk> All { get; } = new List<FalloutNVPerk>
        {
            // =====================================================
            // Challenge Perks (Ranked)
            // =====================================================
            new()
            {
                Name = "Abominable",
                MaxRank = 3,
                Description = "Deal +3%/+6%/+10% damage to abominations. Kill 50/100/150 abominations."
            },
            new()
            {
                Name = "Animal Control",
                MaxRank = 3,
                Description = "Deal +3%/+6%/+10% damage to mutated animals. Kill 50/100/150 mutated animals."
            },
            new()
            {
                Name = "Bug Stomper",
                MaxRank = 3,
                Description = "Deal +3%/+6%/+10% damage to mutated insects. Kill 50/100/150 mutated insects."
            },
            new()
            {
                Name = "Lord Death",
                MaxRank = 3,
                Description = "Deal an extra +1%/+2%/+4% damage to all enemies. Kill 200/700/1000 enemies."
            },
            new()
            {
                Name = "Machine Head",
                MaxRank = 3,
                Description = "Deal +3%/+6% damage to robots. Kill 50/100 robots. (Bugged: rank 3 unachievable)"
            },
            new()
            {
                Name = "Mutant Massacrer",
                MaxRank = 3,
                Description = "Deal +3%/+6%/+10% damage to super mutants. Kill 50/100/150 super mutants."
            },

            // =====================================================
            // Challenge Perks (Single Rank)
            // =====================================================
            new()
            {
                Name = "Beautiful Beatdown",
                Description = "AP costs for unarmed attacks are reduced by 10%. Complete A Fistful of Hollars and inflict 10,000 damage with unarmed weapons."
            },
            new()
            {
                Name = "Camel of the Mojave",
                Description = "Water items hydrate and heal you 15% better. Drink 100 water items."
            },
            new()
            {
                Name = "Day Tripper",
                Description = "The effects of addictive Chems last 33% longer. Use 25 addictive Chems."
            },
            new()
            {
                Name = "Dine and Dash",
                Description = "Get option to take Human remains when using Cannibal. Eat 25 corpses with Cannibal perk."
            },
            new()
            {
                Name = "Fast Times",
                Description = "Turbo's effects last +50% longer. Use turbo 20 times."
            },
            new()
            {
                Name = "Free Radical",
                Description = "10 additional rads removed upon each RadAway usage. Use 20 RadAway."
            },
            new()
            {
                Name = "Friendly Help",
                Description = "Mysterious Stranger and Miss Fortune are twice as likely to appear in V.A.T.S. Receive 15 Mysterious Stranger or Miss Fortune visits."
            },
            new()
            {
                Name = "Meat of Champions",
                Description = "+1 Luck, Strength, Charisma, and Intelligence for 60 seconds each time a corpse is consumed. Cannibalize Mr. House, Caesar, the King and Aaron Kimball."
            },
            new()
            {
                Name = "Melee Hacker",
                MaxRank = 2,
                Description = "Level 1: +5% melee weapon attack speed. Level 2: +10% melee weapon attack speed. Kill 125 enemies with melee weapons, deal 10,000 damage with one-handed and two-handed melee weapons."
            },
            new()
            {
                Name = "Set Lasers for Fun",
                MaxRank = 2,
                Description = "ALL energy weapons gain an extra +2%/+4% chance to get a critical hit. Deal 16,000 damage with pistol grip and 25,000 with rifle grip energy weapons."
            },
            new()
            {
                Name = "Tough Guy",
                Description = "Take 20% less limb damage. Become crippled 50 times."
            },

            // =====================================================
            // Power Armor Training
            // =====================================================
            new()
            {
                Name = "Power Armor Training",
                Description = "Ability to wear all power armor variants. Complete Eyesight to the Blind, Tend to Your Business, or For Auld Lang Syne."
            },

            // =====================================================
            // Combat Moves
            // =====================================================
            new()
            {
                Name = "Khan Trick",
                Description = "Temporarily stun opponents by throwing dust or dirt at their eyes (requires dirt or sand ground). Perform all Great Khan drug runs."
            },
            new()
            {
                Name = "Legion Assault",
                Description = "Knock back opponents. Talk to Lucius with 50 Unarmed and a Legion reputation of Neutral or better."
            },
            new()
            {
                Name = "Ranger Takedown",
                Description = "Knock down opponents. Talk to Ranger Andy with 30 Speech or complete the unmarked quest, Andy and Charlie."
            },
            new()
            {
                Name = "Scribe Counter",
                Description = "Quick counter-attack after Block. Put Formal wear or White Glove Society attire into Veronica Santangelo's inventory."
            },

            // =====================================================
            // Dead Money Perks
            // =====================================================
            new()
            {
                Name = "Coin Operator",
                Description = "Adds recipe: 1 fission battery + 2 scrap metal = 50 Sierra Madre chips. Dialogue with Christine."
            },
            new()
            {
                Name = "Ghost Hunter",
                Description = "Ghost people are more likely to die outright without needing to be dismembered or disintegrated. Dialogue with Dog."
            },
            new()
            {
                Name = "Sierra Madre Martini",
                Description = "Adds recipe to create Sierra Madre martinis with 2 junk food, 1 tin can, and 1 jar of Cloud residue. Dialogue with Dean Domino."
            },
            new()
            {
                Name = "Elijah's Last Words",
                Description = "Boosts Veronica's melee attack speed 150% & gives her a 25% chance for melee attacks to knock down enemies. Give Veronica holomessage from Elijah.",
                MutexGroup = MutexElijahGroup
            },
            new()
            {
                Name = "Elijah's Ramblings",
                Description = "Boosts melee critical damage by +50%. Keep holomessage from Elijah after Veronica unlocks it.",
                MutexGroup = MutexElijahGroup
            },

            // =====================================================
            // Old World Blues - Organs (Mutex Pairs)
            // =====================================================
            new()
            {
                Name = "Brainless",
                Description = "Head can no longer be crippled, +25% resistance to chem addiction, +5% DT (min +1). Automatically acquired during Welcome to the Big Empty.",
                MutexGroup = MutexBrainGroup
            },
            new()
            {
                Name = "Big Brained",
                Description = "Head can no longer be crippled, +10% resistance to chem addiction, +10% DT (min +1). (Note: bugged, does not function) Optionally given by the Sink Auto-Doc after Old World Blues.",
                MutexGroup = MutexBrainGroup
            },
            new()
            {
                Name = "Heartless",
                Description = "Poison immunity, -50% chance to get critical hits from robots, lesser consumable healing. Automatically acquired during Welcome to the Big Empty.",
                MutexGroup = MutexHeartGroup
            },
            new()
            {
                Name = "Cardiac Arrest",
                Description = "+50% to poison resistance, -25% chance to get critical hits from robots, bonus consumable healing. Optionally given by the Sink Auto-Doc during/after Old World Blues.",
                MutexGroup = MutexHeartGroup
            },
            new()
            {
                Name = "Spineless",
                Description = "Torso can no longer be crippled. +1 Strength, +1 DT. Automatically acquired during Welcome to the Big Empty.",
                MutexGroup = MutexSpineGroup,
                ApplyEffect = c => { c.Strength += 1; c.DamageThreshold += 1; }
            },
            new()
            {
                Name = "Reinforced Spine",
                Description = "Torso can still be crippled, +2 Strength, +2 Damage Threshold. Optionally given by the Sink Auto-Doc during/after Old World Blues.",
                MutexGroup = MutexSpineGroup,
                ApplyEffect = c => { c.Strength += 2; c.DamageThreshold += 2; }
            },

            // =====================================================
            // Old World Blues - Other Perks
            // =====================================================
            new()
            {
                Name = "DNAgent",
                Description = "10% bonus to damage dealt against night stalkers. Complete X-8 Data Retrieval Test in the X-8 research center."
            },
            new()
            {
                Name = "DNAvenger",
                MaxRank = 3,
                Description = "10%/20%/30% bonus to damage dealt against cazadores. Complete ranks of the Caza-Death Dealer challenge (kill 2/3/5 cazadores in Big MT)."
            },
            new()
            {
                Name = "Stealth Suit Mk II (perk)",
                MaxRank = 4,
                Description = "+20% increased sneak speed. (Note: bugged, does not function) Complete the last test of Project X-13 in the X-13 research facility."
            },

            // =====================================================
            // Lonesome Road Perks
            // =====================================================
            new()
            {
                Name = "Lonesome Road",
                Description = "+10% damage and +10% V.A.T.S. hit chance when you have no companions. Leave ED-E behind in Ulysses' Temple."
            },
            new()
            {
                Name = "Marked",
                Description = "+10% damage and +10 DT against Marked Men. Kill three named Marked Men."
            },
            // Missile Launch Endings (Mutex - only one)
            new()
            {
                Name = "The Bear-Slayer",
                Description = "+1 SPECIAL point, Caesar's Legion Fame, NCR Infamy. Launch nuclear missiles at NCR.",
                MutexGroup = MutexMissileGroup
            },
            new()
            {
                Name = "Dead Man's Burden",
                Description = "+1 SPECIAL point, Boomers and Powder Gangers Fame, Caesar's Legion and NCR Infamy. Launch nuclear missiles at both Caesar's Legion and NCR.",
                MutexGroup = MutexMissileGroup
            },
            new()
            {
                Name = "Divide Survivor",
                Description = "+1 SPECIAL point, Brotherhood of Steel and Followers of the Apocalypse Fame. Stop the missile launch.",
                MutexGroup = MutexMissileGroup
            },
            new()
            {
                Name = "Scourge of the East",
                Description = "+1 SPECIAL point, NCR Fame, Caesar's Legion Infamy. Launch nuclear missiles at Caesar's Legion.",
                MutexGroup = MutexMissileGroup
            },
        };

        /// <summary>
        /// Returns the blocking perk name if the given perk cannot be taken due to a mutex.
        /// Returns null if the perk can be taken.
        /// </summary>
        public static string GetBlockingPerk(FalloutNVCharacter c, string perkName)
        {
            var perk = GetPerkByName(perkName);
            if (perk == null || string.IsNullOrEmpty(perk.MutexGroup))
                return null;

            foreach (var other in All)
            {
                if (other.Name == perkName)
                    continue;
                if (other.MutexGroup == perk.MutexGroup && c.HasQuestPerk(other.Name))
                    return other.Name;
            }
            return null;
        }

        /// <summary>
        /// Returns whether the perk is blocked by a mutex (another perk in the same group is owned).
        /// </summary>
        public static bool IsPerkBlocked(FalloutNVCharacter c, string perkName)
        {
            return GetBlockingPerk(c, perkName) != null;
        }

        public static FalloutNVPerk GetPerkByName(string name)
        {
            foreach (var p in All)
            {
                if (p.Name == name)
                    return p;
            }
            return null;
        }

        /// <summary>
        /// Remove stat effects from misc perks that have ApplyEffect (called when swapping mutex perks).
        /// </summary>
        public static void RemoveMiscPerkEffects(FalloutNVCharacter c, QuestPerkState state)
        {
            // Handle reversible effects for spine perks
            if (state.Name == "Spineless")
            {
                c.Strength -= 1;
                c.DamageThreshold -= 1;
            }
            else if (state.Name == "Reinforced Spine")
            {
                c.Strength -= 2;
                c.DamageThreshold -= 2;
            }
            // Other OWB organ perks don't have simple reversible stat effects
        }
    }
}
