using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCompletedTrigger : MonoBehaviour
{
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
