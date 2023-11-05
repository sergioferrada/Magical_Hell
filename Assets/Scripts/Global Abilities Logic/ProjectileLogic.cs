using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    protected Rigidbody2D rb2d;
    public Vector2 direction;
    protected Animator animator;

    public float Damage{ get; protected set; }
    [SerializeField] protected float speed, lifeTime;

    protected virtual void Start()
    {
        Destroy(gameObject, lifeTime);
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb2d.velocity = speed * direction;
    }

    public void SetDamage(float damage)
    {
        Damage = damage;
    }
}
