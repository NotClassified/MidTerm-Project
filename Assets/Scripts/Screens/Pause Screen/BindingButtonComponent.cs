using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class BindingButtonComponent : MonoBehaviour
{
    public PlayerBindings binding;
    public int playerNum;

    private PlayerBindings lastBinding;
    private TextMeshProUGUI name_Text;
    private TextMeshProUGUI keyCode_Text;

    private void Awake()
    {
        lastBinding = binding;
        name_Text = transform.Find("Binding Name").GetComponent<TextMeshProUGUI>();
        keyCode_Text = transform.Find("KeyCode").GetComponent<TextMeshProUGUI>();

        PauseScreenUIManager manager = FindObjectOfType<PauseScreenUIManager>();
        GetComponent<Button>().onClick.AddListener(delegate { manager.SetBinding(this); });
    }

    private void Update()
    {
        if (lastBinding != binding)
        {
            lastBinding = binding;
            name = binding.ToString();
            GetComponentInChildren<TextMeshProUGUI>().text = binding.ToString();
        }
    }

    public void SetTextColor()
    {
        name_Text.color = GameManager.instance.playerMaterials[playerNum].color;
    }

    public void SetKeyCodeText(KeyCode keyCode)
    {
        keyCode_Text.text = keyCode.ToString();
    }
}
