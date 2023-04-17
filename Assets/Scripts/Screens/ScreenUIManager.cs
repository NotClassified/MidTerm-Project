using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ScreenButton
{
    Play, Settings, Bindings
}

public class ScreenUIManager : MonoBehaviour
{
    ScreenFSM fsm;

    private void Awake()
    {
        fsm = FindObjectOfType<ScreenFSM>();
        if (fsm == null)
            Debug.LogError("could not find screen fsm");
    }

    public void ClickButton(ScreenButtonComponent component)
    {
        switch (component.button)
        {
            case ScreenButton.Play:
                fsm.ChangeState(ScreenType.Game);
                break;
            case ScreenButton.Settings:
                break;
            case ScreenButton.Bindings:
                break;
            default:
                break;
        }
    }
    public void SetBinding(BindingButtonComponent component)
    {
        //switch (component.binding)
        //{
        //    case PlayerBindings.Up:
        //        break;
        //    case PlayerBindings.Down:
        //        break;
        //    case PlayerBindings.Right:
        //        break;
        //    case PlayerBindings.Left:
        //        break;
        //    case PlayerBindings.Shoot:
        //        break;
        //    default:
        //        break;
        //}
    }
}
