using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrBeansAbilitieItem : MonoBehaviour
{
    [SerializeField] private GameObject Bean;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.AddComponent<MrBeanAbilitie>();
            collision.gameObject.GetComponent<MrBeanAbilitie>().bean = Bean;
            animator.Play("MrBean_collection_item_animation");
            Destroy(gameObject, 1f);
        }
    }
}
