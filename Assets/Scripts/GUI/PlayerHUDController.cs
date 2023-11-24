using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDController : MonoBehaviour
{
    private PlayerController player;
    private Animator animator;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        InitializeHealthHUD();
        UpdateExpBarHUD();
        UpdateStatsHUD();
        UpdateAbilitiesContainerHUD();
    }

    public void PlayAnimationHUD(string animationName)
    {
        animator.Play(animationName);
    }

    #region HEALTH BAR HUD
    private GameObject[] heartContainers;
    private Image[] heartFills;

    [Header("Health Bar HUD")]
    public Transform heartsParent;
    public GameObject heartContainerPrefab;

    private void InitializeHealthHUD()
    {
        heartContainers = new GameObject[(int)30];
        heartFills = new Image[(int)30];

        InstantiateHeartContainers();
        UpdateHeartsHUD();
    }

    public void UpdateHeartsHUD()
    {
        SetHeartContainers();
        SetFilledHearts();
    }

    void SetHeartContainers()
    {
        for (int i = 0; i < heartContainers.Length; i++)
        {
            if (i < player.MaxLife)
            {
                heartContainers[i].SetActive(true);
            }
            else
            {
                heartContainers[i].SetActive(false);
            }
        }
    }

    void SetFilledHearts()
    {
        for (int i = 0; i < heartFills.Length; i++)
        {
            if (i < player.Life)
            {
                heartFills[i].fillAmount = 1;
            }
            else
            {
                heartFills[i].fillAmount = 0;
            }
        }

        if (player.Life % 1 != 0)
        {
            int lastPos = Mathf.FloorToInt(player.Life);
            heartFills[lastPos].fillAmount = player.Life % 1;
        }
    }

    void InstantiateHeartContainers()
    {
        for (int i = 0; i < 30; i++)
        {
            GameObject temp = Instantiate(heartContainerPrefab);
            temp.transform.SetParent(heartsParent, false);
            heartContainers[i] = temp;
            heartFills[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
        }
    }
    #endregion

    #region EXP BAR HUD
    
    private float currentExp, targetExp;
    [Header("Exp Bar HUD")]
    [SerializeField] private Image progressBar;
    [SerializeField] private TMP_Text levelText;

    public void UpdateExpBarHUD()
    {
        currentExp = player.GetCurrentExp();
        targetExp = player.GetMaxExp();
        progressBar.fillAmount = (currentExp / targetExp);
        levelText.text = "LVL " + player.GetCurrentLevel();
    }
    #endregion

    #region STATS HUD
    [Header("Player Stats HUD")]
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text attackDelayText;
    [SerializeField] private TMP_Text areaText;
    [SerializeField] private TMP_Text forceImpulseText;
    [SerializeField] private TMP_Text speedMovementText;
    [SerializeField] private TMP_Text healthText;

    public void UpdateStatsHUD()
    {
        damageText.text = player.Damage.ToString();
        speedMovementText.text = player.MovementSpeed.ToString();
        areaText.text = player.AttackRange.ToString();
        forceImpulseText.text = player.ImpulseForce.ToString();
        attackDelayText.text = player.AttackDelay.ToString();
        healthText.text = player.MaxLife.ToString();
    }
    #endregion

    #region ABILITIES HUD
    [Header("Abilities HUD")]
    public AbilityStats[] abilityStats = new AbilityStats[5];
    private PlayerAbility[] playerAbilities = new PlayerAbility[5];

    public void UpdateAbilitiesContainerHUD()
    {
        playerAbilities = player.GetComponents<PlayerAbility>();

        for (int i = 0; i < playerAbilities.Length; i++)
        {
            if (abilityStats[i].activate == false)
                abilityStats[i].Activate();

            abilityStats[i].ability = playerAbilities[i];
            abilityStats[i].abilityIcon.sprite = playerAbilities[i].IconSprite;
            abilityStats[i].damageText.SetText(playerAbilities[i].damage.ToString());
            abilityStats[i].cdText.SetText(playerAbilities[i].cooldown.ToString());
            abilityStats[i].areaText.SetText(playerAbilities[i].damageObjectScale.ToString());

            if (playerAbilities[i].actualLevel < playerAbilities[i].maxLevel)
                abilityStats[i].levelText.text = "LVL " + playerAbilities[i].actualLevel.ToString();
            else if (playerAbilities[i].actualLevel >= playerAbilities[i].maxLevel)
                abilityStats[i].levelText.text = "MAX";

            abilityStats[i].expAbilityBar.fillAmount = playerAbilities[i].actualExp / playerAbilities[i].targetExp;
        }
    }
    #endregion
}
