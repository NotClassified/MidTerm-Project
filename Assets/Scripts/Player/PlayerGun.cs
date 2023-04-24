using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : Player
{
    LineRenderer laser;
    Vector3[] laserPositions;
    [SerializeField] float laserSpeed;
    [SerializeField] float laserChargeDuration;
    [SerializeField] AnimationCurve laserWidth;
    [SerializeField] float laserChargeLength;
    [SerializeField] float laserWidthMultiplier;
    [SerializeField] LayerMask shootableLayer;

    PlayerMoveFSM moveFSM;
    PlayerInventory inventory;

    ///<summary> (int playerNum, string hit'sObjectTag) </summary>
    public static event Action<int, string> gunAction;

    private void Awake()
    {
        laser = GetComponent<LineRenderer>();
        laser.enabled = false;
        laserPositions = new Vector3[2];

        moveFSM = GetComponent<PlayerMoveFSM>();
        inventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        if (Input.GetKey(bm.GetKeyCode(playerNum, PlayerBindings.Shoot)) && !laser.enabled)
        {
            if (inventory.Ammo > 0 && !moveFSM.IsCurrentState(PlayerMoveFSM.MoveStates.CarryingBomb))
            {
                StartCoroutine(ChargingLaserRoutine());
                StartCoroutine(ShootingLaserRoutine());
            }
        }
    }

    IEnumerator ChargingLaserRoutine()
    {
        laser.enabled = true;

        float time = 0;
        float width;
        while (time / laserSpeed < laserChargeDuration)
        {
            if (time / laserSpeed < .5f)
            {
                laserPositions[0] = transform.position;
                laserPositions[1] = transform.position + transform.forward * laserChargeLength;
                laser.SetPositions(laserPositions);
            }

            width = laserWidth.Evaluate(time / laserSpeed) * laserWidthMultiplier;
            if (width < 0)
                width = 0;

            laser.startWidth = width;
            laser.endWidth = width;


            time += Time.deltaTime;
            yield return null;            
        }
        laser.enabled = false;
    }

    IEnumerator ShootingLaserRoutine()
    {
        yield return new WaitForSeconds(laserSpeed / 2); //wait for charge
        ShootLaser();
    }

    void ShootLaser()
    {
        inventory.Ammo--;

        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity, shootableLayer);
        if (hit.transform == null)
            return;

        //make laser full length since it is now shooting
        laserPositions[0] = transform.position;
        laserPositions[1] = hit.point;
        laser.SetPositions(laserPositions);

        //laser has hit an object, do something:
        string hitsTag = hit.transform.tag;
        if (hitsTag.Equals(ObjectTags.Player)) //damage opposing player
        {
            hit.transform.GetComponent<PlayerInventory>().DealDamage(1);
        }
        else if (hitsTag.Equals(ObjectTags.BreakableWall)) //destroy wall
        {
            hit.transform.gameObject.SetActive(false);
        }
        else if (hitsTag.Equals(ObjectTags.Chest)) //open chest
        {
            FindObjectOfType<ChestManager>().OpenChest(hit.transform.gameObject, gameObject, true);
        }

        if (gunAction != null)
            gunAction(playerNum, hitsTag);
    }

}
