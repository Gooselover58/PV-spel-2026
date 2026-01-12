using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicsounds, sfxsounds;
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
    public void PlayMusic(AudioClip clip)
        {
            Sound s = Array.Find(musicsounds, x => x.name == name);
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
    public void PlaySFX(AudioClip clip)
    {
        Sound s = Array.Find(sfxsounds, x => x.name == name);
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

