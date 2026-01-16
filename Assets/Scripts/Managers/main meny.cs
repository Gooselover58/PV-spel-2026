using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class mainmeny : MonoBehaviour
{
    // Start is called before the first frame update
   public void PlayGame()
    {
        //AudioManager.Instance.musicSource.Pause();
        SceneManager.LoadScene(1);
        
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

}
