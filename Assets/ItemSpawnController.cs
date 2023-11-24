using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSpawnController : MonoBehaviour
{
    [SerializeField] private GameObject ItemsPrefab;
    [SerializeField] private PlayerAbility[] AbilitiesPrefabs;
    [SerializeField] private bool spawnRepeatedItems = true;

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
        if (spawnRepeatedItems)
        {
            var item = Instantiate(ItemsPrefab, transform.position - new Vector3(0.2f, 0.5f), Quaternion.identity);
            var ability = AbilitiesPrefabs[Random.Range(0, AbilitiesPrefabs.Length)];
            item.GetComponent<AbilityItemBase>().abilityToAdd = ability;
            item.GetComponent<AbilityItemBase>().IconChild.sprite = ability.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            var playerAbilities = player.GetComponents<PlayerAbility>();

            List<PlayerAbility> list = AbilitiesPrefabs.ToList();

            for (int i = 0; i < AbilitiesPrefabs.Length; i++)
            {
                foreach (var abilitie in playerAbilities)
                {
                    if (abilitie.GetType() == AbilitiesPrefabs[i].GetType())
                    {
                        list.Remove(AbilitiesPrefabs[i]);
                    }
                }
            }

            if(list.Count != 0)
                AbilitiesPrefabs = list.ToArray();

            var item = Instantiate(ItemsPrefab, transform.position - new Vector3(0.2f, 0.5f), Quaternion.identity);
            var ability = AbilitiesPrefabs[Random.Range(0, AbilitiesPrefabs.Length)];
            item.GetComponent<AbilityItemBase>().abilityToAdd = ability;
            item.GetComponent<AbilityItemBase>().IconChild.sprite = ability.GetComponent<SpriteRenderer>().sprite;
        }
    }

    private void DestroyChest()
    {
        Destroy(gameObject);
    }
}
