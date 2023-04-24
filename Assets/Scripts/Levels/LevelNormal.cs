using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNormal : LevelState
{
    public static event System.Action TimeFinished;
    float time;
    [SerializeField] float timerDuration;

    public override void OnEnter()
    {
        base.OnEnter();

        activeLevel.gameObject.SetActive(true);

        foreach (Transform child in activeLevel)
        {
            child.gameObject.SetActive(true);
        }
        time = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        time += Time.deltaTime;
        if (time > timerDuration)
        {
            TimeFinished();
        }
    }
}
