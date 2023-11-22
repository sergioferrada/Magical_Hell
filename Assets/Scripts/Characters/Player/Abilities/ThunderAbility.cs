using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ThunderLevelStatistics : SkillLevelStatistics
{
    // Nuevas variables específicas para las clases hijas de PlayerAbility
    public int numberOfProjectiles;
    public float radius;

    public ThunderLevelStatistics(float damage, float cooldown, int numberOfProjectiles, float objectScale, float radius, float targetExp)
            : base(damage, cooldown, objectScale, targetExp)
    {
        this.numberOfProjectiles = numberOfProjectiles;
        this.radius = radius;
    }
}

public class ThunderAbility : PlayerAbility
{
    [Header("Ability Stats (Child)")]
    public int numberOfProjectiles;
    public float radius;
    public float targetProbability = 0.5f; // Valor inicial de la probabilidad

    public List<ThunderLevelStatistics> SkillLevelsStats;

    protected override void Start()
    {
        SkillLevelsStats = new List<ThunderLevelStatistics>
        {                         
                                    //  [D]     [CD]    [NP]    [OS]    [R] [EXP]
            new ThunderLevelStatistics(2.0f,   5.0f,    2,     1.0f,   5.0f, 4f),  //Level 1
            new ThunderLevelStatistics(3.0f,   4.5f,    2,     1.0f,   5.0f, 8f),  //Level 2
            new ThunderLevelStatistics(3.0f,   4.5f,    3,     1.0f,   5.5f, 12f),  //Level 3
            new ThunderLevelStatistics(3.5f,   4.0f,    3,     1.0f,   6.0f, 18f),  //Level 4
            new ThunderLevelStatistics(4.5f,   4.0f,    4,     1.15f,  6.5f, 24f),  //Level 5
            new ThunderLevelStatistics(5.0f,   3.75f,   4,     1.15f,  7.0f, 30f),  //Level 6
            new ThunderLevelStatistics(6.0f,   3.5f,    4,     1.15f,  7.0f, 45f),  //Level 7
            new ThunderLevelStatistics(6.5f,   3.0f,    4,     1.35f,  8.0f, 50f),  //Level 8
            new ThunderLevelStatistics(7.5f,   2.5f,    5,     1.5f,   8.0f, 57f),  //Level 9
            new ThunderLevelStatistics(8.0f,   2.0f,    6,     1.5f,   8.0f, 65f),  //Level 10
        };

        base.Start();
    }

    protected override IEnumerator ActivateAbility()
    {
        SoundManager.Instance.PlaySound("Electric_Impact");
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            float randomAngle = Random.Range(0f, 360f);
            float randomRadius = Random.Range(0f, radius);

            Vector3 direction = Quaternion.Euler(0f, 0f, randomAngle) * Vector3.up;
            Vector3 spawnPosition = transform.position + direction * randomRadius;

            // Calcular si el proyectil debe apuntar al enemigo más cercano
            bool targetEnemy = Random.value < targetProbability;

            if (targetEnemy)
            {
                // Obtener el enemigo más cercano
                Enemy nearestEnemy = FindNearestEnemy();
                if (nearestEnemy != null)
                {
                    // Calcular la posicion hacia el enemigo más cercano
                    spawnPosition = nearestEnemy.transform.position;
                    targetProbability = 0.05f;
                }
            }

            var projectile = Instantiate(objectAttack, spawnPosition, Quaternion.identity);
            projectile.GetComponent<ThunderLogic>().damage = damage;
            projectile.GetComponent<ThunderLogic>().objectScale = damageObjectScale;
            projectile.GetComponent<ThunderLogic>().parentAbility = this;
        }

        // Aumentar la probabilidad poco a poco
        targetProbability += 0.01f; // Puedes ajustar este valor según tus necesidades

        return base.ActivateAbility();
    }

    protected override void ApplySkillLevel()
    {
        base.ApplySkillLevel();
        ThunderLevelStatistics currentSkillLevel = SkillLevelsStats[actualLevel - 1];

        damage = currentSkillLevel.damage;
        cooldown = currentSkillLevel.cooldown;
        damageObjectScale = currentSkillLevel.objectScale;
        numberOfProjectiles = currentSkillLevel.numberOfProjectiles;
        radius = currentSkillLevel.radius;
        targetExp = currentSkillLevel.targetExp;
    }
}
