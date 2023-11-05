using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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

    private Dictionary<DifficultManager.DifficultyLevel, DifficultyStats> difficultyStats;
    #endregion

    [Header("Item Spawn Stats")]
    [SerializeField] private GameObject healthItemPrefab;
    [SerializeField] [Range(0f, 100f)] private float spawnItemProbability;

    [Header("Exp Item Stats")]
    [SerializeField] private GameObject expItemPrefab;
    [SerializeField] private float expAmount;

    protected override void Awake()
    {
        base.Awake();
        playerTransform = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        difficultyStats = new Dictionary<DifficultManager.DifficultyLevel, DifficultyStats> {

            { DifficultManager.DifficultyLevel.Very_Easy,    Very_Easy   },
            { DifficultManager.DifficultyLevel.Easy,         Easy        },
            { DifficultManager.DifficultyLevel.Medium,       Medium      },
            { DifficultManager.DifficultyLevel.Hard,         Hard        },
            { DifficultManager.DifficultyLevel.Very_Hard,    Very_Hard   }
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
        DifficultyStats selectedStats = difficultyStats[DifficultManager.Instance.actualDifficultyLevel];

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

    protected override void CheckDeath()
    {
        if(Life <= 0)
        {
            SoundManager.Instance.PlaySound("Enemy_Blood_2", .5f);
        }
        else
        {
            SoundManager.Instance.PlaySound("Enemy_Hit");
        }

        base.CheckDeath();
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
        SpawnExpItems();
        base.Death();
    }

    protected void SpawnHealthItem()
    {
        // Genera un número aleatorio entre 0 y 100
        float randomValue = UnityEngine.Random.Range(0.0f, 100.0f);

        // Compara el número aleatorio con la probabilidad de spawn
        if (randomValue <= spawnItemProbability)
        {
            // Instancia el prefab si el número aleatorio es menor o igual a la probabilidad
            Instantiate(healthItemPrefab, transform.position, Quaternion.identity);
        }
    }

    protected void SpawnExpItems()
    {
        float itemsAmount = expAmount / expItemPrefab.GetComponent<ExperienceItem>().GetItemExpPoints();
        
        for(int i = 0; i < itemsAmount; i++)
        {
            Instantiate(expItemPrefab, transform.position, Quaternion.identity);
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("P.Proyectiles"))
            ReciveDamage(collision.gameObject.GetComponent<ProjectileLogic>().Damage);
    }
}
