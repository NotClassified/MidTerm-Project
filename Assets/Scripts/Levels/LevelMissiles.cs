using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMissiles : LevelState
{
    private MissilePooler missilePooler;
    private MissilePooler PoolerInstance
    {
        get
        {
            if (missilePooler == null)
            {
                var pooler = FindObjectOfType<MissilePooler>();
                if (pooler == null)
                    Debug.LogError("chestpooler not found");
                else
                    missilePooler = pooler;
            }
            return missilePooler;
        }
    }

    float time;
    public float missileFrequency;
    public float missileFrequencyAcceleration;
    public float missileSpeed;
    public float missileEndScale;
    public float missileEndTransparency;
    public float missileRadius;
    public int missileDamage;

    Vector3 spawnBoundary1;
    Vector3 spawnBoundary2;

    public override void OnEnter()
    {
        base.OnEnter();

        spawnBoundary1 = fsm.spawnBoundary1;
        spawnBoundary2 = fsm.spawnBoundary2;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        time += Time.deltaTime;
        if (time > missileFrequency)
        {
            var missile = PoolerInstance.GetMissileInstance();
            missile.transform.position = LevelFSM.RandomVector3Range(spawnBoundary1, spawnBoundary2);
            time = 0;
        }
        if (missileFrequency > .2f)
        {
            missileFrequency -=  (missileFrequencyAcceleration / 1000f) * missileFrequency * Time.deltaTime;
        }
    }
}
