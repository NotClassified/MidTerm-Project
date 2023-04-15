using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindingManager : MonoBehaviour
{
    public static BindingManager instance;

    ///<summary> 5 Bindings for Player </summary>
    public enum Player
    {
        Up, Down, Right, Left, Shoot,
    }

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
        EditBinding(playerNum, Player.Up, up);
        EditBinding(playerNum, Player.Down, down);
        EditBinding(playerNum, Player.Right, right);
        EditBinding(playerNum, Player.Left, left);
        EditBinding(playerNum, Player.Shoot, shoot);
    }
    public void EditBinding(int playerNum, Player binding, KeyCode newKeyCode)
    {
        CheckBindingSetExists(playerNum);

        playersBindings[playerNum].keyCodes[(int)binding] = newKeyCode;
    }
    ///<summary> To make sure there are enough binding instances, if not then create enough </summary>
    void CheckBindingSetExists(int playerNum)
    {
        while (playersBindings.Count <= playerNum)
        {
            playersBindings.Add(gameObject.AddComponent<BindingsPlayer>());
        }
    }

    public KeyCode GetKeyCode(int playerNum, Player binding) => playersBindings[playerNum].keyCodes[(int)binding];
    

    //void SetPlayerBindings(PlayerInventory inventory)
    //{
    //    int playerNum = inventory.GetPlayerNumber();

    //    playersBindings[playerNum].keyCodes = 
    //}
    //public KeyCode GetPlayerBinding(int playerNum, Player binding)
    //{
    //    return 
    //}
}
