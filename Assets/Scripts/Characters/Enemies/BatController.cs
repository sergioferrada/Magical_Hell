using System.Collections;
using UnityEngine;

public class BatController : Enemy
{
    [Header("Character Stats (Child)")]
    [SerializeField] private float moveRadius = 5f; // Radio dentro del cual se moverá el enemigo.
    [SerializeField] private float moveInterval = 2f; // Tiempo entre movimientos.
    [SerializeField] private float maxMoveTime = 3f; // Máximo tiempo permitido para llegar al objetivo.

    private int movesMade = 0;
    float elapsedMoveTime = 0f;

    protected override void Start()
    {
        base.Start();
        // Llama a la función para iniciar el movimiento aleatorio.
        StartCoroutine(MoveToRandomPoints());
    }

    private IEnumerator MoveToRandomPoints()
    {
        while (true)
        {
            if (state != State.Injured && state != State.Death)
            {
                // Genera un punto aleatorio dentro del radio de movimiento.
                Vector2 randomPoint = transform.position + Random.insideUnitSphere * moveRadius;

                // Mueve el enemigo hacia el punto aleatorio.
                StartCoroutine(MoveToPosition(randomPoint));
            }

            yield return new WaitForSeconds(moveInterval);

            movesMade++;

            // Si se han realizado 3 movimientos, ajusta el intervalo a 5 segundos y reinicia el contador de movimientos.
            if (movesMade >= 3)
            {
                moveInterval = 5f;
                movesMade = 0;
            }
            else
            {
                // De lo contrario, vuelve al intervalo de 2 segundos.
                moveInterval = 2f;
            }
        }
    }

    private IEnumerator MoveToPosition(Vector2 targetPosition)
    {
        float moveStartTime = Time.time;
        SetState(State.Move);

        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Mueve gradualmente al enemigo hacia el punto aleatorio.
            elapsedMoveTime = Time.time - moveStartTime;
            if (elapsedMoveTime > maxMoveTime)
            {
                // Genera un nuevo punto aleatorio y cambia la dirección del movimiento.
                targetPosition = transform.position + Random.insideUnitSphere * moveRadius;
                //targetPosition.z = transform.position.z;
                moveStartTime = Time.time;
            }

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, MovementSpeed * Time.deltaTime);

            yield return null;
        }

        SetState(State.Idle); // Indica que el movimiento ha terminado.
    }
    // ...
}
