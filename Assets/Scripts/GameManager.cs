using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public enum SpawnPoints
    {
        Player1, Player2
    }

    [SerializeField] GameObject playerPrefab;
    public GameObject player1;
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

        //set player's health and ammo
        player1.GetComponent<PlayerInventory>().health = startingHealth;
        player2.GetComponent<PlayerInventory>().health = startingHealth;
        player1.GetComponent<PlayerInventory>().ammo = startingAmmo;
        player2.GetComponent<PlayerInventory>().ammo = startingAmmo;
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
