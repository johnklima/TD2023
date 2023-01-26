using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// not 100% sure why we need this
[RequireComponent(typeof(SphereCollider))]

public class FlockAgent : MonoBehaviour
{
    // This script is attached to the bat gameobject

    // local variable of this gameObjects spherecollider
    private SphereCollider agentCollider;
    
    // this is essentially a public method that we are making to be able to access the private
    // spherecollider variable. Its a public function which returns the collider of an instanced spherecollider
    public SphereCollider AgentCollider {get{return agentCollider;}}

    [SerializeField] LayerMask layerMask;

    float moveSpeed = 3f;

    void Start()
    {
        agentCollider = GetComponent<SphereCollider>();
    }

    // Move function to pass through other scripts vector3's that get made through a FlockBehaviour script
    public void Move(Vector3 velocity)
    {
        // this objects forward transform equals the passed vector3
        // speed managed through Flock script
        transform.forward = velocity;
        // this objects position equals the vector3 forward movement, over time.
        transform.position += velocity * moveSpeed * Time.deltaTime;

        RayCheck();
    }
    
    void RayCheck()
    {
        Ray ray = new Ray (transform.position, transform.forward);
        RaycastHit hit;
        float distance = 2f;

        Debug.DrawLine(transform.position, transform.forward * distance, Color.red);
        
        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            Debug.Log("Hit" + hit.transform.name);
            transform.position -= hit.transform.position;
        }
    }
}