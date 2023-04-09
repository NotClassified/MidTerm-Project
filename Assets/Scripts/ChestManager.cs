using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ChestManager : MonoBehaviour
{
    List<GameObject> chests = new List<GameObject>();
    [SerializeField] GameObject chestPrefab;

    [SerializeField] int chestAmount;
    [SerializeField] float chestSpawnDelay;
    [SerializeField] Vector3 spawnBoundary1;
    [SerializeField] Vector3 spawnBoundary2;

    public enum Items
    {
        Health, Bomb,
    }
    [Range(0, 100)] public int chanceOfBombItem;
    public int minAmmoAmount;
    public int maxAmmoAmount;

    IEnumerator Start()
    {

        foreach(Transform child in transform)
        {
            chests.Add(child.gameObject);
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

        chests.Add(Instantiate(chestPrefab, spawnPos, new Quaternion(), transform));
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

    public void OpenChest(GameObject chest, GameObject player, bool shotChest)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        ChestObject chestData = chest.GetComponent<ChestObject>();

        if (shotChest) //player shot the chest, give player either health or a bomb
        {
            if (chestData.specialItem == Items.Health)
            {
                inventory.Health++;
            }
            else if (chestData.specialItem == Items.Bomb)
            {
                inventory.CarryBombRoutine();
            }
        }
        else //player collided with the chest, give player ammo
        {
            inventory.Ammo += chestData.ammoAmount;
        }

        chests.Remove(chest);
        Destroy(chest);
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
