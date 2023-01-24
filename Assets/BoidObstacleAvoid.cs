using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BoidObstacleAvoid : MonoBehaviour
{

    public Boids boids;
    public Transform boid;

    private void Start()
    {
        boids = GetComponent<Boids>();
        boid = transform.parent;
    }

    private void OnTriggerStay(Collider other)
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 6; //ground

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        //layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(boid.position, boid.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(boid.position, boid.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(boid.position, boid.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }

}

