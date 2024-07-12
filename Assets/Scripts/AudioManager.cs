using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AudioManager : MonoBehaviour
{
    #region Singleton
    private static AudioManager _instance;
    public static AudioManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {

        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

        }

    }

    #endregion Singleton
    public sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    public string musicName;

    private void Start()
    {
        PlayMusic(musicName);
        musicSource.mute = GameManager.Instance.isMuteMusic();
        sfxSource.mute = GameManager.Instance.isMuteSfx();
       

    }
    public void PlayMusic(string name)
    {
        sound s = Array.Find(musicSounds, x => x.name == name);
        if (s != null)
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }
    public void PlaySFX(string name)
    {
        sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s != null)
        {
            sfxSource.clip = s.clip;
            sfxSource.Play();
        }
    }
    public void ToggleMusic()
    {
        musicSource.mute = GameManager.Instance.ToggleMusic();
    }
    public void ToggleSfx()
    {
        sfxSource.mute = GameManager.Instance.ToggleSfx();
    }
    public AudioSource GetAudioSourceReference()
    {
        return musicSource;
    }
    public void Wait(float t)
    {
        StartCoroutine(wait(t));
    }
    IEnumerator wait(float t)
    {
        yield return new WaitForSeconds(t);
    }
}
