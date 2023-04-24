using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    ScreenFSM screenFSM;

    public enum SpawnPoints
    {
        Player1, Player2
    }

    public int playerAmount;
    [SerializeField] GameObject playerPrefab;
    [HideInInspector] public GameObject[] playerObjects;
    public Material[] playerMaterials;
    [SerializeField] int startingHealth;
    [SerializeField] int startingAmmo;
    public int maxHealth;
    public int maxAmmo;

    private void Awake()
    {
        instance = this;
        screenFSM = FindObjectOfType<ScreenFSM>();

        playerObjects = new GameObject[playerAmount];

        PlayerInventory.playerDied += GameOver;
        ScreenGame.GameHasStarted += GameStart;
    }
    private void OnDestroy()
    {
        PlayerInventory.playerDied -= GameOver;
        ScreenGame.GameHasStarted -= GameStart;
    }

    public void GameStart()
    {
        for (int i = 0; i < playerAmount; i++)
        {
            playerObjects[i] = SetUpPlayer(i);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && screenFSM.IsCurrentState(ScreenType.Game))
        {
            screenFSM.ChangeState(ScreenType.Pause);
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
