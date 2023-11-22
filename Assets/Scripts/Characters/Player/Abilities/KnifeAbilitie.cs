using System.Collections;
using System.Collections.Generic;
using System.Speech.Synthesis;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class KnifeLevelStatistics : SkillLevelStatistics
{
    // Nuevas variables específicas para las clases hijas de PlayerAbility
    public float speed;
    public int maxCollidableObjects;

    public KnifeLevelStatistics(float damage, float cooldown, float speed, float objectScale, int maxCollidableObjects, float targetExp)
            : base(damage, cooldown, objectScale, targetExp)
    {
        this.speed = speed;
        this.maxCollidableObjects = maxCollidableObjects;
    }
}

public class KnifeAbilitie : PlayerAbility
{
    private PlayerController player;
    private Vector2 knifeDirection = Vector2.right;

    public float speed;
    public int maxCollidableObjects;
    public List<KnifeLevelStatistics> SkillLevelsStats;

    protected override void Start()
    {
        player = GetComponent<PlayerController>();

        SkillLevelsStats = new List<KnifeLevelStatistics>
        {                         
                                 //  [D]     [CD]     [s]       [OS]   [CO] [EXP]
            new KnifeLevelStatistics(.5f,    1.25f,   8.0f,     1.0f,   1,  15f),  //Level 1
            new KnifeLevelStatistics(.75f,   1.25f,   8.0f,     1.0f,   1,  25f),  //Level 2
            new KnifeLevelStatistics(.75f,   1.0f,    8.5f,     1.0f,   2,  40f),  //Level 3
            new KnifeLevelStatistics(1.0f,   1.0f,    8.5f,     1.0f,   2,  55f),  //Level 4
            new KnifeLevelStatistics(1.0f,    .9f,    9.0f,     1.15f,  3,  75f),  //Level 5
            new KnifeLevelStatistics(1.25f,   .9f,    9.0f,     1.15f,  4,  90f),  //Level 6
            new KnifeLevelStatistics(1.25f,  .75f,    9.5f,     1.15f,  5,  110f),  //Level 7
            new KnifeLevelStatistics(1.5f,   .75f,    10.0f,    1.35f,  6,  140f),  //Level 8
            new KnifeLevelStatistics(1.5f,   .75f,    11.5f,     1.5f,  7,  150f),  //Level 9
            new KnifeLevelStatistics(2.0f,    .5f,    13.5f,     1.5f,  8,  200f),  //Level 10
        };

        base.Start();
    }

    private void Update()
    {
        if (player.direction != Vector2.zero)
            knifeDirection = player.direction;
    }

    protected override IEnumerator ActivateAbility()
    {
        SoundManager.Instance.PlaySound("Knife_Slice_Sound", .4f);
        var projectile = Instantiate(objectAttack, transform.position, Quaternion.identity);
        projectile.GetComponent<KnifeLogic>().damage = damage;
        projectile.GetComponent<KnifeLogic>().objectScale = damageObjectScale;
        projectile.GetComponent<KnifeLogic>().speed = speed;
        projectile.GetComponent<KnifeLogic>().maxCollidableObjects = maxCollidableObjects;
        projectile.GetComponent<KnifeLogic>().direction = knifeDirection;
        projectile.GetComponent<KnifeLogic>().parentAbility = this;

        return base.ActivateAbility();
    }

    protected override void ApplySkillLevel()
    {
        base.ApplySkillLevel();
        KnifeLevelStatistics currentSkillLevel = SkillLevelsStats[actualLevel - 1];

        damage = currentSkillLevel.damage;
        cooldown = currentSkillLevel.cooldown;
        damageObjectScale = currentSkillLevel.objectScale;
        speed = currentSkillLevel.speed;
        maxCollidableObjects = currentSkillLevel.maxCollidableObjects;
        targetExp = currentSkillLevel.targetExp;
    }
}
