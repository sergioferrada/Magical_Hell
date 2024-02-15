using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public class BoomerangLevelStatistics : SkillLevelStatistics
{
    public float speed;
    public float maxRange;

    public BoomerangLevelStatistics(float damage, float cooldown, float speed, float objectScale, float maxRange, float targetExp)
        : base(damage, cooldown, objectScale, targetExp) 
    {
        this.speed = speed;
        this.maxRange = maxRange;

    }
}

public class BoomerangAbilitie : PlayerAbility
{

    public float speed;
    public float maxRange;
    public List<BoomerangLevelStatistics> BoomerangLevelStats;
    protected override void Start()
    {
        BoomerangLevelStats = new List<BoomerangLevelStatistics>
        {
            new BoomerangLevelStatistics(.5f,    15f,   8.0f,     1.0f,   8,  15f),  //Level 1
            new BoomerangLevelStatistics(.5f,    15f,   8.0f,     1.0f,   9,  15f),  //Level 1
            new BoomerangLevelStatistics(.5f,    15f,   8.0f,     1.0f,   10,  15f),  //Level 1
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
        var boomerangLogic = projectile.GetComponent<BoomerangLogic>();

        Debug.Log("Instanciado");

        boomerangLogic.damage = damage;
        boomerangLogic.objectScale = damageObjectScale;
        boomerangLogic.speed = speed;
        boomerangLogic.maxRange = maxRange;
        boomerangLogic.parentAbility = this;
        boomerangLogic.playerLocation = transform; // Set playerLocation to the player's position

        return base.ActivateAbility();
    }

    protected override void ApplySkillLevel()
    {
        base.ApplySkillLevel();
        BoomerangLevelStatistics currentSkillLevel = BoomerangLevelStats[actualLevel - 1];

        damage = currentSkillLevel.damage;
        cooldown = currentSkillLevel.cooldown;
        damageObjectScale = currentSkillLevel.objectScale;
        speed = currentSkillLevel.speed;
        maxRange = currentSkillLevel.maxRange;
        targetExp = currentSkillLevel.targetExp;
    }

}
