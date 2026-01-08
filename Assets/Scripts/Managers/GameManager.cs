using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        Global.groundLayer = LayerMask.GetMask("Ground");
    }
}
