using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
       
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadSounds();
        if (SceneManager.GetActiveScene().name == "s")
        {
            musicSource.enabled = true;
            PlayMusic("MainMenu");
        }
    }

    // Loads all sounds from Audio folder automatically
    private void LoadSounds()
    {
        AudioClip[] sounds = Resources.LoadAll<AudioClip>("Audio");
        sfxSounds = new Sound[sounds.Length];
        for (int i = 0; i < sounds.Length; i++)
        {
            sfxSounds[i] = new Sound();
            sfxSounds[i].clip = sounds[i];
            sfxSounds[i].name = sounds[i].name;
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Music not found!");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("SFX not found!");

        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void toggleSFX()
    {
        sfxSource.mute =!sfxSource.mute;
    }
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
    public void toggleMusic()
    {
        musicSource.mute =!musicSource.mute;
    }
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
}

