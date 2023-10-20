using System;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting.Dependencies.Sqlite;

public static class GameManager
{
    #region GAME STATES
    public enum GameState { MainMenu, LoadLevelData, PreLoadRoom, InGameRoom, RoomFinished, LevelFinished, GameOver, GameCompleted }
    public static GameState gameState;
    #endregion

    #region DIFFICULT VARIABLES
    private static float dynamicDifficultValue = 3f;
    public static float timePerRoom { get; private set; }
    public static float maxExpectedTime { get; private set; }
    public static float totalPlayerLife { get; private set; }
    public static float playerMaxLife { get; private set; }
    public static float successfulAttacksPerRoom { get; private set; }
    public static float totalAttacks { get; private set; }

    private static float lifeWeight = 4.5f;
    private static float timeWeight = 4.5f;
    private static float accuracyWeight = 1.0f;
    #endregion

    public static void SetGameState(GameState newGameState)
    {
        gameState = newGameState;
    }

    public static bool CompareGameStates(GameState newGameState)
    {
        return gameState == newGameState;
    }

    public static void CalculateDynamicDifficult()
    {
        float lifeDifficulty = 10.0f * (totalPlayerLife / playerMaxLife);

        float timeDifficulty = 10.0f * ((maxExpectedTime - timePerRoom) / maxExpectedTime);

        float accuracyDifficulty = 10.0f * (successfulAttacksPerRoom / totalAttacks);


        // Puedes diseñar una fórmula que combine la vida del jugador y el tiempo de finalización.
        // Asegúrate de ajustar esta fórmula según tus necesidades y preferencias.

        // Por ejemplo, puedes ponderar más la vida del jugador y menos el tiempo completado.
        float totalDifficulty = (lifeDifficulty * lifeWeight + timeDifficulty * timeWeight + accuracyDifficulty * accuracyWeight) / (lifeWeight + timeWeight + accuracyWeight);

        // Asegúrate de que el valor de dificultad sea positivo o cero.
        SetDynamicDifficult(Mathf.Max(0f, totalDifficulty));
    }

    public static void GoToNextRoom(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void SetTimePerRoom(float time)
    {
        timePerRoom = time;
    }

    public static void ResetTimePerRoom()
    {
        timePerRoom = 0f;
    }

    public static void SetMaxExpectedTime(float time)
    {
        maxExpectedTime = time;
    }

    /// <summary>
    /// Set the totalPlayerLife variable in GameManager used for calculated difficult 
    /// </summary>
    /// <param name="value"></param>
    public static void SetTotalPlayerLife(float value)
    {
        totalPlayerLife = value;
    }

    public static void SetPlayerMaxLife(float value)
    {
        playerMaxLife = value;
    }

    public static void AddSuccefulAttack()
    {
        successfulAttacksPerRoom++;
    }

    public static void ResetSuccefulAttacks()
    {
        successfulAttacksPerRoom = 0;
    }

    public static void AddTotalAttacks()
    {
        totalAttacks++;
    }

    public static void ResetTotalAttacks()
    {
        totalAttacks = 0;
    }

    public static float GetDynamicDifficult() { 

        return dynamicDifficultValue; 
    }

    public static void SetDynamicDifficult(float value)
    {
        dynamicDifficultValue = value;
    }
}
