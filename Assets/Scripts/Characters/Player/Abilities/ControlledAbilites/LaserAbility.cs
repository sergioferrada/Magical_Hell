using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LaserAbility : PlayerAbility
{
    public MeleePlayerAttack laser;

    private void Update()
    {
        
            //rotar con el jugador
            Vector2 to = PlayerController.Instance.direction;
            float z = Mathf.Atan2(to.y, to.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, 0, z);

            //tirar un raycast para las paredes
            //RaycastHit2D wall = Physics2D.Raycast(transform.position,PlayerController.Instance.direction,100,LayerMask.GetMask("Wall"));

            //Debug.DrawRay(transform.position, PlayerController.Instance.lastDirection * 10, Color.red, 0, false);

            //cambiar longitud segun raycast
           // transform.localScale = new Vector3(1, wall.distance, 1);
            //Debug.Log("Distance To Wall: " + wall.distance);
       
    }

    protected override void Start()
    {
        auto = false;
        base.Start();
    }

    protected override IEnumerator ActivateAbility()
    {
        laser.Activate();
        return base.ActivateAbility();
    }
}