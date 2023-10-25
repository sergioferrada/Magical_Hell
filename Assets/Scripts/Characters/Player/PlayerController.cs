using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : CharacterBase
{
    public Transform attackPoint;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        GameManager.SetTotalPlayerLife(Life);
        GameManager.SetPlayerMaxLife(Life);
    }

    private void Update()
    {
        if (!CompareState(State.Death) /*&& !CompareState(State.Injured)*/)
        {
            ProcessInputs();
            //Tiempo para el siguiente ataque
            if (passedTime < AttackDelay)
                passedTime += Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        // Physic Calculation
        if (!CompareState(State.Death) /*&& !CompareState(State.Injured)*/)
        {
            Move();
        }
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0)
            transform.localScale = new Vector2(moveX, transform.localScale.y);


        if (Input.GetKeyDown(KeyCode.Space) && passedTime >= AttackDelay
            && !CompareState(State.Injured))
            SetState(State.ShortAttack);

        // Cambia los estados en función del movimiento o ataque
        if (!CompareState(State.ShortAttack))
        {
            if (!CompareState(State.Injured))
            {
                if (direction.magnitude > 0) SetState(State.Move);
                else if (moveX == 0 && moveY == 0) SetState(State.Idle);
            }
        }
        //else { moveX = 0; moveY = 0; }

        direction = new Vector2(moveX, moveY).normalized;
    }

    protected override void Move()
    {
        rb2d.velocity = new Vector2(direction.x * MovementSpeed, direction.y * MovementSpeed);
    }

    void MeleeAttack()
    {
        GameManager.AddTotalAttacks();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, attackableLayers);

        foreach (Collider2D enemy in hitEnemies) {

            enemy.GetComponent<Enemy>().ReciveDamage(Damage);
            Vector2 direction = (enemy.GetComponent<Collider2D>().transform.position - transform.position).normalized;
            enemy.GetComponent<Rigidbody2D>().AddForce(direction * 25, ForceMode2D.Impulse);
            GameManager.AddSuccefulAttack();
        }
    }

    public override void ReciveDamage(float damage)
    {
        base.ReciveDamage(damage);
        GameManager.SetTotalPlayerLife(Life);
    }

    protected override void Death()
    {
        GameManager.SetGameState(GameManager.GameState.GameOver);
        base.Death();
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
            //rb2d.AddForce(direction * 25, ForceMode2D.Impulse);
        }

        if (collision.gameObject.layer == 10)
        {
            ReciveDamage(collision.gameObject.GetComponent<ProjectileLogic>().Damage);
        }
    }
}
