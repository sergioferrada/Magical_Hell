using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MrBeansLogic : ProjectileLogic
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

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (!collision.gameObject.CompareTag("Enemy"))
        {
            foreach (ContactPoint2D contactPoint in collision.contacts)
                direction = Vector2.Reflect(direction, contactPoint.normal);

            rb2d.velocity = speed * direction;
        }

        //Se redondean las coordenadas del vector par solo obtener valores de 1 o -1
        direction.x = Mathf.Sign(direction.x);
        transform.localScale = new Vector2(direction.x, transform.localScale.y);
    }
}
