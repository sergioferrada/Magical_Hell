using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeController : Enemy
{
    private Vector2[] diagonalDirections = { new Vector2(1, 1), new Vector2(-1, 1), new Vector2(-1, -1), new Vector2(1, -1) };
    private Vector2 currentDirection;
    private float distance;

    private void Start()
    {
        ChangeState(State.Move);
        // Inicializa la dirección inicial de manera aleatoria
        currentDirection = GetRandomDirection(); 
        transform.localScale = new Vector2(currentDirection.x, transform.localScale.y);
    }

    private void Update()
    {
        if ((playerTransform == null) || (state == State.Death))
        {
            return;
        }

        distance = Vector2.Distance(transform.position, playerTransform.position);   

        if (state != State.Injured)
        {
            //Dentro de rango de ataque
            if (distance <= attackDistance)
            {
                //Se detiene
                transform.position = transform.position;

                //Puede atacar?
                if (passedTime >= attackDelay)
                {
                    ChangeState(State.Attack);
                }
            }
            //Fuera de rango de ataque
            else
            {
                if (state != State.Attack)
                {
                    Move();
                }
            }  
        }

        //Tiempo para el siguiente ataque
        if (passedTime < attackDelay)
        {
            passedTime += Time.deltaTime;
        }
    }

    protected override void ShortRangeAttack()
    {
        rb2d.AddForce(currentDirection * 15, ForceMode2D.Impulse);
    }

    private Vector2 GetRandomDirection()
    {
        // Genera una dirección aleatoria utilizando valores entre -1 y 1 para x e y
        int randomIndex = Random.Range(0, diagonalDirections.Length);
        return diagonalDirections[randomIndex];
    }

    private void Move()
    {
        ChangeState(State.Move);
        transform.Translate(currentDirection * movementSpeed * Time.deltaTime);
    }

    override protected void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        Vector2 reflection = Vector2.Reflect(currentDirection, collision.contacts[0].normal);

        //Se redondean las coordenadas del vector par solo obtener valores de 1 o -1
        reflection.x = Mathf.Sign(reflection.x);
        reflection.y = Mathf.Sign(reflection.y);

        currentDirection = reflection;
        transform.localScale = new Vector2(currentDirection.x, transform.localScale.y);
    }
}
