using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    private int extraJump;
    public int extraJumpValue; 
    public float moveSpeed = 5f; // Speed of the player
    public float jumpForce = 10f; // Jump force
    public Transform groundCheck; // Reference point for ground check
    public LayerMask groundLayer; // Layer of the ground

    private Rigidbody2D rb;
    private bool isGrounded;
    

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();


    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
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
        else if(!isGrounded)
        {
            Debug.Log("Not grounded");
        }
    }



}
