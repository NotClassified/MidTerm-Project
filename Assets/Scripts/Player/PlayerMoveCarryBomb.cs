using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCarryBomb : PlayerMoveNormal
{
    private bool thisStateActive;
    public override void OnEnter()
    {
        base.OnEnter();
        thisStateActive = true;
    }
    public override void OnExit()
    {
        base.OnExit();
        thisStateActive = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //when player is carrying bomb, the bomb auto activates if in contact with another player
        if (thisStateActive && collision.gameObject.tag == ObjectTags.Player)
        {
            GetComponent<PlayerInventory>().collidedPlayerWithBomb = true;
        }
    }
}
