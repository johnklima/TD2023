using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeIK : MonoBehaviour
{

    //things that move with the snake head, but are not children per se.
   
    public Transform snakeAgent;
    public Transform snakeHeadIK;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        snakeHeadIK.position = snakeAgent.position;
        snakeHeadIK.rotation = snakeAgent.rotation;

       
    }
}
