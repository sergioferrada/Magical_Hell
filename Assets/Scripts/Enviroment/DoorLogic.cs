using System.Collections;
using System.Collections.Generic;
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

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.IsRoomEnded())
        {
            doorCollider.enabled = true;
            spriteRenderer.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            GameManager.Instance.GoToNextRoom(nextRoomName);
        }
    }
}
