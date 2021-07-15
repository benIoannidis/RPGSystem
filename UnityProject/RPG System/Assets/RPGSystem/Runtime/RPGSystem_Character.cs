using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGSystem
{
    public class RPGSystem_Character : MonoBehaviour
    {
        [SerializeField]
        private string m_name;

        [SerializeField]
        [Tooltip("Set the maximum level for stats")]
        private static int m_maxLevel = 20; //Character level will be capped at this int

        [SerializeField]
        [Tooltip("Current XP total")]
        private int m_currentXP; //current XP total

        [SerializeField]
        [Tooltip("Integer array to store required XP to advance through each character level")]
        private int[] m_xpLevels = new int[m_maxLevel]; //this array will store the values required to initiate levelup

        [SerializeField]
        [Tooltip("Character's current level")]
        private int m_currentLevel; //current level


        [Header("Strength")]
        [SerializeField]
        [Tooltip("Affects:\n->Attack Power\n->Max HP\n->Carry Capacity")]
        private int m_strengthLevel; 

        private int m_strength; //this is the actual strength of the character (5 x strengthLevel)

        [SerializeField]
        [Tooltip("Calculated using strength, and equipped item's damage and attack rate")]
        private float m_dps;

        [SerializeField]
        private int m_maxHP; //updated when strength level is (HP = 50 + strengthLevel)
        [SerializeField]
        private int m_currentHP; //keeps track of current HP
        [SerializeField]
        private bool isDead = false; //will be set to true once players health falls to or below 0 (on receive attack), can also be queried

        public int m_carryCapacity = 100; //updated every 5 levels of strength level (if strengthLevel % 5 == 0 -> carryCap + 10)
        private int m_remainingCapacity; //keeps track of usage of carryCap - checked when giving characters items

        [Header("Constitution")]
        [SerializeField]
        [Tooltip("Affects:\n->Endurance\n->Max MP\n->Stamina")]
        private int m_constitutionLevel;

        [SerializeField]
        private int m_maxMP; //updated when constitution level is (MP = 100 + (constitutionLevel x 5))
        private int m_currentMP; //keeps track of current stamina

        public int m_maxStamina; //updated when constitution level is (stamina = 100 + (constitutionLevel x 5))
        private int m_currentStamina; //keeps track of current stamina

        [Header("Dexterity")]
        [SerializeField]
        [Tooltip("Affects:\n->Agility\n->Speed")]
        private int m_dexterityLevel; //used when calculating chance of critical hit on attack

        [Header("Willpower")]
        [SerializeField]
        [Tooltip("Affects:\n->Resistance")]
        private int m_willpowerLevel;

        private int m_resistance; //used when calculating chance of reduced damage, or effects

        [Header("Inventory")]
        [SerializeField]
        [Tooltip("Will set slot count in inventory")]
        private int m_inventorySlots; //total slots to be allowed in inventory

        Dictionary<int, RPGSystem_Item> inventory; //weapon's are stored in - IMPORTANT! KEY 0 WILL BE SET TO "UNARMED" SEE m_unarmedItem below

        [SerializeField]
        [Tooltip("This will be used when determining DPS, and what to attack with if 'Attack' is called")]
        private RPGSystem_Item m_equippedItem; //currently equipped item

        [SerializeField]
        [Tooltip("If left empty, this script will auto-create the object to take the place")]
        private RPGSystem_Item m_unarmedItem; //permanent item, position in dictionary will be at 0, with a key of int.MaxValue

        void Start()
        {
            //Set initial stats based on current level
            m_strength = m_strengthLevel * 5;
            m_maxHP = 50 + m_strength;
            m_maxMP = 100 + (m_constitutionLevel * 5);
            m_maxStamina = 100 + (m_constitutionLevel * 5);
            m_resistance = m_willpowerLevel;

            //create inventory dictionary
            inventory = new Dictionary<int, RPGSystem_Item>();

            //This assumes because the dictionary was created above, that the inventory is empty, this can be commented out if not the case
            m_remainingCapacity = m_carryCapacity;

            m_currentHP = m_maxHP;

            //on startup, check whether an item for unarmed exists (has been added through inspector), if there isn't one, create one and add to inventory
            if (m_unarmedItem == null)
            {
                GameObject unarmed = new GameObject();
                unarmed.AddComponent<RPGSystem_Item>();
                unarmed.GetComponent<RPGSystem_Item>().SetName("UNARMED");
                unarmed.transform.parent = this.gameObject.transform;
                unarmed.name = "UNARMED_ITEM";
                m_unarmedItem = unarmed.GetComponent<RPGSystem_Item>();
            }
            else if (!m_unarmedItem.gameObject.GetComponent<RPGSystem_Item>())
            {
                m_unarmedItem.gameObject.AddComponent<RPGSystem_Item>();
                m_unarmedItem.SetName("UNARMED");
                m_unarmedItem.gameObject.transform.parent = this.gameObject.transform;
            }

            m_unarmedItem.SetAttackRate(1);
            //This has been set as the maximum value, so if you traverse through your alloted inventory slots, and don't find a weapon,
            //you can reference at this key to ensure you can always can get the unarmed item
            inventory[int.MaxValue] = m_unarmedItem.GetComponent<RPGSystem_Item>();

            //This line will auto-equip the unarmed item, you can replace the max integer with which ever item you would like
            //WARNING - ENSURE YOUR KEY ENTERED HERE IS PRESENT IN THE DICTIONARY, OR YOU WILL END UP WITH NOTHING
            EquipItem(int.MaxValue);

            //initial calculation of DPS
            CalculateDPS();
        }

        //return maximum HP value
        public int GetMaxHP()
        {
            return m_maxHP;
        }

        //return current HP value
        public int GetCurrentHP()
        {
            return m_currentHP;
        }

        //set hp to provided int, will check whether this will create a situation with more HP than MaxHP allows
        public void SetCurrentHP(int newHP)
        {
            if (newHP > m_maxHP)
            {
                m_currentHP = m_maxHP;
            }
            else
            {
                m_currentHP = newHP;
            }
        }

        //public accessor for checking whether character is alive
        public bool IsDead()
        {
            return isDead;
        }

        //Should be called when leveling up the Strength stat
        //This will also update relevant sub-stats
        virtual public void StrengthLevelUp()
        {
            if (m_strengthLevel < m_maxLevel)
            {
                m_strengthLevel++;
                m_strength = m_strengthLevel * 5;
                m_maxHP = 50 + m_strength;
                m_currentHP = m_maxHP;
                if (m_strengthLevel % 5 == 0)
                {
                    m_carryCapacity += 10;
                }
            }
        }

        //Return strength level
        public int GetStrengthLevel()
        {
            return m_strengthLevel;
        }

        //Should be called when leveling up the Dexterity stat
        virtual public void DexterityLevelUp()
        {
            if (m_dexterityLevel < m_maxLevel)
            {
                m_dexterityLevel++;
            }
        }

        //return dexterity level
        public int GetDexterityLevel()
        {
            return m_dexterityLevel;
        }

        //Should be called when leveling up the Constitution stat
        //This will also update relevant sub-stats
        virtual public void ConstitutionLevelUp()
        {
            if (m_constitutionLevel < m_maxLevel)
            {
                m_constitutionLevel++;
                m_maxMP = 100 + (m_constitutionLevel * 5);
                m_maxStamina = 100 + (m_constitutionLevel * 5);
            }
        }

        //return constitution level
        public int GetConstitutionLevel()
        {
            return m_constitutionLevel;
        }

        //initiate levelup of willpower stat
        virtual public void WillPowerLevelUp()
        {
            if (m_willpowerLevel < m_maxLevel)
            {
                m_willpowerLevel++;
            }
        }

        //return willpower level
        public int GetWillPowerLevel()
        {
            return m_willpowerLevel;
        }

        //Should be called on attack
        //Will check for crit and apply appropriate damage
        virtual public int CalculateHitDamage()
        {
            if (CheckCrit())
            {
                int damage = (int)(m_strength + m_equippedItem.GetDamage() + ((m_strength + m_equippedItem.GetDamage()) * .1f));
                return damage;
            }

            return m_strength + m_equippedItem.GetDamage();
        }

        //Called while calculating hit damage
        virtual public bool CheckCrit()
        {
            int crit = Random.Range(0, 99);

            if (crit <= m_dexterityLevel)
            {
                return true;
            }

            return false;
        }

        //Takes in an integer (using CalculateHitDamage is the idea), checks for dodge chance, and calculates resistance
        //will set character to dead if health hits 0
        virtual public void ReceiveAttack(int damage)
        {
            if (!CalculateDodge())
            {
                damage = CalculateResistance(damage);
                m_currentHP -= damage;

                if (m_currentHP <= 0)
                {
                    isDead = true;
                    m_currentHP = 0;
                    return;
                }
            }
            return;
        }

        //Should be called on RECEIVE attack
        //Calculate total resistance percentage (addition of total resistance / 100) then modify the damage based on this
        virtual public int CalculateResistance(int damage)
        {
            int totalResistance = 0;

            
            totalResistance += m_willpowerLevel;

            foreach (RPGSystem_Item item in inventory.Values)
            {
                totalResistance += item.GetResistance();
            }

            if (totalResistance > 40)
            {
                totalResistance = 40;
            }

            float percent = totalResistance / 100;
            //ensure damage doesn't get set to zero accidentally
            if (percent > 0)
            {
                damage = (int)(damage * percent);
            }
            return damage;
        }

        //Should be called on RECEIVE attack
        virtual public bool CalculateDodge()
        {
            int dodge = Random.Range(1, 100);

            if (dodge <= m_dexterityLevel)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// If a weapon exists in that position already, it will be overriden.
        /// POSITION MUST BE LOWER THAN INVENTORY SLOT COUNT
        /// </summary>
        /// <param name="position">inventory dictionary key</param>
        /// <param name="newItem">item to add at key position</param>
        virtual public void AddItem(int position, RPGSystem_Item newItem)
        {
            if (position > m_inventorySlots)
            {
                Debug.LogAssertion("POSITION OUT OF RANGE OF INVENTORY");
            }
            else
            {
                if (newItem.GetWeight() > m_remainingCapacity)
                {
                    Debug.Log("Your character is not strong enough to carry this.\nTry dropping something first.");
                }
                else
                {
                    inventory[position] = newItem;
                    newItem.transform.parent = this.gameObject.transform;
                    m_remainingCapacity -= newItem.GetWeight();
                }
            }
        }

        /// <summary>
        /// Will iterate through numerically ordered keys for dictionary, once it finds an open key, the item will be added
        /// </summary>
        /// <param name="newItem">item to add at next free numerically ordered dictionary key</param>
        virtual public void AddItem(RPGSystem_Item newItem)
        {
            if (newItem.GetWeight() > m_remainingCapacity)
            {
                Debug.Log("Your character is not strong enough to carry this.\nTry dropping something first.");
                return;
            }
            int count = 1;
            for (int i = 0; i < m_inventorySlots; i++)
            {
                RPGSystem_Item temp;
                if (!inventory.TryGetValue(i, out temp))
                {
                    inventory[i] = newItem;
                    newItem.transform.parent = this.gameObject.transform;
                    m_remainingCapacity -= newItem.GetComponent<RPGSystem_Item>().GetWeight();
                    break;
                }
                else
                {
                    count++;
                }
            }
            if (count > m_inventorySlots)
            {
                Debug.LogAssertion("ATTEMPTING TO ADD WEAPON TO FULL INVENTORY");
            }
        }

        //try to find key in inventory, check if consumable, call OnUse item function, and remove from dictionary if consumable
        virtual public void UseInventoryItem(int key)
        {
            RPGSystem_Item item;
            if (inventory.TryGetValue(key, out item))
            {
                if (inventory[key].IsConsumable())
                {
                    inventory[key].OnUse(this);
                    inventory.Remove(key);
                    return;
                }
                inventory[key].OnUse(this);
            }
            else
            {
                Debug.LogAssertion("NO ITEM UNDER PROVIDED KEY: " + key);
            }
        }

        //try to find key in inventory, check if consumable, check if usage is allowed, use if all true, and remove if consumable
        virtual public void UseInventoryItem(int key, RPGSystem_Character target)
        {
            RPGSystem_Item item;
            if (inventory.TryGetValue(key, out item))
            {
                if (inventory[key].IsConsumable())
                {
                    if (inventory[key].OnUse(this, target))
                    {
                        inventory.Remove(key);
                        return;
                    }
                    Debug.LogAssertion("COULD NOT USE " + inventory[key].GetName() + " ON TARGET: " + target.name);
                    return;
                }
                inventory[key].OnUse(this, target);
            }
            else
            {
                Debug.LogAssertion("NO ITEM FOUND UNDER PROVIDED KEY: " + key);
            }
        }

        //return inventory dictionary
        public Dictionary<int, RPGSystem_Item> GetInventory()
        {
            return inventory;
        }

        //check current XP against levels of XP from current character level, and up
        //if found that XP is suitable, add a level to character
        virtual public bool ShouldLevelUp()
        {
            bool didLevelUp = false;
            for (int i = m_currentLevel; i < m_xpLevels.Length; i++)
            {
                if (m_currentXP >= m_xpLevels[i])
                {
                    m_currentLevel++;
                    didLevelUp = true;
                }
            }
            return didLevelUp;
        }

        //add XP to characters current XP
        public void AddXP(int XP)
        {
            m_currentXP += XP;
        }

        //return remaining weight in inventory
        public int GetCurrentInventoryWeight()
        {
            return m_remainingCapacity;
        }

        //return current character level
        public int GetCurrentLevel()
        {
            return m_currentLevel;
        }

        //check whether inventory holds key, if it does, set equipped item to the associated item
        public void EquipItem(int key)
        {
            //check whether the key is present in the dictionary
            if (!inventory.ContainsKey(key))
            {
                Debug.LogAssertion("THIS KEY " + key + " IS NOT IN THE INVENTORY");
            }
            else
            {
                m_equippedItem = inventory[key];
            }
        }

        //return currently equipped item
        public RPGSystem_Item GetEquippedItem()
        {
            return m_equippedItem;
        }

        //calculate DPS based on character strength, and weapon damage / attack rate
        public float CalculateDPS()
        {
            m_dps = (m_equippedItem.GetDamage() + m_strength) / m_equippedItem.GetAttackRate();
            return m_dps;
        }

        //Get/set functions for character name
        public string GetName()
        {
            return m_name;
        }
        public void SetName(string name)
        {
            m_name = name;
        }
    }
}