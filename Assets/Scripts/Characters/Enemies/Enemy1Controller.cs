
using UnityEngine;

public class Enemy1Controller : Enemy
{
    private float distance;

    [Header("Combat Stats (Child)")]
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float chaseDistance;

    protected override void Start()
    {
        base.Start();
    }

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
                if (passedTime >= AttackDelay)
                    //Empieza el ataque
                    SetState(State.ShortAttack);
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
            SetState(State.Idle);
        
        //Tiempo para el siguiente ataque
        if (passedTime < AttackDelay)
            passedTime += Time.deltaTime;
    }

    protected override void Move()
    {
        base.Move();
        ChaseGameObject(playerTransform);
    }
}


