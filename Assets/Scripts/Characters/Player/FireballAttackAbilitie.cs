using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttackAbilitie : MonoBehaviour
{
    [SerializeField] public GameObject fireball;
    [SerializeField] private float fireballDamage = 1;
    [SerializeField] private float attackDelay = 1;

    private void Start()
    {
        InvokeRepeating(nameof(FireballAttack), 0, attackDelay);
    }

    void FireballAttack()
    {
        var projectile = Instantiate(fireball,transform.position, Quaternion.identity);
        projectile.GetComponent<FireballLogic>().SetDamage(fireballDamage);
    }
}
