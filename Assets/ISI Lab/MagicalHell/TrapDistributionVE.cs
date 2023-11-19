using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[LBSCustomEditor("TrapDistribution", typeof(TrapDistribution))]

public class TrapDistributionVE : LBSCustomEditor
{
    DynamicFoldout SpawnCharacteristic;
    DynamicFoldout trapsCharacteristic;
    public TrapDistributionVE(object Target) : base(Target)
    {
        Add(CreateVisualElement());
        SetInfo(target);
    }

    public override void SetInfo(object target)
    {
        this.target = target;
        var eval = target as TrapDistribution;
        if (eval.SpawnCharacteristic != null)
        {
            SpawnCharacteristic.SetInfo(eval.SpawnCharacteristic);
        }
        if (eval.trapsCharacteristic != null)
        {
            trapsCharacteristic.SetInfo(eval.trapsCharacteristic);
        }
    }

    protected override VisualElement CreateVisualElement()
    {
        var ve = new VisualElement();
        var eval = target as TrapDistribution;

        SpawnCharacteristic = new DynamicFoldout(typeof(LBSCharacteristic));
        SpawnCharacteristic.Label = "Spawn Characteristic";

        if (eval != null && eval.SpawnCharacteristic != null)
        {
            SpawnCharacteristic.Data = eval.SpawnCharacteristic;
        }

        SpawnCharacteristic.OnChoiceSelection += () => { eval.SpawnCharacteristic = SpawnCharacteristic.Data as LBSCharacteristic; };

        trapsCharacteristic = new DynamicFoldout(typeof(LBSCharacteristic));
        trapsCharacteristic.Label = "Trap Characteristic";

        if (eval != null && eval.trapsCharacteristic != null)
        {
            trapsCharacteristic.Data = eval.trapsCharacteristic;
        }

        trapsCharacteristic.OnChoiceSelection += () => { eval.trapsCharacteristic = trapsCharacteristic.Data as LBSCharacteristic; };

        ve.Add(SpawnCharacteristic);
        ve.Add(trapsCharacteristic);

        return ve;
    }
}
