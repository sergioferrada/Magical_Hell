using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrBeansAbilitieItem : MonoBehaviour
{
    [SerializeField] private GameObject BeanPrefab;
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
            if (collision.GetComponent<MrBeanAbilitie>() == null)
            {
                var script = collision.gameObject.AddComponent<MrBeanAbilitie>();
                script.SetAbilityStats(damageAbilitiy, cooldownAbility, BeanPrefab);
            }

            animator.Play("MrBean_collection_item_animation");
            Destroy(gameObject, 1f);
        }
    }
}
