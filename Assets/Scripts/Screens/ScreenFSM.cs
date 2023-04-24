using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ScreenType
{
    Game, Pause, End
}

public class ScreenFSM : MonoBehaviour
{
    ScreenState[] states;
    ScreenState currentState;

    private void Awake()
    {
        states = GetComponents<ScreenState>();
        PlayerInventory.playerDied += EndingScreen;
    }
    private void OnDestroy()
    {
        PlayerInventory.playerDied -= EndingScreen;
    }

    void EndingScreen(int losingPlayerNum)
    {
        ChangeState(ScreenType.End);
    }

    public void ChangeState(Type stateType)
    {
        ScreenState next = null;
        foreach (ScreenState state in states)
        {
            if (state.GetType() == stateType)
            {
                if (currentState == state)
                {
                    Debug.LogWarning("This State is already active: " + currentState);
                    return;
                }

                next = state;
                break;
            }
        }

        if (currentState != null)
        {
            currentState.OnExit();
        }
        next.OnEnter();
        currentState = next;
    }

    public void ChangeState(ScreenType state)
    {
        //do not change the state to the same state
        if (currentState != null && currentState == GetState(state))
            return;
        ChangeState(GetStateType(state));
    }

    private Type GetStateType(ScreenType index) => states[(int)index].GetType();
    private ScreenState GetState(ScreenType index) => states[(int)index];
    public ScreenType GetCurrentState()
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i] == currentState)
            {
                return (ScreenType)i;
            }
        }
        Debug.LogError("Could not get current state");
        return 0;
    }
    public bool IsCurrentState(ScreenType state)
    {
        return currentState == GetState(state);
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
        else //begin with pause menu
            ChangeState(ScreenType.Pause);

    }
}
