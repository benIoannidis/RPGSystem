using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGSystem_Sample_HitText : MonoBehaviour
{
    private float moveY = 2f;

    public GameObject cam;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        StartCoroutine("DestroySelf");
    }

    //ensure text is always facing toward camera
    private void Update()
    {
        Vector3 newPos = this.transform.position;
        newPos.y += moveY;

        this.transform.position = newPos;
        moveY *= 0.35f;

        Transform camPos = cam.transform;
        this.transform.LookAt(camPos);
        this.gameObject.transform.localEulerAngles = new Vector3(0, this.gameObject.transform.eulerAngles.y + 180, 0);

    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
