using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BodySnake : MonoBehaviour
{
    public float moveSpeed = .1f;
    public float turningSpeed = 180f;
    public int gap = 280;
    public float bodySpeed = 5f;
    public List<GameObject> snakeBodyParts = new List<GameObject>();
    public bool isMoving = true;
    
    [SerializeField] List<Vector3> positionHistory = new List<Vector3>();
    
    private float maxDistanceIndex = 5000;
    public float sineWaveSpeed = 3.5f;
    public float amplitude = 0.0005f;
    public SnakeNavMesh NavMeshScript;
    public float bodySpeedValue = 60f;

    private void Start()
    {
        
        Transform trs;

        //build history and joint arrays
        trs = transform.GetChild(0);
        int safe = 0;
        while (trs!= null)
        {
            safe++;
            if (safe > 64)
                break;

            //add history points
            for(int i = 0; i < gap; i++)
                positionHistory.Insert(0, transform.position);

            //add gobjs to list
            snakeBodyParts.Insert(snakeBodyParts.Count, trs.gameObject);
            

            if (trs.childCount > 0)
                trs = trs.GetChild(0);
            else
                trs = null;

        }

        //continue
    }

    void FixedUpdate()
    {
        //if (isMoving)
        {
            positionHistory.Insert(0, transform.position);
            positionHistory.RemoveAt(positionHistory.Count - 1);
            bodySpeed = bodySpeedValue;            
        }

        // Move Body parts
        int Index = 0;
        foreach (var body in snakeBodyParts)
        {
            if (isMoving)
            {
                Vector3 point = positionHistory[Mathf.Min(Index * gap, positionHistory.Count - 1)];
                
                Vector3 moveDirection = point - body.transform.position;

                body.transform.position += Vector3.MoveTowards(body.transform.position, moveDirection * bodySpeed * Time.deltaTime, maxDistanceIndex);
                
                transform.LookAt(point);


                Vector3 rotBody = new Vector3(body.transform.rotation.eulerAngles.x, body.transform.rotation.eulerAngles.y, 0);
                body.transform.rotation = Quaternion.Euler(rotBody);
                
                Index++;
            }
        }
    }
    

}

