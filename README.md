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
• m_name: A string that can be queried
• m_weight: An integer that will affect a characters carry amount
• m_damage: An integer that will be added to character strength when 
calculating hit damage• m_attackRate: Time in seconds between attacks, this can be left empty 
(equal to 0) if not required
• m_dps: Gives you the ability to see the weapons DPS when not being 
used by a character
• m_resistance: This will be added to character resistance, to calculate the 
percentage of damage that will be mitigated on receiving an attack
• isConsumable: A bool that if set to "true", will lead to the Item being 
destroyed upon use
• m_sprite: A sprite that can be assigned to this Item if required, if not, it 
can be left empty
