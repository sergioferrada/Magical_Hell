using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultManager : MonoBehaviour
{
    public static DifficultManager Instance;

    #region DIFFICULT VARIABLES
    public enum DifficultyLevel
    {
        Very_Easy,
        Easy,
        Medium,
        Hard,
        Very_Hard
    }

    public DifficultyLevel actualDifficultyLevel;

    private float dynamicDifficultValue = 1f;
    private int consecutiveDifficultyIncreaseCount = 0;
    private int consecutiveDifficultyDecreaseCount = 0;

    public float timePerRoom { get; private set; }
    public float maxExpectedTime { get; private set; }
    public float totalPlayerLife { get; private set; }
    public float playerMaxLife { get; private set; }
    public float successfulAttacksPerRoom { get; private set; }
    public float totalAttacks { get; private set; }

    public float playerAbilitiesCount { get; private set; }

    private float lifeWeight = 1.2f;
    private float timeWeight = 1.5f;
    private float accuracyWeight = 1.0f;

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MapDifficultyLevel()
    {
        if (dynamicDifficultValue <= 2.0f)
        {
            actualDifficultyLevel = DifficultyLevel.Very_Easy;
        }
        else if (dynamicDifficultValue <= 4.0f)
        {
            actualDifficultyLevel = DifficultyLevel.Easy;
        }
        else if (dynamicDifficultValue <= 6.0f)
        {
            actualDifficultyLevel = DifficultyLevel.Medium;
        }
        else if (dynamicDifficultValue <= 8.0f)
        {
            actualDifficultyLevel = DifficultyLevel.Hard;
        }
        else
        {
            actualDifficultyLevel = DifficultyLevel.Very_Hard;
        }
    }

    public void CalculateDynamicDifficult()
    {
        // Calcular la dificultad basada en la vida del jugador, el tiempo por sala y la precisión de los ataques.
        float lifeDifficulty =      ((totalPlayerLife / playerMaxLife) - 0.5f) * 2f * lifeWeight;
        float timeDifficulty =      (((maxExpectedTime - timePerRoom) / maxExpectedTime)) * timeWeight;
        float accuracyDifficulty =  ((successfulAttacksPerRoom - totalAttacks) / successfulAttacksPerRoom) * accuracyWeight;

        // Calcular la dificultad total ponderada por los pesos asignados a cada factor.
        float totalDifficulty = (lifeDifficulty + timeDifficulty + accuracyDifficulty) / (lifeWeight + timeWeight + accuracyWeight);

        float modificationPercentage = .05f + (playerAbilitiesCount / 100);
        
        if(totalDifficulty < 0)
            modificationPercentage -= playerAbilitiesCount / 150;

        float newDifficulty = ((dynamicDifficultValue / 10) + (totalDifficulty * modificationPercentage)) * 10.0f;

        // Incrementar la dificultad de manera más agresiva si la dificultad total es significativamente mayor que el valor dinámico.
         if (newDifficulty > dynamicDifficultValue + 0.5f)
         {
            consecutiveDifficultyIncreaseCount++;

            // Aplicar un aumento adicional si la dificultad ha aumentado durante varias iteraciones.
            if (consecutiveDifficultyIncreaseCount >= 3)
                newDifficulty += 0.15f;
        }
        else
            consecutiveDifficultyIncreaseCount = 0;

        // Decrementar la dificultad de manera más agresiva si la dificultad total es significativamente menor que el valor dinámico.
        if (newDifficulty < dynamicDifficultValue - 0.5f)
        {
            consecutiveDifficultyDecreaseCount++;

            // Aplicar una disminución adicional si la dificultad ha disminuido durante varias iteraciones.
            if (consecutiveDifficultyDecreaseCount >= 3)
                newDifficulty -= 0.15f;
        }
        else
            consecutiveDifficultyDecreaseCount = 0;

        dynamicDifficultValue = newDifficulty;
        // Asegúrate de que el valor de dificultad sea positivo o cero.
        dynamicDifficultValue = Mathf.Max(0f, dynamicDifficultValue);

        //Mapear la dificultad de acuerdo al valor obtenido
        MapDifficultyLevel();
    }

    public void SetTimePerRoom(float time)
    {
        timePerRoom = time;
    }

    public void ResetTimePerRoom()
    {
        timePerRoom = 0f;
    }

    public void SetMaxExpectedTime(float time)
    {
        maxExpectedTime = time;
    }

    /// <summary>
    /// Set the totalPlayerLife variable in GameManager used for calculated difficult 
    /// </summary>
    /// <param name="value"></param>
    public void SetTotalPlayerLife(float value)
    {
        totalPlayerLife = value;
    }

    public void SetPlayerMaxLife(float value)
    {
        playerMaxLife = value;
    }

    public void AddSuccefulAttack()
    {
        successfulAttacksPerRoom++;
    }

    public void ResetSuccefulAttacks()
    {
        successfulAttacksPerRoom = 0;
    }

    public void AddTotalAttacks()
    {
        totalAttacks++;
    }

    public void ResetTotalAttacks()
    {
        totalAttacks = 0;
    }

    public float GetDynamicDifficult()
    {
        return dynamicDifficultValue;
    }

    public void SetDynamicDifficult(float value)
    {
        dynamicDifficultValue = value;
    }

    public void AddPlayerAbilty()
    {
        playerAbilitiesCount++;
    }
}
