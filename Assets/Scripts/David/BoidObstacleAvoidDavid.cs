using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BoidObstacleAvoidDavid : MonoBehaviour
{

    private BoidsDavid boids;
    private Transform boid;

    [SerializeField] LayerMask avoidMask;

    private void Start()
    {
        boid = transform.parent;
        boids = boid.GetComponent<BoidsDavid>();
    }


    private void Update()
    {
        bool didHit = false;
        RaycastHit hit;
        // Does the ray intersect any objects in the layer mask
        if (Physics.Raycast(boid.position, boid.forward, out hit, 10, avoidMask))
        {
            Debug.DrawRay(boid.position, boid.forward * hit.distance, Color.red);

            boids.accumAvoid(hit.point);

            didHit = true;
        }
        if (Physics.Raycast(boid.position, -boid.up, out hit, 10, avoidMask))
        {
            Debug.DrawRay(boid.position, -boid.up * hit.distance, Color.red);

            boids.accumAvoid(hit.point);

            didHit = true;
        }
        if (Physics.Raycast(boid.position, boid.up, out hit, 10, avoidMask))
        {
            Debug.DrawRay(boid.position, boid.up * hit.distance, Color.red);

            boids.accumAvoid(hit.point);

            didHit = true;
        }
        if (Physics.Raycast(boid.position, boid.right, out hit, 10, avoidMask))
        {
            Debug.DrawRay(boid.position, boid.right * hit.distance, Color.red);

            boids.accumAvoid(hit.point);

            didHit = true;
        }
        if (Physics.Raycast(boid.position, -boid.right, out hit, 10, avoidMask))
        {
            Debug.DrawRay(boid.position, -boid.right * hit.distance, Color.red);

            boids.accumAvoid(hit.point);

            didHit = true;
        }

        if (!didHit)
            boids.resetAvoid();
    }
    private void OnTriggerExit(Collider other)
    {
        boids.resetAvoid();
    }
}

