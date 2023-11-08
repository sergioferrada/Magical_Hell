using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleFireWormController : Enemy
{
    [Header("Combat Stats (Child)")]
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float chaseDistance;

    [Header("Projectile Stats")]
    [SerializeField] private GameObject fireballPrefab; // Prefab de la bola de fuego.
    [SerializeField] private Transform fireballSpawnPoint; // Punto de origen para las bolas de fuego.
    [SerializeField] private float projectileDamage;
    [SerializeField] private int numFireballs; // Cantidad de bolas de fuego a disparar.

    // Inicializa las probabilidades de los ataques.
    float midAttackProbability = 0.5f; // Misma probabilidad para midattack y longattack.

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        // Llama a la función para iniciar el comportamiento aleatorio.
        StartCoroutine(RandomBehavior());
    }

    private IEnumerator RandomBehavior()
    {
        if (state != State.Injured && state != State.Death)
        {
            while (true)
            {
                // Genera un número aleatorio para determinar qué comportamiento realizar en función de las probabilidades.
                float randomValue = Random.value;

                if (randomValue <= midAttackProbability)
                {
                    // Realiza un ataque de carga hacia el jugador.
                    SetState(State.MidAttack);
                }
                else
                {
                    // Dispara 3 bolas de fuego en forma de abanico.
                    direction = (playerTransform.position - transform.position).normalized;
                    SetState(State.LongAttack);
                }

                // Espera un tiempo antes de realizar el próximo comportamiento.
                yield return new WaitForSeconds(AttackDelay);
            }
        }
    }

    protected override void MidRangeAttack()
    {
        // Calcula la dirección hacia el jugador.
        direction = (playerTransform.position - transform.position).normalized;
        rb2d.drag = 5;
        MovementSpeed = 35;

        // Impulsa al Fire Worm hacia el jugador.
        rb2d.AddForce(direction * MovementSpeed, ForceMode2D.Impulse);

        // Espera un tiempo antes de detener el ataque.
        StartCoroutine(StopAttack());
    }

    protected override void LongRangeAttack()
    {
        // Dispara un abanico de bolas de fuego.
        for (int i = 0; i < numFireballs; i++)
        {
            // Calcula un ángulo para la dirección de disparo.
            float angle = (i - (numFireballs - 1) / 2.0f) * 15f;

            // Calcula la dirección de disparo basada en el ángulo.
            Vector2 projectileDirection = Quaternion.Euler(0, 0, angle) * direction;

            // Instancia una bola de fuego en la dirección calculada.
            GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
            fireball.GetComponent<WormFireballLogic>().direction = projectileDirection;
            fireball.GetComponent<WormFireballLogic>().damage = projectileDamage;
        }
    }
    private IEnumerator StopAttack()
    {
        // Espera un tiempo antes de detener el ataque.
        yield return new WaitForSeconds(2f);

        // Detiene el ataque y reinicia la velocidad.
        SetState(State.Idle);
        MovementSpeed = 6;
        rb2d.drag = 10;
    }
}
