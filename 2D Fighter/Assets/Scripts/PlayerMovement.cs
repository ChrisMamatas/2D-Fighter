using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D rb;
    private Animator animator;
    public Transform attackSpot;
    public SpriteRenderer sprite;
    public LayerMask enemyLayers;
    private float moveX;

    public float attackRadius = 0.4f;
    private bool facingRight = true;
    public int health;
    public float fallSpeed = 2f;
    public int speed, jumpForce;
    public bool isGrounded;
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

    private void FixedUpdate() {
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            isGrounded = true;
            currentJump = 0;
            animator.SetBool("isJumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        isGrounded = false;
    }

    // Changes players velocity in x, leaving current y velocity unchanged
    private void OnRun(InputValue value) {
        
        moveX = (int)value.Get<float>();

        // Flips sprite to face direction its moving
        if (moveX < 0 && facingRight) {
            sprite.flipX = true;
            facingRight = false;
        }
        else if (moveX > 0 && !facingRight){
            sprite.flipX = false;
            facingRight = true;
        }
        animator.SetFloat("Speed", Mathf.Abs(moveX));
    }

    private void OnJump() {
        if (isGrounded && currentJump < maxJumps || currentJump < maxJumps) {
            //rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            currentJump++;
            animator.SetBool("isJumping", true);
        }
    }

    private void OnAttack1() {
        if (Time.time >= nextAttackTime1) {
            animator.SetTrigger("attack1");
            // Code attack here
            nextAttackTime1 = Time.time + 1f / attack1Rate;

            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackSpot.transform.position, attackRadius);
            for (int i = 0; i < hitEnemys.Length; i++) {
                DamageEnemy(hitEnemys[i], 5, 0.3f);
            }

        }
    }

    private void OnAttack2() {
        if (Time.time >= nextAttackTime2) {
            animator.SetTrigger("attack2");
            // Code attack here
            nextAttackTime2 = Time.time + 1f / attack2Rate;

            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackSpot.transform.position, attackRadius);
            for (int i = 0; i < hitEnemys.Length; i++) {
                DamageEnemy(hitEnemys[i], 7, 0.5f);
            }

        }
    }

    private void OnAttack3() {
        if (Time.time >= nextAttackTime2) {
            animator.SetTrigger("attack3");
            // Code attack here
            nextAttackTime3 = Time.time + 1f / attack3Rate;

            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackSpot.transform.position, attackRadius);
            for (int i = 0; i < hitEnemys.Length; i++) {
                DamageEnemy(hitEnemys[i], 10, 1f);
            }
        }
    }

    public void DamageEnemy(Collider2D enemy, int damage, float nockBack) {
        enemy.gameObject.GetComponent<PlayerMovement>().TakeDamage(damage, nockBack);
    }

    public void TakeDamage(int damage, float nockBack) {
        health += damage;
        rb.AddForce(Vector2.left * nockBack, ForceMode2D.Impulse);
        Debug.Log(health);
    }

}