using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;       //<JPK> why?
using UnityEngine;
using UnityEngine.AI;

public class SnakeScriptKlima : MonoBehaviour
{
    public float moveSpeed = .1f;
    public float turningSpeed = 180f;
    public GameObject snakeBody;
    public int gap = 280;
    public int segments = 16;
    public float bodySpeed = 5f;
    private List<GameObject> snakeBodyParts = new List<GameObject>();
    private bool moving = false;
    public List<Vector3> positionHistory = new List<Vector3>();
    private float maxDistanceIndex = 5000;
    public float sineWaveSpeed = 3.5f;
    public float amplitude = 0.0001f;
    private NavMeshAgent agent;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        for (int i = 0; i < segments; i++)
            GrowSnake();

    }

    // Update is called once per frame
    void Update()
    {

        agent.SetDestination(target.position);

        /*
        
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
        */

        if (agent.remainingDistance > 1)
            moving = true;
        else
            moving = false;                

        if (moving)
        {
            positionHistory.Insert(0, transform.position);

            //limit the size of the position buffer
            if (positionHistory.Count > snakeBodyParts.Count * gap)
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
        //add as child of snake head, for organizational reasons
        GameObject body = Instantiate(snakeBody, transform.position, transform.rotation, transform.parent);
        snakeBodyParts.Add(body);

        //make sure there is a position history for each link, by gap padding
        for (int i = 0; i < gap; i++)
            positionHistory.Insert(0, transform.position);

    }
    //Trigger GrowSnake
    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Food"))
    //     {
    //         Destroy(other.gameObject);
    //         GrowSnake();
    //     }
    // }
    //Sinewave movement
    private void Sine(float speed, float Amplitude)
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Sin(Time.time * speed) * Amplitude;
        transform.position += transform.right * pos.x;
    }
}