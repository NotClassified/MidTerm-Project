using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMissiles : LevelState
{
    [SerializeField] GameObject missilePrefab;
    float time;
    [SerializeField] float missileFrequency;
    [SerializeField] float missileSpeed;
    [SerializeField] float missileEndScale;
    [SerializeField] float missileEndTransparency;
    [SerializeField] float missileRadius;
    [SerializeField] int missileDamage;

    Vector3 spawnBoundary1;
    Vector3 spawnBoundary2;

    public override void OnEnter()
    {
        base.OnEnter();

        spawnBoundary1 = fsm.spawnBoundary1;
        spawnBoundary2 = fsm.spawnBoundary2;
        Missile missileScript = missilePrefab.GetComponent<Missile>();
        missileScript.missileSpeed = this.missileSpeed;
        missileScript.missileEndScale = this.missileEndScale;
        missileScript.missileEndTransparency = this.missileEndTransparency;
        missileScript.missileRadius = this.missileRadius;
        missileScript.missileDamage = this.missileDamage;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        time += Time.deltaTime;
        if (time > missileFrequency)
        {
            Vector3 spawn = LevelFSM.RandomVector3Range(spawnBoundary1, spawnBoundary2);
            Instantiate(missilePrefab, spawn, new Quaternion());
            time = 0;
        }
    }
}
