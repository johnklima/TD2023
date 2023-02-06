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

    // velicoty for our boid, obstacles to avoid + amount of obstalces avoided
    public Vector3 velocity;
    Vector3 avoidObst;
    float avoidCount;

    // variable to check if the player is in vicinity of a boids collider
    SphereCollider boidColl;
    bool pcInRange;
    float distance = 10f;
    [SerializeField] LayerMask playerLayer;
    Collider[] playerColl;
    [SerializeField] GameObject player;

    int damage = 5;

    Animator animator;
    Enemy batEnemy;
    string isFlying = "isFlying";
    string isAttacking = "isAttacking";
    bool flyBool;
    bool atkBool;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        batEnemy = GetComponent<Enemy>();
        // boidColl = gameObject.GetComponent<SphereCollider>();
        flock = transform.parent;
        constrainPoint = flock.position;

        // Vector3 pos = new Vector3(Random.Range(0f, 20f), Random.Range(0f, 20f), Random.Range(0f, 20f));
        // Vector3 look = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        // float speed = Random.Range(0f, 1f);

        // transform.position = pos;
        // transform.LookAt(look);
        // velocity = (look - pos) * speed;

        velocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
        pcInRange = Physics.CheckSphere(transform.position, distance, playerLayer);

        if (velocity != Vector3.zero && pcInRange)
        {
            flyBool = true;
            batEnemy.PlayAnim(animator, isFlying, flyBool);

            Debug.Log("Player in range");
            Vector3 newVelocity = new Vector3(0f, 0f, 0f);
            newVelocity += GoToPlayer();
            Vector3 slerpVelo = Vector3.Slerp(velocity, newVelocity, Time.deltaTime);
    
            velocity = slerpVelo.normalized;
    
            transform.position += velocity * Time.deltaTime * startSpeed;
            transform.LookAt(playerColl[0].transform.position + velocity);
        }

        else if (velocity != Vector3.zero && !pcInRange)
        {
            flyBool = true;
            batEnemy.PlayAnim(animator, isFlying, flyBool);

            Debug.Log("PC not in range");
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
            atkBool = true;
            batEnemy.PlayAnim(animator, isAttacking, atkBool);
            collider.gameObject.GetComponent<Player>().healthSystem.DealDamage(damage);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        pcInRange = false;
        atkBool = false;
        batEnemy.PlayAnim(animator, isAttacking, atkBool);
    }
}
