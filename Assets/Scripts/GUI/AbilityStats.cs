using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityStats : MonoBehaviour
{
    public PlayerAbility ability;

    public Image abilityIcon;
    public TMP_Text levelText;
    public Image expAbilityBar;

    public bool activate = false;

    private void Start()
    {
        abilityIcon.enabled = false;
        levelText.enabled = false;
        expAbilityBar.enabled = false;
    }

    public void Activate()
    {
        activate = true;
        abilityIcon.enabled = true;
        levelText.enabled = true;
        expAbilityBar.enabled = true;
    }
}
