using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    Transform playerPosition;
    [SerializeField] private float healthPoint;
    [SerializeField] private float moveSpeed; // Velocidad de movimiento modificable

    private void Awake()
    {
        playerPosition = FindAnyObjectByType<PlayerController>().GetComponent<Transform>();
    }
    void Start()
    {
        StartCoroutine(MoveToRandomPosition());
    }

    // Update is called once per frame
    void Update()
    {
        // Calcula la dirección hacia el jugador
        Vector3 directionToPlayer = playerPosition.position - transform.position;

        // Normaliza la dirección para moverse a una velocidad constante
        directionToPlayer.Normalize();

        // Mueve el objeto hacia el jugador con la velocidad especificada
        transform.Translate(directionToPlayer * moveSpeed * Time.deltaTime);
    }

    IEnumerator MoveToRandomPosition()
    {
        // Espera el tiempo especificado antes de comenzar a seguir al jugador
        yield return new WaitForSeconds(1);

        // Mueve el objeto a una posición aleatoria
        transform.position = GetRandomPosition();

        // Puedes ajustar el tiempo de espera adicional antes de comenzar a seguir al jugador
        yield return new WaitForSeconds(1.0f);
    }

    Vector3 GetRandomPosition()
    {
        // Puedes personalizar los límites del área en la que quieres que aparezca el objeto
        float randomX = Random.Range(-5.0f, 5.0f);
        float randomY = Random.Range(-5.0f, 5.0f);

        return new Vector3(randomX, randomY, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            player.RecoverLife(healthPoint);
            Destroy(gameObject);
        }
    }
}
