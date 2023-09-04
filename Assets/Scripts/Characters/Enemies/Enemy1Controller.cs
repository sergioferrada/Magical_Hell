
using UnityEngine;

public class Enemy1Controller : Enemy
{
    private float distance;

    private void Update()
    {
        if ((playerTransform == null) || (state == State.Death))
            return;

        distance = Vector2.Distance(transform.position, playerTransform.position);

        if (state != State.Injured)
        {
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
                    {
                        //Empieza el ataque
                        ChangeState(State.Attack);
                    }
                }
                //Fuera de rango de ataque
                else
                {
                    if (state != State.Attack)
                    {
                        ChaseGameObject(playerTransform);
                    }
                }
            }
            //Fuera de rango de persecucion
            else
            {
                ChangeState(State.Idle);
            }
        }

        //Tiempo para el siguiente ataque
        if (passedTime < attackDelay)
        {
            passedTime += Time.deltaTime;
        }
    }

    protected override void ShortRangeAttack()
    {
        base.ShortRangeAttack();
    }
}


