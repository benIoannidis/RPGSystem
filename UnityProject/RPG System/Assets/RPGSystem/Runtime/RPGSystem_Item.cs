using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPGSystem
{
    /// <summary>
    /// This class is implemented as to allow creation of derived classes for your own needs
    /// </summary>
    public class RPGSystem_Item : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Name shown when queried on this item")]
        private string m_name;

        [SerializeField]
        [Tooltip("Will impact carry capacity of character")]
        protected int m_weight;

        [SerializeField]
        [Tooltip("Positive numbers will inflict damage\nNegative numbers act as healing\nIf this is any other sort of item, leave this at 0")]
        protected int m_damage;

        [SerializeField]
        [Tooltip("Time in seconds between attacks\nCan be left empty if not required")]
        protected float m_attackRate;

        [SerializeField]
        [Tooltip("This can be seen by running your scene.\nTo see total character DPS with this weapon, put the weapon in a character inventory, then check this field on the character while your scene is running.")]
        protected float m_dps;

        [SerializeField]
        [Tooltip("Additive with character resistance and will power\nTOTAL RESISTANCE SHOULD BE CAPPED AT 40(%) FOR BALANCE")]
        private int m_resistance;

        [SerializeField]
        [Tooltip("If set to true, this item will be destroyed upon use")]
        protected bool isConsumable;

        [SerializeField]
        [Tooltip("Add a sprite here if you would like to call it when needed for an inventory")]
        private Sprite m_sprite;

        //calculate DPS for item
        public void CalculateDPS()
        {
            m_dps = m_damage / m_attackRate;
        } 
        
        //return item's DPS
        public float GetDPS()
        {
            CalculateDPS();
            return m_dps;
        }

        //return item's attack rate (seconds)
        public float GetAttackRate()
        {
            return m_attackRate;
        }
        
        //set item's attack rate (seconds)
        public void SetAttackRate(float rate)
        {
            if (rate > 0)
            {
                m_attackRate = rate;
            }
            else
            {
                m_attackRate = 0;
            }
        }

        //return item's weight
        public int GetWeight()
        {
            return m_weight;
        }

        //return item's damage
        public int GetDamage()
        {
            return m_damage;
        }

        //return item's resistance
        public int GetResistance()
        {
            return m_resistance;
        }

        //Should be overriden
        //provide charcter that is using item
        //Destroy if consumable
        public virtual bool OnUse(RPGSystem_Character user)
        {
            if (isConsumable)
            {
                for (int i = 1; i < user.GetInventory().Count - 1; i++)
                {
                    if (user.GetInventory()[i] == this)
                    {
                        user.GetInventory().Remove(i);
                        break;
                    }
                }
                Object.Destroy(this.gameObject);
            }
            return false;
        }

        //Should be overriden
        //provide character using item, and a target
        //destroy if consumable
        public virtual bool OnUse(RPGSystem_Character user, RPGSystem_Character target)
        {
            if (isConsumable)
            {
                Object.Destroy(this.gameObject);
            }
            return false;
        }

        //return consumable bool
        public bool IsConsumable()
        {
            return isConsumable;
        }

        //return item's sprite
        public Sprite GetSprite()
        {
            return m_sprite;
        }

        //set item's name to provided string
        public void SetName(string name)
        {
            m_name = name;
        }

        //return item's name
        public string GetName()
        {
            return m_name;
        }
    }
}
