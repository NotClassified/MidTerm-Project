using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    LineRenderer laser;
    Vector3[] laserPositions;
    [SerializeField] float laserSpeed;
    [SerializeField] float laserDuration;
    [SerializeField] AnimationCurve laserWidth;
    [SerializeField] float laserWidthMultiplier;
    [SerializeField] LayerMask laserMask;

    PlayerMoveFSM moveFSM;
    PlayerInventory inventory;

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
        if (Input.GetKey(moveFSM.GetKeyCode(PlayerMoveFSM.Binding.Shoot)) && !laser.enabled)
        {
            if (inventory.EnoughAmmo())
            {
                StartCoroutine(ChargeLaser());
                StartCoroutine(ShootLaser());
            }
        }
    }

    IEnumerator ChargeLaser()
    {
        laser.enabled = true;

        float time = 0;
        float width;
        //bool hasShot = false;
        while (time / laserSpeed < laserDuration)
        {
            if (time / laserSpeed < .5f)
            {
                Physics.Raycast(transform.position, transform.forward, out RaycastHit hit);
                laserPositions[0] = transform.position;
                laserPositions[1] = hit.point;
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

    IEnumerator ShootLaser()
    {
        yield return new WaitForSeconds(laserSpeed / 2); //wait for charge

        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit);
        string hitsTag = hit.transform.tag;
        if (hitsTag.Equals(ObjectTags.Player)) //damage opposing player
        {
            hit.transform.GetComponent<PlayerInventory>().DealDamage(1);
        }
        else if (hitsTag.Equals(ObjectTags.Wall)) //do nothing -_-
        {
            print(ObjectTags.Wall); 
        }
        else if (hitsTag.Equals(ObjectTags.BreakableWall)) //destroy wall
        {
            Destroy(hit.transform.gameObject); 
        }
        else if (hitsTag.Equals(ObjectTags.Chest)) //open chest
        {
            hit.transform.GetComponent<ChestObject>().Open(); 
        }
    }
}
