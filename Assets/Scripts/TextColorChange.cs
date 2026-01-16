using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TextColorChange : MonoBehaviour
{
    private TextMeshProUGUI text;

    [SerializeField] Color startColor;
    [SerializeField] Color endColor;
    [SerializeField] float switchSpeed;
    private float time;
    private float timeMod;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.color = startColor;
        time = 0f;
        timeMod = 1f;
    }

    private void Update()
    {
        text.color = Color.Lerp(startColor, endColor, time);
        time += Time.deltaTime * timeMod * switchSpeed;
        if (time > 1f)
        {
            timeMod = -1f;
        }
        if (time < 0f)
        {
            timeMod = 1f;
        }
    }
}
