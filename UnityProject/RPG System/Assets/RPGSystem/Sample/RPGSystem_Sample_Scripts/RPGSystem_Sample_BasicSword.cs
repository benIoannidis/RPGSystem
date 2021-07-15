using RPGSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RPGSystem_Sample_BasicSword : RPGSystem.RPGSystem_Item
{
    [SerializeField]
    private float m_range;

    private bool canAttack = true;

    [SerializeField]
    private GameObject hitText;

    public override bool OnUse(RPGSystem_Character user)
    {
        //check if waiting for cooldown based on attack rate
        if (canAttack)
        {
            List<GameObject> hits = AttackAnim(user.gameObject);

            for (int i = 0; i < hits.Count; i++)
            {
                if (hits[i].GetComponent<RPGSystem_Character>() == user)
                {
                    continue;
                }
                else if (!hits[i].GetComponent<RPGSystem_Character>().IsDead())
                {
                    GameObject hitInfo = Instantiate(hitText);
                    //hitInfo.GetComponentInChildren<Text>().fontSize = 2;
                    hitInfo.transform.position = new Vector3(hits[i].transform.position.x, hits[i].transform.position.y + 1, hits[i].transform.position.z);
                    hitInfo.GetComponentInChildren<Text>().text = user.GetComponent<RPGSystem_Character>().CalculateHitDamage().ToString();
                    hits[i].GetComponent<RPGSystem_Character>().ReceiveAttack(user.CalculateHitDamage());                    
                    //Debug.Log("Hit " + hits[i].name + " for " + user.GetEquippedItem().GetDamage());
                }
            }
            StartCoroutine("WaitForAttackRate");
        }
        return true;
    }

    //return list of characters in weapon range
    private List<GameObject> AttackAnim(GameObject user)
    {
        Collider[] cols = Physics.OverlapSphere(user.transform.position, m_range);

        List<GameObject> characters = new List<GameObject>();

        foreach (Collider c in cols)
        {
            if (c.gameObject.GetComponent<RPGSystem_Character>() != null)
            {
                characters.Add(c.gameObject);
            }
        }

        return characters;
    }

    //wait for attack rate (seconds) before next attack is allowed
    IEnumerator WaitForAttackRate()
    {
        canAttack = false;        
        yield return new WaitForSeconds(m_attackRate);
        canAttack = true;
    }
}
