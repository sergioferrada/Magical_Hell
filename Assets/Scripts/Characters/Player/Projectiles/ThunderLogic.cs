using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderLogic : StationaryDamageObject
{
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
