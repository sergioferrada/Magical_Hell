using UnityEngine.SceneManagement;
using UnityEngine;
using EasyTransition;
using Newtonsoft.Json;

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

        if (CompareGameStates(GameState.MainMenu))
        {
            if(FindObjectOfType<MainCameraController>())
                Destroy(FindObjectOfType<MainCameraController>().gameObject);

            if (FindObjectOfType<PlayerController>())
                Destroy(FindObjectOfType<PlayerController>().gameObject);

            if (FindObjectOfType<PlayerHUDController>())
                Destroy(FindObjectOfType<PlayerHUDController>().gameObject);
        }
    }

    public static GameManager Instance;

    public void SetRandomGameVersion()
    {
        int idVersion = Random.Range(1, 3);
        idVersion = 1;
        dynamicDifficultActivate = (idVersion == 1);
    }

    public void ChangeGameVersion()
    {
        dynamicDifficultActivate = !dynamicDifficultActivate;
    }

    public void GenerateVersionNumber()
    {
        int firstGroupNumbers = Random.Range(1000, 3000);
        int secondGroupNumbers = Random.Range(1000, 3000);

        int idVersion = dynamicDifficultActivate ? 1 : 2;
        
        versionNumber = firstGroupNumbers.ToString() + (idVersion) + secondGroupNumbers.ToString();
    }

    private void Awake()
    {
        SetRandomGameVersion();
        GenerateVersionNumber();

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
        if (newGameState == GameState.Playing)
            HideCursor();
        else if (newGameState == GameState.Paused)
            ShowCursor();
        else if (newGameState == GameState.MainMenu)
            ShowCursor();
        else if (newGameState == GameState.GameOver)
        {
            ShowCursor();
            Destroy(FindObjectOfType<MainCameraController>().gameObject);
            Destroy(FindObjectOfType<SimpleUIController>().gameObject);
            DifficultManager.Instance.ResetDifficult();
            DifficultManager.Instance.MapDifficultyLevel();
            TransitionManager.Instance().Transition("GameOver", transition, 0.0f);
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

    public void ChangeScene(string SceneName, float transitionSpeed = 1.0f)
    {
        //SoundManager.Instance.PlayGUISound("Transition_Sound");
        TransitionSettings transitionCopy = transition;
        transitionCopy.transitionSpeed = transitionSpeed;
        
        TransitionManager.Instance().Transition(SceneName, transitionCopy, 0.0f);  
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

    public void QuitGame()
    {
#if UNITY_EDITOR
        // Si est�s en el editor de Unity, simplemente det�n la reproducci�n
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Si no est�s en el editor, utiliza la funci�n de Application.Quit
        Application.Quit();
#endif
    }

    public void HideCursor()
    {
      /*  Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; */
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
