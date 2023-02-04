using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpore : MonoBehaviour
{
    // player parent + all the "importance" values to determine how important one behaviour is over another
    [SerializeField] Transform player;
    [SerializeField] float cohesionFactor = 0.2f;
    [SerializeField] float separationFactor = 6.0f;
    [SerializeField] float alignFactor = 1.0f;
    [SerializeField] float constrainFactor = 2.0f;
    [SerializeField] float avoidFactor = 2.0f;
    [SerializeField] float collisionDistance = 6.0f;
    private float speed = 1.5f;
    Vector3 constrainPoint;

    Transform flockparent;

    // velicoty for our boid, obstacles to avoid + amount of obstalces avoided
    public Vector3 velocity;
    Vector3 avoidObst;
    float avoidCount;

    // variables for checking for plant stations
    bool inStationRange;
    float distance = 3f;
    [SerializeField] LayerMask stationLayer;
    Collider[] stationColl;
    
    // Start is called before the first frame update
    void Start()
    {
        flockparent = transform.parent;

        velocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
        constrainPoint = flockparent.position;
        inStationRange = Physics.CheckSphere(transform.position, distance, stationLayer);

        if (inStationRange)
        {
            Debug.Log("Spore in range of station");
            
            Vector3 newVelocity = new Vector3(0f, 0f, 0f);
            newVelocity += GoToStation();
            Vector3 slerpVelo = Vector3.Slerp(velocity, newVelocity, Time.deltaTime);
    
            velocity = slerpVelo.normalized;
    
            transform.position += velocity * Time.deltaTime * speed;
        }

        else if (!inStationRange)
        {

            Debug.Log("Spore NOT in range of station");
            Vector3 newVelocity = new Vector3(0, 0, 0);
            
            // rule 1 all boids steer towards center of mass - cohesion
            newVelocity += cohesion() * cohesionFactor;
    
            // rule 2 all boids steer away from each other - avoidance        
            newVelocity += separation() * separationFactor;
    
            // rule 3 all boids match velocity - allignment
            newVelocity += align() * alignFactor;
    
            newVelocity += constrain() * constrainFactor;

            newVelocity += avoid() * avoidFactor;
           
            Vector3 slerpVelo = Vector3.Slerp(velocity, newVelocity, Time.deltaTime);
    
            velocity = slerpVelo.normalized;
    
            transform.position += velocity * Time.deltaTime * speed;
            transform.LookAt(transform.position + velocity);
        }
    }
    
    Vector3 constrain()
    {
        Vector3 steer = new Vector3(0, 0, 0);

        steer += (constrainPoint - transform.position);

        steer.Normalize();

        return steer;
    }
    Vector3 avoid()
    {

        if (avoidCount > 0)
        {
            return (avoidObst / avoidCount).normalized ;
        }

        return Vector3.zero;
    }

    Vector3 cohesion()
    {
        Vector3 steer = new Vector3(0, 0, 0);

        int sibs = 0;           //count the boids, it might change

        foreach (Transform boid in flockparent)
        {
            if (boid != transform)
            {
                steer += boid.transform.position;
                sibs++;
            }

        }

        steer /= sibs; //center of mass is the average position of all        

        steer -= transform.position;

        steer.Normalize();


        return steer;
    }

    Vector3 separation()
    {
        Vector3 steer = new Vector3(0, 0, 0);

        int sibs = 0;


        foreach (Transform boid in flockparent)
        {
            // if boid is not itself
            if (boid != transform)
            {
                // if this boids position is within the collision distance of a neighbouring boid
                if ((transform.position - boid.transform.position).magnitude < collisionDistance)
                {
                    // our vector becomes this boids pos - neighbouring boids pos
                    steer += (transform.position - boid.transform.position);
                    sibs++;
                }

            }

        }
        steer /= sibs;
        steer.Normalize();        //unit, just direction
        return steer;

    }

    Vector3 align()
    {
        Vector3 steer = new Vector3(0, 0, 0);
        int sibs = 0;

        foreach (Transform boid in flockparent)
        {
            if (boid != transform)
            {
                steer += boid.GetComponent<BoidSpore>().velocity;
                sibs++;
            }

        }
        steer /= sibs;

        steer.Normalize();

        return steer;
    }
    public void accumAvoid(Vector3 avoid)
    {
        avoidObst += transform.position - avoid;
        avoidCount++;

    }
    public void resetAvoid()
    {
        avoidCount = 0;
        avoidObst *= 0;
    }

    Vector3 GoToStation()
    {
        Vector3 steer = new Vector3(0, 0, 0);

        stationColl = Physics.OverlapSphere(transform.position, distance, stationLayer);

        for(int i = 0; i < stationColl.Length; i++)
        {
          steer += (stationColl[i].transform.position - transform.position);
        }


        steer.Normalize();

        return steer;
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Interactable")
        {
            velocity = new Vector3 (0f, 0f, 0f);
            gameObject.SetActive(false);
        }
    }
}
