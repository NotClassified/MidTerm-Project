using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenButtonComponent : MonoBehaviour
{
    public PauseScreenButton button;

    private void Awake()
    {
        PauseScreenUIManager manager = FindObjectOfType<PauseScreenUIManager>();
        GetComponent<Button>().onClick.AddListener(delegate { manager.ClickButton(this); });
    }
}
