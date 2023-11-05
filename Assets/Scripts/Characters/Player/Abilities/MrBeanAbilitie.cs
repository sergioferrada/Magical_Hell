using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrBeanAbilitie : PlayerAbility
{
    public GameObject bean;

    public override void SetAbilityStats(float d, float cd, GameObject obj)
    {
        base.SetAbilityStats(d, cd);
        bean = obj;
    }

    protected override IEnumerator ActivateAbility()
    {
        var beanPrefab = Instantiate(bean, transform.position, Quaternion.identity);
        beanPrefab.GetComponent<MrBeansLogic>().SetDamage(damage);
        return base.ActivateAbility();
    }
}
