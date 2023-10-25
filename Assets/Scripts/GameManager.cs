using System;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor;
using System.IO;

public static class GameManager
{
    #region GAME STATES
    public enum GameState {

        MainMenu,
        LoadLevelData,
        PreLoadRoom,
        InGameRoom,
        RoomFinished,
        LevelFinished,
        Paused,
        GameOver,
        GameCompleted
    }

    public static GameState gameState;
    #endregion

    public static void SetGameState(GameState newGameState)
    {
        gameState = newGameState;
    }

    #region LEVEL VARIABLES
    public enum GameLevel {
        
        Tutorial,
        Level_1,
        Level_2,
        Level_3,
        Level_4,
    }

    public static GameLevel actualGameLevel;
    #endregion  

    public static void SetGameLevel(GameLevel newGameLevel)
    {
        actualGameLevel = newGameLevel;
    }

    #region DIFFICULT VARIABLES
    public enum DifficultyLevel{

        Very_Easy,
        Easy,
        Medium,
        Hard,
        Very_Hard
    }

    public static DifficultyLevel difficultyLevel;

    private static float dynamicDifficultValue = 8f;
    public static float auxDynamicDifficultValue = 8f;
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


    public static bool CompareGameStates(GameState newGameState)
    {
        return gameState == newGameState;
    }

    public static void MapDifficultyLevel()
    {
        if (auxDynamicDifficultValue <= 2.0f)
        {
            difficultyLevel = DifficultyLevel.Very_Easy;
        }
        else if (auxDynamicDifficultValue <= 4.0f)
        {
            difficultyLevel = DifficultyLevel.Easy;
        }
        else if (auxDynamicDifficultValue <= 6.0f)
        {
            difficultyLevel = DifficultyLevel.Medium;
        }
        else if (auxDynamicDifficultValue <= 8.0f)
        {
            difficultyLevel = DifficultyLevel.Hard;
        }
        else
        {
            difficultyLevel = DifficultyLevel.Very_Hard;
        }
    }

    public static void CalculateDynamicDifficult()
    {
        float lifeDifficulty = 10.0f * (totalPlayerLife / playerMaxLife);

        float timeDifficulty = 10.0f * ((maxExpectedTime - timePerRoom) / maxExpectedTime);

        float accuracyDifficulty = 10.0f * (successfulAttacksPerRoom / totalAttacks);


        // Puedes dise�ar una f�rmula que combine la vida del jugador y el tiempo de finalizaci�n.
        // Aseg�rate de ajustar esta f�rmula seg�n tus necesidades y preferencias.

        // Por ejemplo, puedes ponderar m�s la vida del jugador y menos el tiempo completado.
        float totalDifficulty = (lifeDifficulty * lifeWeight + timeDifficulty * timeWeight + accuracyDifficulty * accuracyWeight) / (lifeWeight + timeWeight + accuracyWeight);

        // Aseg�rate de que el valor de dificultad sea positivo o cero.
        SetDynamicDifficult(Mathf.Max(0f, totalDifficulty));
    }

    public static void CalculateDynamicDifficult2_V2()
    {
        float lifeDifficulty = 10.0f * (totalPlayerLife / playerMaxLife);
        float timeDifficulty = 10.0f * ((maxExpectedTime - timePerRoom) / maxExpectedTime);
        float accuracyDifficulty = 10.0f * (successfulAttacksPerRoom / totalAttacks);

        // Define una velocidad de cambio para cada componente de dificultad.
        float lifeChangeSpeed = 0.05f; // Ajusta esta velocidad seg�n lo dr�stico que quieras que sea el cambio.
        float timeChangeSpeed = 0.05f;
        float accuracyChangeSpeed = 0.02f;

        // Calcula la diferencia entre la dificultad actual y la nueva.
        float lifeDifficultyDifference = lifeDifficulty - auxDynamicDifficultValue;
        float timeDifficultyDifference = timeDifficulty - auxDynamicDifficultValue;
        float accuracyDifficultyDifference = accuracyDifficulty - auxDynamicDifficultValue;

        // Aplica cambios graduales.
        float newDifficulty = auxDynamicDifficultValue +
            lifeDifficultyDifference * lifeChangeSpeed +
            timeDifficultyDifference * timeChangeSpeed +
            accuracyDifficultyDifference * accuracyChangeSpeed;

        // Aseg�rate de que el valor de dificultad est� entre 0 y 10.
        newDifficulty = Mathf.Clamp(newDifficulty, 0f, 10f);

        // Establece el nuevo valor de dificultad.
        auxDynamicDifficultValue = newDifficulty;
        MapDifficultyLevel();
    }

    /// <summary>
    /// Caculate and return the next room to go based in different variables
    /// </summary>
    /// <returns></returns>
    public static string GetNextRoomName()
    {
        string levelFolder = actualGameLevel.ToString();
        string roomType = "Normal_Rooms";
        string difficulty = difficultyLevel.ToString();
        string[] roomSizes = { "Small", "Medium", "Large" };
        string roomSize = roomSizes[UnityEngine.Random.Range(0, roomSizes.Length)];
        
        string path = "Assets/Scenes/Levels/" + levelFolder + "/" + roomType + "/" + difficulty + "/" + roomSize;

        List<string> sceneNames = new List<string>();
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { path });

        foreach (string guid in sceneGuids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            sceneNames.Add(sceneName);
        }

        return sceneNames[UnityEngine.Random.Range(0,sceneNames.Count)];
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
