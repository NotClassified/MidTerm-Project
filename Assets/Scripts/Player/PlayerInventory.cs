using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerInventory : Player
{
    [SerializeField] float bombRadius;
    [SerializeField] int bombDamage;
    [SerializeField] Image bombIndicatorSprite;
    [SerializeField] AnimationCurve bombIndicatorTransparencyCurve;
    [SerializeField] float bombRadiusTransparencyDuration;

    int health;
    int ammo;
    bool hasBomb;
    public int Health
    {
        get => health;
        set
        {
            health = value;
            if (health > GameManager.instance.maxHealth)
                health = GameManager.instance.maxHealth;

            healthChange(playerNum, health);
        }
    }
    public int Ammo
    {
        get => ammo;
        set
        {
            ammo = value;
            if (ammo > GameManager.instance.maxAmmo)
                ammo = GameManager.instance.maxAmmo;

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
    ///<summary> int playerNumber, Vector3 damageSourcePos </summary>
    public static event Action<int, Vector3> playerDamaged;
    public static event Action<int, int> ammoChange;
    public static event Action<int, bool> bombChange;
    public static event Action<int> playerDied;

    private void Awake()
    {
        moveFSM = GetComponent<PlayerMoveFSM>();
        bombIndicatorSprite.gameObject.SetActive(false);
    }

    ///<summary> returns health amount left </summary>
    public int DealDamage(int damage, Vector3 damagePos)
    {
        Health -= damage;
        if (Health <= 0 && playerDied != null)
        {
            playerDied(playerNum);
        }
        playerDamaged(playerNum, damagePos);
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

        bombIndicatorSprite.gameObject.SetActive(true);
        var color = bombIndicatorSprite.color;
        float time = 0;
        if (bombRadiusTransparencyDuration == 0f)
            bombRadiusTransparencyDuration = 1f;

        KeyCode shootKey = bm.GetKeyCode(playerNum, PlayerBindings.Shoot);
        while (!Input.GetKeyDown(shootKey) && !collidedPlayerWithBomb)
        {
            if (time < bombRadiusTransparencyDuration)
            {
                time += Time.deltaTime;
                color.a = bombIndicatorTransparencyCurve.Evaluate(time / bombRadiusTransparencyDuration);
                bombIndicatorSprite.color = color;
            }
            else
                time = 0f;
            yield return null;
        }
        bombIndicatorSprite.gameObject.SetActive(false);

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
                    collidedPlayerInventory.DealDamage(bombDamage, transform.position);
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
