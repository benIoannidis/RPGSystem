using RPGSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGSystem_Sample_SplashPotionOfDamage : RPGSystem_Item
{
    RPGSystem_Sample_SplashPotionOfDamage()
    {
        m_damage = 50;
        isConsumable = true;
        m_weight = 1;
    }

    ~RPGSystem_Sample_SplashPotionOfDamage()
    {
        Debug.Log("I have been destroyed");
    }

    public override bool OnUse(RPGSystem_Character user, RPGSystem_Character target)
    {
        if (!target.IsDead())
        {
            target.ReceiveAttack(m_damage);
            //base.OnUse(user, target);
            Destroy(this);
            return true; 
        }

        Debug.Log("That target is already dead!");
        return false;
    }
}
