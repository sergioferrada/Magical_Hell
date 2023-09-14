using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttackAbilitie : MonoBehaviour
{
    public GameObject fireball;

    private void Start()
    {
        InvokeRepeating(nameof(FireballAttack), 0, 1f);
    }

    void FireballAttack()
    {
         Instantiate(fireball,transform.position, Quaternion.identity);
    }
}
