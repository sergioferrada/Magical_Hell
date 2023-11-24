using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityStats : MonoBehaviour
{
    [Header("Ability Player")]
    public PlayerAbility ability;

    [Header("UI Components")]
    public Image border;
    public Image abilityIcon;
    public TMP_Text levelText;
    public Image expAbilityBar;
    public GameObject damage, cooldown, area;
    public TMP_Text damageText, cdText, areaText;
    public bool activate = false;

    private void Awake()
    {
        damageText = damage.GetComponentInChildren<TMP_Text>();
        cdText = cooldown.GetComponentInChildren<TMP_Text>();
        areaText = area.GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        Color actualColor = border.color;
        actualColor.a = .5f;
        border.color = actualColor;

        abilityIcon.enabled = false;
        levelText.enabled = false;
        expAbilityBar.enabled = false;
        damage.GetComponent<Image>().enabled = false;
        area.GetComponent<Image>().enabled = false;
        cooldown.GetComponent<Image>().enabled = false;
        damageText.enabled = false;
        areaText.enabled = false;
        cdText.enabled = false;
    }

    public void Activate()
    {
        activate = true;

        Color actualColor = border.color;
        actualColor.a = 1f;
        border.color = actualColor;

        abilityIcon.enabled = true;
        levelText.enabled = true;
        expAbilityBar.enabled = true;
        damage.GetComponent<Image>().enabled = true;
        area.GetComponent<Image>().enabled = true;
        cooldown.GetComponent<Image>().enabled = true;
        damageText.enabled = true;
        areaText.enabled = true;
        cdText.enabled = true;
    }
}
