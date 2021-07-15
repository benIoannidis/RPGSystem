# RPGSystem
RPG System for Unity


### Introduction
The goal of this package is to allow users to quickly create their own RPG 
system through adding their own Item classes, that are derived from my base 
Item class, as well as adding my Character class onto character objects.

### Adding the package to your project
Open your project in Unity. In the menu bar, go to “Assets/Import
Package/Custom Package…”. Locate the RPGSystem_Package.unitypackage, 
and add it to your project. You will be prompted to select which elements of 
the package you would like to import. 

## Classes

### RPGSystem_Item
This class was created in such a way as to allow coverage of all types of items, 
from weapons and armour to potions and buffs.

#### Creating an Item
Once you have added this package to your project, you will see in the Unity 
Editor menu bar, "RPG System" now has it's own tab, if you create an Item 
through here, a new class will be created, and dropped into the Assets folder 
for you. This class is derived from the base Item class, and will have 2 overrided 
functions for you to fill out yourself to suit your needs. You can simply apply 
this new class to an existing item, or create a new one, and apply it to that. All 
the below variables are exposed through the Inspector for in-engine tweaking.

#### Item member variables and functions

##### Member variables
- m_name: A string that can be queried
- m_weight: An integer that will affect a characters carry amount
- m_damage: An integer that will be added to character strength when 
calculating hit damage• m_attackRate: Time in seconds between attacks, this can be left empty 
(equal to 0) if not required
- m_dps: Gives you the ability to see the weapons DPS when not being 
used by a character
- m_resistance: This will be added to character resistance, to calculate the 
percentage of damage that will be mitigated on receiving an attack
- isConsumable: A bool that if set to "true", will lead to the Item being 
destroyed upon use
- m_sprite: A sprite that can be assigned to this Item if required, if not, it 
can be left empty

##### Member functions
- Get/SetAttackRate: Get and Set functions to set/retrieve the Item's 
attack rate
- GetWeight: Function to return the Item's weight
- GetDamage: Function to return the Item's damage
- GetResistance: Function to return the Item's resistance
- IsConsumable: Function to return whether the Item is consumable or 
not
- GetSprite: Function to return the Item's sprite
- Get/SetName: Get and Set functions to set/retrieve the Item's name

##### Virtual Member Functions
- OnUse(RPGSystem_Character user): Should be called on Character's equipped 
item when required, this needs to be overriden to function as you intend 
it to
- OnUse(RPGSystem_Character user, RPGSystem_Character target): Similar to the above 
function, and should be overriden, though this allows for another input 
parameter for cases such as targeted healing and the like

#### Creating a Character
Using the same method as above, Character's can be added through the Unity 
Editor menu bar. When adding a character, a new empty GameObject will be 
added to your scene with the RPGSystem_Character script already attatched. 
Much like the Item's, most to all of the member variables that will be of use, 
have been exposed in the Inspector for in-engine tweaking.

#### Character member variables and functions

##### Member variables
- m_name: A string that can be updated and queried
- m_maxLevel: An integer that will set the Character and stats maximum 
level
- m_currentXP: An intger to keep track of Character's current XP
- m_xpLevels: An intger array where the XP required for each level up is 
kept, the size of this array will be determined by your maxLevel in the 
script's Start function
- m_currentLevel: An integer to keep track of the Character's current level
- m_strengthLevel: An integer that affects attack power(damage), 
maximum HP, and the Character's carry capacity
- m_strength: This is a private integer that is the actual strength of the 
Character (this is calculated as: 5 x strengthLevel)
- m_dps: Float calculated by adding up the Item's dps, as well as the 
Character's strength
- m_maxHP: Integer that holds the maximum HP amount of the Character, 
this is affected by strengthLevel
- m_currentHP: Integer to track Character's current HP, this is affected 
upon ReceiveAttack being called
- isDead: Bool to track whether the Character is dead or not
- m_carryCapacity: Integer that tracks the Character's maximum weight 
they are capable of holding
- m_remainingCapacity: Integer to keep track of how much of the 
Character's capacity has already been used
- m_constitutionLevel: An integer that affects endurance, maximumMP, 
and stamina
- m_maxMP: Integer that holds maximum MP that the Character has
- m_currentMP: Integer to keep track of Character's current MP
- m_maxStamina: Integer that holds maximum stamina that the Character 
has
- m_currentStamina: Integer to keep track of Character's current stamina
- m_dexterityLevel: Integer used when calculating chance of a critical hit 
on an attack
- m_willpowerLevel: Integer used when calculating chance of de-buffing 
incoming damage
- m_inventorySlots: Integer signifying total number of item slots for 
Character
- inventory: A dictionary that holds RPGSystem_Item's, and can be 
added/removed using a key• m_equippedItem: An Item, this will be used when you call 
CalculateHitDamage
- m_unarmedItem: Item that will be the base equipped Item if the 
Character has no other items

