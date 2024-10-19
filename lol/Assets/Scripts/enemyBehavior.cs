using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBehavior : MonoBehaviour
{
   public Transform pointA;         
    public Transform pointB;        
    public float speed = 2.0f;      
    public float detectionRange = 5.0f;
    public Transform player;        

    private Vector3 target;           
    private bool movingToPointA = true; 

    void Start()
    {
        target = pointA.position;
    }

    void Update()
    {
        DetectPlayer();

        MoveTowardsTarget();

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            SwitchTarget();
        }
    }

    void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer < detectionRange)
        {
            target = player.position;
        }
        else
        {
            target = movingToPointA ? pointA.position : pointB.position;
        }
    }

    void MoveTowardsTarget()
    {
        // Move towards the target positio        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void SwitchTarget()
    {
        movingToPointA = !movingToPointA;
        target = movingToPointA ? pointA.position : pointB.position;
    }
} 
