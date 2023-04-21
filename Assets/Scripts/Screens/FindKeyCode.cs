using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class FindKeyCode : MonoBehaviour
{
    public static FindKeyCode instance;

    private static readonly KeyCode[] keyCodes = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>()
        .Where(k => ((int)k < (int)KeyCode.Mouse0))
        .ToArray();

    private List<KeyCode> _keysDown;

    public void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }

        _keysDown = new List<KeyCode>();
    }
    public void OnDisable()
    {
        _keysDown = null;
    }

    public KeyCode FindKey()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in keyCodes)
            {
                if (Input.GetKeyDown(key))
                {
                    return key;
                }
            }
        }
        return KeyCode.None;
    }
}
