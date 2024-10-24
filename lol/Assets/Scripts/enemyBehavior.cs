using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBehavior : MonoBehaviour
{
    public LayerMask groundLayer;
    public float jumpForce;
    private bool wallFound;
    public Transform pointA;         // First patrol point
    public Transform pointB;         // Second patrol point
    public float speed = 2.0f;       // Speed of the enemy
    public float detectionRange = 5.0f; // Range to detect the player
    public Transform player;          // Reference to the player

    private Vector3 target;           // Current target position
    private bool movingToPointA = true; // Is the enemy moving to point A?
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool jump = false;
    public float enemyDirection;
    public GameObject RayObject;
    [SerializeField] float RayDistance;
    public LayerMask layerMask;

    void Start()
    {
        // Start by setting the initial target to point A
        target = pointA.position;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyDirection = 0f;
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
        ObstacleDetect();
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
        //flip enemy sprite direction based on the target position
        enemyDirection = target.x - transform.position.x;
        spriteRenderer.flipX = enemyDirection < 0; // Face left or right based on enemy direction

        
        // Move towards the target position
        transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
    }

    void SwitchTarget()
    {
        movingToPointA = !movingToPointA;
        target = movingToPointA ? pointA.position : pointB.position;
    }

   
    void ObstacleDetect()
    {
        //check if there is an obstacle infront of the enemy with a raycast
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right * new Vector2(enemyDirection, 0f), RayDistance, layerMask);
        Debug.DrawRay (transform.position, Vector2.right * new Vector2(enemyDirection, 0f), Color.red);

        //if the enemy direction changes then change the raycast direction also
        if (enemyDirection!= hitRight.transform.position.x - transform.position.x)
        {
            enemyDirection = hitRight.transform.position.x - transform.position.x;
        }

        // if there is an obstacle, jump over it
        if (hitRight)
        {
            rb.AddForce (Vector2.up * jumpForce);
        }
    }
} 
