using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMoveFSM : MonoBehaviour
{
    PlayerMoveState[] states;
    PlayerMoveState currentState;

    public enum MoveStates
    {
        Normal, Shooting, CarryingBomb
    }

    KeyCode[] inputBindings = { KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.A, KeyCode.Space };
    public enum Binding
    {
        Up, Down, Right, Left, Shoot,
    }
    public KeyCode GetKeyCode(Binding b)
    {
        return inputBindings[(int) b];
    }
    public void SetBinding(Binding binding, KeyCode key)
    {
        inputBindings[(int)binding] = key;
    }

    private void Awake()
    {
        states = GetComponents<PlayerMoveState>();
        PlayerInventory.bombChange += SetBombState;
    }
    private void OnDestroy()
    {
        PlayerInventory.bombChange -= SetBombState;
    }

    public void ChangeState(Type stateType)
    {
        PlayerMoveState next = null;
        foreach(PlayerMoveState state in states)
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

    public void ChangeStateIfNotCurrent(MoveStates state)
    {
        if (currentState == null)
            return;

        if (currentState != GetState(state))
        {
            ChangeState(GetStateType(state));
        }
    }

    private Type GetStateType(MoveStates index) => states[(int)index].GetType();
    private PlayerMoveState GetState(MoveStates index) => states[(int)index];
    public MoveStates GetCurrentState()
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i] == currentState)
            {
                return (MoveStates) i;
            }
        }
        Debug.LogError("Could not get current state");
        return 0;
    }
    public bool IsCurrentState(MoveStates state)
    {
        return currentState == GetState(state);
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
        else //begin with normal movement
            ChangeState(GetStateType(MoveStates.Normal));

        if (currentState != GetState(MoveStates.CarryingBomb))
        {
            if (Input.GetKey(GetKeyCode(Binding.Shoot))) //player is holding down shoot key
            {
                ChangeStateIfNotCurrent(MoveStates.Shooting);
            }
            else
            {
                ChangeStateIfNotCurrent(MoveStates.Normal);
            }
        }

    }

    void SetBombState(int playerNum, bool hasBomb)
    {
        if (GetComponent<PlayerInventory>().GetPlayerNumber() == playerNum)
        {
            if (hasBomb)
            {
                ChangeStateIfNotCurrent(MoveStates.CarryingBomb);
            }
            else
            {
                ChangeStateIfNotCurrent(MoveStates.Normal);
            }
        }
    }
}
