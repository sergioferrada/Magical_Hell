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
    public static float timePerRoom { get; private set; }
    public static float totalPlayerLife { get; private set; }
    private static float abilitiesUsedPerRoom;
    #endregion

    #region LEVEL VARIABLES
    private static float dynamicDifficultValue = 5f;
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
        // Puedes diseñar una fórmula que combine la vida del jugador y el tiempo de finalización.
        // Asegúrate de ajustar esta fórmula según tus necesidades y preferencias.

        // Por ejemplo, puedes ponderar más la vida del jugador y menos el tiempo completado.
        float difficulty = (totalPlayerLife * 0.7f) - (timePerRoom * 0.3f);

        // Asegúrate de que el valor de dificultad sea positivo o cero.
        SetDynamicDifficult(Mathf.Max(0f, difficulty));
    }

    public static void GoToNextRoom(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void SetTimePerRoom(float time)
    {
        timePerRoom = time;
    }

    /// <summary>
    /// Set the totalPlayerLife variable in GameManager used for calculated difficult 
    /// </summary>
    /// <param name="value"></param>
    public static void SetTotalPlayerLife(float value)
    {
        totalPlayerLife = value;
    }

    public static float GetDynamicDifficult() { 

        return dynamicDifficultValue; 
    }

    public static void SetDynamicDifficult(float value)
    {
        dynamicDifficultValue = value;
    }
}
