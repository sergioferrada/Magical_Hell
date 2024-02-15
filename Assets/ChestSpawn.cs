using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//genera un cofre en 0,0,0 ???
public class ChestSpawn : MonoBehaviour
{
    [SerializeField] private GameObject ChestPrefab;
    private bool active = false;

    public void Activate()
    {
        if (!active) { 
            active = true;
            SoundManager.Instance.PlaySound("Item_Room_Completed_Sound", .5f);
            Instantiate(ChestPrefab, transform.position, Quaternion.identity);
        }
    }

}
