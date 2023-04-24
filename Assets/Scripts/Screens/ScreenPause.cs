using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPause : ScreenState
{
    private GameObject pauseManager;

    private void Awake()
    {
        pauseManager = FindObjectOfType<PauseScreenUIManager>().gameObject;
    }
    private void Start()
    {
        pauseManager.SetActive(false);
    }

    public override void OnEnter()
    {
        base.OnEnter();

        pauseManager.SetActive(true);
        FindObjectOfType<LevelFSM>().activeLevel.SetActive(false);

        Time.timeScale = 0; //freeze time
    }
    public override void OnExit()
    {
        base.OnExit();

        pauseManager.SetActive(false);
        FindObjectOfType<LevelFSM>().activeLevel.SetActive(true);

        Time.timeScale = 1; //unfreeze time
    }
}
