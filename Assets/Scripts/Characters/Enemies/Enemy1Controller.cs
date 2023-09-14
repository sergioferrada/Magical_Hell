
using UnityEngine;

public class Enemy1Controller : Enemy
{
    private float distance;

    protected override void Update()
    {
        base.Update();

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
                    //Empieza el ataque
                    ChangeState(State.ShortAttack);
            }
            //Fuera de rango de ataque
            else
            {
                if (state != State.ShortAttack)
                    Move();
            }
        }
        //Fuera de rango de persecucion
        else
            ChangeState(State.Idle);
        
        //Tiempo para el siguiente ataque
        if (passedTime < attackDelay)
            passedTime += Time.deltaTime;
    }

    protected override void Move()
    {
        base.Move();
        ChaseGameObject(playerTransform);
    }
}


