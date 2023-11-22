using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownBarController : MonoBehaviour
{
    private float currentTime, totalTime;
    public Canvas CloseAttackUI;
    public Image cooldownBars;
    public Image filledCompleted;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void UpdateHUD()
    {
        currentTime = playerController.passedTime;
        totalTime = playerController.AttackDelay;

        cooldownBars.fillAmount = currentTime / totalTime;

        if (cooldownBars.fillAmount >= 1)
        {
            SoundManager.Instance.PlaySound("Meele_Attack_Enable_Sound", .5f);
            filledCompleted.enabled = true;
            //cooldownBars.enabled = false;
        }
        else
        {
            filledCompleted.enabled = false;
            cooldownBars.enabled = true;
        }
    }

}
