using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsDavid : MonoBehaviour
{
    // flock parent + all the "importance" values to determine how important one behaviour is over another
    [SerializeField] Transform flock;
    [SerializeField] float cohesionFactor = 0.2f;
    [SerializeField] float separationFactor = 6.0f;
    [SerializeField] float allignFactor = 1.0f;
    [SerializeField] float constrainFactor = 2.0f;
    [SerializeField] float avoidFactor = 2.0f;

    [SerializeField] float collisionDistance = 6.0f;
    private float maxSpeed = 10f;
    private float startSpeed = 0.5f;
    Vector3 constrainPoint;

    // animator component
    Animator batController;
    bool canFly;

    // velicoty for our boid, obstacles to avoid + amount of obstalces avoided
    public Vector3 velocity;
    Vector3 avoidObst;
    float avoidCount;

    // variable to check if the player is in vicinity of a boids collider
    bool pcInRange;
    float distance = 5f;
    [SerializeField] LayerMask playerLayer;
    Collider[] playerColl;
    [SerializeField] GameObject player;

    int batHealthNum = 1;
    int damage = 5;
    HealthSystem batHealthSyst;
    
    // Start is called before the first frame update
    void Start()
    {
        flock = transform.parent;
        constrainPoint = flock.position;

        batController = GetComponent<Animator>();

        velocity = new Vector3(0, 0, 0);

        batHealthSyst = new HealthSystem(batHealthNum);
    }

    // Update is called once per frame
    void Update()
    {
        
        pcInRange = Physics.CheckSphere(transform.position, distance, playerLayer);

        if (velocity != Vector3.zero && pcInRange)
        {
            canFly = true;
            Vector3 newVelocity = new Vector3(0f, 0f, 0f);
            newVelocity += GoToPlayer();
            Vector3 slerpVelo = Vector3.Slerp(velocity, newVelocity, Time.deltaTime);
    
            velocity = slerpVelo.normalized;
    
            transform.position += velocity * Time.deltaTime * startSpeed;
            transform.LookAt(playerColl[0].transform.position + velocity);
        }

        else if (velocity != Vector3.zero && !pcInRange)
        {
            canFly = true;
            Vector3 newVelocity = new Vector3(0, 0, 0);
            
            // rule 1 all boids steer towards center of mass - cohesion
            newVelocity += cohesion() * cohesionFactor;
    
            // rule 2 all boids steer away from each other - avoidance        
            newVelocity += separation() * separationFactor;
    
            // rule 3 all boids match velocity - allignment
            newVelocity += align() * allignFactor;
    
            newVelocity += constrain() * constrainFactor;
    
            newVelocity += avoid() * avoidFactor;
           
            Vector3 slerpVelo = Vector3.Slerp(velocity, newVelocity, Time.deltaTime);
    
            velocity = slerpVelo.normalized;
    
            transform.position += velocity * Time.deltaTime * startSpeed;
            transform.LookAt(transform.position + velocity);
        }
        if (velocity != Vector3.zero)
        {
            startSpeed += maxSpeed * Time.deltaTime;
            if (startSpeed >= maxSpeed)
            {
                startSpeed = maxSpeed;
            }
        }
        if (canFly)
        {
            batController.SetBool("isFlying", true);
        }
    }
    Vector3 avoid()
    {

        if (avoidCount > 0)
        {
            return (avoidObst / avoidCount).normalized ;
        }

        return Vector3.zero;
    }
    
    Vector3 constrain()
    {
        Vector3 steer = new Vector3(0, 0, 0);

        steer += (constrainPoint - transform.position);

        steer.Normalize();

        return steer;
    }

    Vector3 cohesion()
    {
        Vector3 steer = new Vector3(0, 0, 0);

        int sibs = 0;           //count the boids, it might change

        foreach (Transform boid in flock)
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


        foreach (Transform boid in flock)
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

        foreach (Transform boid in flock)
        {
            if (boid != transform)
            {
                steer += boid.GetComponent<BoidsDavid>().velocity;
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

    Vector3 GoToPlayer()
    {
        Vector3 steer = new Vector3(0, 0, 0);

        playerColl = Physics.OverlapSphere(transform.position, distance, playerLayer);

        steer += (playerColl[0].transform.position - transform.position);

        steer.Normalize();

        return steer;
    }

    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.GetComponent<Player>() != null)
        {
            batController.SetBool("isAttacking", true);
            collider.gameObject.GetComponent<Player>().healthSystem.DealDamage(damage);
        }
        if (collider.gameObject.GetComponent<Mycelium>() != null)
        {
            // change condition to component tag later
            // currently not working
            transform.position += new Vector3(0, -1f, 0);
            batController.SetBool("isDead", true);

            float timer = 0f;
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                gameObject.SetActive(false);
                timer -= Time.deltaTime;
                timer = 0f;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        pcInRange = false;
        batController.SetBool("isAttacking", false);
    }
}