##### Member functions
- GetMaxHP: Return the Character's maximum HP value
- GetCurrentHP: Return the Character's current HP value
- SetCurrentHP: Takes in an integer, useful for things like healing potions, 
will also check to ensure current HP does not end up higher than the 
max HP for the Character
- IsDead: Returns whether the Character has died or not
- GetStrengthLevel: Returns Character's current strength level
- GetDexterityLevel: Returns Character's current dexterity level
- GetConstitutionLevel: Returns Character's current constitution level
- GetWillPowerLevel: Return's Character's current willpower level
- GetInventory: Returns the Character's inventory as the dictionary
- AddXP: Takes an integer as input, and adds to Character's current XP 
value
- GetCurrentInventoryWeight: returns the remaining capacity of the 
Character's inventory
- GetCurrentLevel: Returns the Character's current level
- EquipItem: Takes an integer as input (dictionary key), and will check if 
that dictionary position holds an Item, if it does, it will be set as the 
Character's equipped Item
- GetEquippedItem: Returns the Character's currently equipped Item
- CalculateDPS: Uses the equipped Item's damage and attack rate, as well 
as the Character's strength level to calculate
- Get/SetName: Get and Set functions for the Character's name

##### Virtual member functions
The below functions were created as virtual, if you need the stats to be affected differently, I would recommend deriving your own class from mthe RPGSystem.RPGSystemCharacter class, and overriding these functions.

Default implementations of functions:

- StrengthLevelUp()
```
  if (m_strengthLevel < m_maxLevel)
  {
     m_strengthLevel++;
     m_strength = m_strengthLevel * 5; m_maxHP = 50 + m_strength;
     m_currentHP = m_maxHP;
     if (m_strengthLevel % 5 == 0)
     {
      m_carryCapacity += 10;
     }
  }
``` 
- DexterityLevelUp()
```
  if (m_dexterityLevel < m_maxLevel)
  {
    m_dexterityLevel++;
  }
``` 
• ConstitutionLevelUP()
```
  if (m_constitutionLevel < m_maxLevel)
  {
     m_constitutionLevel++;
     m_maxMP = 100 + (m_constitutionLevel * 5);
     m_maxStamina = 100 + (m_constitutionLevel * 5);
  }
```
- WillPowerLevelUp()
```
  if (m_willpowerLevel < m_maxLevel)
  {
    m_willpowerLevel++;
  }
```
- CheckCrit()
```
  int crit = Random.Range(0, 99);
  if (crit <= m_dexterityLevel)
  {
    return true;
  }
  return false;
```
- CalculateHitDamage()
```
  if (CheckCrit())
  {
     int damage = (int)(m_strength + m_equippedItem.GetDamage() + ((m_strength + 
     m_equippedItem.GetDamage()) * .1f));
     
     return damage;
  }
  return m_strength + m_equippedItem.GetDamage();
```
- CalculateDodge()
```
  int dodge = Random.Range(1, 100);
  if (dodge <= m_dexterityLevel)
  {
    return true;
  }
  return false;
```
- CalculateResistance(int damage)
```
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
```
- ReceiveAttack(int damage)
```
  if (!CalculateDodge())
  {
     damage = CalculateResistance(damage);
     m_currentHP -= damage;
     if (m_currentHP <= 0)
     {
       isDead = true; m_currentHP = 0;
       return;
     }
  }
  return;
```
- AddItem(int position, RPGSystem_Item newItem)
```
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
```
- AddItem(RPGSystem_Item)
```
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
  if (count > m_inventorySlots){
    Debug.LogAssertion("ATTEMPTING TO ADD WEAPON TO FULL INVENTORY");
  }
```
- UseInventoryItem(int key)
```
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
```
- UseInventoryItem(int key, RPGSystem_Character target)
```
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
       Debug.LogAssertion("COULD NOT USE " + inventory[key].GetName() + " ON TARGET: " + 
      target.name);
       return;
     }
     inventory[key].OnUse(this, target);
  }
  else
  {
    Debug.LogAssertion("NO ITEM FOUND UNDER PROVIDED KEY: " + key);
  }
```
- ShouldLevelUp()
```
  bool didLevelUp = false;
  for (int i = m_currentLevel; i < m_xpLevels.Length; i++)
  { 
    if (m_currentXP >= m_xpLevels[i])
     {
       m_currentLevel++;
       didLevelUp = true;
     }
  }
  return didLevelUp
```
