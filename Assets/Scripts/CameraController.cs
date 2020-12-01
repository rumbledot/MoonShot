using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float v, rotX, oldRotX;
    private void Update()
    {
        v = Input.GetAxis("Mouse Y");
        oldRotX = transform.rotation.x;
        if (v != 0)
        {
            rotX = v * 12f;
            Debug.Log("ROT CAM : " + rotX + " V : " + v);
            if (rotX <= 0 || rotX >= 5) rotX = oldRotX;
            transform.Rotate(rotX, 0, 0, Space.Self);
        }
    }
}
