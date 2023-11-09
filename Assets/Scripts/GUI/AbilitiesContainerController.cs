using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesContainerController : MonoBehaviour
{
    private PlayerController playerController;

    public AbilityStats[] abilityStats = new AbilityStats[5];
    public PlayerAbility[] playerAbilities = new PlayerAbility[5];

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void UpdateAbilitiesContainerHUD() { 
    
        playerAbilities = playerController.GetComponents<PlayerAbility>();

        for(int i = 0; i < playerAbilities.Length; i++)
        {
            if (abilityStats[i].activate == false)
                abilityStats[i].Activate();

            abilityStats[i].ability = playerAbilities[i];
            abilityStats[i].abilityIcon.sprite = playerAbilities[i].IconSprite;

            if (playerAbilities[i].actualLevel < playerAbilities[i].maxLevel)
                abilityStats[i].levelText.text = "LVL " + playerAbilities[i].actualLevel.ToString();
            else if (playerAbilities[i].actualLevel >= playerAbilities[i].maxLevel)
                abilityStats[i].levelText.text = "MAX";

            abilityStats[i].expAbilityBar.fillAmount = playerAbilities[i].actualExp / playerAbilities[i].targetExp;
        }
    }

}
