using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExtendedSkillLevel : SkillLevel
{
    // Nuevas variables específicas para las clases hijas de PlayerAbility
    public int timesItCanExplodeIncrease;
    public int newVariable2;

    public ExtendedSkillLevel(int level, float damageIncrease, float cooldownReduction, float objectScaleIncrease, int timesItCanExplodeIncrease, int newVariable2)
        : base(level, damageIncrease, cooldownReduction, objectScaleIncrease)
    {
        this.timesItCanExplodeIncrease = timesItCanExplodeIncrease;
        this.newVariable2 = newVariable2;
    }
}

public class FireballAttackAbilitie : PlayerAbility
{
    [SerializeField] private List<ExtendedSkillLevel> SkillLevels;
    private int timesItCanExplode;

    protected override IEnumerator ActivateAbility()
    {
        SoundManager.Instance.PlaySound("Flame_Attack3", .6f);
        var projectile = Instantiate(objectAttack, transform.position, Quaternion.identity);
        projectile.GetComponent<FireballLogic>().damage = damage;
        projectile.GetComponent<FireballLogic>().objectScale = damageObjectScale;
        projectile.GetComponent<FireballLogic>().timesItcanExplode = timesItCanExplode;
        projectile.GetComponent<FireballLogic>().parentAbility = this;
        return base.ActivateAbility();
    }

    protected override void ApplySkillLevel()
    {
        base.ApplySkillLevel();
        ExtendedSkillLevel currentSkillLevel = SkillLevels[actualLevel - 1];

        damage = currentSkillLevel.damageIncrease;
        cooldown = currentSkillLevel.cooldownReduction;
        damageObjectScale = currentSkillLevel.objectScaleIncrease;
        timesItCanExplode = currentSkillLevel.timesItCanExplodeIncrease;
    }
}

