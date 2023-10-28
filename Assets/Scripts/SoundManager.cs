using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _musicSource, _effectSource;
    public float fadeDuration = 2.0f;
    private bool isFading = false;
    private float startVolume;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.PlayOneShot(clip);
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    public void StopMusicWithFade(float fadeDuration)
    {
        if (!isFading)
        {
            isFading = true;
            startVolume = _musicSource.volume;
            StartCoroutine(FadeOut(fadeDuration));
        }
    }

    private IEnumerator FadeOut(float fadeDuration)
    {
        float currentTime = 0;
        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            _musicSource.volume = Mathf.Lerp(startVolume, 0, currentTime / fadeDuration);
            yield return null;
        }

        _musicSource.volume = 0;
        isFading = false;
        _musicSource.Stop();
        _musicSource.volume = 1;
    }

    public void PlaySound(AudioClip clip)
    {
        _effectSource.PlayOneShot(clip);
    }

    public void PlaySoundWithVolume(AudioClip clip, float volume)
    {
        _effectSource.PlayOneShot(clip, volume);
    }

    public void StopSound()
    {
        _effectSource.Stop();
    }

    public void ChangeMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
