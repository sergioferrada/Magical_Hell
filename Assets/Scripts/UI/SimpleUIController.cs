using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static RoomsManager;
using static GameManager;
using System;

public class SimpleUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerLife;
    [SerializeField] private TextMeshProUGUI generalDifficult;
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private TextMeshProUGUI levelNumber;
    [SerializeField] private TextMeshProUGUI gameStateText;
    [SerializeField] private TextMeshProUGUI timePerRoomText;
    [SerializeField] private TextMeshProUGUI timeExpectedText;
    [SerializeField] private TextMeshProUGUI totalPlayerAttacks;
    [SerializeField] private TextMeshProUGUI succesAttacks;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        UpdatePlayerLife(totalPlayerLife);
        UpdateDynamicDifficult(GetDynamicDifficult());
        UpdateGameState(gameState);
        UpdateTimePerRoom(timePerRoom);
        UpdateRoomName(roomsManager.GetActualRoomName());
        levelNumber.SetText("N° Enemigos: " + roomsManager.GetEnemiesEnScene().ToString());
        totalPlayerAttacks.SetText("Att. Totales: "+totalAttacks);
        succesAttacks.SetText("Att. Acertados: " + successfulAttacksPerRoom);
        UpdateTimeExpected(maxExpectedTime);
    }

    public void UpdatePlayerLife(float value)
    {
        playerLife.SetText("Player Life: " + value.ToString());
    }

    public void UpdateTimePerRoom(float value)
    {
        var ts = TimeSpan.FromSeconds(value);
        timePerRoomText.SetText("Time: " + string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds));
    }
    public void UpdateTimeExpected(float value)
    {
        var ts = TimeSpan.FromSeconds(value);
        timeExpectedText.SetText("Time Expected: " + string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds));
    }

    public void UpdateGameState(GameState state)
    {
        gameStateText.SetText("Actual GameState: " + state.ToString());
    }

    public void UpdateDynamicDifficult(float value)
    {
        generalDifficult.SetText("General Difficult: " + value.ToString());
    }

    public void UpdateRoomName(string value)
    {
        roomNameText.SetText("Room name: "+value);
    }


}
