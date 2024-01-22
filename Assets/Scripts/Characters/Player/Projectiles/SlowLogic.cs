using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowLogic : StationaryDamageObject
{
    public float RootTime = 1f;
    public float RootInterval;
    public float SlowDuration = 5f; 
    private float timer = 0f;
    public float slowPercentage = 0.3f;
    private bool isSlowed = false; // para controlar la ralentización
    public LayerMask enemyLayer;

    public void ActivateRootArea()
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

                    if (enemy != null && enemy.state != CharacterBase.State.Rooted)
                    {
                        enemy.ApplyRoot(RootTime);

                        if (!isSlowed)
                        {
                            // Aplicar ralentización si no esta ralentizado actualmente
                            enemy.ApplySlow(slowPercentage);
                            Debug.Log("enemigo slowed");
                            StartCoroutine(SlowDurationCoroutine());
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError("No hay SpriteRenderer");
        }
    }

    //  para controlar la duración de la ralentización
    private IEnumerator SlowDurationCoroutine()
    {
        isSlowed = true;
        yield return new WaitForSeconds(SlowDuration);
        isSlowed = false;
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

            if (timer >= RootInterval)
            {
                // Resetear el temporizador
                timer = 0.0f;

                // Realizar el root
                ActivateRootArea();
            }
        }
    }
}