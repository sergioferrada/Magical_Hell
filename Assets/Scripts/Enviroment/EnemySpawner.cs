using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameManager;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public GameLevel[] levelsToApper;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawnInfo[] enemyTypes;
    private List<GameObject> enemiesSelected;
    private List<GameObject> SelectEnemiesToSpawn()
    {
        List<GameObject> list = new List<GameObject>();

        foreach (var enemyType in enemyTypes)
        {
            foreach(var level in enemyType.levelsToApper)
            {
                if(level == actualGameLevel)
                {
                    list.Add(enemyType.enemyPrefab);
                    break;
                }
            }
        }

        return list;
    }

    public void SpawnEnemyV2()
    {
        enemiesSelected = SelectEnemiesToSpawn();

        if (enemiesSelected != null)
        {
            var randomEnemy = enemiesSelected[Random.Range(0, enemiesSelected.Count)];
            Instantiate(randomEnemy, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("enemiesSelected List is null");
        }
    }
}
