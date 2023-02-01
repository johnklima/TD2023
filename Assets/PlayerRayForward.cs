using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayForward : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 30, Color.yellow);

        int avoidMask = 1 << 6;

        bool didHit = false;
        RaycastHit hit;
        // Does the ray intersect any objects in the layer mask
        if (Physics.Raycast(transform.position, transform.forward, out hit, 30, avoidMask))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);

           

            didHit = true;
        }
    }
}
