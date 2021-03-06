﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private float height, oldHeight;

    private void Awake()
    {
        height = transform.localPosition.y;

        Destroy(gameObject, 3f);
    }
    private void Update()
    {
        oldHeight = height;
        height = transform.localPosition.y;
        if (oldHeight > height)
        {
            transform.Rotate(Mathf.Lerp(transform.localRotation.x, 80f, Time.deltaTime), 0f, 0f, Space.Self);
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "TheSurface")
        {
            Destroy(gameObject);
        }
    }
}
