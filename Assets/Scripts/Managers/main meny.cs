using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class mainmeny : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);      
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void PlayMenuSFX(string soundName)
    {
        AudioManager.Instance.PlaySFX(soundName);
    }
}
