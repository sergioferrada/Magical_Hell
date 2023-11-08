using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttackAbilitie : PlayerAbility
{
    protected override IEnumerator ActivateAbility()
    {
        SoundManager.Instance.PlaySound("Flame_Attack3", .6f);
        var projectile = Instantiate(objectAttack, transform.position, Quaternion.identity);
        projectile.GetComponent<FireballLogic>().damage = damage;
        projectile.GetComponent<FireballLogic>().objectScale = damageObjectScale;
        projectile.GetComponent<FireballLogic>().parentAbility = this;
        return base.ActivateAbility();
    }
}
