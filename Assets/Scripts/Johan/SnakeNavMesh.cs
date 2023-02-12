using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SnakeNavMesh : MonoBehaviour
{
    public float amplitude = 0.005f;
    public float sineWaveSpeed = 3.5f;
    private NavMeshAgent navMeshagent;
    public LayerMask groundLayer, playerLayer;

    public float speed;
    public float chasingSpeed;
    public Transform NMAtarget;
    public BodySnake bodyScript;

    private void Awake()
    {
       
        navMeshagent = GetComponent<NavMeshAgent>();
        
        
    }

    private void Start()
    {
        navMeshagent.SetDestination(NMAtarget.position);
    }

    private void Update()
    {      
        
        if ( Chase() )     
            Sine(sineWaveSpeed, amplitude);
    }

    
    private bool Chase()
    {

        navMeshagent.SetDestination(NMAtarget.position);

        if (navMeshagent.remainingDistance < 1.0f)
        {
            bodyScript.isMoving = false;
            navMeshagent.speed = 0;
            speed = 0;
            return false;
        }
        else        
        {
            bodyScript.isMoving = true;
            navMeshagent.speed = chasingSpeed;
            speed = chasingSpeed;
            return true;
        }
        


    }
    private void Sine(float speed, float Amplitude)
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Sin(Time.time * speed) * Amplitude;
        transform.position += transform.right * pos.x;
    }

}