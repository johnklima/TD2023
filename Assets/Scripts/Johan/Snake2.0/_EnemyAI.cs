using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class _EnemyAI : MonoBehaviour
{
    public float amplitude = 0.0005f;
    public float sineWaveSpeed = 3.5f;
    public NavMeshAgent navMeshagent;
    private Transform player;
    public LayerMask groundLayer, playerLayer;

    //Patroling
    private Vector3 curPoint;
    public Vector3 walkPoint;
    private bool walkPointSet;
    private Vector3 walkPointMiddle;
    public float walkPointRange;
    public float rotationSpeed;
    
    //Attacking
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    
    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //Animations
    public float headHeight;
    public float speedHeadTurn;
    public bool inRangeForAttack;
    
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        navMeshagent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInAttackRange && !playerInSightRange) Patroling();
        if (!playerInAttackRange && playerInSightRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) Attack();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
        
        

        if (walkPointSet)
        {
            //var currentDestination = EvaluateSlerpPoints(curPoint, walkPoint, SlerpCircleOffset);
            navMeshagent.SetDestination(transform.forward);
            Vector3 directionToLook = walkPoint - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToLook);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            var angle = Vector3.Angle(transform.forward, walkPoint);
            
            if (Mathf.Abs(angle) < 45)
            {
                navMeshagent.SetDestination(walkPoint);
            }
            
            float turnAmount = 0f;
            Vector3 dirToMove = (walkPoint - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, dirToMove);
        
            float angleToDir = Vector3.SignedAngle(transform.forward, dirToMove, Vector3.up);
        
            if (angleToDir > 0) turnAmount = 1f;
            else turnAmount = -1f;
            
            if (angleToDir > -45 || angleToDir < 45)
            
            Debug.Log(angleToDir);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        Sine(sineWaveSpeed, amplitude);
        
        //WalkPoint reached
        if (distanceToWalkPoint.magnitude < 1) walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        curPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        

            if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer)) walkPointSet = true;
    }
    private void ChasePlayer()
    {
        navMeshagent.SetDestination(player.position);
        Sine(sineWaveSpeed, amplitude);
    }
    private void Attack()
    {
        navMeshagent.SetDestination(player.position);
        transform.LookAt(player);
        AttackAnim();
        if (!alreadyAttacked)
        {
            //Attack code goes here:
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void AttackAnim()
    {
        Transform headTransform = transform.GetChild(1);
        headTransform.localPosition = Vector3.Lerp(headTransform.localPosition, new Vector3(0,headHeight,1), speedHeadTurn * Time.deltaTime);
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void Sine(float speed, float Amplitude)
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Sin(Time.time * speed) * Amplitude;
        transform.position += transform.right * pos.x;
    }

    IEnumerable<Vector3> EvaluateSlerpPoints(Vector3 start, Vector3 end, float offset)
    {
        var centerPivot = (start + end) * 0.5f;
        centerPivot -= new Vector3(0, -offset);

        var startRelativeCenter = start - centerPivot;
        var endRelativeCenter = end - centerPivot;

        var f = 1f / 10;

        for (var i = 0f; i < 1 + f; i += f)
        {
            yield return Vector3.Slerp(startRelativeCenter, endRelativeCenter, i) + centerPivot;
        }
    }
}