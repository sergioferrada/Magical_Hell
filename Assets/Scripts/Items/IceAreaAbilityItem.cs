using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAreaAbilityItem : AbilityItemBase
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        animator.Play("Ice_Area_Collection_Item_Animation");
        base.OnTriggerEnter2D(collision);
    }
}
