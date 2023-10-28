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
            CalculateDynamicDifficult();
            //MapDifficultyLevel();
            SetRoomName(SceneManager.GetActiveScene().name);
            ResetTimePerRoom();
            ResetTotalAttacks();
            ResetSuccefulAttacks();
            ActivateSpawnsInScene();
            CalculateEnemiesInScene();
            CalculateExpectedTimeRoom();
            SetGameState(GameState.InGameRoom);
        }

        if (CompareGameStates(GameState.InGameRoom))
        {
            
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
            CalculateDynamicDifficult2_V2();
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
        float extraTime = 0;

        Enemy[] enemiesInEscene = FindObjectsOfType<Enemy>();
        PlayerController player = FindObjectOfType<PlayerController>();

        foreach (var enemy in enemiesInEscene)
        {
            if (enemy is BatController) extraTime = 3f;
            else if (enemy is SlimeController) extraTime = 5f;
            else if(enemy is Enemy1Controller) extraTime = 3f;
            else if (enemy is Enemy2Controller) extraTime = 3f;
            else if (enemy is FireWormController) extraTime = 15f;

            auxTimeExpected += (enemy.Life / player.Damage * player.AttackDelay) + extraTime;
        }

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
            spawn.SpawnEnemyV2();
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

    

}
