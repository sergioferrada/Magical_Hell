using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCompletedTrigger : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void Update()
    {
        if(RoomsManager.Instance.CompareRoomStates(RoomsManager.RoomState.RoomFinished))
            GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.actualGameLevel = GameManager.GameLevel.Level_1;
            GameManager.Instance.tutorialCompleted = true;
            Destroy(gameObject);
        }
    }
}
