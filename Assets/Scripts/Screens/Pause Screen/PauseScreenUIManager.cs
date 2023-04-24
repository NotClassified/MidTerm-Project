using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum PauseScreenButton
{
    Play, Restart, Settings, Bindings, Back, Save, RestartYes, RestartNo
}
public enum PauseScreen
{
    Main, Settings, Bindings, RestartPrompt
}

public class PauseScreenUIManager : MonoBehaviour
{
    ScreenFSM fsm;
    BindingManager bindingManager;
    GameObject[] pauseScreens;
    GameObject backButton;
    BindingButtonComponent[] bindingButtonComponents;

    private void Awake()
    {
        fsm = FindObjectOfType<ScreenFSM>();
        if (fsm == null)
            Debug.LogError("could not find screen fsm");
        bindingManager = FindObjectOfType<BindingManager>();
        if (bindingManager == null)
            Debug.LogError("could not find BindingManager");

        //setting references to the different pause screens and back button
        pauseScreens = new GameObject[System.Enum.GetNames(typeof(PauseScreen)).Length];
        pauseScreens[(int)PauseScreen.Main] = transform.Find("Main Screen").gameObject;
        pauseScreens[(int)PauseScreen.Settings] = transform.Find("Settings Screen").gameObject;
        pauseScreens[(int)PauseScreen.Bindings] = transform.Find("Bindings Screen").gameObject;
        pauseScreens[(int)PauseScreen.RestartPrompt] = transform.Find("Restart Prompt").gameObject;

        backButton = transform.Find("Back Button").gameObject;
        transform.Find("Background").gameObject.SetActive(true);

        bindingButtonComponents =
            pauseScreens[(int)PauseScreen.Bindings].GetComponentsInChildren<BindingButtonComponent>();
    }

    private void Start()
    {
        ChangePauseScreen(PauseScreen.Main);

        //set color for binding text settings
        foreach (BindingButtonComponent comp in bindingButtonComponents)  
        {
            comp.SetTextColor();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0); //reload this scene
    }

    public void ClickButton(ScreenButtonComponent component)
    {
        switch (component.button)
        {
            case PauseScreenButton.Play: //Unpause
                fsm.ChangeState(ScreenType.Game);
                break;

            case PauseScreenButton.Restart:
                ChangePauseScreen(PauseScreen.RestartPrompt);
                break;

            case PauseScreenButton.RestartYes:
                Restart();
                break;

            case PauseScreenButton.RestartNo:
                ChangePauseScreen(PauseScreen.Main);
                break;

            case PauseScreenButton.Settings:
                ChangePauseScreen(PauseScreen.Settings);
                break;

            case PauseScreenButton.Bindings:
                ChangePauseScreen(PauseScreen.Bindings);
                break;

            case PauseScreenButton.Back:
                ChangePauseScreen(PauseScreen.Main);
                break;

            case PauseScreenButton.Save:
                bindingManager.CreateAndSaveDataCollection();
                break;
            default:
                Debug.LogError(component.button + " does not have a case set up");
                break;
        }
    }
    public void SetBinding(BindingButtonComponent component)
    {
        StartCoroutine(SetBinding(component.playerNum, component.binding, component.GetComponent<Image>()));
    }
    IEnumerator SetBinding(int playerNum, PlayerBindings binding, Image buttonColor)
    {
        //darken button to show that the player is editing this binding
        var initialColor = buttonColor.color;
        buttonColor.color = new Color(.5f, .5f, .5f);

        KeyCode newKey = KeyCode.None;
        while (newKey == KeyCode.None)
        {
            newKey = FindKeyCode.instance.FindKey();
            yield return null;
        }

        bindingManager.EditBinding(playerNum, binding, newKey);
        UpdateBindings();

        //change the button color back to original
        buttonColor.color = initialColor;
    }
    public void UpdateBindings()
    {
        BindingsPlayer[] bindingSets = new BindingsPlayer[bindingManager.GetBindingSetAmount()];
        for (int i = 0; i < bindingSets.Length; i++)
        {
            bindingSets[i] = bindingManager.GetBindingSet(i);
        }

        //activate bindings screen if not active, to make sure all binding component references are set
        GameObject bindingsScreen = pauseScreens[(int)PauseScreen.Bindings];
        bool activatedBindingsScreen = false;
        if (!bindingsScreen.activeSelf)
        {
            bindingsScreen.SetActive(true);
            activatedBindingsScreen = true;
        }

        foreach (BindingButtonComponent component in bindingButtonComponents)
        {
            component.SetKeyCodeText(bindingSets[component.playerNum].keyCodes[(int)component.binding]);
        }
        //deactivate bindings screen if it was activated
        pauseScreens[(int)PauseScreen.Bindings].SetActive(!activatedBindingsScreen);
    }

    void ChangePauseScreen(PauseScreen newScreen)
    {
        //activate "newScreen" and deactivate all other screens
        for (int i = 0; i < pauseScreens.Length; i++)
        {
            if (i == (int)newScreen)
            {
                pauseScreens[i].SetActive(true);
            }
            else if (pauseScreens[i].activeSelf)
            {
                pauseScreens[i].SetActive(false);
            }
        }
        //show back button if not on main pause screen
        backButton.SetActive(!pauseScreens[(int)PauseScreen.Main].activeSelf);
    }
    void HideAllPauseScreens()
    {
        pauseScreens[GetActivePauseScreen()].SetActive(false);
    }

    int GetActivePauseScreen()
    {
        for (int i = 0; i < pauseScreens.Length; i++)
        {
            if (pauseScreens[i].activeSelf)
            {
                return i;
            }
        }
        return -1;
    }
}
