using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBarController : MonoBehaviour
{
    private float currentTime, totalTime;
    public Image[] cooldownBars;
    public Image[] filledCompleted;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void UpdateHUD()
    {
        currentTime = playerController.passedTime;
        totalTime = playerController.AttackDelay;

        cooldownBars[0].fillAmount = currentTime / totalTime;
        cooldownBars[1].fillAmount = currentTime / totalTime;

        if (cooldownBars[0].fillAmount >= 1 && cooldownBars[1].fillAmount >= 1)
        {
            filledCompleted[0].enabled = true;
            filledCompleted[1].enabled = true;
            cooldownBars[0].enabled = false;
            cooldownBars[1].enabled = false;
        }
        else
        {
            filledCompleted[0].enabled = false;
            filledCompleted[1].enabled = false;
            cooldownBars[0].enabled = true;
            cooldownBars[1].enabled = true;
        }
    }

}
