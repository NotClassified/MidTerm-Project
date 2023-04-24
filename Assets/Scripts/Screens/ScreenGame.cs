using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenGame : ScreenState
{
    private GameObject playerUIManager;
    public static event System.Action GameHasStarted;

    private void Awake()
    {
        playerUIManager = FindObjectOfType<PlayerUIManager>().gameObject;
    }
    private void Start()
    {
        playerUIManager.SetActive(false);
    }

    public override void OnEnter()
    {
        base.OnEnter();

        playerUIManager.SetActive(true);

        if (GameHasStarted != null)
        {
            GameHasStarted();
            GameHasStarted = null;
        }
    }
    public override void OnExit()
    {
        base.OnExit();

        playerUIManager.SetActive(false);
    }
}
