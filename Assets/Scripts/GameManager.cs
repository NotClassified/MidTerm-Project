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

    private void Start()
    {
        player1 = Instantiate(playerPrefab, GetSpawnPoint(SpawnPoints.Player1), new Quaternion());
    }

    public Vector3 GetSpawnPoint(SpawnPoints point)
    {
        return GameObject.Find("SpawnPoints").transform.GetChild((int)point).position;
    }
}
