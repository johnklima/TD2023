using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCannon : MonoBehaviour
{

    Camera cam;
    [SerializeField] Transform target;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        // Bit shift the index of the layer (7) to get a bit mask
        int layerMask = 1 << 7; //enemy

        Vector3 campos = cam.transform.position + cam.transform.forward * 3;
        Vector3 fwd = cam.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(campos, fwd, out hit, 300, layerMask))
        {
            //Debug.DrawRay(campos, fwd * hit.distance, Color.red);
            target.position = hit.point;
           
        }
    }
}
