using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : CharacterBase
{
    public Transform attackPoint;

    private void Update()
    {
        if (state != State.Death && state != State.Injured)
        {
            ProcessInputs();
            //Tiempo para el siguiente ataque
            if (passedTime < attackDelay)
                passedTime += Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        // Physic Calculation
        if (state != State.Death && state != State.Injured)
        {
            Move();
        }   
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if(moveX != 0)
            transform.localScale = new Vector2 (moveX, transform.localScale.y);

        if (Input.GetKeyDown(KeyCode.Space) && passedTime >= attackDelay)
            ChangeState(State.ShortAttack);

        // Cambia los estados en función del movimiento o ataque
        if (state != State.ShortAttack)
        {
            if (direction.magnitude > 0)
            {
                ChangeState(State.Move);
            }
            else if (moveX == 0 && moveY == 0)
            {
                ChangeState(State.Idle);
            }
        }
        else { moveX = 0; moveY = 0;}

        direction = new Vector2(moveX, moveY).normalized;
    }

    protected override void Move()
    {   
        rb2d.velocity = new Vector2(direction.x * movementSpeed, direction.y * movementSpeed);   
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Cuando colisione con un objeto en el layer de "Enemies"
        if (collision.gameObject.layer == 7)
        {
            ReciveDamage(collision.gameObject.GetComponent<Enemy>().Damage);
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            rb2d.AddForce(direction * 25, ForceMode2D.Impulse);
        }

        if (collision.gameObject.layer == 10)
        {
            ReciveDamage(collision.gameObject.GetComponent<ProjectileLogic>().Damage);
        }
    }
}
