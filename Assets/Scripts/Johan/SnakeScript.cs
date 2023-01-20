using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;       //<JPK> why?
using UnityEngine;

public class SnakeScript : MonoBehaviour
{
    public float moveSpeed = .1f;
    public float turningSpeed = 180f;
    public GameObject snakeBody;
    public int gap = 280;
    public float bodySpeed = 5f;
    private List<GameObject> snakeBodyParts = new List<GameObject>();
    private bool moving = false;
    public List<Vector3> positionHistory = new List<Vector3>();
    private float maxDistanceIndex = 5000;
    public float sineWaveSpeed = 3.5f;
    public float amplitude = 0.0001f;
    // Start is called before the first frame update
    void Start()
    {
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
        GrowSnake();
    }

    // Update is called once per frame
    void Update()
    {
        //Move Forward
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            moving = true;
        }
        else
        {
            moving = false;
        }

        //Turning
        float turningDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, turningDirection * turningSpeed * Time.deltaTime);
        
        //Store position history
        if (moving)
        {
            positionHistory.Insert(0, transform.position);

            if (positionHistory.Count > snakeBodyParts.Count * gap )
                positionHistory.RemoveAt(positionHistory.Count - 1);

            //Wiggle
            Sine(sineWaveSpeed, amplitude);
        }
        
        // Move Body parts
        int Index = 0;
        foreach (var body in snakeBodyParts)
        {
            if (moving)
            {
                Vector3 point = positionHistory[Mathf.Min(Index * gap, positionHistory.Count - 1)];
                Vector3 moveDirection = point - body.transform.position;
               
                body.transform.position += Vector3.MoveTowards(body.transform.position, moveDirection * bodySpeed * Time.deltaTime, maxDistanceIndex);
                
                body.transform.LookAt(point); 
                
                Index++;
            }
        }
    }
    //Add bodyparts to the snake
    private void GrowSnake()
    {
        GameObject body = Instantiate(snakeBody, transform.position, transform.rotation);
        snakeBodyParts.Add(body);
    }
    //Trigger GrowSnake
     void OnTriggerEnter(Collider other) 
     {
        if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            GrowSnake();
        }
     }
    //Sinewave movement
     private void Sine(float speed, float Amplitude)
     {
         Vector3 pos = transform.position;
         pos.x = Mathf.Sin(Time.time * speed) * Amplitude;
         transform.position += transform.right * pos.x;
     }
}
