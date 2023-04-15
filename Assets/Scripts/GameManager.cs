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

    [SerializeField] GameObject playerPrefab;
    public GameObject player1;
    public Material[] playerMaterials;
    public GameObject player2;
    [SerializeField] int startingHealth;
    [SerializeField] int startingAmmo;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player1 = Instantiate(playerPrefab, GetSpawnPoint(SpawnPoints.Player1), new Quaternion());
        PlayerInventory p1Inventory = player1.GetComponent<PlayerInventory>();

        player2 = Instantiate(playerPrefab, GetSpawnPoint(SpawnPoints.Player2), new Quaternion());
        PlayerInventory p2Inventory = player2.GetComponent<PlayerInventory>();

        //set player's number, health, ammo, and bomb status
        p1Inventory.SetPlayerNumber(0, playerMaterials[0]);
        p2Inventory.SetPlayerNumber(1, playerMaterials[1]);

        p1Inventory.Health = startingHealth;
        p2Inventory.Health = startingHealth;

        p1Inventory.Ammo = startingAmmo;
        p2Inventory.Ammo = startingAmmo;

        p1Inventory.HasBomb = false;
        p2Inventory.HasBomb = false;
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

    public void GameOver(GameObject losingPlayer)
    {
        if (losingPlayer == player1)
        {
            Destroy(player1);
        }
        else if (losingPlayer == player2)
        {
            Destroy(player2);
        }
        else
            Debug.LogError("player not found");
    }
}
