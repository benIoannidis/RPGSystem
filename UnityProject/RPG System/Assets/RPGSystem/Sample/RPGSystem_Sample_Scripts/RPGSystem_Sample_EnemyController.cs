using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RPGSystem_Sample_EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float m_moveSpeed;

    [SerializeField]
    private Vector3 targetPos;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    //move towards player object
    void Update()
    {
        targetPos = target.transform.position - this.transform.position;
        targetPos = targetPos.normalized;
        targetPos *= m_moveSpeed;
        
    }

    void FixedUpdate()
    {
        if (!this.GetComponent<RPGSystem.RPGSystem_Character>().IsDead())
        {
            this.GetComponent<Rigidbody>().velocity = targetPos;
        }
        else
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;

            this.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
    }
}
