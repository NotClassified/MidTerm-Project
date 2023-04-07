using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMoveFSM : MonoBehaviour
{
    PlayerMoveState[] states;
    PlayerMoveState currentState;

    enum StateIndex
    {
        Idle, Normal, Shooting
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

    //private bool IsCurrentState(StateIndex index) => currentState == states[(int)index];
    private Type GetStateType(StateIndex index) => states[(int)index].GetType();

    private void Update()
    {
        currentState.OnUpdate();

        if (Input.GetKey(KeyCode.Space))
        {
            ChangeState(GetStateType(StateIndex.Shooting));
        }
        else
        {
            //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)
            //|| Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            //{
                ChangeState(GetStateType(StateIndex.Normal));
            //}
            //else
            //{
            //    ChangeState(GetStateType(StateIndex.Idle));
            //}
        }
    }
}
