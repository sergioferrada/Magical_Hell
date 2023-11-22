using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceItem : MonoBehaviour
{
    Transform playerPosition;
    [SerializeField] private float expPoint;
    [SerializeField] private float moveSpeed; // Velocidad de movimiento modificable

    private void Awake()
    {
        playerPosition = FindAnyObjectByType<PlayerController>().GetComponent<Transform>();
    }

    void Start()
    {
        StartCoroutine(MoveToRandomPosition());
    }

    IEnumerator MoveToRandomPosition()
    {
        // Espera el tiempo especificado antes de comenzar a seguir al jugador
        //yield return new WaitForSeconds(1.0f);

        // Mueve el objeto a una posici�n aleatoria
        transform.position = GetRandomPosition();

        // Puedes ajustar el tiempo de espera adicional antes de comenzar a seguir al jugador
        yield return new WaitForSeconds(2.0f);
    }

    Vector3 GetRandomPosition()
    {
        // Definir una distancia m�xima desde la posici�n actual
        float maxDistance = 1.5f;

        // Generar coordenadas aleatorias en un c�rculo alrededor de la posici�n actual
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float randomDistance = Random.Range(0f, maxDistance);

        // Calcular las nuevas coordenadas
        float offsetX = Mathf.Cos(randomAngle) * randomDistance;
        float offsetY = Mathf.Sin(randomAngle) * randomDistance;

        // Aplicar las coordenadas alrededor de la posici�n actual
        Vector3 randomPosition = transform.position + new Vector3(offsetX, offsetY, 0f);

        return randomPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Calcula la direcci�n hacia el jugador
        Vector3 directionToPlayer = playerPosition.position - transform.position;

        // Normaliza la direcci�n para moverse a una velocidad constante
        directionToPlayer.Normalize();

        // Mueve el objeto hacia el jugador con la velocidad especificada
        transform.Translate(directionToPlayer * moveSpeed * Time.deltaTime);
    }

    public float GetItemExpPoints()
    {
        return expPoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            player.AddExp(expPoint);
            Destroy(gameObject);
        }
    }
}
