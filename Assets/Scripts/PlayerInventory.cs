using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    int playerNum;

    int health;
    int ammo;
    bool hasBomb;
    public int Health
    {
        get => health;
        set
        {
            health = value;
            healthChange(playerNum, health);
        }
    }
    public int Ammo
    {
        get => ammo;
        set
        {
            ammo = value;
            ammoChange(playerNum, ammo);
        }
    }
    public bool HasBomb
    {
        get => hasBomb;
        set
        {
            hasBomb = value;
            bombChange(playerNum, hasBomb);
        }
    }

    public void SetPlayerNumber(int num, Material material)
    {
        playerNum = num;
        GetComponent<MeshRenderer>().material = material;
    }
    public int GetPlayerNumber() => playerNum;


    PlayerMoveFSM moveFSM;

    public static event Action<int, int> healthChange;
    public static event Action<int, int> ammoChange;
    public static event Action<int, bool> bombChange;

    private void Awake()
    {
        moveFSM = GetComponent<PlayerMoveFSM>();
    }

    ///<summary> returns health amount left </summary>
    public int DealDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            GameManager.instance.GameOver(gameObject);
        }
        return Health;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Open Chest
        if (collision.gameObject.tag == ObjectTags.Chest)
        {
            FindObjectOfType<ChestManager>().OpenChest(collision.gameObject, gameObject, false);
        }
    }

    public void CarryBombRoutine() => StartCoroutine(CarryBomb());
    IEnumerator CarryBomb()
    {
        HasBomb = true;
        while (!Input.GetKeyUp(moveFSM.GetKeyCode(PlayerMoveFSM.Binding.Shoot)))
        {
            yield return null;
        }
        print("Bomb Exploded");
        HasBomb = false;
    }
}
