using NUnit.Framework;
using System;
using System.Collections;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform waypointsParent;
    private Transform[] waypoints;
    private int currentWaypointIndex;
    public enum enemyState
    {
        Patrolling,
        Chasing,
        Returning,
        Trembling
    }
    private enemyState currentState;
    

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentState = enemyState.Patrolling;
        currentWaypointIndex = 0;

        if (waypointsParent != null)
        {
            waypoints = new Transform[waypointsParent.childCount];
            for (int i = 0; i < waypointsParent.childCount; i++)
            {
                waypoints[i] = waypointsParent.GetChild(i).transform;
            }
        }
        //Debug.Log(waypoints[0].ToString() + waypoints[1].ToString() + waypoints[2].ToString() + waypoints[3].ToString());
    }

    // Update is called once per frame
    void Update()
    {

        switch (currentState)
        {
            case enemyState.Patrolling:
                if (waypoints != null)
                {
                    Patrol();
                }
                break;
            case enemyState.Chasing:
                Chase();
                break;
            case enemyState.Trembling:
                StartCoroutine(BulletHitReaction());
                break;
        }
    }

    void Patrol()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
    }
    void Chase()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }

    public void TriggerBulletHitReaction()
    {
        //enemyState previousState = currentState;
        currentState = enemyState.Trembling;

    }

    public void TriggerChaseState()
    {
        currentState = enemyState.Chasing;
    }

    public void TriggerPatrolState()
    {
        currentState = enemyState.Patrolling;
    }

    private IEnumerator BulletHitReaction()
    {
        //Debug.Log("Trembling");
        float trembleDuration = 0.5f; // Total time to tremble
        float trembleSpeed = 30f;    // Speed of trembling
        float trembleAmount = 0.3f;  // Distance to move back and forth

        Vector3 originalPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < trembleDuration)
        {
            // Calculate a small offset
            float offset = Mathf.Sin(elapsedTime * trembleSpeed) * trembleAmount;

            // Apply the offset to the enemy's position
            transform.position = originalPosition + transform.forward * offset;

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Reset position and state
        transform.position = originalPosition;

    }
}
    
