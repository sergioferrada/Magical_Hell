using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    protected Rigidbody2D rb2d;
    public Vector2 direction { get; set; }
    protected Animator animator;

    [SerializeField]
    private float damage;
    public float Damage{ get { return damage; } protected set { damage = value; } }
    [SerializeField]
    protected float speed, lifeTime;

    protected virtual void Start()
    {   
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Destroy(gameObject, lifeTime);
    }

}
