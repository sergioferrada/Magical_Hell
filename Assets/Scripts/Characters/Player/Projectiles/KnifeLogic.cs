using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeLogic : ProjectileBase
{

    protected override void Start()
    {
        base.Start();
    }

    //protected override void OnCollisionEnter2D(Collision2D collision)
    //{
    //    base.OnCollisionEnter2D (collision);    
    //}

    protected override void DestroyProjectile()
    {
        rb2d.velocity = Vector3.zero;
        GetComponent<Collider2D>().enabled = false;
        animator.Play("Impact_Animation");
        Destroy(gameObject, .5f);
    }

}
