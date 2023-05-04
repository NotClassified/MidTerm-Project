using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestPooler : MonoBehaviour
{
    public GameObject chestPrefab;
    List<GameObject> chests = new List<GameObject>();

    [SerializeField] int chestAmount;

    private void Start()
    {
        for (int i = 0; i < chestAmount; i++)
        {
            GameObject chest = Instantiate(chestPrefab, transform);
            chest.SetActive(false);
            chests.Add(chest);
        }
    }

    public GameObject GetChestInstance()
    {
        foreach (GameObject chest in chests)
        {
            if (!chest.activeSelf)
            {
                chest.SetActive(true);
                return chest;
            }
        }
        return null;
    }

    public void ReleaseChestInstance(GameObject chest)
    {
        chest.SetActive(false);
    }
}
