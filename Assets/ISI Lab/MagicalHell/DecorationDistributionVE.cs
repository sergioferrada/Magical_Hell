using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[LBSCustomEditor("DecorationDistribution", typeof(DecorationDistribution))]

public class DecorationDistributionVE : LBSCustomEditor
{
    DynamicFoldout spawnCharacteristic;
    DynamicFoldout decorationCharacteristic;
    DynamicFoldout decoration2Characteristic;

    public DecorationDistributionVE(object Target) : base(Target)
    {
        Add(CreateVisualElement());
        SetInfo(target);
    }

    public override void SetInfo(object target)
    {
        this.target = target;
        var eval = target as DecorationDistribution;

        if (eval.spawnCharacteristic != null)
        {
            spawnCharacteristic.SetInfo(eval.spawnCharacteristic);
        }
        if (eval.decorationCharacteristic != null)
        {
            decorationCharacteristic.SetInfo(eval.decorationCharacteristic);
        }
        if (eval.decoration2Characteristic != null)
        {
            decoration2Characteristic.SetInfo(eval.decoration2Characteristic);
        }
    }

    protected override VisualElement CreateVisualElement()
    {
        var ve = new VisualElement();
        var eval = target as DecorationDistribution;

        spawnCharacteristic = new DynamicFoldout(typeof(LBSCharacteristic));
        spawnCharacteristic.Label = "Spawn Characteristic";

        if(eval != null && eval.spawnCharacteristic != null)
        {
            spawnCharacteristic.Data = eval.spawnCharacteristic;
        }

        spawnCharacteristic.OnChoiceSelection += () => { eval.spawnCharacteristic = spawnCharacteristic.Data as LBSCharacteristic; };

        decorationCharacteristic = new DynamicFoldout(typeof(LBSCharacteristic));
        decorationCharacteristic.Label = "Decoration Characteristic";

        if (eval != null && eval.decorationCharacteristic != null)
        {
            decorationCharacteristic.Data = eval.decorationCharacteristic;
        }

        decorationCharacteristic.OnChoiceSelection += () => { eval.decorationCharacteristic = decorationCharacteristic.Data as LBSCharacteristic; };

        decoration2Characteristic = new DynamicFoldout(typeof(LBSCharacteristic));
        decoration2Characteristic.Label = "Decoration 2 Characteristic";

        if (eval != null && eval.decoration2Characteristic != null)
        {
            decoration2Characteristic.Data = eval.decoration2Characteristic;
        }

        decoration2Characteristic.OnChoiceSelection += () => { eval.decoration2Characteristic = decoration2Characteristic.Data as LBSCharacteristic; };

        ve.Add(spawnCharacteristic);
        ve.Add(decorationCharacteristic);
        ve.Add(decoration2Characteristic);

        return ve;
    }
}
