using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbilityItemBase : MonoBehaviour
{
    [SerializeField] public PlayerAbility abilityToAdd;
    [SerializeField] public SpriteRenderer IconChild;

    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected void Start()
    {
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent(abilityToAdd.GetType()) == null)
            {
                var auxRef = collision.gameObject.AddComponent(abilityToAdd.GetType());

                System.Reflection.FieldInfo[] fields = abilityToAdd.GetType().GetFields();

                foreach (var item in fields)
                {
                    item.SetValue(auxRef, item.GetValue(abilityToAdd));
                }    
            }
            else
            {
                var playerAbilities = collision.gameObject.GetComponents<PlayerAbility>();

                foreach (var item in playerAbilities)
                {
                    if(item.GetType() == abilityToAdd.GetType())
                    {
                        item.LevelUp();
                        break;
                    }
                }
            }

            animator.Play("Collected_Animation");
            SoundManager.Instance.PlaySound("eminyildirim_Holy_1_Sound");
            Destroy(gameObject, 1f);
        }
    }
}
