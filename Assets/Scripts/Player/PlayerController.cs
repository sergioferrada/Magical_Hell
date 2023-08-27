using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public float moveSpeedX;
    public float moveSpeedY;

    public Rigidbody2D rb;
    private Vector2 moveDirection;
    private Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;

    public float Damage;
    public float life;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            MeleeAttack();

        // Physic Calculation
        ProcessInputs();
        
    }
    void FixedUpdate()
    {
        // Physic Calculation
        Move();       
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {   
        rb.velocity = new Vector2(moveDirection.x * moveSpeedX, moveDirection.y * moveSpeedY);
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }

    void MeleeAttack()
    {
        animator.SetTrigger("MeleeAttacking");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies) {

            enemy.GetComponent<Enemy1Controller>().ReciveDamage(Damage);
            Vector2 direction = (enemy.GetComponent<Collider2D>().transform.position - transform.position).normalized;
            enemy.GetComponent<Rigidbody2D>().AddForce(direction*25, ForceMode2D.Impulse);
            print("pum pum");
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
