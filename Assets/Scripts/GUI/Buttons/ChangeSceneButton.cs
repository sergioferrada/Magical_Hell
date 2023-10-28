using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField] private string sceneName; // Nombre de la escena a la que deseas cambiar.
    [SerializeField] private GameManager.GameLevel levelSelected;
    private Animator parentAnimator;
    private void Awake()
    {
        parentAnimator = GetComponentInParent<Animator>();
        if (parentAnimator == null)
        {
            Debug.LogError("Animator not found in parent!");
        }
    }

    public void ChangeScene()
    {
        //parentAnimator.Play("Open_Animation");

        GameManager.SetGameLevel(levelSelected);
        SceneManager.LoadScene(sceneName);
        
    }

    public void ActivateAnimation(string animationName)
    {
        parentAnimator.Play(animationName);
    }
}
