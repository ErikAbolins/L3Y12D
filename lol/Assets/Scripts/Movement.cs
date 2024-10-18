using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Animator animator; // Reference to the animator component
    private float currentSpeed;
    public float sprintSpeed = 20f; // Speed of the player when sprinting
    public float moveSpeed = 5f; // Speed of the player
    public float jumpForce = 10f; // Jump force
    public Transform groundCheck; // Reference point for ground check
    public LayerMask groundLayer; // Layer of the ground

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isGrounded;
    private int extraJump;
    public int extraJumpValue; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed; // Initialize current speed
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // Flip the character based on movement direction
         if (moveInput > 0)
        {
            spriteRenderer.flipX = false; // Face right
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true; // Face left
        }

        // Use currentSpeed instead of moveSpeed
        rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);
    }


   void Jump()
    {
        // Check if the character is grounded at the start of the method
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Reset jump animation when grounded and update extra jumps
        if (isGrounded)
        {
            extraJump = extraJumpValue;
            if (animator.GetBool("isJumping"))
            {
                animator.SetBool("isJumping", false); // Only reset animation if it's currently in the jumping state
            }
        }

        // Handle the first jump when the character is grounded
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
                Debug.Log("First Jump");
            rb.velocity = new Vector2(rb.velocity.x, 0); // Reset vertical velocity for consistent jump height
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("isJumping", true); // Trigger jump animation immediately
        }
        // Handle additional jumps (double jump)
        else if (!isGrounded && Input.GetButtonDown("Jump") && extraJump > 0)
        {
            Debug.Log("Double Jump");
            rb.velocity = new Vector2(rb.velocity.x, 0); // Reset vertical velocity for consistent jump height
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            extraJump--;
            animator.SetBool("isJumping", true); // Ensure jump animation plays on double jump
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
