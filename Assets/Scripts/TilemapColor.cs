using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapColor : MonoBehaviour
{
    private Tilemap tilemap;

    [SerializeField] Color startColor;
    [SerializeField] Color endColor;
    [SerializeField] float switchSpeed;
    private float time;
    private float timeMod;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        tilemap.color = startColor;
        time = 0f;
        timeMod = 1f;
    }

    private void Update()
    {
        tilemap.color = Color.Lerp(startColor, endColor, time);
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
