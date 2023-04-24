using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEnd : ScreenState
{
    StatsManager statsManager;

    private void Awake()
    {
        statsManager = FindObjectOfType<StatsManager>();
        statsManager.statsParent.SetActive(false);
    }

    public override void OnEnter()
    {
        base.OnEnter();

        statsManager.statsParent.SetActive(true);
        Time.timeScale = 0; //freeze
    }

    public override void OnExit()
    {
        base.OnExit();

        statsManager.statsParent.SetActive(false);
        Time.timeScale = 1; //unfreeze
    }
}
