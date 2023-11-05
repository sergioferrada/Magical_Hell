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

    private float dynamicDifficultValue = 2f;
    private int consecutiveDifficultyIncreaseCount = 0;
    private int consecutiveDifficultyDecreaseCount = 0;

    public float timePerRoom { get; private set; }
    public float maxExpectedTime { get; private set; }
    public float totalPlayerLife { get; private set; }
    public float playerMaxLife { get; private set; }
    public float successfulAttacksPerRoom { get; private set; }
    public float totalAttacks { get; private set; }

    private float lifeWeight = 4.5f;
    private float timeWeight = 4.5f;
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
        float lifeDifficulty = (10.0f * (totalPlayerLife / playerMaxLife)) * lifeWeight;
        float timeDifficulty = (10.0f * ((maxExpectedTime - timePerRoom) / maxExpectedTime)) * timeWeight;
        float accuracyDifficulty = (10.0f * (successfulAttacksPerRoom / totalAttacks) * accuracyWeight);

        float totalDifficulty = (lifeDifficulty + timeDifficulty + accuracyDifficulty) / (lifeWeight + timeWeight + accuracyWeight);

        float difference = (((totalDifficulty + dynamicDifficultValue) / 2) * 0.95f) - dynamicDifficultValue;

        if (Math.Abs(difference) >= 0.15)
            dynamicDifficultValue += Mathf.Sign(difference) * 0.15f;

        if (totalDifficulty > dynamicDifficultValue + 0.5f)
        {
            consecutiveDifficultyIncreaseCount++;

            if (consecutiveDifficultyIncreaseCount >= 3)
                dynamicDifficultValue += 0.15f;
        }
        else
            consecutiveDifficultyIncreaseCount = 0;

        if (totalDifficulty < dynamicDifficultValue - 0.5f)
        {
            consecutiveDifficultyDecreaseCount++;

            if (consecutiveDifficultyDecreaseCount >= 3)
                dynamicDifficultValue -= 0.15f;
        }
        else
            consecutiveDifficultyDecreaseCount = 0;

        // Asegúrate de que el valor de dificultad sea positivo o cero.
        dynamicDifficultValue = Mathf.Max(0f, dynamicDifficultValue);
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
}
