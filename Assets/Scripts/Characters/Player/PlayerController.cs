using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : CharacterBase
{
    [Header("Combat Stats (Child)")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private MeleePlayerAttack meleeAttack;

    [Header("Level Character Stats")]
    [SerializeField] private float currentExperience, maxExperiencie;
    [SerializeField] private int currentLevel, maxLevel;

    private PlayerHUDController playerHUDController;
    private CooldownBarController cooldownBarController;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        DifficultManager.Instance.SetTotalPlayerLife(Life);
        DifficultManager.Instance.SetPlayerMaxLife(MaxLife);

        playerHUDController = FindFirstObjectByType<PlayerHUDController>();
        cooldownBarController = GetComponent<CooldownBarController>();
    }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (!CompareState(State.Death) /*&& !CompareState(State.Injured)*/)
        {
            ProcessInputs();
            //Tiempo para el siguiente ataque
            if (passedTime < AttackDelay)
            {
                passedTime += Time.deltaTime;
                cooldownBarController.UpdateHUD();
            }
        }
    }

    private void FixedUpdate()
    {
        // Physic Calculation
        if (!CompareState(State.Death) /*&& !CompareState(State.Injured)*/)
        {
            Move();
        }
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0)
            transform.localScale = new Vector2(moveX, transform.localScale.y);


        if (Input.GetKeyDown(KeyCode.Space) && passedTime >= AttackDelay)
        {
            SetState(State.ShortAttack);
            meleeAttack.Activate();
            meleeAttack.objectScale = 1;
            meleeAttack.damage = Damage;
        }

        // Cambia los estados en funci�n del movimiento o ataque
        if (!CompareState(State.ShortAttack))
        {
            if (!CompareState(State.Injured))
            {
                if (direction.magnitude > 0) SetState(State.Move);
                else if (moveX == 0 && moveY == 0) {
                    
                    SetState(State.Idle); 
                    lastDirection = direction;
                }
            }
        }

        direction = new Vector2(moveX, moveY).normalized;
    }

    protected override void Move()
    {
        rb2d.velocity = new Vector2(direction.x * MovementSpeed, direction.y * MovementSpeed);
    }

    public void RecoverLife(float lifePoints)
    {
        if (!SoundManager.Instance.IsClipPlaying("Health_Recover"))
            SoundManager.Instance.PlaySound("Health_Recover", .7f);

        Life += lifePoints;
        if (Life > MaxLife) Life = MaxLife;
        playerHUDController.UpdateHeartsHUD();
    }

    public void AddExp(float exp)
    {
        if(!SoundManager.Instance.IsClipPlaying("Exp_Collected"))
            SoundManager.Instance.PlaySound("Exp_Collected", .7f);

        currentExperience += exp;

        if(currentExperience >= maxExperiencie)
        {
            LevelUp();
            currentExperience = 0;
        }

        playerHUDController.UpdateExpBarHUD();
    }

    public float GetCurrentExp()
    {
        return currentExperience;
    }

    public float GetCurrentLevel()
    {
        return currentLevel;
    }

    public float GetMaxExp()
    {
        return maxExperiencie;
    }

    public void LevelUp()
    {
        if(currentLevel < maxLevel)
        {
            currentLevel++;
            MaxLife += 1;
            Damage += 0.5f;
            MovementSpeed += 0.25f;
            AttackDelay -= 0.15f;
            maxExperiencie += 100;

            if (PopUpDamagePrefab != null)
            {
                var aux = Instantiate(PopUpDamagePrefab, transform.position, Quaternion.identity);
                aux.GetComponent<PopUpController>().SetText("Level Up");
                SoundManager.Instance.PlaySound("Level_Up");
            }

            playerHUDController.UpdateHeartsHUD();
            playerHUDController.UpdateExpBarHUD();
            playerHUDController.UpdateStatsHUD();
            cooldownBarController.UpdateHUD();
        }
    }

    public override void ReciveDamage(float damage)
    {
        //if(!SoundManager.Instance.IsClipPlaying("Player_Hurt"))
        SoundManager.Instance.PlaySound("Player_Hurt");
        //playerHUDController.PlayAnimationHUD("Healthbar_Damage_Animation");

        base.ReciveDamage(damage);
        DifficultManager.Instance.SetTotalPlayerLife(Life);
        playerHUDController.UpdateHeartsHUD();
        
    }

    protected override void Death()
    {
        GameManager.Instance.SetActualGameState(GameManager.GameState.GameOver);
        base.Death();
    }

    protected void ResetAttack()
    {
        passedTime = 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Cuando colisione con un objeto en el layer de "Enemies"
        if (collision.gameObject.layer == 7)
        {
            ReciveDamage(collision.gameObject.GetComponent<Enemy>().Damage);
        }

        if (collision.gameObject.layer == 10)
        {
            ReciveDamage(collision.gameObject.GetComponent<ProjectileBase>().damage);
        }
    }
}
