using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace FalloutPlanner.Games.Fallout3
{
    public class Fallout3Character :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private Stack<Fallout3CharacterStats> _history = new();

        public Fallout3CharacterStats Current => _history.Peek();

        //Skill Logic
        private Dictionary<string, int> _skillIncreasesThisLevel = new();
        public bool ModifySkill(string skillName, int amount)
        {
            var property = GetType().GetProperty(skillName);

            if (property == null || property.PropertyType != typeof(int))
                return false;

            int currentValue = (int)property.GetValue(this);
            int newValue = currentValue + amount;

            // Increase
            if (amount > 0)
            {
                if (SkillPoints <= 0 || currentValue >= 100)
                    return false;

                property.SetValue(this, Math.Min(newValue, 100));
                SkillPoints--;

                if (!_skillIncreasesThisLevel.ContainsKey(skillName))
                    _skillIncreasesThisLevel[skillName] = 0;

                _skillIncreasesThisLevel[skillName]++;

                NotifySkillChanged(skillName);
                return true;
            }

            // Decrease
            if (amount < 0)
            {
                if (!_skillIncreasesThisLevel.ContainsKey(skillName) ||
                    _skillIncreasesThisLevel[skillName] <= 0)
                    return false;

                property.SetValue(this, currentValue - 1);
                SkillPoints++;
                _skillIncreasesThisLevel[skillName]--;

                NotifySkillChanged(skillName);
                return true;
            }

            return false;
        }

        private void NotifySkillChanged(string skillName)
        {
            OnPropertyChanged(skillName);
            OnPropertyChanged(nameof(SkillPoints));
            OnPropertyChanged($"CanIncrease{skillName}");
            OnPropertyChanged($"CanDecrease{skillName}");
        }

        public bool CanIncreaseSkill(string skillName)
        {
            var property = GetType().GetProperty(skillName);
            if (property == null) return false;

            int value = (int)property.GetValue(this);
            return SkillPoints > 0 && value < 100;
        }

        public bool CanDecreaseSkill(string skillName)
        {
            return _skillIncreasesThisLevel.ContainsKey(skillName) &&
                   _skillIncreasesThisLevel[skillName] > 0;
        }

        // Barter
        public bool CanIncreaseBarter => CanIncreaseSkill(nameof(Barter));
        public bool CanDecreaseBarter => CanDecreaseSkill(nameof(Barter));

        // Big Guns
        public bool CanIncreaseBigGuns => CanIncreaseSkill(nameof(BigGuns));
        public bool CanDecreaseBigGuns => CanDecreaseSkill(nameof(BigGuns));

        // Energy Weapons
        public bool CanIncreaseEnergyWeapons => CanIncreaseSkill(nameof(EnergyWeapons));
        public bool CanDecreaseEnergyWeapons => CanDecreaseSkill(nameof(EnergyWeapons));

        // Explosives
        public bool CanIncreaseExplosives => CanIncreaseSkill(nameof(Explosives));
        public bool CanDecreaseExplosives => CanDecreaseSkill(nameof(Explosives));

        // Lockpick
        public bool CanIncreaseLockpick => CanIncreaseSkill(nameof(Lockpick));
        public bool CanDecreaseLockpick => CanDecreaseSkill(nameof(Lockpick));

        // Medicine
        public bool CanIncreaseMedicine => CanIncreaseSkill(nameof(Medicine));
        public bool CanDecreaseMedicine => CanDecreaseSkill(nameof(Medicine));

        // Melee Weapons
        public bool CanIncreaseMeleeWeapons => CanIncreaseSkill(nameof(MeleeWeapons));
        public bool CanDecreaseMeleeWeapons => CanDecreaseSkill(nameof(MeleeWeapons));

        // Repair
        public bool CanIncreaseRepair => CanIncreaseSkill(nameof(Repair));
        public bool CanDecreaseRepair => CanDecreaseSkill(nameof(Repair));

        // Science
        public bool CanIncreaseScience => CanIncreaseSkill(nameof(Science));
        public bool CanDecreaseScience => CanDecreaseSkill(nameof(Science));

        // Small Guns
        public bool CanIncreaseSmallGuns => CanIncreaseSkill(nameof(SmallGuns));
        public bool CanDecreaseSmallGuns => CanDecreaseSkill(nameof(SmallGuns));

        // Sneak
        public bool CanIncreaseSneak => CanIncreaseSkill(nameof(Sneak));
        public bool CanDecreaseSneak => CanDecreaseSkill(nameof(Sneak));

        // Speech
        public bool CanIncreaseSpeech => CanIncreaseSkill(nameof(Speech));
        public bool CanDecreaseSpeech => CanDecreaseSkill(nameof(Speech));

        // Unarmed
        public bool CanIncreaseUnarmed => CanIncreaseSkill(nameof(Unarmed));
        public bool CanDecreaseUnarmed => CanDecreaseSkill(nameof(Unarmed));

        //Constructor
        public string Name { get; set; }
        public int Level { get; private set; } = 1;
        public Fallout3Character(Fallout3CharacterStats baseStats)
        {
            Strength = baseStats.Strength;
            Perception = baseStats.Perception;
            Endurance = baseStats.Endurance;
            Charisma = baseStats.Charisma;
            Intelligence = baseStats.Intelligence;
            Agility = baseStats.Agility;
            Luck = baseStats.Luck;
            Points = baseStats.Points;

            Barter = baseStats.Strength;
            BigGuns = baseStats.Perception;
            EnergyWeapons = baseStats.Endurance;
            Explosives = baseStats.Charisma;
            Lockpick = baseStats.Intelligence;
            Medicine = baseStats.Agility;
            MeleeWeapons = baseStats.Luck;
            Repair = baseStats.Points;
            Science = baseStats.Strength;
            SmallGuns = baseStats.Perception;
            Sneak = baseStats.Endurance;
            Speech = baseStats.Charisma;
            Unarmed = baseStats.Intelligence;

            _history.Push(baseStats);
            RecalculateDerivedStats();
        }

        public void SaveInitialState()
        {
            var snapshot = new Fallout3CharacterStats
            {
                // Not Derived Stats
                Strength = Strength,
                Perception = Perception,
                Endurance = Endurance,
                Charisma = Charisma,
                Intelligence = Intelligence,
                Agility = Agility,
                Luck = Luck,
                Points = Points,

                CharacterName = Name,
                Level = Level,
                TaggedSkills = TaggedSkills,

                Barter = Barter,
                BigGuns = BigGuns,
                EnergyWeapons = EnergyWeapons,
                Explosives = Explosives,
                Lockpick = Lockpick,
                Medicine = Medicine,
                MeleeWeapons = MeleeWeapons,
                Repair = Repair,
                Science = Science,
                SmallGuns = SmallGuns,
                Sneak = Sneak,
                Speech = Speech,
                Unarmed = Unarmed
            };

            _history.Push(snapshot);
        }

        //SPECIAL
        public void ModifySpecial(string statName, int amount)
        {
            var property = GetType().GetProperty(statName);

            if (property == null || property.PropertyType != typeof(int))
                return;

            int currentValue = (int)property.GetValue(this);
            int newValue = currentValue + amount;

            // SPECIAL range check
            if (newValue < 1 || newValue > 10)
                return;

            // Remaining points check
            if (amount > 0)
            {
                if (Points <= 0)
                    return;

                Points -= 1;
            }
            else // decreasing logic
            {
                Points += 1;
            }

            property.SetValue(this, newValue);

            RecalculateDerivedStats();
        }

        //TAG logic
        private HashSet<string> _taggedSkillNames = new();
        public bool ModifyTaggedSkill(string skillName, bool isToggled)
        {
            // Prevent toggling if already at 3 skills
            if (isToggled && TaggedSkills >= 3)
                return false; // 4th toggle not allowed

            // Only update TaggedSkills if allowed
            if (isToggled)
                TaggedSkills++;
            else
                TaggedSkills--;

            // Update the actual skill value (+15 or -15)
            var property = GetType().GetProperty(skillName);
            if (property != null && property.PropertyType == typeof(int))
            {
                int currentValue = (int)property.GetValue(this);
                int change = isToggled ? 15 : -15;
                property.SetValue(this, currentValue + change);

                // Notify UI of property change
                OnPropertyChanged(skillName);
            }

            // Notify UI about TaggedSkills and confirm button
            OnPropertyChanged(nameof(TaggedSkills));
            OnPropertyChanged(nameof(CanConfirm));

            return true; // toggle allowed
        }

        //SPECIAL

        private int _strength = 5;
        public int Strength
        {
            get => _strength;
            set
            {
                _strength = value;
                OnPropertyChanged();
                RecalculateDerivedStats();
            }
        }

        private int _perception = 5;
        public int Perception
        {
            get => _perception;
            set
            {
                _perception = value;
                OnPropertyChanged();
                RecalculateDerivedStats();
            }
        }

        private int _endurance = 5;
        public int Endurance
        {
            get => _endurance;
            set
            {
                _endurance = value;
                OnPropertyChanged();
                RecalculateDerivedStats();
            }
        }

        private int _charisma = 5;
        public int Charisma
        {
            get => _charisma;
            set
            {
                _charisma = value;
                OnPropertyChanged();
                RecalculateDerivedStats();
            }
        }

        private int _intelligence = 5;
        public int Intelligence
        {
            get => _intelligence;
            set
            {
                _intelligence = value;
                OnPropertyChanged();
                RecalculateDerivedStats();
            }
        }

        private int _agility = 5;
        public int Agility
        {
            get => _agility;
            set
            {
                _agility = value;
                OnPropertyChanged();
                RecalculateDerivedStats();
            }
        }

        private int _luck = 5;
        public int Luck
        {
            get => _luck;
            set
            {
                _luck = value;
                OnPropertyChanged();
                RecalculateDerivedStats();
            }
        }

        private int _points = 5;
        public int Points
        {
            get => _points;
            set
            {
                if (_points != value)
                {
                    _points = value;
                    OnPropertyChanged(nameof(Points));
                    OnPropertyChanged(nameof(CanConfirm));
                }
                RecalculateDerivedStats();
            }
        }

        //Character STATS
        private int _actionPoints;
        public int ActionPoints
        {
            get => _actionPoints;
            set
            {
                _actionPoints = value;
                OnPropertyChanged();
            }
        }

        private int _health;
        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                OnPropertyChanged();
            }
        }

        private int _carryWeight;
        public int CarryWeight
        {
            get => _carryWeight;
            set
            {
                _carryWeight = value;
                OnPropertyChanged();
            }
        }

        private int _criticalChance;
        public int CriticalChance
        {
            get => _criticalChance;
            set
            {
                _criticalChance = value;
                OnPropertyChanged();
            }
        }

        private int _damageResist;
        public int DamageResist
        {
            get => _damageResist;
            set
            {
                _damageResist = value;
                OnPropertyChanged();
            }
        }

        private int _fireResist;
        public int FireResist
        {
            get => _fireResist;
            set
            {
                _fireResist = value;
                OnPropertyChanged();
            }
        }

        private int _poisonResist;
        public int PoisonResist
        {
            get => _poisonResist;
            set
            {
                _poisonResist = value;
                OnPropertyChanged();
            }
        }

        private int _radiationResist;
        public int RadiationResist
        {
            get => _radiationResist;
            set
            {
                _radiationResist = value;
                OnPropertyChanged();
            }
        }

        private double _meleeDamage;
        public double MeleeDamage
        {
            get => _meleeDamage;
            set
            {
                _meleeDamage = value;
                OnPropertyChanged();
            }
        }

        private int _unarmedDamage;
        public int UnarmedDamage
        {
            get => _unarmedDamage;
            set
            {
                _unarmedDamage = value;
                OnPropertyChanged();
            }
        }

        private int _skillPoints;
        public int SkillPoints
        {
            get => _skillPoints;
            set
            {
                _skillPoints = value;
                OnPropertyChanged();
            }
        }

        private int _taggedSkills;
        public int TaggedSkills
        {
            get => _taggedSkills;
            set
            {
                if (_taggedSkills != value)
                {
                    _taggedSkills = value;
                    OnPropertyChanged(nameof(TaggedSkills));
                    OnPropertyChanged(nameof(CanConfirm));
                }
                
            }
        }

        //Character Skills
        private int _barter;
        public int Barter
        {
            get => _barter;
            set
            {
                _barter = value;
                OnPropertyChanged();
            }
        }

        private int _bigGuns;
        public int BigGuns
        {
            get => _bigGuns;
            set
            {
                _bigGuns = value;
                OnPropertyChanged();
            }
        }

        private int _energyWeapons;
        public int EnergyWeapons
        {
            get => _energyWeapons;
            set
            {
                _energyWeapons = value;
                OnPropertyChanged();
            }
        }

        private int _explosives;
        public int Explosives
        {
            get => _explosives;
            set
            {
                _explosives = value;
                OnPropertyChanged();
            }
        }

        private int _lockpick;
        public int Lockpick
        {
            get => _lockpick;
            set
            {
                _lockpick = value;
                OnPropertyChanged();
            }
        }

        private int _medicine;
        public int Medicine
        {
            get => _medicine;
            set
            {
                _medicine = value;
                OnPropertyChanged();
            }
        }

        private int _meleeWeapons;
        public int MeleeWeapons
        {
            get => _meleeWeapons;
            set
            {
                _meleeWeapons = value;
                OnPropertyChanged();
            }
        }

        private int _repair;
        public int Repair
        {
            get => _repair;
            set
            {
                _repair = value;
                OnPropertyChanged();
            }
        }

        private int _science;
        public int Science
        {
            get => _science;
            set
            {
                _science = value;
                OnPropertyChanged();
            }
        }

        private int _smallGuns;
        public int SmallGuns
        {
            get => _smallGuns;
            set
            {
                _smallGuns = value;
                OnPropertyChanged();
            }
        }

        private int _sneak;
        public int Sneak
        {
            get => _sneak;
            set
            {
                _sneak = value;
                OnPropertyChanged();
            }
        }

        private int _speech;
        public int Speech
        {
            get => _speech;
            set
            {
                _speech = value;
                OnPropertyChanged();
            }
        }

        private int _unarmed;
        public int Unarmed
        {
            get => _unarmed;
            set
            {
                _unarmed = value;
                OnPropertyChanged();
            }
        }

        //Special change calculations
        private int CalculateSkill(int baseValue, string skillName)
        {
            if (_taggedSkillNames.Contains(skillName))
                return baseValue + 15;

            return baseValue;
        }
        private void RecalculateDerivedStats()
        {
            ActionPoints = 65 + (Agility * 2);
            Health = 100 + (Endurance * 20);
            CarryWeight = 150 + (Strength * 10);
            MeleeDamage = (Strength * .5);
            PoisonResist = ((Endurance - 1) * 5);
            RadiationResist = ((Endurance - 1) * 2);           
            CriticalChance = Luck;
            SkillPoints = 10 + Intelligence;
            Barter = CalculateSkill(2 + (Luck / 2) + (Luck % 2) + (Charisma * 2), nameof(Barter));
            BigGuns = CalculateSkill(2 + (Luck / 2) + (Luck % 2) + (Endurance * 2), nameof(BigGuns));
            EnergyWeapons = CalculateSkill(2 + (Luck / 2) + (Luck % 2) + (Perception * 2), nameof(EnergyWeapons));
            Explosives = CalculateSkill(2 + (Luck / 2) + (Luck % 2) + (Perception * 2), nameof(Explosives));
            Lockpick = CalculateSkill(2 + (Luck / 2) + (Luck % 2) + (Perception * 2), nameof(Lockpick));
            Medicine = CalculateSkill(2 + (Luck / 2) + (Luck % 2) + (Intelligence * 2), nameof(Medicine));
            MeleeWeapons = CalculateSkill(2 + (Luck / 2) + (Luck % 2) + (Strength * 2), nameof(MeleeWeapons));
            Repair = CalculateSkill(2 + (Luck / 2) + (Luck % 2) + (Intelligence * 2), nameof(Repair));
            Science = CalculateSkill(2 + (Luck / 2) + (Luck % 2) + (Intelligence * 2), nameof(Science));
            SmallGuns = CalculateSkill(2 + (Luck / 2) + (Luck % 2) + (Agility * 2), nameof(SmallGuns));
            Sneak = CalculateSkill(2 + (Luck / 2) + (Luck % 2) + (Agility * 2), nameof(Sneak));
            Speech = CalculateSkill(2 + (Luck / 2) + (Luck % 2) + (Charisma * 2), nameof(Speech));
            Unarmed = CalculateSkill(2 + (Luck / 2) + (Luck % 2) + (Endurance * 2), nameof(Unarmed));
            UnarmedDamage = Unarmed switch
            {
                <= 10 => 1,
                <= 30 => 2,
                <= 50 => 3,
                <= 70 => 4,
                <= 90 => 5,
                _ => 6
            };
        }

        //Create Character Confirmation
        public bool CanConfirm => Points == 0 && TaggedSkills == 3;

        //Character Leveling
        public void LevelUp(Action<Fallout3CharacterStats> applyChanges)
        {
            var newStats = Current.Clone();
            applyChanges(newStats);
            _history.Push(newStats);
        }

        public void UndoLevel()
        {
            if (_history.Count > 1)
                _history.Pop();
        }
    }
}
