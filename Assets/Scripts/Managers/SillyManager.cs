using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SillyManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera, sillyCamera;
    private bool isMainCamera;

    private void Awake()
    {
        SetCamera(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SceneManager.LoadScene("Game");
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            SetCamera(!isMainCamera);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            DialogueManager.Instance.WriteDialogue("Trailer_1");
        }
    }

    private void SetCamera(bool isMain)
    {
        isMainCamera = isMain;
        mainCamera.enabled = (isMainCamera) ? true : false;
        sillyCamera.enabled = (!isMainCamera) ? true : false;
    }
}
