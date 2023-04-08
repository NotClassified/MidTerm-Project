using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveShooting : PlayerMoveState
{
    Vector3 movementVertical;
    Vector3 movementHorizontal;

    public override void OnEnter()
    {
        base.OnEnter();

        Vector3 snapRotation = transform.forward;
        snapRotation.x = Mathf.Round(snapRotation.x);
        snapRotation.z = Mathf.Round(snapRotation.z);

        transform.forward = snapRotation;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (Input.GetKey(fsm.GetKeyCode(PlayerMoveFSM.Binding.Up)))
        {
            movementVertical = Vector3.forward;
        }
        else if (Input.GetKey(fsm.GetKeyCode(PlayerMoveFSM.Binding.Down)))
        {
            movementVertical = Vector3.back;
        }
        else
            movementVertical = Vector3.zero;

        if (Input.GetKey(fsm.GetKeyCode(PlayerMoveFSM.Binding.Right)))
        {
            movementHorizontal = Vector3.right;
        }
        else if (Input.GetKey(fsm.GetKeyCode(PlayerMoveFSM.Binding.Left)))
        {
            movementHorizontal = Vector3.left;
        }
        else
            movementHorizontal = Vector3.zero;

        transform.position += (movementVertical + movementHorizontal).normalized * speed * Time.deltaTime;
    }
}
