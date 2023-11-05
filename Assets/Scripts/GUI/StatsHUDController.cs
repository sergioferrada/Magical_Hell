using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsHUDController : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText, speedMovementText, projectileDamageText, AttackDelayText, HealthText;

    private PlayerController pc;

    private void Start()
    {
        // Should I use lists? Maybe :)
        pc = GetComponent<PlayerController>();
        UpdateStatsHUD();
    }

    public void UpdateStatsHUD()
    {
        damageText.text = pc.Damage.ToString();
        speedMovementText.text = pc.MovementSpeed.ToString();
        projectileDamageText.text = pc.Damage.ToString();
        AttackDelayText.text = pc.AttackDelay.ToString();
        HealthText.text = pc.MaxLife.ToString();
    }
}
