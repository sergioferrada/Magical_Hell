using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Vector2 direction;

    [SerializeField]
    private float damage;
    public float Damage{ get { return damage; } protected set { damage = value; } }
    [SerializeField]
    protected float speed, lifeTime;

    protected virtual void Start()
    {
        Destroy(gameObject, lifeTime);
    }

}
