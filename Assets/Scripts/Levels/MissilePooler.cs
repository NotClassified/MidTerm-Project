using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePooler : MonoBehaviour
{
    LevelMissiles levelMissiles;

    public GameObject missilePrefab;
    List<GameObject> missiles = new List<GameObject>();

    [SerializeField] int startingPoolSize;

    private void Start()
    {
        levelMissiles = FindObjectOfType<LevelMissiles>();

        for (int i = 0; i < startingPoolSize; i++)
        {
            CreateMissileInstance().SetActive(false);
        }
    }

    GameObject CreateMissileInstance()
    {
        GameObject missile = Instantiate(missilePrefab, transform);
        missiles.Add(missile);

        var script = missile.GetComponent<Missile>();
        script.missilePooler = this;
        script.levelMissiles = levelMissiles;

        return missile;
    }

    public GameObject GetMissileInstance()
    {
        foreach (GameObject missile in missiles)
        {
            if (!missile.activeSelf)
            {
                missile.SetActive(true);
                return missile;
            }
        }

        return CreateMissileInstance();
    }

    public void ReleaseMissileInstance(GameObject missile)
    {
        missile.SetActive(false);
        ResetMissile(missile);
    }

    void ResetMissile(GameObject missile)
    {
        missile.transform.position = missilePrefab.transform.position;

        var script = missile.GetComponent<Missile>();
        script.time = 0;
        script.indicatorSize = 0;
    }
}
