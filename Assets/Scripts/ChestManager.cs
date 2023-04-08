using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ChestManager : MonoBehaviour
{
    List<ChestObject> chests = new List<ChestObject>();
    [SerializeField] GameObject chestPrefab;

    [SerializeField] int chestAmount;
    [SerializeField] float chestSpawnDelay;
    [SerializeField] Vector3 spawnBoundary1;
    [SerializeField] Vector3 spawnBoundary2;


    IEnumerator Start()
    {

        foreach(Transform child in transform)
        {
            chests.Add(child.GetComponent<ChestObject>());
        }

        while (true)
        {
            while (chests.Count >= chestAmount)
                yield return null;
            yield return new WaitForSeconds(chestSpawnDelay);

            NewChest();
        }
    }

    void NewChest()
    {
        float snap = .25f;
        var spawnPos = FindEmptySpace(spawnBoundary1, spawnBoundary2, chestPrefab.transform.lossyScale / 2f, snap);

        ChestObject chestScript = Instantiate(chestPrefab, spawnPos, new Quaternion(), transform).GetComponent<ChestObject>();

        chests.Add(chestScript);
    }
    Vector3 FindEmptySpace(Vector3 boundary1, Vector3 boundary2, Vector3 halfExtent, float snap)
    {
        Vector3 randSpace = RandomVector3Range(spawnBoundary1, spawnBoundary2, snap);
        if (Physics.CheckBox(randSpace, halfExtent))
        {
            return FindEmptySpace(boundary1, boundary2, halfExtent, snap);
        }

        return randSpace;
    }

    public void OpenChest(ChestObject chest)
    {
        chests.Remove(chest);
        Destroy(chest.gameObject);
    }

    public static Vector3 RandomVector3Range(Vector3 min, Vector3 max)
    {
        return new Vector3(UnityEngine.Random.Range(min.x, max.x), 
                            UnityEngine.Random.Range(min.y, max.y), 
                            UnityEngine.Random.Range(min.z, max.z));
    }
    public static Vector3 RandomVector3Range(Vector3 min, Vector3 max, float snap)
    {
        float x = SnapNum(UnityEngine.Random.Range(min.x, max.x), snap);
        float y = SnapNum(UnityEngine.Random.Range(min.y, max.y), snap);
        float z = SnapNum(UnityEngine.Random.Range(min.z, max.z), snap);

        return new Vector3(x, y, z);
    }

    public static float SnapNum (float num, float snap) => MathF.Round(num / snap) * snap;

    public static bool InBoundary(Vector3 point, Vector3 boundary1, Vector3 boundary2)
    {
        return boundary1.x <= point.x && boundary1.y <= point.y && boundary1.z <= point.z
               && boundary2.x >= point.x && boundary2.y >= point.y && boundary2.z >= point.z;
    }
}
