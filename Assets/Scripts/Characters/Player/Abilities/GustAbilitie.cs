using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GustLevelStatistics : SkillLevelStatistics
{
    public float maxRange;

    public GustLevelStatistics(float damage, float cooldown, float speed, float objectScale, float maxRange, float targetExp, float slowPercentage)
        : base(damage, cooldown, objectScale, targetExp)
    {
        this.maxRange = maxRange;

    }
}


public class GustAbilitie : PlayerAbility
{
    public float speed;
    public float maxRange;
    public List<GustLevelStatistics> GustLevelStats;
    protected override void Start()
    {
        GustLevelStats = new List<GustLevelStatistics>
        {
            new GustLevelStatistics(0f,    2f,   8.0f,     1.0f,   8,  15f, 0.15f),  //Level 1
            new GustLevelStatistics(0f,    15f,   8.0f,     1.0f,   9,  15f,  0.15f),  //Level 1
            new GustLevelStatistics(0f,    15f,   8.0f,     1.0f,   10,  15f, 0.15f),  //Level 1
        };
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override IEnumerator ActivateAbility()
    {
        var projectile = Instantiate(objectAttack, transform.position, Quaternion.identity);
        var GustLogic = projectile.GetComponent<GustLogic>();

        Debug.Log("Instanciado");

        GustLogic.damage = damage;
        GustLogic.objectScale = damageObjectScale;
        GustLogic.speed = speed;
        GustLogic.maxRange = maxRange;
        GustLogic.parentAbility = this;

        return base.ActivateAbility();
    }

    protected override void ApplySkillLevel()
    {
        base.ApplySkillLevel();
        GustLevelStatistics currentSkillLevel = GustLevelStats[actualLevel - 1];

        damage = currentSkillLevel.damage;
        cooldown = currentSkillLevel.cooldown;
        damageObjectScale = currentSkillLevel.objectScale;
        maxRange = currentSkillLevel.maxRange;
        targetExp = currentSkillLevel.targetExp;
    }
}
