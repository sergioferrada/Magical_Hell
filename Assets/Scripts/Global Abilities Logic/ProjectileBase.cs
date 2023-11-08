using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : DamageObjectBase
{
    protected Rigidbody2D rb2d;

    public Vector2 direction;
    public float speed, lifeTime;

    protected override void Awake()
    {
        base.Awake();
        rb2d = GetComponent<Rigidbody2D>();

        Destroy(gameObject, lifeTime);
    }

    protected override void Start()
    {
        base.Start();
        SetProyectileVelocity(speed, direction);
    }

    protected virtual void SetProyectileVelocity(float speed, Vector2 direction)
    {
        rb2d.velocity = speed * direction;
    }
}
