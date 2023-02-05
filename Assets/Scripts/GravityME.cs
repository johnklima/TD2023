using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityME : MonoBehaviour
{
    //gravity in meters per second per second
    public float GRAVITY_CONSTANT = -9.8f;                      // -- for earth,  -1.6 for moon 

    public Vector3 velocity = new Vector3(0, 0, 0);             //current direction and speed of movement
    public Vector3 acceleration = new Vector3(0, 0, 0);         //movement controlled by player movement force and gravity

    public Vector3 thrust = new Vector3(0, 0, 0);               //player applied thrust vector
    public Vector3 finalForce = new Vector3(0, 0, 0);           //final force to be applied this frame

    public float mass = 1.0f;

    public float height = 0;
    
    public float timeScalar = 1.0f;
    
    public Vector3 impulse = new Vector3(0, 0, 0);
    public Vector3 curPos = new Vector3(0, 0, 0); //begin position

    public bool inAir = false;

    // Update is called once per frame
    void Update()
    {
        handleMovement();
    }

    void handleMovement()
    {
        //set the rate of integration, which is (almost) equivalent to
        //explosion by mass for impulse calc. problem is, gravity
        //is no longer a constant. but for gameplay, maybe not an issue?
        float forceDeltaTime = Time.deltaTime * timeScalar; 
        
        //reset final force to the initial force of gravity
        finalForce.Set(0, GRAVITY_CONSTANT * mass, 0);
        finalForce += thrust;

        acceleration = finalForce / mass;        
        velocity += acceleration * forceDeltaTime;
        velocity += impulse;

        //move the object
        transform.position += velocity * forceDeltaTime;

        //this is only useful on a flat surface
        //handle with collision box or raycast to ground
        if (transform.position.y < height)
        {
            transform.position = curPos;       //hard reset to the surface
            acceleration *= 0;
            velocity *= 0;
            inAir = false;
            gameObject.SetActive(false);
        }

        //reset impulse
        impulse *= 0;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>() != null) //gonna need some "Or's" here, LayerMask? 
        {
            Debug.Log("ball hit " + other.name);
            other.GetComponent<Enemy>().healthSystem.DealDamage(1);
            reset();
        }
    }
    public void reset()
    {
        velocity *= 0;
        acceleration *= 0;
        impulse *= 0;
        thrust *= 0;
        
        inAir = false;
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}
