using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttackLogic : MeleePlayerAttack
{
    public SpriteRenderer sprite;
    public bool activated = false;
    public Vector3 offset;

    public void ActivateLaser()
    {
        activated = true;
        sprite.enabled = true;
        collider2d.enabled = true;
    }

    public void Deactivate()
    {
        activated = false;
        sprite.enabled = false;
        collider2d.enabled = false; 
    }

    private void Update()
    {
        if (activated)
        {
            //tirar un raycast para las paredes
            RaycastHit2D wall = Physics2D.Raycast(
                transform.position,
                PlayerController.Instance.direction,
                100,
                LayerMask.GetMask("Wall")
                );

            //Debug.DrawRay(transform.position, PlayerController.Instance.lastDirection * 10, Color.red, 0, false);

            //cambiar longitud segun raycast
            transform.localPosition = new Vector3(0, wall.distance/2, 0)+offset;
            transform.localScale = new Vector3(1,wall.distance*objectScale,1);
            //Debug.Log("Distance To Wall: "+wall.distance);

            
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            DifficultManager.Instance.AddSuccefulAttack();
        }
    }
}
