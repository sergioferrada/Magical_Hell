using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpController : MonoBehaviour
{
    [SerializeField] private float destroyTime;
    [SerializeField] private TMP_Text textMeshPro;
    [SerializeField] private SpriteRenderer abilityIcon;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        abilityIcon = GetComponentInChildren<SpriteRenderer>();
        Destroy(gameObject, destroyTime);
    }

    private void SetText(string text)
    {
        
    }

    public void PopUpText(string text)
    {
        animator.Play("PopUp_Animation");
        textMeshPro.SetText(text);
    }

    public void PopUpTextSprite(string text, Sprite icon = null)
    {
        animator.Play("Level_Up_Ability_Animation");
        textMeshPro.SetText(text);

        if (icon != null)
        {
            abilityIcon.enabled = true;
            abilityIcon.sprite = icon;
        }
    }

}
