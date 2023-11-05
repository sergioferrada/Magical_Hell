using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarController : MonoBehaviour
{
    private float currentExp, targetExp;
    [SerializeField] private Image progressBar;
    [SerializeField] private TMP_Text levelText;

    private PlayerController pc;
    private void Start()
    {
        pc = GetComponent<PlayerController>();
        UpdateExpBarHUD();
    }

    public void UpdateExpBarHUD()
    {
        currentExp = pc.GetCurrentExp();
        targetExp = pc.GetMaxExp();
        progressBar.fillAmount = (currentExp / targetExp);
        levelText.text = "LVL " + pc.GetCurrentLevel();
    }
}
