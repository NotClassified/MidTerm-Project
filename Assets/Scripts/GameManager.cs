using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum SpawnPoints
    {
        Player1, Player2
    }

    [SerializeField] GameObject playerPrefab;
    public GameObject player1;
    [SerializeField] Material[] playerMaterials;
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

        player2 = Instantiate(playerPrefab, GetSpawnPoint(SpawnPoints.Player2), new Quaternion());
        //set key bindings for 2nd player
        PlayerMoveFSM player2Movefsm = player2.GetComponent<PlayerMoveFSM>();
        player2Movefsm.SetBinding(PlayerMoveFSM.Binding.Up, KeyCode.UpArrow);
        player2Movefsm.SetBinding(PlayerMoveFSM.Binding.Down, KeyCode.DownArrow);
        player2Movefsm.SetBinding(PlayerMoveFSM.Binding.Right, KeyCode.RightArrow);
        player2Movefsm.SetBinding(PlayerMoveFSM.Binding.Left, KeyCode.LeftArrow);
        player2Movefsm.SetBinding(PlayerMoveFSM.Binding.Shoot, KeyCode.Return);

        //set player's number, health, ammo, and bomb status
        PlayerInventory p1Inventory = player1.GetComponent<PlayerInventory>();
        PlayerInventory p2Inventory = player2.GetComponent<PlayerInventory>();
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
        if (Input.GetKeyDown(KeyCode.Escape))
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
