using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class SillyCamera : MonoBehaviour
{
    private Volume volume;

    [SerializeField] float sillyIncrease;
    [SerializeField] float sillySpeed;

    private void Awake()
    {
        volume = GetComponent<Volume>();
        volume.weight = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            volume.weight += sillyIncrease;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene("Game");
        }
    }
}
