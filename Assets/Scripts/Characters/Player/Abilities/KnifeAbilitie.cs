using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeAbilitie : PlayerAbility
{
    private PlayerController player;
    private Vector2 knifeDirection = Vector2.right;

    protected override void Start()
    {
        player = GetComponent<PlayerController>();
        base.Start();
    }

    protected override IEnumerator ActivateAbility()
    {
        if(player.direction != Vector2.zero)
            knifeDirection = player.direction;
        
        var projectile = Instantiate(objectAttack, transform.position, Quaternion.identity);
        projectile.GetComponent<KnifeLogic>().SetDamage(damage);
        projectile.GetComponent<KnifeLogic>().direction = knifeDirection;
        projectile.GetComponent<KnifeLogic>().parentAbility = this;

        return base.ActivateAbility();
    }
}
