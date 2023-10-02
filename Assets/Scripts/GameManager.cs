using System;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject playerPrefab;

    #region GAME STATES
    public enum GameState { MainMenu, InGame, GameOver, Completed }
    public GameState gameState;
    #endregion

    #region DIFFICULTY FUNCTION VARIABLES
    float timePerRoom;
    float totalLifePointsInRoom;
    float abilitiesUsedPerRoom;
    #endregion

    int enemiesInScene;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (gameState == GameState.InGame)
        {
            InitializeRoomData();
            SpawnPlayerInScene();
            print("Nombre de la escena: " + SceneManager.GetActiveScene().name);
        }
        
        //print("Enemigos en escena:" + enemiesInScene);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsRoomEnded())
        {
            timePerRoom += Time.unscaledDeltaTime;
            var ts = TimeSpan.FromSeconds(timePerRoom);
            //print("Tiempo: " + string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds));
        }
        else
        { 
            //ShowStats();
            NextRoomCreation();
            //CalculateDifficulty();
        }
    }

    public void ChangeGameState(GameState newGameState)
    {
        gameState = newGameState;
    }

    private void SpawnPlayerInScene()
    {
        // Buscar al jugador por etiqueta "Player"
        GameObject playerInScene = GameObject.FindGameObjectWithTag("Player");
        GameObject playerSpawner = GameObject.Find("PlayerSpawn");

        if (playerInScene != null) {
            playerInScene.transform.position = playerSpawner.transform.position;
        }    
        // Si no se encuentra al jugador, instanciarlo desde el prefab
        else if (playerPrefab != null && playerInScene == null)
        {
            Instantiate(playerPrefab, playerSpawner.transform.position, Quaternion.identity);
        }
    }

    public void UpdateEnemiesArray()
    {
        enemiesInScene -= 1;
        //print("Enemigos en escena:" + enemiesInScene);
    }

    public void UpdateLife(float life)
    {
        totalLifePointsInRoom = life;
    }

    public float CalculateDifficulty()
    {
        //Obtenemos la vida del jugador 
        float playerLife = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Life;

        // Puedes diseñar una fórmula que combine la vida del jugador y el tiempo de finalización.
        // Asegúrate de ajustar esta fórmula según tus necesidades y preferencias.

        // Por ejemplo, puedes ponderar más la vida del jugador y menos el tiempo completado.
        float difficulty = (playerLife * 0.7f) - (timePerRoom * 0.3f);

        // Asegúrate de que el valor de dificultad sea positivo o cero.
        difficulty = Mathf.Max(0f, difficulty);

        return difficulty;
    }

    void ShowStats()
    {
        print("Todos los enemigos derrotados");
        var ts = TimeSpan.FromSeconds(timePerRoom);
        print("Tiempo final de la sala: " + string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds));
        UpdateLife(FindObjectOfType<PlayerController>().Life);
        print("Vida final de la sala: " + totalLifePointsInRoom);
    }

    public bool IsRoomEnded()
    {
        if (enemiesInScene <= 0) return true;

        return false;
    }

    void InitializeRoomData()
    {
        timePerRoom = 0;
        enemiesInScene = FindObjectsOfType<Enemy>().Length;
    }

    void NextRoomCreation()
    {
        
    }

    public void GoToNextRoom(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
