using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    [SerializeField] private string nextRoomName;
    private Collider2D doorCollider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        doorCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Activate() {

        doorCollider.enabled = true;
        spriteRenderer.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            GameManager.GoToNextRoom(nextRoomName);
        }
    }
}
