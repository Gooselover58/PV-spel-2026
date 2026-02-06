using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    public static bool isMusicOn = true;
    public static bool isSFXOn = true;
    public static bool isPostProcessingOn = true;

    public Image _musicToggle, _sfxToggle, _postProcessingToggle;
    public Slider _music, _sfx, _postProcessing;

    [SerializeField] Color toggleOnColor;
    [SerializeField] Color toggleOffColor;

    private void Awake()
    {
        _musicToggle.color = (isMusicOn) ? toggleOnColor : toggleOffColor;
        _sfxToggle.color = (isSFXOn) ? toggleOnColor : toggleOffColor;
        _postProcessingToggle.color = (isPostProcessingOn) ? toggleOnColor : toggleOffColor;

        _music.value = AudioManager.musicVolume;
        _sfx.value = AudioManager.sfxVolume;
        _postProcessing.value = CameraScript.postProcessingWeight;
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        _musicToggle.color = (isMusicOn) ? toggleOnColor : toggleOffColor;
        AudioManager.Instance.toggleMusic();
    }

    public void ToggleSFX()
    {
        isSFXOn = !isSFXOn;
        _sfxToggle.color = (isSFXOn) ? toggleOnColor : toggleOffColor;
        AudioManager.Instance.toggleSFX();
    }

    public void TogglePostProcessing()
    {
        isPostProcessingOn = !isPostProcessingOn;
        _postProcessingToggle.color = (isPostProcessingOn) ? toggleOnColor : toggleOffColor;
        CameraScript.TogglePostProcessing();
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
