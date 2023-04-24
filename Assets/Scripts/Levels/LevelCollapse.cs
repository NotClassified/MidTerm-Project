using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCollapse : LevelState
{
    [SerializeField] float collapseFrequency;
    public static event System.Action allObjectsCollapsed;

    public override void OnEnter()
    {
        base.OnEnter();

        StartCoroutine(CollapseLevel());
    }

    IEnumerator CollapseLevel()
    {
        yield return new WaitForEndOfFrame();

        List<GameObject> levelObjects = new List<GameObject>();
        foreach (Transform child in activeLevel)
        {
            levelObjects.Add(child.gameObject);
        }

        float time = 0;
        while (levelObjects.Count > 0)
        {
            if (time > collapseFrequency)
            {
                //choose random level object and deactivate it
                int rand = Random.Range(0, levelObjects.Count);
                if (levelObjects[rand] != null)
                {
                    levelObjects[rand].SetActive(false);
                }
                levelObjects.RemoveAt(rand);

                time = 0;
            }
            else
                time += Time.deltaTime;

            yield return null;
        }
        yield return new WaitForEndOfFrame();
        allObjectsCollapsed();
    }
}
