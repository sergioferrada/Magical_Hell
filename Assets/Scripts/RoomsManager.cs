using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using UnityEngine.SceneManagement;
using System;

public class RoomsManager : MonoBehaviour
{
    public static RoomsManager roomsManager;

    #region ROOM VARIABLES
    private int enemiesInScene;
    private string roomName;
    #endregion

    #region AUX VARIABLES
    private float timePassed;
    #endregion

    private void Awake()
    {
        if (roomsManager == null)
        {
            roomsManager = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    private void Start()
    {
        if (CompareGameStates(GameState.PreLoadRoom))
        {
            ActivateSpawnsInScene();
            SetGameState(GameState.InGameRoom);
        }

        if (CompareGameStates(GameState.InGameRoom))
        {
            InitializeRoomData();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetGameState(GameState.PreLoadRoom);
        Start();
    }

    // Update is called once per frame
    private void Update()
    {
        if (CompareGameStates(GameState.GameOver)) {
            GoToNextRoom("GameOver");
            Destroy(gameObject);
        }

        if (CompareGameStates(GameState.RoomFinished))
        {
            //ShowStats();
            //NextRoomCreation();
            ActivateDoorInScene();
            CalculateDynamicDifficult();
        }

        if (CompareGameStates(GameState.InGameRoom))
        {
            timePassed += Time.unscaledDeltaTime;
            SetTimePerRoom(timePassed);
        }  
    }

    private void ActivateSpawnsInScene()
    {
        //Buscar spawn del jugador
        PlayerSpawner playerSpawn = FindObjectOfType<PlayerSpawner>();

        if (playerSpawn != null) { playerSpawn.SpawnPlayerInScene(); }

        //Buscar Spawner de enemigos
        EnemySpawner[] EnemySpawners = FindObjectsOfType<EnemySpawner>();
        
        if(EnemySpawners.Length == 0) { 
            
            return;
        }

        foreach (EnemySpawner spawn in EnemySpawners)
        {
            spawn.SpawnEnemy();
        }
    }

    private void ActivateDoorInScene()
    {
        //Buscar Spawner de enemigos
        DoorLogic door = FindObjectOfType<DoorLogic>();
        if(door != null) door.Activate();  
    }

    public void CalculateEnemiesInScene()
    {
        Enemy[] enemiesArray = FindObjectsOfType<Enemy>();
        enemiesInScene = enemiesArray.Length;

        foreach (Enemy enemy in enemiesArray)
        {
            if (enemy.state == CharacterBase.State.Death)
            {
                enemiesInScene--;
            }
        }
        
        if (enemiesInScene <= 0)
        {
            SetGameState(GameState.RoomFinished);
        }
    }

    public int GetEnemiesEnScene()
    {
        return enemiesInScene;
    }

    public string GetActualRoomName()
    {
        return roomName;
    }

    public void SetRoomName(string name)
    {
        roomName = name;
    }

    void InitializeRoomData()
    {
        SetRoomName(SceneManager.GetActiveScene().name);
        SetTimePerRoom(0);
        CalculateEnemiesInScene();
    }

}
