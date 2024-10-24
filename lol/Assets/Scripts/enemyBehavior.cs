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
        //Flip direction
        if (speed > 0)
        {
            spriteRenderer.flipX = false; // Face right
            enemyDirection = 1f;
        }
        else if (speed < 0)
        {
            spriteRenderer.flipX = true; // Face left
            enemyDirection = -1f;
        }
        else 
        {
            enemyDirection = 0;
        }
        
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
        RaycastHit2D hitRight = Physics2D.Raycast(RayObject.transform.position, Vector2.right * new Vector2(enemyDirection, 0f), RayDistance, layerMask);
        Debug.DrawRay (RayObject.transform.position, Vector2.right * new Vector2(enemyDirection, 0f), Color.red);

        if (hitRight)
        {
            Debug.Log("colision detected to the right");
            rb.AddForce (Vector2.up * jumpForce);
        
        }
       

    }
} 
