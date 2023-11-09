using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MrBeansLogic : ProjectileBase
{
    private List<Vector2> directions = new List<Vector2>();

    // Start is called before the first frame update
    protected override void Start()
    {
        SoundManager.Instance.PlaySound("Slime_Spawn_1", .5f);

        directions.Add(new Vector2(1, 1));
        directions.Add(new Vector2(-1, 1));
        directions.Add(new Vector2(1, -1));
        directions.Add(new Vector2(-1, -1));

        direction = directions[Random.Range(0, 3)];
        base.Start();

        transform.localScale = new Vector2(direction.x, transform.localScale.y);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Wall"))
        {
            // Calcula la normal en el punto de colisión
            Vector2 collisionPoint = collision.ClosestPoint(transform.position);
            Vector2 normal = (collisionPoint - (Vector2)transform.position).normalized;
            direction = Vector2.Reflect(direction, normal).normalized;
            SetProyectileVelocity(speed, direction);
        }

        // Redondea las coordenadas del vector para obtener valores de 1 o -1
        direction.x = Mathf.Sign(direction.x);
        transform.localScale = new Vector2(direction.x * transform.localScale.x, transform.localScale.y);
    }
}
