using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAreaAbilitiy : PlayerAbility
{
    public List<SkillLevelStatistics> SkillLevelsStats;

    protected override void Start()
    {
        SkillLevelsStats = new List<SkillLevelStatistics>
        {
                                 //  [D]     [CD]     [OS]  [EXP]
            new SkillLevelStatistics(2.0f,   3.0f,   1.0f,   5f),  //Level 1
            new SkillLevelStatistics(2.0f,   2.75f,  1.0f,   10f),  //Level 2
            new SkillLevelStatistics(2.5f,   2.75f,  1.1f,   15f),  //Level 3
            new SkillLevelStatistics(2.5f,   1.65f,  1.2f,   20f), //Level 4
            new SkillLevelStatistics(3.0f,   1.65f,  1.3f,   30f), //Level 5
            new SkillLevelStatistics(3.0f,   1.5f,   1.4f,   40f), //Level 6
            new SkillLevelStatistics(4.5f,   1.25f,  1.5f,   60f), //Level 7
            new SkillLevelStatistics(5.5f,   1.25f,  1.6f,   90f), //Level 8
            new SkillLevelStatistics(6.5f,   1.0f,   1.7f,   130f),  //Level 9
            new SkillLevelStatistics(7.0f,   1.0f,   1.8f,   150f),  //Level 10
        };

        base.Start();
    }

    protected override IEnumerator ActivateAbility()
    {
        var projectile = Instantiate(objectAttack, transform.position, Quaternion.identity);
        projectile.GetComponent<IceAreaLogic>().damage = damage;
        projectile.GetComponent<IceAreaLogic>().objectScale = damageObjectScale;
        projectile.GetComponent<IceAreaLogic>().parentAbility = this;
        return base.ActivateAbility();
    }

    protected override void ApplySkillLevel()
    {
        base.ApplySkillLevel();
        SkillLevelStatistics currentSkillLevel = SkillLevelsStats[actualLevel - 1];

        damage = currentSkillLevel.damage;
        cooldown = currentSkillLevel.cooldown;
        damageObjectScale = currentSkillLevel.objectScale;
        targetExp = currentSkillLevel.targetExp;
    }
}
