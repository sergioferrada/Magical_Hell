using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : CharacterBase
{
    [Header("Combat Stats (Child)")]
    [SerializeField] private Transform attackPoint;

    [Header("Level Character Stats")]
    [SerializeField] private float currentExperience, maxExperiencie;
    [SerializeField] private int currentLevel, maxLevel;
    //public Animator lvlUpAnimator;

    private HealthBarController healthBarController;
    private ExpBarController expBarController;
    private StatsHUDController statsHUDController;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        DifficultManager.Instance.SetTotalPlayerLife(Life);
        DifficultManager.Instance.SetPlayerMaxLife(MaxLife);

        healthBarController = GetComponent<HealthBarController>(); 
        expBarController = GetComponent<ExpBarController>();
        statsHUDController = GetComponent<StatsHUDController>();
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
                passedTime += Time.deltaTime;
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
            SetState(State.ShortAttack);

        // Cambia los estados en función del movimiento o ataque
        if (!CompareState(State.ShortAttack))
        {
            if (!CompareState(State.Injured))
            {
                if (direction.magnitude > 0) SetState(State.Move);
                else if (moveX == 0 && moveY == 0) SetState(State.Idle);
            }
        }
        //else { moveX = 0; moveY = 0; }

        direction = new Vector2(moveX, moveY).normalized;
    }

    protected override void Move()
    {
        rb2d.velocity = new Vector2(direction.x * MovementSpeed, direction.y * MovementSpeed);
    }

    void MeleeAttack()
    {
        DifficultManager.Instance.AddTotalAttacks();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, attackableLayers);

        foreach (Collider2D enemy in hitEnemies) {

            enemy.GetComponent<Enemy>().ReciveDamage(Damage);
            Vector2 direction = (enemy.GetComponent<Collider2D>().transform.position - transform.position).normalized;
            enemy.GetComponent<Rigidbody2D>().AddForce(direction * 25, ForceMode2D.Impulse);
            DifficultManager.Instance.AddSuccefulAttack();
        }
    }

    public void RecoverLife(float lifePoints)
    {
        if (!SoundManager.Instance.IsClipPlaying("Health_Recover"))
            SoundManager.Instance.PlaySound("Health_Recover", .7f);

        Life += lifePoints;
        if (Life > MaxLife) Life = MaxLife;
        healthBarController.UpdateHeartsHUD();
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

        expBarController.UpdateExpBarHUD();
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

            healthBarController.UpdateHeartsHUD();
            expBarController.UpdateExpBarHUD();
            statsHUDController.UpdateStatsHUD();
        }
    }

    public override void ReciveDamage(float damage)
    {
        //if(!SoundManager.Instance.IsClipPlaying("Player_Hurt"))
        SoundManager.Instance.PlaySound("Player_Hurt");

        base.ReciveDamage(damage);
        DifficultManager.Instance.SetTotalPlayerLife(Life);
        healthBarController.UpdateHeartsHUD();
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
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            //rb2d.AddForce(direction * 25, ForceMode2D.Impulse);
        }

        if (collision.gameObject.layer == 10)
        {
            ReciveDamage(collision.gameObject.GetComponent<ProjectileLogic>().Damage);
        }
    }
}
