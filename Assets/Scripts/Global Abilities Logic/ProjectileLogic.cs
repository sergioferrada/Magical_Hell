using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    protected Rigidbody2D rb2d;
    public Vector2 direction { get; set; }
    protected Animator animator;

    public float Damage{ get; protected set; }
    [SerializeField] protected float speed, lifeTime;

    protected virtual void Start()
    {   
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Destroy(gameObject, lifeTime);
    }

    public void SetDamage(float damage)
    {
        Damage = damage;
    }
}
