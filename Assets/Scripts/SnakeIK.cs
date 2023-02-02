using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeIK : MonoBehaviour
{

    //things that move with the snake head, but are not children per se.
    public Transform IK;    
    public Transform targetHolder;

    //the snake head
    public Transform snakeHead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IK.position = snakeHead.position;
        IK.rotation = snakeHead.rotation;

        targetHolder.position = snakeHead.position;
        targetHolder.rotation = snakeHead.rotation;
    }
}
