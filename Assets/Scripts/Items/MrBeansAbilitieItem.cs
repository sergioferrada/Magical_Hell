using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrBeansAbilitieItem : AbilityItemBase
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);   
        animator.Play("MrBean_collection_item_animation");
    }
}
