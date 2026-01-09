using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static Transform playerTrans;
    public static Rigidbody2D playerRb;
    public static PlayerMovement playerMovement;

    public static CameraScript gameCamScript;

    public static LayerMask groundLayer;

    public static float playerGravityScale = 4f;
    public static float jumpPadBoostModifier = 1f;
}
