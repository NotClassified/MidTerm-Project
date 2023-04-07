using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    List<ChestObject> chests = new List<ChestObject>();
    [SerializeField] GameObject chestPrefab;

    [SerializeField] int chestAmount;
    [SerializeField] float chestSpawnDelay;
    [SerializeField] bool chestSpawnOnStart;
    [SerializeField] Vector3 spawnBoundary1;
    [SerializeField] Vector3 spawnBoundary2;


    IEnumerator Start()
    {

        if (chestSpawnOnStart)
        {
            NewChest();
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
        Vector3 spawnPos = RandomVector3Range(spawnBoundary1, spawnBoundary2);
        ChestObject chestScript = Instantiate(chestPrefab, spawnPos, new Quaternion(), transform).GetComponent<ChestObject>();

        chests.Add(chestScript);
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
}
