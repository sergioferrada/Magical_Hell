using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAreaAbilitiy : PlayerAbility
{
    protected override IEnumerator ActivateAbility()
    {
        var projectile = Instantiate(objectAttack, transform.position, Quaternion.identity);
        projectile.GetComponent<IceAreaLogic>().damage = damage;
        projectile.GetComponent<IceAreaLogic>().objectScale = damageObjectScale;
        projectile.GetComponent<IceAreaLogic>().parentAbility = this;
        return base.ActivateAbility();
    }
}
