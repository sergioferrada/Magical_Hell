using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private bool isActivate = false;

    public void ActivateSpawn()
    {
        if (!isActivate) { isActivate = true; }
    }

    public void DeactivateSpawn()
    {
        if (isActivate) { isActivate = false; }
    }

    private void Update()
    {
        if (isActivate) { SpawnPlayerInScene(); }
    }

    public void SpawnPlayerInScene()
    {
        // Buscar al jugador por etiqueta "Player"
        GameObject playerInScene = GameObject.FindGameObjectWithTag("Player");

        // Si el jugador ya esta en escena moverlo al spawn
        if (playerInScene != null)
        {
            playerInScene.transform.position = transform.position;
        }
        // Si no se encuentra al jugador, instanciarlo desde el prefab
        else if (playerPrefab != null && playerInScene == null)
        {
            Instantiate(playerPrefab, transform.position, Quaternion.identity);
        }

        DeactivateSpawn();
    }
}
