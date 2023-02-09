using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeIK : MonoBehaviour
{

    //things that move with the snake head, but are not children per se.
    public Transform IK;    
    public Transform targetHolder;

    //the snake agent
    public Transform snakeAgent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IK.position = snakeAgent.position;
        IK.rotation = snakeAgent.rotation;

        targetHolder.position = snakeAgent.position;
        targetHolder.rotation = snakeAgent.rotation;
    }
}
