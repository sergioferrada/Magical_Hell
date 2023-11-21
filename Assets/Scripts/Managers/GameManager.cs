using UnityEngine.SceneManagement;
using UnityEngine;
using EasyTransition;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject TransitionManagerPrefab;
    public bool dynamicDifficultActivate;
    public string versionNumber;

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
        Instantiate(TransitionManagerPrefab);
    }

    public static GameManager Instance;

    private void Awake()
    {
        int idVersion = Random.Range(1, 3);

        dynamicDifficultActivate = (idVersion == 1);

        int firstGroupNumber = Random.Range(100, 300);
        int secondGroupNumber = Random.Range(100, 300);

        versionNumber = firstGroupNumber.ToString() + idVersion + secondGroupNumber.ToString();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region GAME STATES
    public enum GameState {

        MainMenu,
        Playing,
        Paused,
        GameOver,
        GameCompleted
    }

    public GameState actualGameState;

    public void SetActualGameState(GameState newGameState)
    {
        if (newGameState == GameState.GameOver)
        {
            Destroy(FindObjectOfType<MainCameraController>().gameObject);
            Destroy(FindObjectOfType<SimpleUIController>().gameObject);
            //DifficultManager.Instance.SetDynamicDifficult(1.75f);
            //DifficultManager.Instance.MapDifficultyLevel();
            ChangeScene("GameOver");
        }

        actualGameState = newGameState;
    }

    public void SetActualGameState(string gameStateName)
    {
        switch (gameStateName)
        {
            case "MainMenu":
                actualGameState = GameState.MainMenu;
                break;
            case "Playing":
                actualGameState = GameState.Playing;
                break;
            case "Paused":
                actualGameState = GameState.Paused;
                break;
            case "GameOver":
                actualGameState = GameState.GameOver;
                break;
            case "GameCompleted":
                actualGameState = GameState.GameCompleted;
                break;
        }
    }

    public bool CompareGameStates(GameState newGameState)
    {
        return actualGameState == newGameState;
    }
    
    public bool tutorialCompleted = false;

    #endregion

    #region LEVEL
    public enum GameLevel {
        
        Tutorial,
        Level_1,
        Level_2,
        Level_3,
        Level_4,
    }

    public GameLevel actualGameLevel;
    public int numberRoomsInLevel = 10;
    public int numberOfPlayedRooms = 0;

    public TransitionSettings transition;
    public TransitionSettings transition2;

    public void SetGameLevel(GameLevel newGameLevel)
    {
        actualGameLevel = newGameLevel;
    }
    #endregion 

    public void GoToNextRoom(string sceneName)
    {
        SoundManager.Instance.PlayGUISound("Transition_Sound_2", .35f);
        TransitionManager.Instance().Transition(sceneName, transition2, 0.0f);
        //SceneManager.LoadScene(sceneName);
    }

    public void ChangeScene(string SceneName)
    {
        //SoundManager.Instance.PlayGUISound("Transition_Sound");
        TransitionManager.Instance().Transition(SceneName, transition, 0.0f);  
        //SceneManager.LoadScene(SceneName);
    }

    // Funci�n para pausar el tiempo
    public void PauseTime()
    {
        Time.timeScale = 0f;
    }

    // Funci�n para reanudar el tiempo
    public void ResumeTime()
    {
        Time.timeScale = 1f;
    }
}
