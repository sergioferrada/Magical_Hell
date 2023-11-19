using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FireballLevelStatics : SkillLevelStatistics
{
    // Nuevas variables específicas para las clases hijas de PlayerAbility
    public int timesItCanExplode;
    public int minRange, maxRange;

    public FireballLevelStatics(float damage, float cooldown, int timesItCanExplode, float objectScale, int minRange, int maxRange)
            : base(damage, cooldown, objectScale)
    {
        this.timesItCanExplode = timesItCanExplode;
        this.minRange = minRange;
        this.maxRange = maxRange;
    }
}

public class FireballAttackAbilitie : PlayerAbility
{
    [Header("Ability Stats (Child)")]
    public int timesItCanExplode;
    public int minRange, maxRange;
    public float targetProbability = 0.1f; // Valor inicial de la probabilidad

    public List<FireballLevelStatics> SkillLevelsStats;

    protected override void Start()
    {
        SkillLevelsStats = new List<FireballLevelStatics>
        {
            new FireballLevelStatics(1.5f,  1.75f, 1, 1.0f,  1, 1),  //Level 1
            new FireballLevelStatics(2.0f,  1.55f, 1, 1.0f,  1, 2),  //Level 2
            new FireballLevelStatics(3.0f,  1.55f, 2, 1.25f, 1, 2),  //Level 3
            new FireballLevelStatics(4.5f,  1.35f, 2, 1.25f, 2, 2),  //Level 4
            new FireballLevelStatics(5.5f,  1.35f, 2, 1.5f,  2, 3),  //Level 5
            new FireballLevelStatics(7.0f,  1.15f, 3, 1.5f,  2, 3),  //Level 6
            new FireballLevelStatics(8.0f,  1.15f, 3, 1.75f, 3, 3),  //Level 7
            new FireballLevelStatics(9.0f,  1.0f,  3, 1.75f, 3, 4),  //Level 8
            new FireballLevelStatics(10.0f, 0.9f,  3, 2.0f,  4, 5),  //Level 9
            new FireballLevelStatics(12.0f, 0.75f, 4, 2.0f,  5, 5),  //Level 10
        };

        base.Start();
    }

    protected override IEnumerator ActivateAbility()
    {
        SoundManager.Instance.PlaySound("Flame_Attack3", .4f);
        var projectile = Instantiate(objectAttack, transform.position, Quaternion.identity);
        projectile.GetComponent<FireballLogic>().damage = damage;
        projectile.GetComponent<FireballLogic>().objectScale = damageObjectScale;
        projectile.GetComponent<FireballLogic>().timesItcanExplode = timesItCanExplode;
        projectile.GetComponent<FireballLogic>().parentAbility = this;

        // Calcular si el proyectil debe apuntar al enemigo más cercano
        bool targetEnemy = Random.value < targetProbability;

        Vector3 targetDirection;
        if (targetEnemy)
        {
            // Obtener el enemigo más cercano
            Enemy nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                // Calcular la dirección hacia el enemigo más cercano
                targetDirection = (nearestEnemy.transform.position - transform.position).normalized;
                projectile.GetComponent<FireballLogic>().direction = targetDirection;
                targetProbability = 0.1f;
            }
        }

        // Aumentar la probabilidad poco a poco
        targetProbability += 0.01f; // Puedes ajustar este valor según tus necesidades

        return base.ActivateAbility();
    }

    protected override void ApplySkillLevel()
    {
        base.ApplySkillLevel();
        FireballLevelStatics currentSkillLevel = SkillLevelsStats[actualLevel - 1];

        damage = currentSkillLevel.damage;
        cooldown = currentSkillLevel.cooldown;
        damageObjectScale = currentSkillLevel.objectScale;
        timesItCanExplode = currentSkillLevel.timesItCanExplode;
        minRange = currentSkillLevel.minRange;
        maxRange = currentSkillLevel.maxRange;
    }
}

