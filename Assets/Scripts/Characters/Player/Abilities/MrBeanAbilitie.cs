using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrBeanAbilitie : PlayerAbility
{
    protected override IEnumerator ActivateAbility()
    {
        var beanPrefab = Instantiate(objectAttack, transform.position, Quaternion.identity);
        beanPrefab.GetComponent<MrBeansLogic>().damage = damage;
        beanPrefab.GetComponent<MrBeansLogic>().objectScale = damageObjectScale;
        beanPrefab.GetComponent<MrBeansLogic>().parentAbility = this;
        return base.ActivateAbility();
    }
}
