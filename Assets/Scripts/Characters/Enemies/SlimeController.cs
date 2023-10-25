using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeController : Enemy
{
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float chaseDistance;
    [SerializeField] private float attackForce;

    private Vector2[] diagonalDirections = { new Vector2(1, 1), new Vector2(-1, 1), new Vector2(-1, -1), new Vector2(1, -1) };
    private float distance;

    protected override void Start()
    {
        base.Start();
        // Inicializa la dirección inicial de manera aleatoria
        direction = GetRandomDirection(); 
    }

    protected override void Update()
    {
        base.Update();

        if ((playerTransform == null) || (state == State.Death) || (state == State.Injured))
            return;

        distance = Vector2.Distance(transform.position, playerTransform.position);   

        //Dentro de rango de ataque
        if (distance <= attackDistance)
        {
            //Se detiene
            transform.position = transform.position;

            //Puede atacar?
            if (passedTime >= AttackDelay)
                SetState(State.ShortAttack);
        }
        //Fuera de rango de ataque
        else
        {
            if (state != State.ShortAttack)
                Move();
        }  
        
        //Tiempo para el siguiente ataque
        if (passedTime < AttackDelay)
            passedTime += Time.deltaTime;
    }

    private Vector2 GetRandomDirection()
    {
        // Genera una dirección aleatoria utilizando valores entre -1 y 1 para x e y
        int randomIndex = Random.Range(0, diagonalDirections.Length);
        return diagonalDirections[randomIndex];
    }

    protected override void ShortRangeAttack()
    {
        rb2d.AddForce(direction * attackForce, ForceMode2D.Impulse);
    }

    protected override void Move()
    {
        base.Move();
        transform.Translate(direction * MovementSpeed * Time.deltaTime);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        Vector2 reflection = Vector2.Reflect(direction, collision.contacts[0].normal);

        //Se redondean las coordenadas del vector par solo obtener valores de 1 o -1
        reflection.x = Mathf.Sign(reflection.x);

        direction = reflection;
        transform.localScale = new Vector2(direction.x, transform.localScale.y);
    }
}
