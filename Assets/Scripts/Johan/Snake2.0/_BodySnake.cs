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
    private List<Vector3> positionHistory = new List<Vector3>();
    private float maxDistanceIndex = 5000;
    public float sineWaveSpeed = 3.5f;
    public float amplitude = 0.0005f;

    void Update()
    {
        if (moving)
        {
            positionHistory.Insert(0, transform.position);
            
            //limit the size of the position buffer
            if (positionHistory.Count > snakeBodyParts.Count * gap)
                positionHistory.RemoveAt(positionHistory.Count - 1);
            
            //Wiggle
            //Sine(sineWaveSpeed, amplitude);
        }
        
        // Move Body parts
        int Index = 0;
        foreach (var body in snakeBodyParts)
        {
            if (moving)
            {
                Vector3 point = positionHistory[Mathf.Min(Index * gap, positionHistory.Count - 1)];
                
                Vector3 moveDirection = point - body.transform.position;
                body.transform.rotation =  Quaternion.AngleAxis(0, Vector3.up);
                
                body.transform.position += Vector3.MoveTowards(body.transform.position, moveDirection * bodySpeed * Time.deltaTime, maxDistanceIndex);

                pointAt(point);
                
                
                
                Index++;
            }
        }
    }
    

    private void Sine(float speed, float Amplitude)
     {
         Vector3 pos = transform.position;
         pos.x = Mathf.Sin(Time.time * speed) * Amplitude;
         transform.position += transform.right * pos.x;
     }
    public void pointAt(Vector3 target)
    {
        transform.LookAt(target);
        Vector3 rot = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(rot);

    }
}

