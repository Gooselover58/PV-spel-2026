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
        inputs.Add("Jump", KeyCode.Space);
        inputs.Add("Grapple", KeyCode.K);
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
}
