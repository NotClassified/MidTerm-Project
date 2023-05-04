using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum LevelMode
{
    Normal, Collapse, Missiles
}

public class LevelFSM : MonoBehaviour
{

    LevelState[] states;
    LevelState currentState;

    [SerializeField] GameObject levelPrefab;
    [HideInInspector] public GameObject activeLevel;

    public Vector3 spawnBoundary1;
    public Vector3 spawnBoundary2;

    private void Awake()
    {
        states = GetComponents<LevelState>();
        LevelCollapse.allObjectsCollapsed += CollapsingFinished;
        LevelNormal.TimeFinished += NormalLevelStateFinished;
        ScreenGame.GameHasStarted += GameStart;

        activeLevel = Instantiate(levelPrefab, transform);
    }
    private void OnDestroy()
    {
        LevelCollapse.allObjectsCollapsed -= CollapsingFinished;
        LevelNormal.TimeFinished -= NormalLevelStateFinished;
        ScreenGame.GameHasStarted -= GameStart;
    }

    void GameStart() => ChangeState(LevelMode.Normal);
    void NormalLevelStateFinished() => ChangeState(LevelMode.Missiles);
    void CollapsingFinished()
    {
        ChangeState(LevelMode.Missiles);
    }

    public void ChangeState(Type stateType)
    {
        LevelState next = null;
        foreach (LevelState state in states)
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

    public void ChangeState(LevelMode state)
    {
        //do not change the state to the same state
        if (currentState != null && currentState == GetState(state))
            return;
        ChangeState(GetStateType(state));
    }

    private Type GetStateType(LevelMode index) => states[(int)index].GetType();
    private LevelState GetState(LevelMode index) => states[(int)index];
    public LevelMode GetCurrentState()
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i] == currentState)
            {
                return (LevelMode)i;
            }
        }
        Debug.LogError("Could not get current state");
        return 0;
    }
    public bool IsCurrentState(LevelMode state)
    {
        return currentState == GetState(state);
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }

    }
    public static Vector3 RandomVector3Range(Vector3 min, Vector3 max)
    {
        return new Vector3(UnityEngine.Random.Range(min.x, max.x),
                            UnityEngine.Random.Range(min.y, max.y),
                            UnityEngine.Random.Range(min.z, max.z));
    }
}
