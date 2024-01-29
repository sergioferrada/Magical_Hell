using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SlowLevelStatics : SkillLevelStatistics
{
 
    public SlowLevelStatics(float damage, float cooldown, float objectScale, float targetExp)
        : base(damage, cooldown, objectScale, targetExp) 

        {

        }

}

public class SlowAbilitie : PlayerAbility
{
    [Header("Ability Stats (Child)")]
    public Transform playerTransform;
    public List<SlowLevelStatics> SlowLevelsStats;
    public bool isActive = false;
    protected override void Start()
    {
        SlowLevelsStats = new List<SlowLevelStatics>
        {                       //   [D]     [CD] [TCE] [OB]  [MAX][MIN][EXP]
            new SlowLevelStatics(0.0f,    5,  1.0f,     7f),  //Level 1
            new SlowLevelStatics(0.0f,    4,  1.5f,     14f),  //Level 2
            new SlowLevelStatics(0.0f,    3,  2.0f,    27f),  //Level 3
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

    protected override IEnumerator ActivateAbility()
    {
        if (isActive)
        {
            // Se activo, hacer nada
            yield break;
        }

        isActive = true;

        var projectile = Instantiate(objectAttack, transform.position, Quaternion.identity);
        projectile.GetComponent<SlowLogic>().damage = damage;
        projectile.GetComponent<SlowLogic>().objectScale = damageObjectScale;
        projectile.GetComponent<SlowLogic>().parentAbility = this;
    
        projectile.GetComponent<SlowLogic>().SetPlayerTransform(playerTransform);

        //UpdateProjectileStats(projectile);
        yield return base.ActivateAbility();

    }
}
