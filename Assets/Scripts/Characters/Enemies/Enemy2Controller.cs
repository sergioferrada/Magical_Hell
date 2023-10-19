using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy2Controller : Enemy
{
    private float distance;
    [SerializeField] private ProjectileLogic boomerangProjectile;

    // Update is called once per frame
    protected override void Update()
    {
        if ((playerTransform == null) || (state == State.Death) || (state == State.Injured))
            return;

        distance = Vector2.Distance(transform.position, playerTransform.position);

        //Dentro de rango de persecucion
        if (distance < chaseDistance)
        {
            //Dentro de rango de ataque
            if (distance <= attackDistance)
            {
                //Se detiene
                transform.position = transform.position;

                //Puede atacar?
                if (passedTime >= attackDelay)            
                    SetState(State.MidAttack);
            }
            //Fuera de rango de ataque
            else
                Move();
        }
        //Fuera de rango de persecucion
        else
            SetState(State.Idle);

        //Tiempo para el siguiente ataque
        if (passedTime < attackDelay)
            passedTime += Time.deltaTime;
    }

    protected override void Move()
    {
        base.Move();
        ChaseGameObject(playerTransform);
    }

    protected override void MidRangeAttack()
    {
        base.MidRangeAttack();
        Instantiate(boomerangProjectile, transform.position, transform.rotation);
        ResetAttack();
    }
}
