using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeAbilitieItem : MonoBehaviour
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
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<KnifeAbilitie>() == null)
            {
                var script = collision.gameObject.AddComponent<KnifeAbilitie>();
                script.SetAbilityStats(damageAbilitiy, cooldownAbility, projectile);
            }
            animator.Play("Knife_collected_animation");
            Destroy(gameObject, 1f);
        }
    }
}
