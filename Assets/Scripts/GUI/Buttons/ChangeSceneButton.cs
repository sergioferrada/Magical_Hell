using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField] private string sceneName; // Nombre de la escena a la que deseas cambiar.
    [SerializeField] private GameManager.GameLevel levelSelected;
    [SerializeField] private GameManager.GameState gameState;

    private Animator parentAnimator;
    private void Awake()
    {
        parentAnimator = GetComponentInParent<Animator>();
        if (parentAnimator == null)
        {
            Debug.LogError("Animator not found in parent");
        }
    }

    public void ChangeScene()
    {
        GameManager.Instance.SetActualGameState(gameState);
        GameManager.Instance.SetGameLevel(levelSelected);
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeSceneDelayed(float delayDuration)
    {
        StartCoroutine(ChangeSceneWithDelay(delayDuration));
    }

    public IEnumerator ChangeSceneWithDelay(float delayDuration)
    {
        yield return new WaitForSeconds(delayDuration);
        GameManager.Instance.SetActualGameState(gameState);

        if (!GameManager.Instance.tutorialCompleted)
            levelSelected = GameManager.GameLevel.Tutorial;

        GameManager.Instance.SetGameLevel(levelSelected);
        GameManager.Instance.ChangeScene(sceneName);
    }

    public void GoToInitialRoom()
    {
        GameManager.Instance.HideCursor();
        if (!GameManager.Instance.tutorialCompleted)
        {
            GameManager.Instance.SetActualGameState(GameManager.GameState.Playing);
            GameManager.Instance.SetGameLevel(GameManager.GameLevel.Tutorial);
            GameManager.Instance.ChangeScene("Tutorial_Room_1");
            SoundManager.Instance.StopMusicWithFade(1.0f);
        }
        else
        {
            GameManager.Instance.SetActualGameState(GameManager.GameState.Playing);
            GameManager.Instance.SetGameLevel(GameManager.GameLevel.Level_1);
            GameManager.Instance.numberOfPlayedRooms = 0;
            RoomsManager.Instance.SetActualRoomType(RoomsManager.RoomType.Normal_Rooms);
            RoomsManager.Instance.SetNextRoomType(RoomsManager.RoomType.Normal_Rooms);
            GameManager.Instance.ChangeScene("Default_Room");
            SoundManager.Instance.StopMusicWithFade(1.0f);
        }
    }

    public void ChangeScene2(string nameScene)
    {
        //parentAnimator.Play("Open_Animation");

        //GameManager.SetGameLevel(levelSelected);
        SceneManager.LoadScene(nameScene);

    }

    public void ActivateAnimation(string animationName)
    {
        parentAnimator.Play(animationName);
    }

    public void PlayButtonSound(string clipName)
    {
        SoundManager.Instance.PlayGUISound(clipName);
    }
}
