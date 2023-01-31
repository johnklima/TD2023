using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BoidObstacleAvoid : MonoBehaviour
{

    private Boids boids;
    private Transform boid;

    public float distanceDetect = 25f;

    private void Start()
    {

        boid = transform.parent;
        boids = boid.GetComponent<Boids>();
    }
    private void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {

        avoid();

    }
    void avoid()
    {

       
        int avoidMask = 1 << 6;

        bool didHit = false;
        RaycastHit hit;
        // Does the ray intersect any objects in the layer mask
        if (Physics.Raycast(boid.position, boid.forward, out hit, distanceDetect, avoidMask))
        {
            Debug.DrawRay(boid.position, boid.forward * hit.distance, Color.red);

            boids.accumAvoid(hit.point);

            didHit = true;
        }
        if (Physics.Raycast(boid.position, -boid.up, out hit, distanceDetect, avoidMask))
        {
            Debug.DrawRay(boid.position, -boid.up * hit.distance, Color.red);

            boids.accumAvoid(hit.point);

            didHit = true;
        }
        if (Physics.Raycast(boid.position, boid.up, out hit, distanceDetect, avoidMask))
        {
            Debug.DrawRay(boid.position, boid.up * hit.distance, Color.red);

            boids.accumAvoid(hit.point);

            didHit = true;
        }
        if (Physics.Raycast(boid.position, boid.right, out hit, distanceDetect, avoidMask))
        {
            Debug.DrawRay(boid.position, boid.right * hit.distance, Color.red);

            boids.accumAvoid(hit.point);

            didHit = true;
        }
        if (Physics.Raycast(boid.position, -boid.right, out hit, distanceDetect, avoidMask))
        {
            Debug.DrawRay(boid.position, -boid.right * hit.distance, Color.red);

            boids.accumAvoid(hit.point);

            didHit = true;
        }
        if(didHit)
            Debug.Log("DidHit " + boid.name);
        if (!didHit)
            boids.resetAvoid();
        
    }
    private void OnTriggerExit(Collider other)
    {
        boids.resetAvoid();
    }
}

