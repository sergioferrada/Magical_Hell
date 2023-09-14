using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoomerangProjectile : ProjectileLogic
{
    private Vector2 initialPosition;
    private Vector2 targetPosition;
    [SerializeField]
    private float curveRadius = 0.5f; // Radio de la curva

    protected override void Start()
    {
        base.Start();
        targetPosition = (Vector2)FindObjectOfType<PlayerController>().transform.position;
        initialPosition = transform.position; 
    }

    private void Update()
    {
        // Si llega a su objetivo, el boomerang regresa
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            targetPosition = initialPosition;

        //Si llega a su posicion inicial, el boomerang se destuye
        if(targetPosition == initialPosition && Vector2.Distance(transform.position, targetPosition) < 0.1f)
            Destroy(gameObject);

        //Se calcula la direccion para el movimiento con corvatura
        direction = CalculateCurveDirection((Vector2)transform.position, targetPosition);
        // Mueve el proyectil en la dirección
        rb2d.velocity = direction * speed;
    }

    private Vector2 CalculateCurveDirection(Vector2 initialPosition, Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - initialPosition).normalized;

        // Aplica la curvatura a la dirección para formar una "C"
        Vector2 curveDirection = Vector2.Perpendicular(direction).normalized;
        curveDirection *= curveRadius;
        return direction += curveDirection;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }
}
