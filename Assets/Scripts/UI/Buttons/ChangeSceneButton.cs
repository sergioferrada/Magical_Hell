using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField] private string sceneName; // Nombre de la escena a la que deseas cambiar.
    [SerializeField] private GameManager.GameLevel levelSelected;

    public void ChangeScene()
    {
        GameManager.SetGameLevel(levelSelected);
        SceneManager.LoadScene(sceneName);
    }
}
