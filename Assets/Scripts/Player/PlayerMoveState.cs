using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : Player
{
    public float speed;
    float kickBackLength = .2f;
    float kickBackSpeed = 4f;

    private void Awake()
    {
        PlayerInventory.playerDamaged += StartDamageKickBackRoutine;
    }
    private void OnDestroy()
    {
        PlayerInventory.playerDamaged -= StartDamageKickBackRoutine;
    }
    void StartDamageKickBackRoutine(int playerNum, Vector3 damageSourcePos)
    {
        if (playerNum.Equals(this.playerNum))
            StartCoroutine(DamageKickBack(playerNum, damageSourcePos));
    }
    IEnumerator DamageKickBack(int playerNum, Vector3 damageSourcePos)
    {
        var angle = Vector3.Angle(transform.position, damageSourcePos);
        float time = 0;
        while (time < kickBackLength)
        {
            time += Time.deltaTime;
            transform.position += (transform.position - damageSourcePos) * kickBackSpeed * Time.deltaTime;
            yield return null;
        }
    }

    public virtual void OnEnter()
    {

    }
    public virtual void OnUpdate()
    {

    }
    public virtual void OnExit()
    {

    }
}
