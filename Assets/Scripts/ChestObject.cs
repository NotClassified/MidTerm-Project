using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestObject : MonoBehaviour
{
    public ChestItems specialItem;
    public int ammoAmount;

    private void OnEnable()
    {
        ChestManager manager = GetComponentInParent<ChestManager>();


        if (manager.chanceOfBombItem != 0 && (int) (Random.Range(0, 100) / (float)manager.chanceOfBombItem) == 0)
        {
            specialItem = ChestItems.Bomb;
        }
        else
        {
            specialItem = ChestItems.Health;
        }

        ammoAmount = Random.Range(manager.minAmmoAmount, manager.maxAmmoAmount + 1);
    }
}
