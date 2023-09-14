using System.Collections;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public float spawnProbability;
}

public class EnemySpawner : MonoBehaviour
{
    public EnemySpawnInfo[] enemyTypes;
    public Transform spawnPoint;
    public float spawnInterval = 5f;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            float totalProbability = 0f;

            // Calcular la suma total de las probabilidades de aparición.
            foreach (var enemyType in enemyTypes)
            {
                totalProbability += enemyType.spawnProbability;
            }

            // Generar un número aleatorio entre 0 y la suma total de probabilidades.
            float randomValue = Random.Range(0f, totalProbability);

            // Elegir un enemigo basado en las probabilidades.
            foreach (var enemyType in enemyTypes)
            {
                if (randomValue <= enemyType.spawnProbability)
                {
                    SpawnEnemy(enemyType.enemyPrefab);
                    break; // Sale del bucle cuando se ha elegido un enemigo.
                }
                randomValue -= enemyType.spawnProbability;
            }
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab != null && spawnPoint != null)
        {
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
