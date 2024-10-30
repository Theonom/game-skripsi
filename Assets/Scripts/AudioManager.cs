using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("BGM1");
        SetMusicVolume(0.5f);
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Music not found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySfx(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sfx not found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void StopSfx()
    {
        sfxSource.Stop();
    }

    public void MuteSfx(bool isMuted)
    {
        sfxSource.mute = isMuted;
    }

    public void SetSfXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public bool IsSfxMuted()
    {
        return sfxSource.mute;
    }

    public float GetSfxVolume()
    {
        return sfxSource.volume;
    }

    public void MuteMusic(bool isMuted)
    {
        musicSource.mute = isMuted;
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public bool IsMusicMuted()
    {
        return musicSource.mute;
    }

    public float GetMusicVolume()
    {
        return sfxSource.volume;
    }

}
