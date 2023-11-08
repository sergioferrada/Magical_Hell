using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePlayerAttack : StationaryDamageObject
{
    public float impulseForce = 25;

    public void Activate()
    {
        DifficultManager.Instance.AddTotalAttacks();
        animator.Play("Attack_Animation");
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            Vector2 direction = (collision.GetComponent<Collider2D>().transform.position - transform.position).normalized;
            collision.GetComponent<Rigidbody2D>().AddForce(direction * impulseForce, ForceMode2D.Impulse);
            DifficultManager.Instance.AddSuccefulAttack();
        }
    }
}
