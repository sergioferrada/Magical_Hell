using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//esto es un ataque, creo que es el q tiene forma de circulo
public class MeleePlayerAttack : StationaryDamageObject
{
    public float impulseForce;

    public void Activate()
    {
        transform.localScale = (Vector2.one * objectScale);
        DifficultManager.Instance.AddTotalAttacks();
        SoundManager.Instance.PlaySound("Melee_Deep_Whoosh_Sound", .7f);
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
