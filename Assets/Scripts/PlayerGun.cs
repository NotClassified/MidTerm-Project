using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    LineRenderer laser;
    Vector3[] laserPositions;
    [SerializeField] float laserDuration;
    [SerializeField] AnimationCurve laserWidth;
    [SerializeField] float laserWidthMultiplier;
    [SerializeField] LayerMask laserMask;


    private void Awake()
    {
        laser = GetComponent<LineRenderer>();
        laser.enabled = false;
        laserPositions = new Vector3[2];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !laser.enabled)
        {
            StartCoroutine(ChargeLaser());
            StartCoroutine(ShootLaser());
        }
    }

    IEnumerator ChargeLaser()
    {
        laser.enabled = true;

        float time = 0;
        float width;
        //bool hasShot = false;
        while (time < laserDuration)
        {
            if (time / laserDuration < .5f)
            {
                Physics.Raycast(transform.position, transform.forward, out RaycastHit hit);
                laserPositions[0] = transform.position;
                laserPositions[1] = hit.point;
                laser.SetPositions(laserPositions);
            }

            width = laserWidth.Evaluate(time / laserDuration) * laserWidthMultiplier;
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
        yield return new WaitForSeconds(laserDuration / 2); //wait for charge

        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit);
        string hitsTag = hit.transform.tag;
        if (hitsTag.Equals(ObjectTags.Player))
        {
            print(ObjectTags.Player);
        }
        else if (hitsTag.Equals(ObjectTags.Wall))
        {
            print(ObjectTags.Wall); 
        }
        else if (hitsTag.Equals(ObjectTags.BreakableWall))
        {
            print(ObjectTags.BreakableWall);
            Destroy(hit.transform.gameObject);
        }
        else if (hitsTag.Equals(ObjectTags.Chest))
        {
            hit.transform.GetComponent<ChestObject>().Open();
        }
    }
}
