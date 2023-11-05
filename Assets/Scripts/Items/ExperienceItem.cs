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
