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

    private string[] lastTwoSceneNames = new string[2];
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
        if (GameManager.Instance.CompareGameStates(GameManager.GameState.Playing))
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

                if (PlayedRooms == RoomsInLevel)
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
                    if (GameManager.Instance.dynamicDifficultActivate)
                    {
                        DifficultManager.Instance.CalculateDynamicDifficult();
                        DifficultManager.Instance.ResetTimePerRoom();
                        DifficultManager.Instance.ResetTotalAttacks();
                        DifficultManager.Instance.ResetSuccefulAttacks();
                    }
                }

                ActivateSpawnsInScene();
                CalculateEnemiesInScene();
                CalculateExpectedTimeRoom();

                SetRoomState(RoomState.PlayingRoom);
            }
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
            ActivateChestSpawnInScene();
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
        PlayerAbility[] playerAbilities = player.GetComponents<PlayerAbility>();

        float totalPlayerDamage = 0;
        float totalPlayerDelay = 0;

        foreach(var ability in playerAbilities)
        {
            totalPlayerDamage += ability.damage;
            totalPlayerDelay += ability.cooldown;
        }

        totalPlayerDamage += player.Damage;
        totalPlayerDelay += player.AttackDelay;

        foreach (var enemy in enemiesInEscene)
        {
            if (enemy is BatController) extraTime = 2f;
            else if (enemy is SlimeController) extraTime = 4f;
            else if (enemy is Enemy1Controller) extraTime = 2f;
            else if (enemy is Enemy2Controller) extraTime = 2f;
            else if (enemy is FireWormController) extraTime = 12f;

            auxTimeExpected += (enemy.Life / (totalPlayerDamage) * (totalPlayerDelay)) + extraTime;
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

    private void ActivateChestSpawnInScene()
    {
        ChestSpawn chestSpawn = FindObjectOfType<ChestSpawn>();

        if (chestSpawn == null)
        {
            Debug.Log("Chest Spawn not found in scene");
            return;
        }

        chestSpawn.Activate();
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
        string nextSceneName, levelFolder, roomType, roomSize, levelDifficulty;
        int sceneObjectsLength;

        Dictionary<string, string> levelNameMappings = new()
        {
            { "Level_1", "LVL1" },
            { "Level_2", "LVL2" },
            { "Level_3", "LVL3" },
            { "Level_4", "LVL4" }
        };

        Dictionary<string, string> typeRoomNameMappings = new()
        {
            { "Normal_Rooms", "NR" },
            { "Final_Rooms", "FR" }
        };

        Dictionary<string, string> sizeNameMappings = new()
        {
            { "Small", "S" },
            { "Medium", "M" },
            { "Large", "L" }
        };

        Dictionary<string, string> difficultyMappings = new()
        {
            { "Very_Easy", "E" },
            { "Easy", "E" },
            { "Medium", "M" },
            { "Hard", "H" },
            { "Very_Hard", "H" }
        };

        do
        {
            //Get the level and room data
            levelFolder = GameManager.Instance.actualGameLevel.ToString();
            roomType = nextRoomType.ToString();
            roomSize = GetRandomRoomSize();
            levelDifficulty = DifficultManager.Instance.actualDifficultyLevel.ToString();
            sceneObjectsLength = LevelInfoReader.GetRoomCount(levelFolder, roomType, roomSize);

            //Mapping level and room data
            levelFolder = GetMappedValue(levelFolder, levelNameMappings);
            roomType = GetMappedValue(roomType, typeRoomNameMappings);
            roomSize = GetMappedValue(roomSize, sizeNameMappings);
            levelDifficulty = GetMappedValue(levelDifficulty, difficultyMappings);

            //Generate Scene Name
            nextSceneName = GenerateSceneName(levelFolder, roomType, roomSize, levelDifficulty, sceneObjectsLength);
        }
        while (IsDuplicateSceneName(nextSceneName));
       
        StoreSceneName(nextSceneName);

        return nextSceneName;
    }

    private string GetRandomRoomSize()
    {
        string[] roomSizes = { "Small", "Medium", "Large" };
        return roomSizes[Random.Range(0, roomSizes.Length)];
    }

    private bool IsDuplicateSceneName(string sceneName)
    {
        if (lastTwoSceneNames[0] == sceneName || lastTwoSceneNames[1] == sceneName)
        {
            return true;
        }

        return false;
    }

    private string GetMappedValue(string originalValue, Dictionary<string, string> mappings)
    {
        if (mappings.TryGetValue(originalValue, out string mappedValue))
        {
            return mappedValue;
        }
        return originalValue;
    }

    private string GenerateSceneName(string levelFolder, string roomType, string roomSize, string levelDifficulty, int sceneObjectsLength)
    {
        if(levelFolder == "LVL4")
        {
            return "LVL4_FR_1";
        }

        if (nextRoomType == RoomType.Normal_Rooms)
        {
            return $"{levelFolder}_{roomType}_{roomSize}_{levelDifficulty}_{Random.Range(1, sceneObjectsLength)}";
        }
        else if (nextRoomType == RoomType.Final_Rooms)
        {
            return $"{levelFolder}_{roomType}_{Random.Range(1, sceneObjectsLength)}";
        }

        return "Default_Room";
    }

    private void StoreSceneName(string sceneName)
    {
        lastTwoSceneNames[1] = lastTwoSceneNames[0];
        lastTwoSceneNames[0] = sceneName;
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
