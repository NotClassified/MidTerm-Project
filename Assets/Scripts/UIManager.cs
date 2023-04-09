using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] ammo_text;
    [SerializeField] TextMeshProUGUI[] health_text;
    [SerializeField] TextMeshProUGUI[] bombStatus_text;

    private void OnEnable()
    {
        PlayerInventory.ammoChange += SetAmmo;
        PlayerInventory.healthChange += SetHealth;
        PlayerInventory.bombChange += SetBombStatus;
    }

    private void OnDisable()
    {
        PlayerInventory.ammoChange -= SetAmmo;
        PlayerInventory.healthChange -= SetHealth;
        PlayerInventory.bombChange -= SetBombStatus;
    }

    void SetAmmo(int playerNum, int ammo)
    {
        ammo_text[playerNum - 1].text = ammo.ToString();
    }
    void SetHealth(int playerNum, int health)
    {
        health_text[playerNum - 1].text = health.ToString();
    }
    void SetBombStatus(int playerNum, bool hasBomb)
    {
        bombStatus_text[playerNum - 1].text = hasBomb ? "Bomb!" : "";
    }
}
