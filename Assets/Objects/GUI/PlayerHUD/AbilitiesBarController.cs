using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesBarController : MonoBehaviour
{
    private GameObject[] abilitiesContainers;
    private GameObject[] abilities;

    public Transform abilitiesParent;
    public GameObject abilitiesContainerPrefab;

    private PlayerController pc;
    private PlayerAbility[] abilitiesArray;

    // Start is called before the first frame update
    void Start()
    {
        abilitiesArray = GetComponents<PlayerAbility>();

        abilitiesContainers = new GameObject[abilitiesArray.Length];
        abilities = new GameObject[abilitiesArray.Length];

        InstantiateAbilitiesContainers();
        UpdateAbilitiesHUD();
    }

    public void UpdateAbilitiesHUD()
    {
        SetAbilitiesContainers();
        SetAbilitiesRecollected();
    }

    void SetAbilitiesContainers()
    {
        for(int i = 0; i < abilitiesContainers.Length; i++)
        {
            if (i < 5)
            {
                abilitiesContainers[i].SetActive(true);
            }
            else
            {
                abilitiesContainers[i].SetActive(false); 
            }    
        }
    }

    void SetAbilitiesRecollected()
    {
        for(int i = 0; i < abilities.Length; i++)
        {
            if(i < abilitiesArray.Length)
            {
               
            }
        }
    }

    void InstantiateAbilitiesContainers()
    {

    }
}
