using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// not 100% sure why we need this
[RequireComponent(typeof(SphereCollider))]

public class FlockAgent : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    Ray ray;
    RaycastHit hit;
    float distance = 2f;

    FlockAgent agent;
    FlockAgent[] thingsWeHit;

    // This script is attached to the bat gameobject

    // local variable of this gameObjects spherecollider
    private SphereCollider agentCollider;
    
    // this is essentially a public method that we are making to be able to access the private
    // spherecollider variable. Its a public function which returns the collider of an instanced spherecollider
    public SphereCollider AgentCollider {get{return agentCollider;}}

    void Start()
    {
        agentCollider = GetComponent<SphereCollider>();
        thingsWeHit = GameObject.FindObjectsOfType<FlockAgent>();
    }

    // Move function to pass through other scripts vector3's that get made through a FlockBehaviour script
    public void Move(Vector3 velocity)
    {
        // this objects forward transform equals the passed vector3
        // speed managed through Flock script
        transform.forward = velocity;
        // this objects position equals the vector3 forward movement, over time.
        transform.position += velocity * Time.deltaTime;

    }
    void FixedUpdate()
    {
        
    }
    // void RayCheck()
    // {
    //     Collider[] colliders = Physics.OverlapSphere(transform.position, distance, layerMask);
    //     for (int i = 0; i < colliders.Length; i++)
    //     {
    //         if (colliders[i] != thingsWeHit)
    //         {
                
    //         }
    //     }GameObject.FindObjectsOfType<FlockAgent>();

    //     foreach(Collider coll in colliders)
    //     {
    //         coll.transform.position -= transform.position;

    //     }

    //     Debug.DrawLine(transform.localPosition, transform.forward * distance, Color.red);
    //     if (Physics.Raycast(ray, out hit, distance, layerMask))
    //     {
    //         // thingWeHit = hit.transform.gameObject;
    //         transform.position -= hit.transform.position;
    //         Debug.Log("Hit Something");
    //     }
    //     if (Physics.Raycast(ray, out hit, distance, layerMask))
    //     {
    //         // start, end, Color color, 
    //         Debug.DrawLine(transform.position, transform.forward * distance, Color.red);
            
    //         FlockAgent agent = hit.transform.gameObject.GetComponent<FlockAgent>();
    //         if (!agent)
    //         {
    //             GameObject thingWeHit = hit.transform.gameObject;
    //             transform.position -= thingWeHit.transform.position;
    //         }
    //     }
    // }
}
