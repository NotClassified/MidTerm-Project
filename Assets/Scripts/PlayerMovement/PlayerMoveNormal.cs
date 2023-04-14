using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveNormal : PlayerMoveState
{
    public float turnSpeed;
    public float speedWithAmmo;
    public float speedNoAmmo;
    Vector3 movementVertical;
    Vector3 movementHorizontal;

    protected PlayerInventory inventory;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
        PlayerInventory.ammoChange += ChangeSpeed;
    }

    private void OnDestroy()
    {
        PlayerInventory.ammoChange -= ChangeSpeed;
    }

    void ChangeSpeed(int playerNum, int ammo)
    {
        if (inventory.GetPlayerNumber() == playerNum)
        {
            speed = ammo > 0 ? speedWithAmmo : speedNoAmmo;
        }
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

        Vector3 movement = movementVertical + movementHorizontal;

        //gradually rotate player to the direction is currently moving towards
        if (movement != Vector3.zero)
        {
            Quaternion endRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, endRotation, 
                                                          turnSpeed * Time.deltaTime);
        }

        transform.position += movement.normalized * speed * Time.deltaTime;

    }
}
