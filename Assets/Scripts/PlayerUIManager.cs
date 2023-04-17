using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] GameObject damageImage;
    [SerializeField] TextMeshProUGUI[] ammo_text;
    [SerializeField] TextMeshProUGUI[] health_text;
    [SerializeField] TextMeshProUGUI[] bombStatus_text;

    private void OnEnable()
    {
        PlayerInventory.ammoChange += SetAmmo;
        PlayerInventory.healthChange += SetHealth;
        PlayerInventory.healthChange += DamageRoutine;
        PlayerInventory.bombChange += SetBombStatus;
    }

    private void OnDisable()
    {
        PlayerInventory.ammoChange -= SetAmmo;
        PlayerInventory.healthChange -= SetHealth;
        PlayerInventory.healthChange -= DamageRoutine;
        PlayerInventory.bombChange -= SetBombStatus;
    }

    void DamageRoutine(int playerNum, int health)
    {
        StartCoroutine(Damage(playerNum, health));
    }
    IEnumerator Damage(int playerNum, int health)
    {
        damageImage.GetComponent<Image>().color = GameManager.instance.playerMaterials[playerNum].color;
        damageImage.SetActive(true);
        yield return new WaitForSeconds(.3f);
        damageImage.SetActive(false);
    }

    void SetAmmo(int playerNum, int ammo)
    {
        ammo_text[playerNum].text = ammo.ToString();
    }
    void SetHealth(int playerNum, int health)
    {
        health_text[playerNum].text = health.ToString();
    }
    void SetBombStatus(int playerNum, bool hasBomb)
    {
        bombStatus_text[playerNum].text = hasBomb ? "Bomb!" : "";
    }
}
