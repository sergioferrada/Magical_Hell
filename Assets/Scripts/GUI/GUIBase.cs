using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class GUIBase : MonoBehaviour
{
    private Animator animator;
    private string currentAnimationName;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayAnimation(string animName)
    {
        animator.Play(animName);
        currentAnimationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

    public string GetCurrentAnimation()
    {
        return currentAnimationName;
    }
}
