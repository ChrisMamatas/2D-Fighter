using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theodore : MonoBehaviour {

    private Rigidbody2D rb;
    public LayerMask ground;
    private Animator animator;
    public BoxCollider2D boxCollider;
    public Transform attackRadius;

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
    private float attack1Rate = 1.5f;
    private float attack2Rate = 1f;
    private float attack3Rate = 2f;
    private float nextAttackTime1, nextAttackTime2, nextAttackTime3 = 0.0f;

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

        animator.SetFloat("Speed", Mathf.Abs(input));

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
            if (currentJump == 0) {
                animator.SetBool("isJumping", true);
            }
            else {
                animator.SetTrigger("doubleJumped");
            }
        }

        // Attack 1
        if (Input.GetMouseButtonDown(0)) {
            if (Time.time >= nextAttackTime1) {
                animator.SetTrigger("attack1");
                // Code attack here
                nextAttackTime1 = Time.time + 1f / attack1Rate;

               // Collider2D[] enemysHit = Physics2D.OverlapCircleAll(attack)
            }
        }

        // Attack 2
        if (Input.GetMouseButtonDown(1)) {
            if (Time.time >= nextAttackTime2) {
                animator.SetTrigger("attack2");
                // Code attack here
                nextAttackTime2 = Time.time + 1f / attack2Rate;
            }
        }

        // Attack 3
        if (Input.GetMouseButtonDown(2)) {
            if (Time.time >= nextAttackTime3) {
                // Code attack here
                animator.SetTrigger("attack3");
                nextAttackTime3 = Time.time + 1f / attack3Rate;
            }
        }
    }

    private void FixedUpdate() {
        // Add velocity in x-axis;
        rb.velocity = new Vector2(input * speed, rb.velocity.y);
        if (downInput == -1) {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.down * gravity * fallSpeed, ForceMode2D.Impulse);
            if (!isGrounded) {
               animator.SetBool("isJumping", false);
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