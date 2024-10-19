using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBehavior : MonoBehaviour
{
   public Transform pointA;         // First patrol point
    public Transform pointB;         // Second patrol point
    public float speed = 2.0f;       // Speed of the enemy
    public float detectionRange = 5.0f; // Range to detect the player
    public Transform player;          // Reference to the player

    private Vector3 target;           // Current target position
    private bool movingToPointA = true; // Is the enemy moving to point A?

    void Start()
    {
        // Start by setting the initial target to point A
        target = pointA.position;
    }

    void Update()
    {
        // Check for player detection
        DetectPlayer();

        // Move towards the target point
        MoveTowardsTarget();

        // Check if the enemy needs to switch targets
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            SwitchTarget();
        }
    }

    void DetectPlayer()
    {
        // Calculate distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // If the player is within detection range, chase them
        if (distanceToPlayer < detectionRange)
        {
            target = player.position;
        }
        else
        {
            // If the player is out of range, return to patrol
            target = movingToPointA ? pointA.position : pointB.position;
        }
    }

    void MoveTowardsTarget()
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void SwitchTarget()
    {
        // Switch the target point
        movingToPointA = !movingToPointA;
        target = movingToPointA ? pointA.position : pointB.position;
    }
} 
