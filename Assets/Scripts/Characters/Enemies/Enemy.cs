using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using static GameManager;
using UnityEngine;
using System;

[Serializable]
public class DifficultyStats
{
                      public float lifeExtraPoints;
    [Range(1f, 500f)] public float damageMultiplicator;
    [Range(1f, 500f)] public float speedMultiplicator;
    [Range(1f, 500f)] public float attackDelayMultiplicator;
                      public Color color;
}


public class Enemy : CharacterBase
{
    #region COMPONENETES
    protected Transform playerTransform;
    #endregion

    #region STATS MODIFIERS
    [Header("Porcent Modifiers Stats")]
    [SerializeField] private DifficultyStats Very_Easy;
    [SerializeField] private DifficultyStats Easy;
    [SerializeField] private DifficultyStats Medium;
    [SerializeField] private DifficultyStats Hard;
    [SerializeField] private DifficultyStats Very_Hard;

    private Dictionary<DifficultyLevel, DifficultyStats> difficultyStats;
    #endregion

    [Header("Item Spawn Stats")]
    [SerializeField] private GameObject healthItemPrefab;
    [SerializeField] [Range(0f, 100f)] private float spawnItemProbability; 

    protected override void Awake()
    {
        base.Awake();
        playerTransform = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        difficultyStats = new Dictionary<DifficultyLevel, DifficultyStats> {

            { DifficultyLevel.Very_Easy,    Very_Easy   },
            { DifficultyLevel.Easy,         Easy        },
            { DifficultyLevel.Medium,       Medium      },
            { DifficultyLevel.Hard,         Hard        },
            { DifficultyLevel.Very_Hard,    Very_Hard   }
        };

        SetupCharacterStats();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected virtual void Update()
    {
       SetLookDirection();
    }

    protected virtual void SetupCharacterStats()
    {
        DifficultyStats selectedStats = difficultyStats[difficultyLevel];

        Life += selectedStats.lifeExtraPoints;
        Damage  += (Damage * (selectedStats.damageMultiplicator / 100));
        MovementSpeed += (MovementSpeed *(selectedStats.speedMultiplicator / 100));
        AttackDelay += (AttackDelay * (selectedStats.attackDelayMultiplicator / 100));
        spriteRenderer.color = selectedStats.color;
    }

    private void SetLookDirection()
    {
        if (direction.x >= 0)
            transform.localScale = new Vector2(1, transform.localScale.y);
        else if (direction.x < 0)
            transform.localScale = new Vector2(-1, transform.localScale.y);
    }

    protected virtual void ShortRangeAttack()
    {
        Collider2D[] hitCharacters = Physics2D.OverlapCircleAll(transform.position, attackRange, attackableLayers);
        foreach (Collider2D character in hitCharacters)
        {
            character.GetComponent<PlayerController>().ReciveDamage(Damage);
            Vector2 direction = (character.GetComponent<Collider2D>().transform.position - transform.position).normalized;
            character.GetComponent<Rigidbody2D>().AddForce(direction * 25, ForceMode2D.Impulse);
        }
    }

    protected virtual void MidRangeAttack()
    {

    }

    protected virtual void LongRangeAttack()
    {

    }

    protected void ResetAttack()
    {
        passedTime = 0;
    }

    protected override void Death()
    {
        SpawnHealthItem();
        base.Death();
    }

    protected void SpawnHealthItem()
    {
        // Genera un n�mero aleatorio entre 0 y 100
        float randomValue = UnityEngine.Random.Range(0.0f, 100.0f);

        // Compara el n�mero aleatorio con la probabilidad de spawn
        if (randomValue <= spawnItemProbability)
        {
            // Instancia el prefab si el n�mero aleatorio es menor o igual a la probabilidad
            Instantiate(healthItemPrefab, transform.position, Quaternion.identity);
        }
    }

    protected void ChaseGameObject(Transform objectiveTransform)
    {
        //chasing the player
        SetState(State.Move);
        direction = objectiveTransform.position - transform.position;
        direction.Normalize();
        transform.position = Vector2.MoveTowards(transform.position, objectiveTransform.position, MovementSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (transform == null)
            return;

        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {   
        if (collision.gameObject.name == "Fireball")
            ReciveDamage(collision.gameObject.GetComponent<ProjectileLogic>().Damage);
    }
}
