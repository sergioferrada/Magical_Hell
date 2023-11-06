using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttackAbilitie : PlayerAbility
{
    protected override IEnumerator ActivateAbility()
    {
        SoundManager.Instance.PlaySound("Flame_Attack3");
        var projectile = Instantiate(objectAttack, transform.position, Quaternion.identity);
        projectile.GetComponent<FireballLogic>().SetDamage(damage);
        projectile.GetComponent<FireballLogic>().parentAbility = this;
        return base.ActivateAbility();
    }
}
