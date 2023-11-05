using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttackAbilitie : PlayerAbility
{
    public GameObject fireball;

    public override void SetAbilityStats(float d, float cd, GameObject obj)
    {
        base.SetAbilityStats(d, cd);
        fireball = obj;
    }

    protected override IEnumerator ActivateAbility()
    {
        var projectile = Instantiate(fireball, transform.position, Quaternion.identity);
        projectile.GetComponent<FireballLogic>().SetDamage(damage);
        projectile.GetComponent<FireballLogic>().parentAbility = this;
        return base.ActivateAbility();
    }
}
