using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpController : MonoBehaviour
{
    [SerializeField] private float destroyTime;
    [SerializeField] private TMP_Text textMeshPro;

    private void Awake()
    {
        Destroy(gameObject, destroyTime);
    }

    public void SetText(string text)
    {
        textMeshPro.SetText(text);
    }
}
