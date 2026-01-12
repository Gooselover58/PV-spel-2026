using System.Collections;
using System.Collections.Generic;
using UnityEngine;
   using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    public Slider _music, _sfx;
    public void toggleM()
    {
        AudioManager.instance.toggleMusic();
    }
    public void toggleS()
    {
        AudioManager.instance.toggleSFX();
    }
    public void musicVolume()
    {
        AudioManager.instance.MusicVolume(_music.value);
    }
    public void sfxVolume()
    {
        AudioManager.instance.SFXVolume(_sfx.value);
    }
    public void fullscreen()
    {
                Screen.fullScreen =!Screen.fullScreen;
    Debug.Log("fullScreen");
    }

}
