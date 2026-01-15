using System.Collections;
using System.Collections.Generic;
using UnityEngine;
   using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    public Slider _music, _sfx;
    public void toggleM()
    {
        AudioManager.Instance.toggleMusic();
    }
    public void toggleS()
    {
        AudioManager.Instance.toggleSFX();
    }
    public void musicVolume()
    {
        AudioManager.Instance.MusicVolume(_music.value);
    }
    public void sfxVolume()
    {
        AudioManager.Instance.SFXVolume(_sfx.value);
    }
    public void fullscreen()
    {
                Screen.fullScreen =!Screen.fullScreen;
    Debug.Log("fullScreen");
    }
    public void fun()
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
