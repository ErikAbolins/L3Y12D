using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float currentSpeed;
    public float sprintSpeed = 20f; // Speed of the player when sprinting
    public float moveSpeed = 5f; // Speed of the player
    public float jumpForce = 10f; // Jump force
    public Transform groundCheck; // Reference point for ground check
    public LayerMask groundLayer; // Layer of the ground

    private Rigidbody2D rb;
    private bool isGrounded;
    private int extraJump;
    public int extraJumpValue; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed; // Initialize current speed
    }

    void Update()
    {
        Move();
        Jump();
        Sprint();
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        // Use currentSpeed instead of moveSpeed
        rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);
    }

    void Jump()
    {
        if (isGrounded == true)
        {
            extraJump = extraJumpValue;
        }
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jumping");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        else if (Input.GetButtonDown("Jump") && extraJump > 0)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            extraJump--;
        }
        else if (!isGrounded)
        {
            Debug.Log("Not grounded");
        }
    }

    void Sprint()
    {
        // Check if the sprint key is pressed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed; // Start sprinting
            Debug.Log("Sprinting");
        }
        else // If the sprint key is not pressed
        {
            currentSpeed = moveSpeed; // Stop sprinting
            Debug.Log("Sprint stopped");
        }
    } 
}
