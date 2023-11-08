using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesContainerController : MonoBehaviour
{
    public PlayerController playerController;

    public AbilityStats[] abilityStats = new AbilityStats[5];
    public PlayerAbility[] playerAbilities = new PlayerAbility[5];

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

}
