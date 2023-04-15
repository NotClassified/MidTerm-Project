using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum SpawnPoints
    {
        Player1, Player2
    }

    [SerializeField] int playerAmount;
    [SerializeField] GameObject playerPrefab;
    public GameObject[] playerObjects;
    public Material[] playerMaterials;
    [SerializeField] int startingHealth;
    [SerializeField] int startingAmmo;

    private void Awake()
    {
        instance = this;
        playerObjects = new GameObject[playerAmount];
    }

    private void Start()
    {
        for (int i = 0; i < playerAmount; i++)
        {
            playerObjects[i] = SetUpPlayer(i);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    public Vector3 GetSpawnPoint(SpawnPoints point)
    {
        return GameObject.Find("SpawnPoints").transform.GetChild((int)point).position;
    }

    GameObject SetUpPlayer(int playerNum)
    {
        GameObject playerObj = Instantiate(playerPrefab, GetSpawnPoint((SpawnPoints)playerNum), new Quaternion());
        PlayerInventory Inventory = playerObj.GetComponent<PlayerInventory>();

        //set player's number, health, ammo, and bomb status
        Inventory.SetPlayerNumber(playerNum, playerMaterials[playerNum]);
        Inventory.Health = startingHealth;
        Inventory.Ammo = startingAmmo;
        Inventory.HasBomb = false;

        return playerObj;
    }

    public void GameOver(int losingPlayerNum)
    {
        Destroy(playerObjects[losingPlayerNum]);
    }
}
