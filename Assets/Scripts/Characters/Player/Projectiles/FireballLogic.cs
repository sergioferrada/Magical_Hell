using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballLogic : ProjectileBase
{
    public int timesItcanExplode;
    public int minFireballRange, maxFireballRange;

    private int timesExploded;

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

    public void ExploteInMore()
    {
        if (timesExploded < timesItcanExplode)
        {   
            for (int i = 0; i < Random.Range(minFireballRange, maxFireballRange); i++)
            {
                GameObject projectile = Instantiate(gameObject, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(direction));
                var fireballScript = projectile.GetComponent<FireballLogic>();

                fireballScript.timesExploded += 1;
                fireballScript.damage = damage * .5f;
                projectile.transform.localScale *= .5f;
            }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        //Cuando colisione con un objeto en el layer de "Enemies"
        if (collision.gameObject.layer == 7)
        {
            //Si el objeto colisonado tiene menos o la misma cantidad de vida que el daño
            if (collision.gameObject.GetComponent<Enemy>().Life <= damage)
                ExploteInMore();
                   
        }

        //DestroyProjectile();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        //Cuando colisione con un objeto en el layer de "Enemies"
        if (collision.gameObject.layer == 7)
        {
            //Si el objeto colisonado tiene menos o la misma cantidad de vida que el daño
            if (collision.gameObject.GetComponent<Enemy>().Life <= damage)
                ExploteInMore();

            DestroyProjectile();
        }
    }

    protected override void DestroyProjectile()
    {
        //GetComponent<CircleCollider2D>().enabled = false;
        animator.Play("Fireball_Explosion");
        rb2d.velocity = Vector2.zero;
        Destroy(gameObject, .4f);
    }

}
