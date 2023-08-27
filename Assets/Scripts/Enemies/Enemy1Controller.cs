
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy1Controller : Enemy
{
    //[SerializeField]
    public GameObject player;

    [SerializeField]
    private float chaseDistance = 0, attackDistance = 0.8f, speed = 1;

    [SerializeField]
    private float attackDelay = 1;
    private float passedTime = 1;

    void Update()
    {
        //if (player == null)
        //{
        //    return;
        //}

        Debug.Log("transform player: "+player.transform.position);
        float distance = Vector2.Distance(player.transform.position, transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        //if (distance < chaseDistance)
        //{
        //    if(distance <= attackDistance)
        //    {   
        //        //attack behavior
        //        if(passedTime >= attackDelay)
        //        {
        //            passedTime = 0;
        //            //codigo para atacar
        //        }
        //    }
        //    else { 
                
        //        //chasing the player
                
        //        //Codigod e movimiento, nmormalizado;
        //    }
        //}

        ////Idle
        //if(passedTime < attackDelay)
        //{
        //    passedTime += Time.deltaTime;
        //}
    }
}
