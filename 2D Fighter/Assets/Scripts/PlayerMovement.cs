using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D rb;
    public LayerMask ground;
    private Animator animator;
    public BoxCollider2D boxCollider;

    public int health;
    public bool facingRight = true;
    public float gravity = 9.81f;
    private float fallSpeed = 1.2f;
    public int speed = 5;
    public int jumpForce;
    private float input;
    private float downInput;
    private bool jumpButton;
    public bool isGrounded;
    public float distanceToGround;
    public int maxJumps = 2;
    private int currentJump;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();        
    }

    void Update() {
        // Collect inputs
        input = Input.GetAxisRaw("Horizontal");
        downInput = Input.GetAxisRaw("Vertical");
        jumpButton = Input.GetButtonDown("Jump");
        
        //Flip sprite
        if (input > 0 && !facingRight)
            Flip();
        else if (input < 0 && facingRight)
            Flip();

        animator.SetFloat("Speed", Mathf.Abs(input)); // Sets parameter to positive input to play run animation

        // Add velocity in y-axis
        if (rb.velocity.y == 0) {
            isGrounded = true;
            currentJump = 0;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        if (jumpButton && isGrounded && currentJump < maxJumps || jumpButton && currentJump < maxJumps ) {
            currentJump++;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }

        // Attack 1
        if (Input.GetMouseButtonDown(0)) {
            
        }
    }

    private void FixedUpdate() {
        // Add velocity in x-axis;
        rb.velocity = new Vector2(input * speed, rb.velocity.y);
        if (downInput == -1) {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.down * gravity * fallSpeed, ForceMode2D.Impulse);
            if (!isGrounded) {
                animator.SetBool("isFalling", true);
            }
        }
    }

    // Flip the sprite
    void Flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}