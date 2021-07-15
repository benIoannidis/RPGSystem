using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Third person camera orbit implemented while following the below video, created by: Zenva on Youtube
/// https://www.youtube.com/watch?v=TnhDwCI4qqg
/// </summary>
public class RPGSystem_Sample_CameraController : MonoBehaviour
{
    [SerializeField]
    private float sensitivity;

    [SerializeField]
    private float minX;

    [SerializeField]
    private float maxX;

    public Transform cameraAnchor;

    private float currentX;

    public bool canUseMouse = true;

    void LateUpdate()
    {
        if (canUseMouse)
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");


            transform.eulerAngles += Vector3.up * x * sensitivity;

            currentX += y * sensitivity;
            currentX = Mathf.Clamp(currentX, minX, maxX);

            Vector3 clampedAngle = cameraAnchor.eulerAngles;
            clampedAngle.x = currentX;

            cameraAnchor.eulerAngles = clampedAngle;
        }
    }
}
