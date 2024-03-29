using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : StationaryDamageObject
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.GetComponent<PlayerController>().ReciveDamage(damage);
        }
    }
}
