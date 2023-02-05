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
    private float amplitude = 0.005f;
    private float sineWaveSpeed = 3.5f;
    private NavMeshAgent navMeshagent;
    private Transform player;
    public LayerMask groundLayer, playerLayer;

    //Patroling
    private Vector3 curPoint;
    private Vector3 walkPoint;
    private bool walkPointSet;
    private Vector3 walkPointMiddle;
    public float walkPointRange;
    public float patrollingSpeed = 3.5f;
    public Transform backUpLocation;

    public float rotationSpeed = 500;
    //Attacking
    public float timeBetweenAttacks;
    public bool alreadyAttacked = false;
    public GameObject headTarget;
    public float attackingWalkingSpeed = 1f;
    private _BodySnake bodyScript;
    public float attackSpeed;
    
    //States
    public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;
    private Transform headOriginalPosition;
    public float HeadHeightOffset;
    private bool highHead = false;

    public float speed;
    //Chasing
    public float chasingSpeed;
    //Animations
    public float headLiftSpeed = 0.5f;
    
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        navMeshagent = GetComponent<NavMeshAgent>();
        bodyScript = GetComponent<_BodySnake>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInAttackRange && !playerInSightRange) Patroling();
        if (!playerInAttackRange && playerInSightRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange && !alreadyAttacked) Attack();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
        
        if (walkPointSet)
        {
            var angle = Vector3.Angle(transform.forward, walkPoint);
            
            if (Mathf.Abs(angle) > 90 || Mathf.Abs(angle) < 290 )
            {
                Quaternion toRotation = quaternion.LookRotation(walkPoint, Vector3.up);
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                navMeshagent.SetDestination(walkPoint);
                Debug.Log(Mathf.Abs(angle));
            }
            else
            {
                Quaternion toRotation = quaternion.LookRotation(backUpLocation.position, Vector3.up);
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
                navMeshagent.SetDestination(backUpLocation.position); 
                Vector3 distanceToBackUpWalkPoint = transform.position - backUpLocation.position;
                if (distanceToBackUpWalkPoint.magnitude > 1) SearchWalkPoint();
            }
            
            
            Vector3 lowHeadVector3 = new Vector3(headTarget.transform.localPosition.x, 0.3f, headTarget.transform.localPosition.z);

            headTarget.transform.localPosition = Vector3.Lerp(headTarget.transform.localPosition, lowHeadVector3, headLiftSpeed * Time.deltaTime);

            navMeshagent.speed = patrollingSpeed;
            speed = patrollingSpeed;
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


        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true;
        }
        if (highHead)
        {
            headTarget.transform.position = headOriginalPosition.transform.position;
        }
    }
    private void ChasePlayer()
    {
        Vector3 highHeadVector3 = new Vector3(headTarget.transform.localPosition.x, HeadHeightOffset, headTarget.transform.localPosition.z);
        navMeshagent.SetDestination(player.position);
        navMeshagent.speed = chasingSpeed;
        Sine(sineWaveSpeed, amplitude);
        
        headTarget.transform.localPosition = Vector3.Lerp(headTarget.transform.localPosition, highHeadVector3, headLiftSpeed * Time.deltaTime);

        speed = chasingSpeed;
    }
    private void Attack()
    {
        // navMeshagent.SetDestination(player.position);
        navMeshagent.speed = attackingWalkingSpeed;
        transform.LookAt(player);
        Transform headPosition = headTarget.transform;
       
        Vector3 attackStartHeadVector3 = new Vector3(headPosition.localPosition.x, 3f, 5f);
        headPosition.localPosition = attackStartHeadVector3;
        speed = attackSpeed;
        if (!alreadyAttacked)
        {
            //Attack code goes here:
            Vector3 attackHeadVector3 = new Vector3(headPosition.localPosition.x, 3f, 2f);
            headPosition.localPosition = attackHeadVector3;
            Patroling();
            
            AttackAnim();
            alreadyAttacked = true;
            
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void AttackAnim()
    {
        
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
        Patroling();
    }

    private void Sine(float speed, float Amplitude)
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Sin(Time.time * speed) * Amplitude;
        transform.position += transform.right * pos.x;
    }
}