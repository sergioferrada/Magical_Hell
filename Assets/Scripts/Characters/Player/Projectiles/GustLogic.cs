using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GustLogic : ProjectileBase
{
    public float radius = 5f;
    public float force = 10f;
    public LayerMask enemyLayer;

    protected override void Awake()
    {
        base.Awake();
        direction = Random.insideUnitSphere;
        direction = direction.normalized;
    }

    void Update()
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                //aplicar la fuerza
                Vector2 direction = (collider.transform.position - transform.position).normalized;
                rb.AddForce(direction * force);
            }
        }

        //dejar pasar el objeto
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, radius, enemyLayer);
        if (hit.collider != null)
        {
          //si toca, aplicar la fuerza pero atravesar 
            Rigidbody2D rb = hit.collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = transform.up;
                rb.AddForce(direction * force);
            }
        }
    }
}
