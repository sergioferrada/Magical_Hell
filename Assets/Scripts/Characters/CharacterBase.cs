using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    #region COMPONENETS
    protected Rigidbody2D rb2d;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    #endregion

    #region CHARACTER STATS
    public enum State { Idle, Move, ShortAttack, MidAttack, LongAttack, Injured, Death, Slowed, Rooted }

    [Header("Character Stats")]
    [SerializeField] protected State _state;
    [SerializeField] protected float _baseLife;
    [SerializeField] protected float _maxLife;
    [SerializeField] protected float _baseMovementSpeed;
    public State state { get { return _state; } private set { _state = value; } }
    public float Life { get { return _baseLife; } protected set { _baseLife = value; } }
    public float MaxLife { get { return _maxLife; } protected set { _maxLife = value; } }
    public float MovementSpeed { get { return _baseMovementSpeed; } protected set { _baseMovementSpeed = value; } }

    //agregado para test 
    public float originalMovementspd;
    #endregion

    #region COMBAT STATS
    [Header("Combat Stats")]
    [SerializeField] protected LayerMask attackableLayers;
    [SerializeField] protected float _baseDamage;
    [SerializeField] protected float _baseAttackDelay;
    [SerializeField] protected float _attackRange;

    public float Damage { get { return _baseDamage; } protected set { _baseDamage = value; } }
    public float AttackDelay { get { return _baseAttackDelay; } protected set { _baseAttackDelay = value; } }
    public float AttackRange { get { return _attackRange; } protected set { _attackRange = value; } }
    #endregion

    #region UI SETTINGS
    [Header("UI Settings")]
    [SerializeField] protected GameObject PopUpDamagePrefab;
    #endregion

    public float passedTime;
    public Vector2 direction;
    public Vector2 lastDirection;


    public bool invulnerable = false;
    public float invulnerableTime = 0.3f;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
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
        if (invulnerable) return; //si esta en invulnerable, no herir

        BecomeInvulnerable();

        SetState(State.Injured);
        Life -= damage;

        if (PopUpDamagePrefab != null)
        {
            var aux = Instantiate(PopUpDamagePrefab, transform.position, Quaternion.identity);
            aux.GetComponent<PopUpController>().PopUpTextSprite(damage.ToString());
        }

        CheckDeath();
    }

    public void BecomeInvulnerable()
    {
        invulnerable = true;
        StartCoroutine(InvulnerabilityFrames());
    }

    public void BecomeInvulnerable(float t)
    {
        invulnerable = true;
        StartCoroutine(InvulnerabilityFrames(t));
    }

    public virtual void ApplySlow(float slow)

    {
        SetState(State.Slowed);
        MovementSpeed *= slow;

    }

    public virtual void ApplyRoot(float rootTime)
    {
        SetState(State.Rooted);
        if (PopUpDamagePrefab != null)
        {
            var aux = Instantiate(PopUpDamagePrefab, transform.position, Quaternion.identity);
            aux.GetComponent<PopUpController>().PopUpTextSprite("Rooted!");
        }
        originalMovementspd = MovementSpeed;
        MovementSpeed = 0;

        Invoke("Unroot", rootTime);

    }

    private void Unroot()
    {
        SetState(State.Move);
        MovementSpeed = originalMovementspd;

    }

    /// <summary>
    /// Check if the life of the character is equal or less than cero
    /// </summary>
    protected virtual void CheckDeath()
    {
        if (Life <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            rb2d.simulated = false;
            SetState(State.Death);
            RoomsManager.Instance.CalculateEnemiesInScene();
        }
    }

    private IEnumerator InvulnerabilityFrames()
    {
        Color c = gameObject.GetComponent<Renderer>().material.color;
        gameObject.GetComponent<Renderer>().material.color = c / 2f;

        yield return new WaitForSeconds(invulnerableTime);

        invulnerable = false;

        gameObject.GetComponent<Renderer>().material.color = c;
    }

    private IEnumerator InvulnerabilityFrames(float t)
    {
        Color c = gameObject.GetComponent<Renderer>().material.color;
        gameObject.GetComponent<Renderer>().material.color = c / 2f;

        yield return new WaitForSeconds(t);

        invulnerable = false;

        gameObject.GetComponent<Renderer>().material.color = c;
    }

    /// <summary>
    /// Destroy the GameObject
    /// </summary>
    protected virtual void Death()
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
