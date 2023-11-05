using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAbilitieItem : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float damageAbilitiy;
    [SerializeField] private float cooldownAbility;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {

            if (collision.GetComponent<FireballAttackAbilitie>() == null)
            {
                var script = collision.gameObject.AddComponent<FireballAttackAbilitie>();
                script.SetAbilityStats(damageAbilitiy, cooldownAbility, projectile);
            }
            animator.Play("Fireball_collected_Item_animation");
            Destroy(gameObject, 1f);
        }    
    }
}
