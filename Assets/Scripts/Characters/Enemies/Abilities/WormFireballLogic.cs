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

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        animator.Play("Fireball_Explosion");
    }

    private void Explote()
    {
        rb2d.velocity = new Vector2(0,0);
        GetComponent<CircleCollider2D>().enabled = false;
        Destroy(gameObject, .4f);
    }
}
