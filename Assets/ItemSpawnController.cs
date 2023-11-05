using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnController : MonoBehaviour
{
    [SerializeField] private GameObject[] ItemsPrefabs;
    private Animator animator;
    private BoxCollider2D bc2d;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        bc2d = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.Play("Open_Metal_Chest_Animation");
            bc2d.enabled = false;
        }
    }

    private void SpawnAbilityItem()
    {
        Instantiate(ItemsPrefabs[Random.Range(0,ItemsPrefabs.Length)], transform.position, Quaternion.identity);
    }

    private void DestroyChest()
    {
        Destroy(gameObject);
    }
}
