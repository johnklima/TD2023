using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCannon : MonoBehaviour
{

    Camera cam;
    [SerializeField] Transform target;
    [SerializeField] Transform player;

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

        Vector3 pos = cam.transform.position + cam.transform.forward * 3;
        Vector3 fwd = cam.transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(pos, fwd, out hit, 3000, layerMask))
        {
            //player facing is independent of player position
            Vector3 v1 = hit.point - player.position;
            v1.Normalize();           


            if (Vector3.Dot(v1,fwd) > 0.3f && hit.distance > 7.0f )
            {
                target.position = hit.point;
                target.LookAt(cam.transform.position);
            }         
           
        }
    }
}
