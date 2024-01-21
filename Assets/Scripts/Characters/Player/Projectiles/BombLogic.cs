using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//requiere un collider2d pero desactivado, al pasar timer se activa
public class BombLogic : StationaryDamageObject
{
    public float timer = 2f;
    protected override void Start()
    {
        base.Start();
    }

    public IEnumerator TicTicBoom()
    {
        yield return new WaitForSeconds(timer);

        ActivateDamageArea();
    }
}