using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCannon : MonoBehaviour
{

    [SerializeField] Camera camera;
    [SerializeField] Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // Bit shift the index of the layer (7) to get a bit mask
        int layerMask = 1 << 7; //enemy

        Vector3 campos = camera.transform.position + camera.transform.forward;
        Vector3 fwd = camera.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(campos, fwd, out hit, 300))
        {
            Debug.DrawRay(campos, fwd * hit.distance, Color.red);
            target.position = hit.point;
           
        }
    }
}
