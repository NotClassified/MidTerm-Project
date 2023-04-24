using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Stats
{
    PlayerHits, ChestHits, WallBreaks, Misses
}

public class StatsManager : MonoBehaviour
{
    public GameObject statsParent;

    StatsObject[] playerObjects;
    [SerializeField] TextMeshProUGUI[] stats_text;
    [SerializeField] GameObject[] winner_texts;
    int statsAmount;

    private void Awake()
    {
        statsAmount = System.Enum.GetValues(typeof(Stats)).Length;
    }

    private void Start()
    {
        playerObjects = new StatsObject[GameManager.instance.playerAmount];

        for (int i = 0; i < GameManager.instance.playerAmount; i++)
        {
            playerObjects[i] = new StatsObject();
            playerObjects[i].statsData = new int[statsAmount];
        }

        foreach (GameObject gameObject in winner_texts)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        PlayerGun.gunAction += UpdateStat;
        PlayerInventory.playerDied += GameOver;
    }
    private void OnDisable()
    {
        PlayerGun.gunAction -= UpdateStat;
        PlayerInventory.playerDied -= GameOver;
    }

    void UpdateStat(int playerNum, string hitsObjectTag)
    {
        int stat = 0;
        if (hitsObjectTag.Equals(ObjectTags.Player))
        {
            stat = (int)Stats.PlayerHits;
            playerObjects[playerNum].statsData[stat]++;
        }
        else if (hitsObjectTag.Equals(ObjectTags.Chest))
        {
            stat = (int)Stats.ChestHits;
            playerObjects[playerNum].statsData[stat]++;
        }
        else if (hitsObjectTag.Equals(ObjectTags.BreakableWall))
        {
            stat = (int)Stats.WallBreaks;
            playerObjects[playerNum].statsData[stat]++;
        }
        else if (hitsObjectTag.Equals(ObjectTags.Wall) || hitsObjectTag.Equals(ObjectTags.BoundaryWall))
        {
            stat = (int)Stats.Misses;
            playerObjects[playerNum].statsData[stat]++;
        }
        stats_text[stat + statsAmount * playerNum].text = playerObjects[playerNum].statsData[stat].ToString();
    }

    void GameOver(int losingPlayerNum)
    {
        if (losingPlayerNum == 0)
            winner_texts[1].SetActive(true); //player 2 is the winning player
        else
            winner_texts[0].SetActive(true); //player 1 is the winning player
    }
}
