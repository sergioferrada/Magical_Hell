using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : PlayerAbility
{
    public Rigidbody2D rb;
    public MeleePlayerAttack attack;
    public float dashForce = 5f;

    protected override void Start()
    {
        if (rb == null) rb = gameObject.GetComponent<Rigidbody2D>();
        base.Start();
    }

    protected override IEnumerator ActivateAbility()
    {
        rb.velocity = Vector2.zero;
        //Debug.Log("" + PlayerController.Instance.direction * dashForce);
        rb.velocity = PlayerController.Instance.direction * dashForce;

        //rb.AddForce(PlayerController.Instance.lastDirection * dashForce, ForceMode2D.Impulse);
        PlayerController.Instance.BecomeInvulnerable(1.0f);
        attack.damage = damage;
        attack.Activate();

        PlayerController.Instance.canMove = false;

        return base.ActivateAbility();
    }

    public void FinishDash()
    {
        PlayerController.Instance.canMove = true;
    }
}
