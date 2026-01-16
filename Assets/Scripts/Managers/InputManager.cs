using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;

    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InputManager>();
            }
            return instance;
        }
    }

    private Dictionary<string, KeyCode[]> inputs = new Dictionary<string, KeyCode[]>();

    private void Awake()
    {
        inputs.Add("Left", new KeyCode[] { KeyCode.A, KeyCode.LeftArrow });
        inputs.Add("Right", new KeyCode[] { KeyCode.D, KeyCode.RightArrow });
        inputs.Add("Up", new KeyCode[] { KeyCode.W, KeyCode.UpArrow });
        inputs.Add("Down", new KeyCode[] { KeyCode.S, KeyCode.DownArrow });

        inputs.Add("Jump", new KeyCode[] { KeyCode.Space });
        inputs.Add("Grapple", new KeyCode[] { KeyCode.LeftShift });
        inputs.Add("Settings", new KeyCode[] { KeyCode.Escape });
    }

    public bool GetInput(string key)
    {
        if (!inputs.ContainsKey(key))
        {
            Debug.LogError("Could not find input in dictionary");
            return false;
        }

        bool isInputDown = false;

        foreach (KeyCode input in inputs[key])
        {
            if (Input.GetKey(input))
            {
                isInputDown = true;
                break;
            }
        }
        return isInputDown;
    }

    public bool GetInputDown(string key)
    {
        if (!inputs.ContainsKey(key))
        {
            Debug.LogError("Could not find input in dictionary");
            return false;
        }

        bool isInputDown = false;

        foreach (KeyCode input in inputs[key])
        {
            if (Input.GetKeyDown(input))
            {
                isInputDown = true;
                break;
            }
        }
        return isInputDown;
    }

    public Vector2 GetMovement()
    {
        float x = 0;
        float y = 0;

        x += (GetInput("Right")) ? 1 : 0;
        x += (GetInput("Left")) ? -1 : 0;

        y += (GetInput("Up")) ? 1 : 0;
        y += (GetInput("Down")) ? -1 : 0;

        int speedInt;
        if (Global.playerGrappling.playerState == PlayerGrappling.PlayerState.FREE)
        {
            speedInt = (x != 0) ? 1 : 0;
        }
        else
        {
            speedInt = 0;
        }
        Global.playerMovement.anim.SetInteger("Speed", speedInt);

        Vector2 movement = new Vector2(x, y);
        return movement;
    }
}
