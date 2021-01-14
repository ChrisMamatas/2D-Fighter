using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theodore : MonoBehaviour {

    private PlayerControls controls; // used for the new input system
    private Rigidbody2D rb;
    public LayerMask ground;
    private Animator animator;
    public Transform attackRadius;
    public Transform feetPos;
    private Vector2 moveX;
    private float moveY;

    public int health;
    private float fallSpeed = 1.2f;
    public int speed, jumpForce;
    public bool isGrounded;
    public int maxJumps = 2;
    private int currentJump;
    private float attack1Rate = 1.5f;
    private float attack2Rate = 1f;
    private float attack3Rate = 2f;
    private float nextAttackTime1, nextAttackTime2, nextAttackTime3 = 0.0f;


    private void OnEnable() {
        controls.Gameplay.Enable();
    }
    private void OnDisable() {
        controls.Gameplay.Disable();
    }

    private void Awake() {
        // Controller object
        controls = new PlayerControls();

        // Running input
        controls.Gameplay.Run.performed += ctx => moveX = ctx.ReadValue<Vector2>();
        controls.Gameplay.Run.canceled += ctx => moveX = Vector2.zero;

        // Jumping input
        controls.Gameplay.Jump.performed += ctx => PlayerJump();
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {

        CheckIfGrounded();

        // Attack 1
        //       if (true == false) {
        //           if (Time.time >= nextAttackTime1) {
        //               animator.SetTrigger("attack1");
        //               // Code attack here
        //               nextAttackTime1 = Time.time + 1f / attack1Rate;

        // Collider2D[] enemysHit = Physics2D.OverlapCircleAll(attack)
        //            }
        //        }
    }

    private void FixedUpdate() {
        PlayerRun();
    }

    // See if feet overlap ground layermask, if so isGrounded
    void CheckIfGrounded() {
    }

    // Changes players velocity in x, leaving current y velocity unchanged
    private void PlayerRun() {
        rb.velocity = new Vector2(moveX.x * speed, rb.velocity.y);
    }

    private void PlayerJump() {
        rb.AddForce(Vector2.up * jumpForce * moveY);
    }

    //Adds force to player in positive y
    //private void PlayerJump() {
    //    if (isGrounded && currentJump < maxJumps || currentJump < maxJumps) {
    //        currentJump++;
    //        rb.velocity = new Vector2(rb.velocity.x, 0f);
    //        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    //        isGrounded = false;
    //        if (currentJump == 0) {
    //            animator.SetBool("isJumping", true);
    //        }
    //        else {
    //            animator.SetTrigger("doubleJumped");
    //        }
    //    }
    //}
}