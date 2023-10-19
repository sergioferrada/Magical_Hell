using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RoomsManager;

public class CharacterBase : MonoBehaviour
{
    #region COMPONENETS
    protected Rigidbody2D rb2d;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    #endregion

    #region GENERAL VARIABLES
    [SerializeField] protected float _life;                                           // Campo privado respaldando la propiedad
    public float Life { get { return _life; } protected set { _life = value; } }
    [SerializeField] protected float _damage;                                           // Campo privado respaldando la propiedad
    public float Damage { get { return _damage; } protected set { _damage = value; } }
    public enum State { Idle, Move, ShortAttack, MidAttack, LongAttack, Injured, Death }
    [SerializeField] protected State _state;
    public State state { get { return _state; } private set { _state = value; } }
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
        SetState(State.Idle);
    }

    // Update is called once per frame
    protected virtual void Move()
    {
        SetState(State.Move);
    }

    public virtual void ReciveDamage(float damage)
    {
        SetState(State.Injured);
        Life -= damage;
        CheckDeath();
    }

    /// <summary>
    /// Check if the life of the character is equal or less than cero
    /// </summary>
    protected void CheckDeath()
    {
        if (Life <= 0)
        {
            SetState(State.Death);
            roomsManager.CalculateEnemiesInScene();
            GetComponent<Collider2D>().enabled = false;
        }
    }

    /// <summary>
    /// Destroy the GameObject
    /// </summary>
    protected void Death()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Set the state of the character with the new state
    /// </summary>
    /// <param name="newState"></param>
    protected void SetState(State newState)
    {
        state = newState;
        animator.SetInteger("State", (int)state);
    }

    /// <summary>
    /// Comparate the actual state of the character with the newState
    /// </summary>
    /// <param name="newState"></param>
    /// <returns>True if they are the same, and false instead</returns>
    protected bool CompareState(State newState)
    {
        return state == newState;
    }
}
