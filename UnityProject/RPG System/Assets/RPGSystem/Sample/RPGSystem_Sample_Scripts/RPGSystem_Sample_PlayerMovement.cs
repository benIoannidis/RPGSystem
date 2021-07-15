using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class RPGSystem_Sample_PlayerMovement : MonoBehaviour
{
    private Vector3 m_input;

    [SerializeField]
    private float m_movementSpeed = 5.0f;

    private bool hasHitStore = false;
    private bool hasHitXP = false;

    [SerializeField]
    private GameObject swordStore;

    [SerializeField]
    private GameObject hitText;

    void Update()
    {
        if (this.GetComponent<RPGSystem.RPGSystem_Character>().IsDead())
        {
            this.GetComponentInChildren<RPGSystem_Sample_CameraController>().enabled = false;
            this.GetComponent<RPGSystem_Sample_PlayerMovement>().enabled = false;
        }
        m_input = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        m_input *= m_movementSpeed;
    }

    void FixedUpdate()
    {
        this.GetComponent<Rigidbody>().velocity = m_input;
    }

    //check whether entered sword store or XP building spheres
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<SphereCollider>().tag == "SwordStore")
        {
            hasHitStore = true;
        }

        else if (other.gameObject.GetComponent<SphereCollider>().tag == "XPStore")
        {
            hasHitXP = true;
        }    
    }

    //check for exit from above buildings spheres
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "SwordStore")
        {
            hasHitStore = false;
        }

        else if (other.gameObject.tag == "XPStore")
        {
            hasHitXP = false;
        }
    }

    //return bool for colliding with sword store
    public bool HasHitStore()
    {
        return hasHitStore;
    }

    //return bool for colliding with XP building
    public bool HasHitXP()
    {
        return hasHitXP;
    }

    //check for collisions with enemies
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemy" || collision.gameObject.GetComponent<RPGSystem.RPGSystem_Character>())
        {
            if (!collision.gameObject.GetComponent<RPGSystem.RPGSystem_Character>().IsDead())
            {
                GameObject hitInfo = Instantiate(hitText);
                RPGSystem.RPGSystem_Character player = this.GetComponent<RPGSystem.RPGSystem_Character>();
                hitInfo.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
                hitInfo.GetComponentInChildren<Text>().text = collision.gameObject.GetComponent<RPGSystem.RPGSystem_Character>().CalculateHitDamage().ToString();
                this.gameObject.GetComponent<RPGSystem.RPGSystem_Character>().ReceiveAttack(collision.gameObject.GetComponent<RPGSystem.RPGSystem_Character>().CalculateHitDamage());
            }
        }
    }
}
