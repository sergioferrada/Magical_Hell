using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormFireballLogic : ProjectileBase
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        gameObject.name = "Fireball";
        rb2d.velocity = speed * direction;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        //Cuando colisione con un objeto en el layer de "Enemies"
        if (collision.gameObject.layer == 6)
        {
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
