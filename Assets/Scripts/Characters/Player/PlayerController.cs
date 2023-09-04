using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : CharacterBase
{
    private Vector2 moveDirection;
    public Transform attackPoint;

    private void Update()
    {
        ProcessInputs();
        //Tiempo para el siguiente ataque
        if (passedTime < attackDelay)
            passedTime += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        // Physic Calculation
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if(moveX != 0)
            transform.localScale = new Vector2 (moveX, transform.localScale.y);

        if (Input.GetKeyDown(KeyCode.Space) && passedTime >= attackDelay)
            ChangeState(State.Attack);

        // Cambia los estados en función del movimiento o ataque
        if (state != State.Attack)
        {
            if (moveDirection.magnitude > 0)
            {
                ChangeState(State.Move);
            }
            else if (moveX == 0 && moveY == 0)
            {
                ChangeState(State.Idle);
            }
        }
        else { moveX = 0; moveY = 0;}

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {      
        rb2d.velocity = new Vector2(moveDirection.x * movementSpeed, moveDirection.y * movementSpeed);   
    }

    void MeleeAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, attackableLayers);

        foreach (Collider2D enemy in hitEnemies) {

            enemy.GetComponent<Enemy>().ReciveDamage(Damage);
            Vector2 direction = (enemy.GetComponent<Collider2D>().transform.position - transform.position).normalized;
            enemy.GetComponent<Rigidbody2D>().AddForce(direction*25, ForceMode2D.Impulse);
        }
    }

    protected void ResetAttack()
    {
        passedTime = 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
