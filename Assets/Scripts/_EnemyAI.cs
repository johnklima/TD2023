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
    public float amplitude = 0.005f;
    public float sineWaveSpeed = 3.5f;
    private NavMeshAgent navMeshagent;
    public GameObject player;
    public LayerMask groundLayer, playerLayer;

    //Patroling
    private Vector3 curPoint;
    private Vector3 walkPoint;
    private bool walkPointSet;
    private Vector3 walkPointMiddle;
    public float walkPointRange;
    public float patrollingSpeed = 3.5f;
    public Transform backUpLocation;
    private Transform startspawn;
    public float rotationSpeed = 500;
    public float rayHeightWalkPointSearch;
    public Vector3 distanceToWalkpoint;

    public Vector3 normalHeightBody;
    //Attacking
    public float timeBetweenAttacks;
    public bool alreadyAttacked = false;
    public GameObject headTarget;
    public float attackingWalkingSpeed = 1f;
    private _BodySnake bodyScript;
    public float attackSpeed;
    public int damage = 5;

    private Vector3 bodyHeightChasing;

    private Vector3 bodyHeightAttacking;
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
    //Life


    public FMODUnity.StudioEventEmitter Music;

    public Transform navTarget;

    private void Awake()
    {
        // player = GameObject.Find("Player").transform;
        navMeshagent = GetComponent<NavMeshAgent>();
        bodyScript = GetComponent<_BodySnake>();
        
    }

    private void Start()
    {
        startspawn= this.transform;
        normalHeightBody = new Vector3(transform.GetChild(0).transform.localPosition.x,
            transform.GetChild(0).transform.localPosition.y, transform.GetChild(0).transform.localPosition.z);
        bodyHeightChasing = new Vector3(normalHeightBody.x, normalHeightBody.y + 1f, normalHeightBody.z);
        bodyHeightAttacking = new Vector3(bodyHeightChasing.x,
            bodyHeightChasing.y, bodyHeightChasing.z + 1);
    }

    private void Update()
    {

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (playerInSightRange)
        {
            Music.SetParameter("Combat", 1.0f);
        }

        if (!playerInAttackRange && !playerInSightRange)
        {
            Patroling();
            Music.SetParameter("Combat", 0);
        }
        if (!playerInAttackRange && playerInSightRange && !alreadyAttacked) ChasePlayer();
        if (playerInAttackRange && playerInSightRange && !alreadyAttacked) Attack();
        if (playerInAttackRange && playerInSightRange && alreadyAttacked) Patroling();
        if (!playerInAttackRange && playerInSightRange && alreadyAttacked) Patroling();
    }

    private void Patroling()
    {

        if (!walkPointSet) 
        {
            SearchWalkPoint();
            if (!walkPointSet)
                return;
        }
       
        
        if (walkPointSet)
        {
            
            Quaternion toRotation = quaternion.LookRotation(walkPoint, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            navMeshagent.SetDestination(walkPoint);

            transform.GetChild(0).transform.localPosition = Vector3.Lerp(transform.GetChild(0).transform.localPosition, normalHeightBody, headLiftSpeed * Time.deltaTime);
            Vector3 lowHeadVector3 = new Vector3(headTarget.transform.localPosition.x, 0.3f, headTarget.transform.localPosition.z);

            headTarget.transform.localPosition = Vector3.Lerp(headTarget.transform.localPosition, lowHeadVector3, headLiftSpeed * Time.deltaTime);

            navMeshagent.speed = patrollingSpeed;
            speed = patrollingSpeed;
        }

        distanceToWalkpoint = transform.position - walkPoint;
        Sine(sineWaveSpeed, amplitude);
        
        //WalkPoint reached
        if (distanceToWalkpoint.magnitude < 5) walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        curPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        walkPoint = new Vector3(startspawn.position.x + randomX, startspawn.position.y , startspawn.position.z + randomZ);

        RaycastHit hit;
        if (Physics.Raycast(walkPoint, transform.up, out hit, rayHeightWalkPointSearch, groundLayer))
        {
            walkPoint = hit.point;
            walkPointSet = true;
        }
        if (Physics.Raycast(walkPoint, -transform.up, out hit, rayHeightWalkPointSearch, groundLayer))
        {
            walkPoint = hit.point;
            walkPointSet = true;
        }

        
        if (highHead)
        {
            headTarget.transform.position = headOriginalPosition.transform.position;
        }

        StartCoroutine(CheckIfStopped());
        
       
    }
    private void ChasePlayer()
    {
        
        Vector3 highHeadVector3 = new Vector3(headTarget.transform.localPosition.x, HeadHeightOffset, headTarget.transform.localPosition.z);
        navMeshagent.SetDestination(player.transform.position);
        navMeshagent.speed = chasingSpeed;
        Sine(sineWaveSpeed, amplitude);
        transform.GetChild(0).transform.localPosition = Vector3.Lerp(transform.GetChild(0).transform.localPosition, bodyHeightChasing, headLiftSpeed * Time.deltaTime);
        headTarget.transform.localPosition = Vector3.Lerp(headTarget.transform.localPosition, highHeadVector3, headLiftSpeed * Time.deltaTime);

        speed = chasingSpeed;

        

    }
    private void Attack()
    {
        transform.GetChild(0).transform.localPosition = Vector3.Lerp(transform.GetChild(0).transform.localPosition, bodyHeightAttacking, headLiftSpeed * Time.deltaTime);
        navMeshagent.SetDestination(player.transform.position);
        navMeshagent.speed = attackingWalkingSpeed;
        //transform.LookAt(player);
        Transform headPosition = headTarget.transform;
        // Sine(sineWaveSpeed, amplitude);
        Vector3 attackStartHeadVector3 = new Vector3(headPosition.localPosition.x, 3f, 5f);
        headPosition.localPosition = attackStartHeadVector3;
        speed = attackSpeed;
    }

    private void FleeAfterAttack()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        
        navMeshagent.SetDestination(walkPoint);
        speed = attackSpeed;
        Sine(sineWaveSpeed, amplitude);
    }

    private void AttackAnim()
    {
        
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
        Patroling();
    }

    private IEnumerator CheckIfStopped()
    {
        yield return new WaitForSeconds(5f);
        var firstPosition = transform.position.x + transform.position.z;
        yield return new WaitForSeconds(5f);
        var secondPosition = transform.position.x + transform.position.z;
        //Debug.Log(firstPosition - secondPosition);  83,000 debug log messages
        if (firstPosition - secondPosition < 30f)
        {
            SearchWalkPoint();
            Debug.Log("It should change");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            Debug.Log("ATTACK !!!!!");
            other.gameObject.GetComponent<Player>().healthSystem.DealDamage(damage);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            walkPointSet = false;
            Patroling();
        }
    }

    private void Sine(float speed, float Amplitude)
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Sin(Time.time * speed) * Amplitude;
        transform.position += transform.right * pos.x;
    }
}