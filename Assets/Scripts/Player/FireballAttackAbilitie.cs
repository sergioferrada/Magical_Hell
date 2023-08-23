using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttackAbilitie : MonoBehaviour
{
    public GameObject fireball;

    private void Start()
    {
        InvokeRepeating(nameof(FireballAttack), 0, 0.25f);
    }

    void FireballAttack()
    {
         Instantiate(fireball, transform.position, transform.rotation);
    }
}
