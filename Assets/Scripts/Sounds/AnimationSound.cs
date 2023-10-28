using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSound : MonoBehaviour
{

    public void PlayAudioWithVolume(AudioClip _clip, float volume)
    {
        SoundManager.Instance.PlaySoundWithVolume(_clip, volume);
    }
    public void PlayAudioClip(AudioClip _clip)
    {
        SoundManager.Instance.PlaySound(_clip);
    }

}
