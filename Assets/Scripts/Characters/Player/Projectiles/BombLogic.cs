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
    public float lingerTime = 2f;
    protected override void Start()
    {

        base.Start();

        StartCoroutine(TicTicBoom());
    }

    public void AssignAbility(PlayerAbility a)
    {
        Debug.Log("Bomb ability assigned");
        parentAbility = a;
    }

    public IEnumerator TicTicBoom()
    {
        yield return new WaitForSeconds(timer);

        ActivateDamageArea();

        yield return new WaitForSeconds(lingerTime);

        Destroy(gameObject);
    }
}