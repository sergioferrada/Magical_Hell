using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeAbilitieItem : AbilityItemBase
{
    
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        animator.Play("Knife_collected_animation");
    }
}
