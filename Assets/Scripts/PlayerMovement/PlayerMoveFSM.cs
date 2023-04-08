using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMoveFSM : MonoBehaviour
{
    PlayerMoveState[] states;
    PlayerMoveState currentState;

    enum States
    {
        Idle, Normal, Shooting
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
    }

    private void Start()
    {
        ChangeState(GetComponent<PlayerMoveIdle>().GetType());
    }

    public void ChangeState(Type stateType)
    {
        PlayerMoveState next = null;
        foreach(PlayerMoveState state in states)
        {
            if (state.GetType() == stateType)
            {
                if (currentState == state)
                    return; 

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

    private Type GetStateType(States index) => states[(int)index].GetType();

    private void Update()
    {
        currentState.OnUpdate();

        if (Input.GetKey(GetKeyCode(Binding.Shoot)))
        {
            ChangeState(GetStateType(States.Shooting));
        }
        else
        {
            ChangeState(GetStateType(States.Normal));
        }
    }
}
