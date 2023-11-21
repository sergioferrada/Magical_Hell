using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSound : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f,1.0f)] private float SoundsEffectVolume = 1.0f;

    public void PlayAudioClip(AudioClip _clip)
    {
        if(!SoundManager.Instance.IsClipPlaying(_clip.name))
            SoundManager.Instance.PlaySound(_clip.name, SoundsEffectVolume);
    }
}
