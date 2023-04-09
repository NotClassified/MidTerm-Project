using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : MonoBehaviour
{
    public float speed;
    [HideInInspector] public PlayerMoveFSM fsm;

    public virtual void OnEnter()
    {
        if (fsm == null)
        {
            fsm = GetComponent<PlayerMoveFSM>();
        }
    }
    public virtual void OnUpdate()
    {

    }
    public virtual void OnExit()
    {

    }
}
