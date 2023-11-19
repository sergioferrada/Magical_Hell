using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BeanLevelStatistics : SkillLevelStatistics
{
    // Nuevas variables específicas para las clases hijas de PlayerAbility
    public float speed;

    public BeanLevelStatistics(float damage, float cooldown, float speed, float objectScale)
            : base(damage, cooldown, objectScale)
    {
        this.speed = speed;
    }
}

public class MrBeanAbilitie : PlayerAbility
{
    public float speed;
    public List<BeanLevelStatistics> SkillLevelsStats;

    protected override void Start()
    {
        SkillLevelsStats = new List<BeanLevelStatistics>
        {                         
                                 //  [D]     [CD]      [s]     [OS]  
            new BeanLevelStatistics(1.0f,    4.0f,    1.0f,    1.0f),  //Level 1
            new BeanLevelStatistics(1.5f,    3.75f,   1.0f,    1.0f),  //Level 2
            new BeanLevelStatistics(1.5f,    3.5f,    2.5f,    1.0f),  //Level 3
            new BeanLevelStatistics(2.0f,    3.25f,   2.5f,    1.15f),  //Level 4
            new BeanLevelStatistics(2.5f,    3.0f,    3.0f,    1.15f), //Level 5
            new BeanLevelStatistics(3.5f,    2.5f,    3.0f,    1.15f), //Level 6
            new BeanLevelStatistics(4.5f,    2.25f,   4.5f,    1.35f), //Level 7
            new BeanLevelStatistics(5.5f,    1.75f,   4.5f,    1.35f), //Level 8
            new BeanLevelStatistics(6.5f,    1.5f,    5.5f,    1.5f),  //Level 9
            new BeanLevelStatistics(7.0f,    1.0f,    6.0f,    1.5f),  //Level 10
        };

        base.Start();
    }

    protected override IEnumerator ActivateAbility()
    {
        var beanPrefab = Instantiate(objectAttack, transform.position, Quaternion.identity);
        beanPrefab.GetComponent<MrBeansLogic>().damage = damage;
        beanPrefab.GetComponent<MrBeansLogic>().objectScale = damageObjectScale;
        beanPrefab.GetComponent<MrBeansLogic>().speed = speed;
        beanPrefab.GetComponent<MrBeansLogic>().parentAbility = this;
        return base.ActivateAbility();
    }

    protected override void ApplySkillLevel()
    {
        base.ApplySkillLevel();
        BeanLevelStatistics currentSkillLevel = SkillLevelsStats[actualLevel - 1];

        damage = currentSkillLevel.damage;
        cooldown = currentSkillLevel.cooldown;
        damageObjectScale = currentSkillLevel.objectScale;
        speed = currentSkillLevel.speed;
    }
}
