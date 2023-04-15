using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInventory : Player
{
    [SerializeField] float bombRadius;
    [SerializeField] int bombDamage;

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
    public bool collidedPlayerWithBomb;

    public void SetPlayerNumber(int num, Material material)
    {
        //sync number with other scripts
        foreach (Player playerScript in GetComponents<Player>())
        {
            playerScript.playerNum = num;
        }

        GetComponent<MeshRenderer>().material = material;
    }


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
        collidedPlayerWithBomb = false;

        KeyCode shootKey = bm.GetKeyCode(playerNum, BindingManager.Player.Shoot);
        while (!Input.GetKeyDown(shootKey) && !collidedPlayerWithBomb)
        {
            yield return null;
        }
        //player has activated bomb or has collided with a player
        //damage all nearby players
        Collider[] colliders = Physics.OverlapSphere(transform.position, bombRadius);
        foreach (Collider col in colliders)
        {
            if (col.tag == ObjectTags.Player)
            {
                PlayerInventory collidedPlayerInventory = col.GetComponent<PlayerInventory>();
                //make sure the collided player is not this player
                if (collidedPlayerInventory.playerNum != this.playerNum) 
                {
                    collidedPlayerInventory.Health -= bombDamage;
                }
            }
        }

        //prevent player from shooting laser right after activating bomb
        while (Input.GetKey(shootKey))
        {
            yield return null;
        }
        HasBomb = false;
    }
}
