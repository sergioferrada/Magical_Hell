using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    #region COMPONENETES
    protected Rigidbody2D rb2d;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    #endregion

    #region VARIABLES GENERALES
    [SerializeField] protected float _life;                                           // Campo privado respaldando la propiedad
    public float Life { get { return _life; } protected set { _life = value; } }
    [SerializeField] protected float _damage;                                           // Campo privado respaldando la propiedad
    public float Damage { get { return _damage; } protected set { _damage = value; } }
    protected enum State { Idle, Move, ShortAttack, MidAttack, LongAttack, Injured, Death }
    [SerializeField] protected State state;
    [SerializeField] protected float movementSpeed = 1, attackDistance = 0.8f;
    [SerializeField] protected float attackRange = 1, attackDelay = 1, passedTime = 1;
    [SerializeField] protected LayerMask attackableLayers;

    protected Vector2 direction;
    #endregion

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ChangeState(State.Idle);
    }

    // Update is called once per frame
    protected virtual void Move()
    {
        ChangeState(State.Move);
    }

    public void ReciveDamage(float damage)
    {
        ChangeState(State.Injured);
        Life -= damage;
        CheckDeath();
    }

    protected void CheckDeath()
    {
        if (Life <= 0)
        {
            ChangeState(State.Death);
            GameManager.Instance.UpdateEnemiesArray();
            GetComponent<Collider2D>().enabled = false;
        }
    }

    protected void Death()
    {
        Destroy(gameObject);
    }

    protected void ChangeState(State newState)
    {
        state = newState;
        animator.SetInteger("State", (int)state);
    }
}
