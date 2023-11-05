using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAbilitieItem : AbilityItemBase
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        animator.Play("Fireball_collected_Item_animation");    
    }
}
