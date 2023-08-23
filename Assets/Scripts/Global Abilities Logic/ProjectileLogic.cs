using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Vector2 direction;
    public float damage, speed, lifeTime;

    public void SetDamage(float d) { damage = d; }
}
