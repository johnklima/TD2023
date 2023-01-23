using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartInformationUI : MonoBehaviour
{
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
