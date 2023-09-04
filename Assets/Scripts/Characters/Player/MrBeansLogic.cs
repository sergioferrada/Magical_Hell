using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrBeansLogic : MonoBehaviour
{
    private List<Vector2> directions = new List<Vector2>();
    Rigidbody2D rb;
    Vector2 currentDirection;
    public float speed, lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        directions.Add(new Vector2(1, 1));
        directions.Add(new Vector2(-1, 1));
        directions.Add(new Vector2(1, -1));
        directions.Add(new Vector2(-1, -1));

        currentDirection = directions[Random.Range(0, 3)];
        rb.velocity = currentDirection * speed;

        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contactPoint in collision.contacts)    
            currentDirection = Vector2.Reflect(currentDirection, contactPoint.normal);
       
        rb.velocity = speed * currentDirection;
    }
}
