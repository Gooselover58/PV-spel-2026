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

    private Dictionary<string, KeyCode> inputs = new Dictionary<string, KeyCode>();

    private void Awake()
    {
        inputs.Add("Left", KeyCode.A);
        inputs.Add("Right", KeyCode.D);
        inputs.Add("Up", KeyCode.W);
        inputs.Add("Down", KeyCode.S);

        inputs.Add("Jump", KeyCode.Space);
        inputs.Add("Grapple", KeyCode.LeftShift);
    }

    public KeyCode GetInput(string key)
    {
        if (!inputs.ContainsKey(key))
        {
            Debug.LogError("Could not find input in dictionary");
            return KeyCode.None;
        }
        return inputs[key];
    }

    public Vector2 GetMovement()
    {
        float x = 0;
        float y = 0;

        x += (Input.GetKey(GetInput("Right"))) ? 1 : 0;
        x += (Input.GetKey(GetInput("Left"))) ? -1 : 0;

        y += (Input.GetKey(GetInput("Up"))) ? 1 : 0;
        y += (Input.GetKey(GetInput("Down"))) ? -1 : 0;

        Vector2 movement = new Vector2(x, y);
        return movement;
    }
}
