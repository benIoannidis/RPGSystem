using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLevelText : MonoBehaviour
{
    float moveDistance = 2f;

    void Update()
    {
        Vector3 newPos = this.transform.position;
        newPos.y += moveDistance;
        this.transform.position = newPos;
        moveDistance -= 0.025f;
    }
}
