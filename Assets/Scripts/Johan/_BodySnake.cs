using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class _BodySnake : MonoBehaviour
{
    public float moveSpeed = .1f;
    public float turningSpeed = 180f;
    public int gap = 280;
    public float bodySpeed = 5f;
    public List<GameObject> snakeBodyParts = new List<GameObject>();
    private bool moving = true;
    
    [SerializeField] List<Vector3> positionHistory = new List<Vector3>();
    
    private float maxDistanceIndex = 5000;
    public float sineWaveSpeed = 3.5f;
    public float amplitude = 0.0005f;
    //public List<Transform> bodyTransforms = new List<Transform>();
    public _EnemyAI enemyAIScript;
    public float bodySpeedValue = 60f;

    private void Start()
    {
        enemyAIScript = FindObjectOfType<_EnemyAI>();
        Transform trs;

        trs = transform.GetChild(0);
        int safe = 0;
        while (trs!= null)
        {
            safe++;
            if (safe > 64)
                break;

            Debug.Log("Add to History");
            for(int i = 0; i < gap; i++)
                positionHistory.Insert(0, transform.position);

            if (trs.childCount > 0)
                trs = trs.GetChild(0);
            else
                trs = null;

        }

        //continue
    }

    void FixedUpdate()
    {
        if (moving)
        {
            positionHistory.Insert(0, transform.position);
            
            //limit the size of the position buffer
            if (positionHistory.Count > snakeBodyParts.Count * gap)
                positionHistory.RemoveAt(positionHistory.Count - 1);
            if (enemyAIScript.distanceToWalkpoint.magnitude < 5f)
            {
                if (enemyAIScript.distanceToWalkpoint.magnitude > 0)
                    bodySpeed = bodySpeedValue / enemyAIScript.distanceToWalkpoint.magnitude;
            }
            if (enemyAIScript.distanceToWalkpoint.magnitude > 5f)
            {
                bodySpeed = bodySpeedValue;
            }
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
                
                transform.LookAt(point);
                Vector3 rotBody = new Vector3(body.transform.rotation.eulerAngles.x, body.transform.rotation.eulerAngles.y, 0);
                body.transform.rotation = Quaternion.Euler(rotBody);
                
                Index++;
            }
        }
    }
    

}

