using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LichLevelStatics : SkillLevelStatistics
{
    // Nuevas variables específicas para las clases hijas de PlayerAbility
    /*  public int TimesItBounces;
      public int minRange, maxRange;

      public LichLevelStatics(float damage, float cooldown, int TimesItBounces, float objectScale, int minRange, int maxRange, float targetExp)
              : base(damage, cooldown, objectScale, targetExp)
      {
          this.TimesItBounces = TimesItBounces;
          this.minRange = minRange;
          this.maxRange = maxRange;
      } */
    public float radius;
    public float cd;

    public LichLevelStatics(float damage, float cooldown, float objectScale, float targetExp, float radius)
        : base(damage, cooldown, objectScale, targetExp)
    {
        this.radius = radius;
        this.cd = cooldown;
    }


}

public class LichAbilitie : PlayerAbility
{
    [Header("Ability Stats (Child)")]
    public float radius = 5f;
    public float cd = 2f;
    public List<LichLevelStatics> SkillLevelsStats;
    public float targetProbability = 0.1f; //prob de apuntar a un enemigo

    protected override void Start()
    {
        SkillLevelsStats = new List<LichLevelStatics>
        {                       //   [D]     [CD] [TCE] [OB]  [MAX][MIN][EXP]
            new LichLevelStatics(1.5f,    2,  1.0f,     7f,  5f),  //Level 1
            new LichLevelStatics(2.0f,    2,  1.0f,     14f,  6f),  //Level 2
            new LichLevelStatics(3.0f,    1,  1.25f,    27f,  7f),  //Level 3
          /*  new FireballLevelStatics(4.5f,  1.35f,  2,  1.25f,  2,  2,  45f),  //Level 4
            new FireballLevelStatics(5.5f,  1.35f,  2,  1.5f,   2,  3,  57f),  //Level 5
            new FireballLevelStatics(7.0f,  1.15f,  3,  1.5f,   2,  3,  70f),  //Level 6
            new FireballLevelStatics(8.0f,  1.15f,  3,  1.75f,  3,  3,  90f),  //Level 7
            new FireballLevelStatics(9.0f,  1.0f,   3,  1.75f,  3,  4,  110f),  //Level 8
            new FireballLevelStatics(10.0f, 0.9f,   3,  2.0f,   4,  5,  150f),  //Level 9
            new FireballLevelStatics(12.0f, 0.75f,  4,  2.0f,   5,  5,  170f),  //Level 10 */
        };

        base.Start();
    }

   /*   protected override IEnumerator ActivateAbility()
      {
          SoundManager.Instance.PlaySound("Flame_Attack3", .4f);

          // Instantiate the projectile
          var projectile = Instantiate(objectAttack, transform.position, Quaternion.identity);
          // Check if the instantiation was successful
          if (projectile != null)
          {
              LichLogic projectileScript = projectile.GetComponent<LichLogic>();

              // Set the direction of the projectile (you can modify this logic as needed)
              Vector3 targetDirection = UnityEngine.Random.onUnitSphere;
              targetDirection.z = 0f; // Ensure the z-component is 0 for 2D
              projectileScript.direction = targetDirection.normalized;

              // Set other projectile properties if needed, e.g., speed
              projectileScript.speed = 5f;
          }
          else
          {
              Debug.LogError("Projectile instantiation failed!");
          }

          yield return null;
      } */

    protected override IEnumerator ActivateAbility()
    {

        bool targetEnemy = UnityEngine.Random.value < targetProbability;
        var projectile = Instantiate(objectAttack, transform.position, Quaternion.identity).GetComponent<LichLogic>();
        Vector3 targetDirection;

        //si persigue al enemigo
        if (targetEnemy)
        {
            // Obtener el enemigo más cercano
            Enemy nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                projectile.SetTarget(nearestEnemy.gameObject);

                // Calcular la dirección hacia el enemigo más cercano
                targetDirection = (nearestEnemy.transform.position - transform.position).normalized;
                projectile.direction = targetDirection;
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
        LichLevelStatics currentSkillLevel = SkillLevelsStats[actualLevel - 1];

        damage = currentSkillLevel.damage;
        cooldown = currentSkillLevel.cooldown;
        damageObjectScale = currentSkillLevel.objectScale;
        targetExp = currentSkillLevel.targetExp;
    }
}