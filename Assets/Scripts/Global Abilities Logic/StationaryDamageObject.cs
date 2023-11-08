using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryDamageObject : DamageObjectBase
{
    private void ActivateDamageArea()
    {
        collider2d.enabled = true;
    }

    private void DeactivateDamageArea()
    {
        collider2d.enabled = false;
    }
}
