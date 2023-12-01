using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;

public class SimpleUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI generalDifficult;
    [SerializeField] private TextMeshProUGUI generalDifficult2v;
    [SerializeField] private TextMeshProUGUI roomNameText;
    [SerializeField] private TextMeshProUGUI enemiNumbersText;
    [SerializeField] private TextMeshProUGUI gameStateText;
    [SerializeField] private TextMeshProUGUI timePerRoomText;
    [SerializeField] private TextMeshProUGUI timeExpectedText;
    [SerializeField] private TextMeshProUGUI totalPlayerAttacks;
    [SerializeField] private TextMeshProUGUI succesAttacks;
    [SerializeField] private TextMeshProUGUI levelDifficultText;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F3))
        {
            generalDifficult.enabled = !generalDifficult.enabled;
            generalDifficult2v.enabled = !generalDifficult2v.enabled;
            roomNameText.enabled = !roomNameText.enabled;
            enemiNumbersText.enabled = !enemiNumbersText.enabled;
            gameStateText.enabled = !gameStateText.enabled;
            timePerRoomText.enabled = !timePerRoomText.enabled;
            timeExpectedText.enabled = !timeExpectedText.enabled;
            totalPlayerAttacks.enabled = !totalPlayerAttacks.enabled;
            succesAttacks.enabled = !succesAttacks.enabled;
            levelDifficultText.enabled = !levelDifficultText.enabled;
        }

        UpdateDynamicDifficult(DifficultManager.Instance.GetDynamicDifficult());
        UpdateGameState(GameManager.Instance.actualGameState);
        UpdateTimePerRoom(DifficultManager.Instance.timePerRoom);
        UpdateRoomName(RoomsManager.Instance.GetActualRoomName());
        enemiNumbersText.SetText("N° Enemigos: " + RoomsManager.Instance.GetEnemiesEnScene().ToString());
        totalPlayerAttacks.SetText("Att. Totales: " + (DifficultManager.Instance.totalAttacks));
        succesAttacks.SetText("Att. Acertados: " + (DifficultManager.Instance.successfulAttacksPerRoom));
        UpdateTimeExpected(DifficultManager.Instance.maxExpectedTime);
        generalDifficult2v.SetText("V2 Dificultad: " + GameManager.Instance.actualGameLevel.ToString());
        levelDifficultText.SetText("Dificultad Estado: " + DifficultManager.Instance.actualDifficultyLevel);
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

    public void UpdateGameState(GameManager.GameState state)
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
