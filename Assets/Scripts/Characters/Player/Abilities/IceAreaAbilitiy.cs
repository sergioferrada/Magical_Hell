using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAreaAbilitiy : PlayerAbility
{
    public GameObject iceAreaPrefab;
    // Start is called before the first frame update
    public override void SetAbilityStats(float d, float cd, GameObject obj)
    {
        base.SetAbilityStats(d, cd);
        iceAreaPrefab = obj;
    }

    protected override IEnumerator ActivateAbility()
    {
        var projectile = Instantiate(iceAreaPrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<IceAreaLogic>().Damage = damage;
        return base.ActivateAbility();
    }
}
