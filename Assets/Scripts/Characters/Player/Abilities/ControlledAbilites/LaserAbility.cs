using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LaserAbility : PlayerAbility
{
    protected override void Start()
    {
        auto = false;
        base.Start();
    }

    protected override IEnumerator ActivateAbility()
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();

        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        filter.SetLayerMask(LayerMask.GetMask("Enemies"));

        //tirar un raycast para las paredes
        RaycastHit2D wall = Physics2D.Raycast(transform.position, PlayerController.Instance.lastDirection,100,LayerMask.GetMask("Wall"));


        //tirar un raycast para tocar enemigos
        Vector2 origin = (Vector2)transform.position + PlayerController.Instance.lastDirection;
        int h = Physics2D.Raycast(origin, PlayerController.Instance.lastDirection, new ContactFilter2D().NoFilter(), hits, wall.distance);

        Debug.DrawRay(origin, PlayerController.Instance.lastDirection*wall.distance, Color.red, 1.5f, false);

        Debug.Log(""+wall.distance);

        foreach (RaycastHit2D r in hits)
        {
            Enemy e = r.collider.gameObject.GetComponent<Enemy>();
            if (e != null) e.ReciveDamage(damage);
        }

        return base.ActivateAbility();
    }
}