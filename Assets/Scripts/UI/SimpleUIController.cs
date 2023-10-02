using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerLife;
    [SerializeField] private TextMeshProUGUI generalDifficult;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();     
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController != null)
        {
            playerLife.SetText("Player Life: " + playerController.Life.ToString());
        }

        generalDifficult.SetText("General Difficult: " + GameManager.Instance.CalculateDifficulty());
    }
}
