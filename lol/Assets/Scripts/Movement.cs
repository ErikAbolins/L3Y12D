using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
   
    private float slideTimer; // Timer to track slide duration
    private float slideCooldownTimer; // Timer to track cooldown duration
    public float slideDuration = 1f; // Duration of the slide in seconds
    public float slideCooldown = 2f; // Cooldown time before the player can slide again
    public float slideSpeed = 11f; // Speed of the slide
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
    private BoxCollider2D boxCollider; 
    private int health = 3;
    public float invincibilityDuration = 1.0f;
    private bool isInvincible;
 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = moveSpeed; // Initialize current speed
        spriteRenderer = GetComponent<SpriteRenderer>();
        slideTimer = 0f;
        slideCooldownTimer = 0f;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            Debug.Log("S is detected in the update");
        }
        
        
        Move();
        Jump();
        Sprint();
        Slide();
        PlayerDeath();
        
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

        // Reset jump and fall animations when grounded
        if (isGrounded)
        {
            extraJump = extraJumpValue;

            // Reset jump and fall animations only when they are currently active
            if (animator.GetBool("isJumping"))
            {
                animator.SetBool("isJumping", false);
            }
            if (animator.GetBool("isFalling"))
            {
                animator.SetBool("isFalling", false);
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

        // Set falling animation if the player is moving downward and not grounded
        if (!isGrounded && rb.velocity.y < -0.1f) // A small threshold to ensure falling animation isn't triggered too soon
        {
            if (!animator.GetBool("isFalling"))
            {
                animator.SetBool("isFalling", true);
            }
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

      void Slide()
    {
        // Check if the slide key is pressed, the player is grounded, and the cooldown timer is zero
        if (Input.GetKey(KeyCode.S) && isGrounded && slideTimer <= 0f && slideCooldownTimer <= 0f)
        {
            Debug.Log("Sliding started");
            animator.SetBool("isSliding", true); // Trigger slide animation

            boxCollider.size = new Vector2(boxCollider.size.x, boxCollider.size.y / 2);

            float slideDirection = spriteRenderer.flipX ? -1 : 1; // Determine slide direction based on character's facing direction

            // Apply a strong initial force for the slide to kick off the movement
            rb.velocity = new Vector2(slideDirection * slideSpeed, rb.velocity.y);

            slideTimer = slideDuration; // Start the slide duration timer
            slideCooldownTimer = slideCooldown; // Set the cooldown timer
        }

        // Handle the slide duration countdown
        if (slideTimer > 0)
        {
            slideTimer -= Time.deltaTime; // Decrease the slide duration timer

            // Ensure the character continues sliding with the same velocity until the slide ends
            float slideDirection = spriteRenderer.flipX ? -1 : 1; // Update direction in case of flip
            rb.velocity = new Vector2(slideDirection * slideSpeed, rb.velocity.y);

            // If the slide duration is over, stop the slide
            if (slideTimer <= 0)
            {
                Debug.Log("Slide ended");
                animator.SetBool("isSliding", false); // Stop slide animation
                rb.velocity = new Vector2(0, rb.velocity.y); // Stop horizontal movement after sliding
                boxCollider.size = new Vector2(boxCollider.size.x, boxCollider.size.y * 2); // Return to original size after sliding ends
            }
        }

        // Countdown for the slide cooldown timer
        if (slideCooldownTimer > 0)
        {
            slideCooldownTimer -= Time.deltaTime; // Decrease the cooldown timer
        }
    }
 
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !isInvincible)
        {
            Debug.Log("Player hit by enemy");
            health--; // Reduce player health by 1 when hit by an enemy

            StartCoroutine(Invincibility()); // Start invincibility coroutine
        }
    }

    void PlayerDeath()
    {
        if (health <= 0)
        {
            //reset the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    IEnumerator Invincibility()
    {
        isInvincible = true; // Set the player as invincible

        yield return new WaitForSeconds(invincibilityDuration); // Wait for the invincibility duration
        isInvincible = false; // Stop the invincibility after the duration ends
        //show timer in debuglog
        Debug.Log("Invincibility ended");
    }

    
}
