using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    public Slider _music, _sfx, _postProcessing;
    public Image[] _toggles;

    private void Awake()
    {
        _music.value = AudioManager.musicVolume;
        _sfx.value = AudioManager.sfxVolume;
        _postProcessing.value = CameraScript.postProcessingWeight;
    }

    public void ToggleMusic()
    {
        AudioManager.Instance.toggleMusic();
    }

    public void ToggleSFX()
    {
        AudioManager.Instance.toggleSFX();
    }

    public void MusicVolume()
    {
        AudioManager.Instance.MusicVolume(_music.value);
    }

    public void SfxVolume()
    {
        AudioManager.Instance.SFXVolume(_sfx.value);
    }

    public void PostProcessingIntensity()
    {
        CameraScript.SetPostProcessingWeight(_postProcessing.value);
    }

    public void FullScreen()
    {
        Screen.fullScreen =!Screen.fullScreen;
        Debug.Log("fullScreen");
    }

    public void Fun()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("fun");
            AudioManager.Instance.PlaySFX("fart");

        }
        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            AudioManager.Instance.PlaySFX("Fart");
        }
    }
}
