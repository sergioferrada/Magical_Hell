using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    [Header("Activation Door Settings")]
    [SerializeField] private Sprite OpenDoorImage;
    [SerializeField] private GameObject DoorObject;
    [SerializeField] private GameObject LightFloorObject;

    [Header("Dynamic Rooms Settings")]
    [SerializeField] private string nextRoomName;
    [SerializeField] private bool dynamicRoomsActivate = true;

    private bool activate = false;

    public void Activate() {

        if (!activate)
        {
            activate = true;
            SoundManager.Instance.PlaySound("Door_Open");
            LightFloorObject.GetComponent<SpriteRenderer>().enabled = true;
            DoorObject.GetComponent<SpriteRenderer>().sprite = OpenDoorImage;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 && activate)
        {
            if(!dynamicRoomsActivate)
            {
                GameManager.Instance.GoToNextRoom(nextRoomName);
                return;
            }

            if (GameManager.Instance.CompareGameStates(GameManager.GameState.GameCompleted))
            {
                GameManager.Instance.ChangeScene("MainMenu");
            }

            if (RoomsManager.Instance.actualRoomType == RoomsManager.RoomType.Normal_Rooms)
                GameManager.Instance.numberOfPlayedRooms++;
            else if (RoomsManager.Instance.actualRoomType == RoomsManager.RoomType.Final_Rooms)
            {
                GameManager.Instance.numberOfPlayedRooms = 0;
                GameManager.Instance.actualGameLevel++;
                RoomsManager.Instance.SetNextRoomType(RoomsManager.RoomType.Normal_Rooms);
            }

            nextRoomName = RoomsManager.Instance.GetNextRoomName();

            GameManager.Instance.GoToNextRoom(nextRoomName);
        }
    }
}
