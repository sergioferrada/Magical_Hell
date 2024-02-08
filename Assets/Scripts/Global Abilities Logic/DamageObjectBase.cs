using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base de todos los ataques del jugador, o de los enemigos?
public class DamageObjectBase : MonoBehaviour
{
    public float damage, objectScale;

    public PlayerAbility parentAbility;
    public Collider2D collider2d;
    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        GetComponent<Transform>().localScale *= (Vector2.one * objectScale);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            if (parentAbility != null)
                parentAbility.AddExp();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            if(parentAbility != null)
                parentAbility.AddExp();
        }
    }
}
