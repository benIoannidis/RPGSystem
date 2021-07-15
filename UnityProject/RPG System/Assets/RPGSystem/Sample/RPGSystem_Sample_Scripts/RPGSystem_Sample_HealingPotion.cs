using RPGSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGSystem_Sample_HealingPotion : RPGSystem_Item //Derived from RPGSystem Item class
{

    RPGSystem_Sample_HealingPotion()
    {
        m_damage = -20; //On creation set to a negative number, which implies healing
        isConsumable = true; //when set to true, this will be destroyed, and cleared from character inventory upon use
        m_weight = 1; //will determine how many of these objects can be carried based on character strength
    }

    RPGSystem_Sample_HealingPotion(int damage, bool consumable, int weight)
    {
        m_damage = damage;
        isConsumable = consumable;
        m_weight = weight;
    }

    public override bool OnUse(RPGSystem_Character user)
    {
        //Check to ensure we don't give the user more than their maximum HP
        if (user.GetCurrentHP() < user.GetMaxHP() + m_damage) //NEGATIVE DAMAGE IMPLIES HEALING EFFECT
        {
            user.SetCurrentHP(user.GetCurrentHP() - m_damage);
        }
        //If the heal will cause us to go over the maxHP, just set current HP to the max value
        else
        {
            user.SetCurrentHP(user.GetMaxHP());
        }

        //This call to the base function will result in auto-destruction of this object if it is set as "isConsumable"
        base.OnUse(user);
        return true;
    }

    public override bool OnUse(RPGSystem_Character user, RPGSystem_Character target)
    {
        if (!target.IsDead())
        {
            //Check to ensure we don't give more than their maximum HP
            if (target.GetCurrentHP() < target.GetMaxHP() + m_damage) //NEGATIVE DAMAGE IMPLIES HEALING EFFECT
            {
                target.SetCurrentHP(target.GetCurrentHP() - m_damage);
            }
            //If the heal will cause us to go over the maxHP, just set current HP to the max value
            else
            {
                target.SetCurrentHP(target.GetMaxHP());
            }
            //This call to the base function will result in auto-destruction of this object if it is set as "isConsumable"
            base.OnUse(user, target);
            return true;
        }
        return false;
    }

}
