using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAbilitieItem : MonoBehaviour
{
    [SerializeField] private GameObject projectileAbilitie;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { 

            collision.gameObject.AddComponent<FireballAttackAbilitie>();
            collision.gameObject.GetComponent<FireballAttackAbilitie>().fireball = projectileAbilitie;
            animator.Play("Fireball_collected_Item_animation");
            Destroy(gameObject, 1f);
        }    
    }
}
