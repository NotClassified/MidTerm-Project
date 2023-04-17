using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum PlayerBindings
{
    Up, Down, Right, Left, Shoot,
}

public class BindingManager : MonoBehaviour
{
    public static BindingManager instance;


    List<BindingsPlayer> playersBindings = new List<BindingsPlayer>();

    private void Awake()
    {
        instance = this;

        //Player 1
        CreateBindingSet(playersBindings.Count, 
            KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.A, KeyCode.Space);
        //Player 2
        CreateBindingSet(playersBindings.Count, 
            KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.Keypad0);
    }

    void CreateBindingSet(int playerNum, KeyCode up, KeyCode down, KeyCode right, KeyCode left, KeyCode shoot)
    {
        EditBinding(playerNum, PlayerBindings.Up, up);
        EditBinding(playerNum, PlayerBindings.Down, down);
        EditBinding(playerNum, PlayerBindings.Right, right);
        EditBinding(playerNum, PlayerBindings.Left, left);
        EditBinding(playerNum, PlayerBindings.Shoot, shoot);
    }
    public void EditBinding(int playerNum, PlayerBindings binding, KeyCode newKeyCode)
    {
        CheckBindingSetExists(playerNum);

        playersBindings[playerNum].keyCodes[(int)binding] = newKeyCode;
    }
    ///<summary> To make sure there are enough binding instances, if not then create enough </summary>
    void CheckBindingSetExists(int playerNum)
    {
        while (playersBindings.Count <= playerNum)
        {
            BindingsPlayer bindings = gameObject.AddComponent<BindingsPlayer>();
            bindings.keyCodes = new KeyCode[System.Enum.GetNames(typeof(PlayerBindings)).Length];
            playersBindings.Add(bindings);
        }
    }

    public KeyCode GetKeyCode(int playerNum, PlayerBindings binding) => playersBindings[playerNum].keyCodes[(int)binding];

}
