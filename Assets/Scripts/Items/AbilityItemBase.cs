using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityItemBase : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefab;
     //public PlayerAbility abilityToAdd;
    //public FireballAttackAbilitie abilityFireball;
    [SerializeField] private float damageAbilitiy;
    [SerializeField] private float cooldownAbility;

    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //var existingAbility = collision.GetComponent(abilityToAdd.GetType());


            //if (existingAbility == null) {

                //var newPlayerAbility = collision.gameObject.AddComponent(abilityToAdd.GetType());
                //newPlayerAbility.GetComponent(abilityToAdd.GetType());
            //}

            //animator.Play("MrBean_collection_item_animation");
            Destroy(gameObject, 1f);
        }
    }
}
