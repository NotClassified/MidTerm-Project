using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [HideInInspector] public int health;
    [HideInInspector] public int ammo;

    public bool EnoughAmmo()
    {
        if (ammo > 0)
        {
            ammo--;
            return true;
        }
        return false;
    }

    ///<summary> returns health amount left </summary>
    public int DealDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.instance.GameOver(gameObject);
        }
        return health;
    }
}
