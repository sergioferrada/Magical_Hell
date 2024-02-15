using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryDamageObject : DamageObjectBase
{
    protected void ActivateDamageArea()
    {
        collider2d.enabled = true;
    }

    protected void DeactivateDamageArea()
    {
        collider2d.enabled = false;
    }
}
