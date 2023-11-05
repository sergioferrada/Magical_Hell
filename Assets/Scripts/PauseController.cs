using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject PausePanelGUI;
    public GameManager.GameState previousGameState;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameManager.Instance.CompareGameStates(GameManager.GameState.Playing))
                PauseGame();
            else if(GameManager.Instance.CompareGameStates(GameManager.GameState.Paused))
                ContinueGame();
        }    
    }

    public void PauseGame()
    {
        previousGameState = GameManager.Instance.actualGameState;
        GameManager.Instance.SetActualGameState(GameManager.GameState.Paused);
        //PausePanelGUI.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void ContinueGame()
    {
        GameManager.Instance.SetActualGameState(previousGameState);
        //PausePanelGUI.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
