using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[System.Serializable]
public class EnemySpawnInfo
{
    public GameObject enemyPrefab;
    public GameManager.GameLevel[] levelsToApper;
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
                if(level == GameManager.Instance.actualGameLevel)
                {
                    list.Add(enemyType.enemyPrefab);
                    break;
                }
            }
        }

        return list;
    }

    private int GetNumberOfEnemiesToSpawn()
    {
        switch (DifficultManager.Instance.actualDifficultyLevel)
        {
            case DifficultManager.DifficultyLevel.Very_Easy:
                return 1;
            case DifficultManager.DifficultyLevel.Easy:
                return 2;
            case DifficultManager.DifficultyLevel.Medium:
                return 3;
            case DifficultManager.DifficultyLevel.Hard:
                return 4;
            case DifficultManager.DifficultyLevel.Very_Hard:
                return 5;
        }

        return 0;
    }

    public void SpawnEnemyV2()
    {
        int enemiesCount = GetNumberOfEnemiesToSpawn();
        enemiesSelected = SelectEnemiesToSpawn();

        if (enemiesSelected != null)
        {
            for (int i = 0; i < enemiesCount; i++)
            {
                var randomEnemy = enemiesSelected[Random.Range(0, enemiesSelected.Count)];
                Instantiate(randomEnemy, transform.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.Log("enemiesSelected List is null");
        }
    }
}
