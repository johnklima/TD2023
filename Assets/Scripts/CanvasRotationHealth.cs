using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRotationHealth : MonoBehaviour
{
    public Transform cam;

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
