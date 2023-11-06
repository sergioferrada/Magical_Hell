using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAbility : PlayerAbility
{
    public float numberOfProjectiles;
    public float radius;

    protected override IEnumerator ActivateAbility()
    {
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float randomAngle = Random.Range(0f, 360f);
            float randomRadius = Random.Range(0f, radius);

            Vector3 direction = Quaternion.Euler(0f, 0f, randomAngle) * Vector3.up;
            Vector3 spawnPosition = transform.position + direction * randomRadius;

            var projectile = Instantiate(objectAttack, spawnPosition, Quaternion.identity);
            projectile.GetComponent<ThunderLogic>().Damage = damage;
            //projectile.GetComponent<ThunderLogic>().parentAbility = this;
        }

        return base.ActivateAbility();
    }

}
