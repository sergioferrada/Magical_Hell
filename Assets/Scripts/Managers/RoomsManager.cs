using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class RoomsManager : MonoBehaviour
{
    public static RoomsManager Instance;

    #region ROOM STATE
    public enum RoomState
    {
        PreLoadRoom,
        PlayingRoom,
        RoomFinished,
    }

    public RoomState actualRoomState;

    public void SetRoomState(RoomState newState)
    {
        actualRoomState = newState;
    }

    public bool CompareRoomStates(RoomState newState)
    {
        return actualRoomState == newState;
    }
    #endregion

    #region ROOM TYPE 
    public enum RoomType
    {
        Normal_Rooms,
        Final_Rooms
    }

    public RoomType actualRoomType;
    public RoomType nextRoomType;

    public void SetActualRoomType(RoomType newRoomType)
    {
        actualRoomType = newRoomType;
    }

    public void SetNextRoomType(RoomType newRoomType)
    {
        nextRoomType = newRoomType;
    }

    #endregion

    #region ROOM VARIABLES
    private int enemiesInScene;
    private string roomName;

    private float timePassed;
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

    private void Start()
    {
        float PlayedRooms = GameManager.Instance.numberOfPlayedRooms;
        float RoomsInLevel = GameManager.Instance.numberRoomsInLevel;

        if (CompareRoomStates(RoomState.PreLoadRoom))
        {
            if (PlayedRooms == RoomsInLevel - 1)
            {
                SetNextRoomType(RoomType.Final_Rooms);
            }
            else
            {
                SetNextRoomType(RoomType.Normal_Rooms);
            }

            if(PlayedRooms == RoomsInLevel)
            {
                SetActualRoomType(RoomType.Final_Rooms);
            }
            else
            {
                SetActualRoomType(RoomType.Normal_Rooms);
            }

            SetRoomName(SceneManager.GetActiveScene().name);

            if (GameManager.Instance.actualGameLevel != GameManager.GameLevel.Tutorial)
            {
                DifficultManager.Instance.CalculateDynamicDifficult();
                DifficultManager.Instance.ResetTimePerRoom();
                DifficultManager.Instance.ResetTotalAttacks();
                DifficultManager.Instance.ResetSuccefulAttacks();
            }

            ActivateSpawnsInScene();
            CalculateEnemiesInScene();
            CalculateExpectedTimeRoom();

            SetRoomState(RoomState.PlayingRoom);
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
        if (GameManager.Instance.actualGameState == GameManager.GameState.Playing)
        {
            SetRoomState(RoomState.PreLoadRoom);
            Start();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (CompareRoomStates(RoomState.RoomFinished))
        {        
            ActivateDoorInScene();
            timePassed = 0;
        }

        if (CompareRoomStates(RoomState.PlayingRoom) &&
            GameManager.Instance.actualGameLevel != GameManager.GameLevel.Tutorial)
        {
            timePassed += Time.deltaTime;
            DifficultManager.Instance.SetTimePerRoom(timePassed);
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

        DifficultManager.Instance.SetMaxExpectedTime(auxTimeExpected);
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
            ActivateDoorInScene();
            return;
        }

        foreach (EnemySpawner spawn in EnemySpawners)
        {
            spawn.SpawnEnemyV2();
        }
    }

    private void ActivateDoorInScene()
    {
        //Search for the door in the scene
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
            SetRoomState(RoomState.RoomFinished);
        }
    }

    public string GetNextRoomName()
    {
        string levelFolder = GameManager.Instance.actualGameLevel.ToString();
        string roomType = nextRoomType.ToString();

        string path = "Assets/Scenes/Levels/" + levelFolder;

        if (nextRoomType == RoomType.Normal_Rooms)
        {
            string[] roomSizes = { "Small", "Medium", "Large" };
            string roomSize = roomSizes[Random.Range(0, roomSizes.Length)];
            path += "/" + roomType + "/" + roomSize;
        }
        else if (nextRoomType == RoomType.Final_Rooms)
            path += "/" + roomType;

        List<string> sceneNames = new List<string>();
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { path });

        foreach (string guid in sceneGuids)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guid);
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            sceneNames.Add(sceneName);
        }

        return sceneNames[Random.Range(0, sceneNames.Count)];
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
