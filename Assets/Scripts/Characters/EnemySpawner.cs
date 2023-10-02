using System.Collections;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public float baseSpawnProbability;
    public float difficulty; // Dificultad del enemigo (ajusta este valor según tus criterios)
}

public class EnemySpawner : MonoBehaviour
{
    public EnemySpawnInfo[] enemyTypes;
    public Transform spawnPoint;
    public float spawnInterval = 5f;

    private void Start()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {   
        // Calcular la suma total de las probabilidades base de aparición.
        float totalBaseProbability = 0f;
        foreach (var enemyType in enemyTypes)
        {
            totalBaseProbability += enemyType.baseSpawnProbability;
        }

        // Calcular la dificultad actual (puedes obtenerla desde GameManager).
        float difficulty = GameManager.Instance.CalculateDifficulty();

        // Calcular la probabilidad ajustada en función de la dificultad.
        foreach (var enemyType in enemyTypes)
        {
            // Usar la dificultad del enemigo para ajustar la probabilidad.
            float adjustedProbability = (enemyType.baseSpawnProbability / totalBaseProbability) * enemyType.difficulty;

            // Si la dificultad es baja, aumentar la probabilidad de enemigos más fáciles.
            if (difficulty < 0.5f)
            {
                adjustedProbability *= 2f;
            }

            if (Random.value <= adjustedProbability)
            {
                if (enemyType.enemyPrefab != null && spawnPoint != null)
                {
                    Instantiate(enemyType.enemyPrefab, spawnPoint.position, Quaternion.identity);
                }

                break; // Salir del bucle cuando se ha elegido un enemigo.
            }
        } 
    }
}
