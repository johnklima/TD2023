using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{

    //gravity in meters per second per second
    public float GRAVITY_CONSTANT = -9.8f;                      // -- for earth,  -1.6 for moon 

    public Vector3 velocity = new Vector3(0, 0, 0);             //current direction and speed of movement
    public Vector3 acceleration = new Vector3(0, 0, 0);         //movement controlled by player movement force and gravity

    public Vector3 thrust = new Vector3(0, 0, 0);               //player applied thrust vector
    public Vector3 finalForce = new Vector3(0, 0, 0);           //final force to be applied this frame

    public float mass = 1.0f;

    public float height = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
    }

    void handleMovement()
    {

        Vector3 curPos = transform.position;  //begin position

        //reset final force to the initial force of gravity
        finalForce.Set(0, GRAVITY_CONSTANT * mass, 0);
        finalForce += thrust;

        acceleration = finalForce / mass;
        velocity += acceleration * Time.deltaTime;

        //move the object
        transform.position += velocity * Time.deltaTime;

        if (transform.position.y < height)
        {
            transform.position = curPos;       //hard reset to the surface
            acceleration *= 0;
            velocity *= 0;


        }
            

        //reset thrust
        thrust.Set(0, 0, 0);


    }

}
