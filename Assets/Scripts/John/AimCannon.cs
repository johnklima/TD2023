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

        //build exclusion layer mask
        int layerMask = (1 << 10);
        layerMask |= (1 << 5);
        layerMask |= (1 << 3);
        layerMask = ~layerMask; //not spore, not player, not UI

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
