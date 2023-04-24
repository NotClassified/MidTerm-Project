using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState : MonoBehaviour
{
    protected LevelFSM fsm;
    protected Transform activeLevel;

    private void Awake()
    {
        fsm = FindObjectOfType<LevelFSM>();
    }

    public virtual void OnEnter()
    {
        if (activeLevel == null)
        {
            activeLevel = fsm.activeLevel.transform;
        }
    }
    public virtual void OnUpdate()
    {

    }
    public virtual void OnExit()
    {

    }
}
