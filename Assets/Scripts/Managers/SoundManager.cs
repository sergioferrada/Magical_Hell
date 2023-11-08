using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioClip[] musicList, effectsList, guiEffectList;
    [SerializeField] private AudioSource _musicSource, _effectSource, _guiEffectSource;

    public float defaultFadeDuration = 2.0f;
    private bool isFading = false;
    private float startVolume;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SelectMusicToPlay();
    }

    /// <summary>
    /// Select the music to reproduce by the actual level of the game
    /// </summary>
    private void SelectMusicToPlay()
    {
        if (GameManager.Instance.actualGameState == GameManager.GameState.MainMenu)
        {
            _musicSource.loop = true;
            //if (_musicSource.clip.name != "1 titles INITIAL Cut")
            //{
            PlayMusicWithFade("1 titles INITIAL Cut", 5.0f);
            
            //}
        }
        else if (GameManager.Instance.CompareGameStates(GameManager.GameState.Playing))
        {
            _musicSource.loop = true;
            switch (GameManager.Instance.actualGameLevel)
            {
                case GameManager.GameLevel.Tutorial:
                    if (_musicSource.clip.name != "dungeon_air_Ambience")
                    {
                        PlayMusicWithFade("dungeon_air_Ambience", .5f);
                    }
                    
                    break;

                case GameManager.GameLevel.Level_1:
                    if (_musicSource.clip.name != "Level_1_Battle_Song")
                    {
                        PlayMusicWithFade("Level_1_Battle_Song", 5.0f);
                    }
                    break;

                case GameManager.GameLevel.Level_2:
                    if (_musicSource.clip.name != "7 battle LOOP")
                    {
                        PlayMusicWithFade("7 battle LOOP", 5.0f);
                    }
                    break;

                case GameManager.GameLevel.Level_3:
                    if (_musicSource.clip.name != "7 battle LOOP")
                    {
                        PlayMusicWithFade("7 battle LOOP", 5.0f);
                    }
                    break;

                case GameManager.GameLevel.Level_4:
                    if (_musicSource.clip.name != "7 battle LOOP")
                    { 
                        PlayMusicWithFade("7 battle LOOP", 5.0f);
                    }
                    break;
            }
        }
        else if (GameManager.Instance.CompareGameStates(GameManager.GameState.GameOver))
        {
            _musicSource.loop = false;
            if (_musicSource.clip.name != "5 rest")
            {
                PlayMusicWithFade("5 rest", 1.0f);
            }
        }
    }

    private AudioClip FindAudioInList(AudioClip[] list, string name)
    {
        AudioClip clip = Array.Find(list, x => x.name == name);

        if (clip == null)
        {
            Debug.Log("Music/SFX " + name + " not found, playing default sound");
            return list[0];
        }

        return clip;
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void PlayMusic(string name)
    {
        _musicSource.clip = FindAudioInList(musicList, name);
        _musicSource.Play();
    }

    public void PlayMusic(string name, float delayTime)
    {
        _musicSource.clip = FindAudioInList(musicList, name);
        _musicSource.PlayDelayed(delayTime);
    }

    public void PlayMusicWithFade(string name, float fadeDuration) 
    {
        startVolume = _musicSource.volume;
        _musicSource.clip = FindAudioInList(musicList, name);
        _musicSource.Play();
        StartCoroutine(FadeOn(fadeDuration));
    }

    private IEnumerator FadeOn(float fadeDuration)
    {
        float currentTime = 0f;
        _musicSource.volume = 0f;

        while(currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            _musicSource.volume = Mathf.Lerp(0, startVolume, currentTime / fadeDuration);
            yield return null;
        }

        _musicSource.volume = startVolume;
    }

    public void ChangeMusic(string name)
    {
        _musicSource.Stop();
        PlayMusic(name);
    }

    public void ChangeMusicWithFade(string name)
    {
        StartCoroutine(ChangeMusicWithDelay(name, defaultFadeDuration));
    }

    public void ChangeMusicWithFade(string name, float fadeDuration)
    {
        StartCoroutine(ChangeMusicWithDelay(name, fadeDuration));
    }

    private IEnumerator ChangeMusicWithDelay(string name, float fadeDuration)
    {
        StopMusicWithFade(fadeDuration);
        // Espera durante la duración del fundido (fadeDuration) antes de reproducir la nueva música.
        yield return new WaitForSeconds(fadeDuration);

        PlayMusicWithFade(name, fadeDuration);
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
        _musicSource.volume = startVolume;
    }

    #region SFX FUNCTIONS
    public void PlaySound(AudioClip clip)
    {
        _effectSource.clip = clip;
        _effectSource.PlayOneShot(_effectSource.clip);
    }

    public void PlaySound(AudioClip clip, float volume = 1.0f)
    {
        _effectSource.clip = clip;
        _effectSource.PlayOneShot(_effectSource.clip, volume);
    }

    public void PlaySound(string name, float volume = 1.0f)
    {
        _effectSource.clip = FindAudioInList(effectsList, name);
        _effectSource.PlayOneShot(_effectSource.clip, volume);
    }

    public bool IsClipPlaying(string clipName)
    {
        AudioClip clip = FindAudioInList(effectsList, clipName);

        if(_effectSource.isPlaying && _effectSource.clip == clip)
        {
            return true;
        }
        else
            return false;
    }

    public void StopSound()
    {
        _effectSource.Stop();
    }
    #endregion

    #region GUI SFX FUNCTIONS
    public void PlayGUISound(AudioClip clip)
    {
        _guiEffectSource.clip = clip;
        _guiEffectSource.PlayOneShot(_guiEffectSource.clip);
    }

    public void PlayGUISound(AudioClip clip, float volume = 1.0f)
    {
        _guiEffectSource.clip = clip;
        _guiEffectSource.PlayOneShot(_guiEffectSource.clip, volume);
    }
    public void PlayGUISound(string name)
    {
        _guiEffectSource.clip = FindAudioInList(guiEffectList, name);
        _guiEffectSource.PlayOneShot(_guiEffectSource.clip);
    }

    public void PlayGUISound(string name, float volume = 1.0f)
    {
        _guiEffectSource.clip = FindAudioInList(guiEffectList, name);
        _guiEffectSource.PlayOneShot(_guiEffectSource.clip, volume);
    }

    public void StopGUISound()
    {
        _guiEffectSource.Stop();
    }
    #endregion

    public void ChangeMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
