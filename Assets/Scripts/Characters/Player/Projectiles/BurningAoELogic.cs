using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningAoELogic : StationaryDamageObject
{
    public float damageInterval = 1f;
    private float timer = 0f;
    public LayerMask enemyLayer;
 

   public void ActivateAttackArea()
     {
         SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

         if (spriteRenderer != null)
         {
             float radius = spriteRenderer.bounds.size.x / 2;

             Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

             foreach (Collider2D enemyCollider in hitEnemies)
             {
                 if (enemyCollider != null)
                 {
                     Enemy enemy = enemyCollider.GetComponent<Enemy>();

                     if (enemy != null)
                     {
                         enemy.ReciveDamage(damage);
                     }
                 }
             }
         }
         else
         {
             Debug.LogError("no hay spriterenderer");
         }
     } 

    void OnDrawGizmos()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float radius = spriteRenderer.bounds.size.x / 2;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector3)transform.position, radius);
    }

    private Transform playerTransform;

    public void SetPlayerTransform(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }
    void Update()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position;

            timer += Time.deltaTime;

            if (timer >= damageInterval)
            {
                // Reset timer
                timer = 0.0f;

                // hacer el daño continuo
                ActivateAttackArea();
            }
        }
    }
}

