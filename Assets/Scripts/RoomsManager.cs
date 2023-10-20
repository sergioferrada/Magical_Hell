using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

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
            timePassed = 0;
        }

        if (CompareGameStates(GameState.InGameRoom))
        {
            timePassed += Time.unscaledDeltaTime;
            SetTimePerRoom(timePassed);
        }  
    }

    public static void CalculateExpectedTimeRoom()
    {
        float auxTimeExpected = 0;

        Enemy[] enemiesInEscene = FindObjectsOfType<Enemy>();

        foreach(var enemy in enemiesInEscene)
        {
            if(enemy is BatController)          auxTimeExpected += 3.5f;
            if(enemy is SlimeController)        auxTimeExpected += 7.0f;
            if(enemy is Enemy1Controller)       auxTimeExpected += 5.0f;
            if(enemy is Enemy2Controller)       auxTimeExpected += 6.0f;
            if(enemy is FireWormController)     auxTimeExpected += 40.0f;
        }

        auxTimeExpected += 7.0f;

        SetMaxExpectedTime(auxTimeExpected);
    }

    private void ActivateSpawnsInScene()
    {
        //Buscar spawn del jugador
        PlayerSpawner playerSpawn = FindObjectOfType<PlayerSpawner>();

        if (playerSpawn != null) 
        { 
            playerSpawn.SpawnPlayerInScene(); 
        }
        else 
        { 
            Debug.Log("PlayerSpawn reference not found in scene");
            return; 
        }

        //Buscar Spawner de enemigos
        EnemySpawner[] EnemySpawners = FindObjectsOfType<EnemySpawner>();
        
        if(EnemySpawners.Length == 0) 
        {
            Debug.Log("Enemy Spawners not found in scene");
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

        if (door != null) { 
        
            door.Activate();
        }
        else
        {
            Debug.Log("Door references not found in scene");
            return;
        }
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
        if(roomName != null) 
        { 
            return roomName; 
        }
        else 
        { 
            Debug.Log("roonName reference is null"); 
            return "0"; 
        }
        
    }

    public void SetRoomName(string name)
    {
        roomName = name;
    }

    void InitializeRoomData()
    {
        SetRoomName(SceneManager.GetActiveScene().name);
        ResetTimePerRoom();
        ResetTotalAttacks();
        ResetSuccefulAttacks();
        CalculateEnemiesInScene();
        CalculateExpectedTimeRoom();
    }

}
