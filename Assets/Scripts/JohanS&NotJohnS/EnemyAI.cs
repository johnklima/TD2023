using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent navMeshagent;
    private Transform player;
    public LayerMask groundLayer, playerLayer;
    
    //Patroling
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;
    
    //Attacking
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    
    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

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

        if (walkPointSet) navMeshagent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
        //WalkPoint reached
        if (distanceToWalkPoint.magnitude < 1) walkPointSet = false; 
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer)) walkPointSet = true;
    }
    private void ChasePlayer()
    {
        navMeshagent.SetDestination(player.position);
    }
    private void Attack()
    {
        navMeshagent.SetDestination(player.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            //Attack code goes here:
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


}
