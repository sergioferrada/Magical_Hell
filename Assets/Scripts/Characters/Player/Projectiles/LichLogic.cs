using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class LichLogic : ProjectileBase
{
    public int timesItcanBounce;
    public int minFireballRange, maxFireballRange;
    public float trackRadius = 3f;
    private int timesExploded;

    private GameObject target;
    private GameObject ultimoEnemigoAlcanzado;
    private bool tracking = false;

    protected override void Awake()
    {
        base.Awake();
        direction = Random.insideUnitSphere;
        direction = direction.normalized;
    }

    protected override void Start()
    {
        base.Start();
    }

    public void SetTarget(GameObject g)
    {
        tracking = true;
        target = g;
    }

    private void Update()
    {
        if (tracking)
        {
            if (target != null)
            {
                // Calcular la dirección hacia el enemigo
                Vector2 direction = target.transform.position - transform.position;
                direction.Normalize();

                // Mover el proyectil hacia el enemigo
                //transform.Translate(direction * speed * Time.deltaTime);
                SetProyectileVelocity(speed, direction);
            }
        }
    }

    public void Track()
    {
        Collider2D[] EnemyInRange = Physics2D.OverlapCircleAll(transform.position, trackRadius, 7);

        GameObject closestEnemy = FindClosestEnemy(EnemyInRange);

        if (closestEnemy != null)
        {
            Debug.Log("Si hay");
            // Calcular la dirección hacia el enemigo
            Vector2 direction = closestEnemy.transform.position - transform.position;
            direction.Normalize();

            // Mover el proyectil hacia el enemigo
            //transform.Translate(direction * speed * Time.deltaTime);
            SetProyectileVelocity(speed, direction);

            // Actualizar el último enemigo alcanzado
            ultimoEnemigoAlcanzado = closestEnemy;

            tracking = true;
            target = closestEnemy;
        }
        else 
        {
            tracking = false;
            Debug.Log("No hay"); 
        }
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

   
        if (collision.gameObject.layer == 7)
        {
            //Track();
            //Si el objeto colisonado tiene menos o la misma cantidad de vida que el daño
            if (collision.gameObject.GetComponent<Enemy>().Life <= damage)
                Track();

        }

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        //Cuando colisione con un objeto en el layer de "Enemies"
        if (collision.gameObject.layer == 7)
        {
            //Track();
            //Si el objeto colisonado tiene menos o la misma cantidad de vida que el daño
            if (collision.gameObject.GetComponent<Enemy>().Life <= damage)
                Track();
            else
                DestroyProjectile();
        }
    }

    protected override void DestroyProjectile()
    {
        rb2d.velocity = Vector2.zero;
        Destroy(gameObject, .4f);
    }


    GameObject FindClosestEnemy(Collider2D[] enemies)
    {
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D enemyCollider in enemies)
        {
            // Verificar si este enemigo ya fue alcanzado
            if (enemyCollider.gameObject == ultimoEnemigoAlcanzado)
                continue;

            // Calcular la distancia
            float distance = Vector2.Distance(transform.position, enemyCollider.transform.position);

            // Actualizar al enemigo más cercano si la distancia es menor
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyCollider.gameObject;
            }
        }

        return closestEnemy;
    } 
    void OnDrawGizmos()
    {
        // Dibujar el área de overlap en el Editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector3)transform.position, trackRadius);
    }



}
