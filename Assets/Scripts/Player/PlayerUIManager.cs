using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] GameObject damageImage;
    [SerializeField] Slider[] ammo_slider;
    [SerializeField] Slider[] health_slider;

    private void OnEnable()
    {
        PlayerInventory.ammoChange += SetAmmo;
        PlayerInventory.healthChange += SetHealth;
    }

    private void OnDisable()
    {
        PlayerInventory.ammoChange -= SetAmmo;
        PlayerInventory.healthChange -= SetHealth;
    }

    //void DamageRoutine(int playerNum, int health)
    //{
    //    StartCoroutine(Damage(playerNum, health));
    //}
    //IEnumerator Damage(int playerNum, int health)
    //{
    //    damageImage.GetComponent<Image>().color = GameManager.instance.playerMaterials[playerNum].color;
    //    damageImage.SetActive(true);
    //    yield return new WaitForSeconds(.3f);
    //    damageImage.SetActive(false);
    //}

    void SetAmmo(int playerNum, int ammo)
    {
        ammo_slider[playerNum].value = ammo;
    }
    void SetHealth(int playerNum, int health)
    {
        health_slider[playerNum].value = health;
    }
}
