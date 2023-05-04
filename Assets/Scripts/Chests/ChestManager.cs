using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ChestItems
{
    Health, Bomb,
}

public class ChestManager : MonoBehaviour
{
    [SerializeField] float chestSpawnDelay;
    Vector3 spawnBoundary1;
    Vector3 spawnBoundary2;

    [Range(0, 100)] public int chanceOfBombItem;
    public int minAmmoAmount;
    public int maxAmmoAmount;

    private ChestPooler chestPooler;
    private ChestPooler PoolerInstance
    {
        get
        {
            if (chestPooler == null)
            {
                var pooler = FindObjectOfType<ChestPooler>();
                if (pooler == null)
                    Debug.LogError("chestpooler not found");
                else
                    chestPooler = pooler;
            }
            return chestPooler;
        }
    }


    private void Awake()
    {
        LevelFSM levelManager = FindObjectOfType<LevelFSM>();
        spawnBoundary1 = levelManager.spawnBoundary1;
        spawnBoundary2 = levelManager.spawnBoundary2;
    }

    IEnumerator Start()
    {
        WaitForSeconds chestCooldown = new WaitForSeconds(chestSpawnDelay);

        yield return chestCooldown;
        while (true)
        {
            NewChest();
            yield return chestCooldown;
        }
    }

    void NewChest()
    {

        var chestObj = PoolerInstance.GetChestInstance();
        if (chestObj != null)
        {
            //set position of the chest
            float snap = .25f;
            var spawnPos = FindEmptySpace(spawnBoundary1, spawnBoundary2, 
                                            PoolerInstance.chestPrefab.transform.lossyScale / 2f, snap);
            chestObj.transform.position = spawnPos;

            //set chest data
            var chestScript = chestObj.GetComponent<ChestObject>();
            if ((int)(Random.Range(0, 100) / (float)chanceOfBombItem) == 0)
                chestScript.specialItem = ChestItems.Bomb;
            else
                chestScript.specialItem = ChestItems.Health;

            chestScript.ammoAmount = Random.Range(minAmmoAmount, maxAmmoAmount + 1);
        }
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
        ChestObject chestScript = chest.GetComponent<ChestObject>();

        if (shotChest) //player shot the chest, give player either health or a bomb
        {
            switch (chestScript.specialItem)
            {
                case ChestItems.Health:
                    inventory.Health++;
                    break;
                case ChestItems.Bomb:
                    inventory.CarryBombRoutine();
                    break;
                default:
                    break;
            }
        }
        else //player collided with the chest, give player ammo
        {
            inventory.Ammo += chestScript.ammoAmount;
        }

        PoolerInstance.ReleaseChestInstance(chest);
    }

    public static Vector3 RandomVector3Range(Vector3 min, Vector3 max, float snap)
    {
        float x = SnapNum(UnityEngine.Random.Range(min.x, max.x), snap);
        float y = SnapNum(UnityEngine.Random.Range(min.y, max.y), snap);
        float z = SnapNum(UnityEngine.Random.Range(min.z, max.z), snap);

        return new Vector3(x, y, z);
    }

    public static float SnapNum (float num, float snap) => Mathf.Round(num / snap) * snap;

    public static bool InBoundary(Vector3 point, Vector3 boundary1, Vector3 boundary2)
    {
        return boundary1.x <= point.x && boundary1.y <= point.y && boundary1.z <= point.z
               && boundary2.x >= point.x && boundary2.y >= point.y && boundary2.z >= point.z;
    }
}
