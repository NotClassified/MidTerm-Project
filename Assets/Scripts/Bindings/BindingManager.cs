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
    private BindingDataManager dataManager;
    private PauseScreenUIManager pauseManager;

    List<BindingsPlayer> playersBindings = new List<BindingsPlayer>();

    private void Awake()
    {
        instance = this;

        pauseManager = FindObjectOfType<PauseScreenUIManager>();
        dataManager = GetComponent<BindingDataManager>();
        dataManager.LoadData();
    }
    private void Start()
    {
        //load in the binding sets
        if (dataManager.currentData.bindingSets.Count >= 2)
        {
            for (int i = 0; i < dataManager.currentData.bindingSets.Count; i++)
            {
                int[] keyCodeIndexData = dataManager.currentData.bindingSets[i].keyCodeIndices;
                CreateBindingSet(i, keyCodeIndexData);
            }
        }
        else //there are no saved bindings, create default
        {
            CreateDefaultBindings();
            CreateAndSaveDataCollection();
        }

        pauseManager.UpdateBindings();
    }

    void CreateBindingSet(int playerNum, int[] keyIndices)
    {
        CreateBindingSet(playerNum, (KeyCode)keyIndices[0], (KeyCode)keyIndices[1], 
                                    (KeyCode)keyIndices[2], (KeyCode)keyIndices[3], (KeyCode)keyIndices[4]);
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

    public KeyCode GetKeyCode(int playerNum, PlayerBindings binding)
    {
        return playersBindings[playerNum].keyCodes[(int)binding];
    }

    void CreateDefaultBindings()
    {
        //Player 1
        CreateBindingSet(playersBindings.Count,
            KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.A, KeyCode.Space);
        //Player 2
        CreateBindingSet(playersBindings.Count,
            KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.Keypad0);
    }

    public void CreateAndSaveDataCollection()
    {
        BindingDataCollection collection = new BindingDataCollection();

        for (int i = 0; i < playersBindings.Count; i++)
        {
            BindingDataObject bindingSet = new BindingDataObject();

            int[] newKeyCodeIndices = new int[GetAmountOfPlayerBindings()];
            for (int keyIndex = 0; keyIndex < playersBindings[i].keyCodes.Length; keyIndex++)
            {
                newKeyCodeIndices[keyIndex] = (int) playersBindings[i].keyCodes[keyIndex];
            }

            bindingSet.keyCodeIndices = newKeyCodeIndices;
            collection.bindingSets.Add(bindingSet);
        }

        dataManager.SaveData(collection);
    }

    public int GetAmountOfPlayerBindings() => System.Enum.GetNames(typeof(PlayerBindings)).Length;

    public BindingsPlayer GetBindingSet(int setIndex) => playersBindings[setIndex];
    public int GetBindingSetAmount() => playersBindings.Count;
}
