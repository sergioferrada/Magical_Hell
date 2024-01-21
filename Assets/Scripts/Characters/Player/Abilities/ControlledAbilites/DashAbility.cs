using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : PlayerAbility
{
    public Rigidbody2D rb;
    public float dashForce = 5f;

    protected override void Start()
    {
        if (rb == null) rb = gameObject.GetComponent<Rigidbody2D>();
        base.Start();
    }

    protected override IEnumerator ActivateAbility()
    {
        rb.AddForce(PlayerController.Instance.direction * dashForce, ForceMode2D.Impulse);
        return base.ActivateAbility();
    }
}
