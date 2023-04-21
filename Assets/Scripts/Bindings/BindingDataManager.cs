using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BindingDataManager : MonoBehaviour
{
    public BindingDataCollection currentData;

    public void SaveData(BindingDataCollection collection)
    {
        currentData = collection;
        string json = JsonUtility.ToJson(collection, true);
        Debug.Log(json);

        using (FileStream stream = File.Open(Application.persistentDataPath + "/Bindings.json", 
            FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(json);
                writer.Flush();
            }
        }
    }
    public void LoadData()
    {
        if (CreateEmptyFile())
            return;
        Debug.Log(Application.persistentDataPath + "/Bindings.json");

        using (FileStream stream = File.Open(Application.persistentDataPath + "/Bindings.json", 
            FileMode.Open, FileAccess.ReadWrite))
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                currentData = JsonUtility.FromJson<BindingDataCollection>(json);
            }
        }
    }

    public bool IsFileExists() => File.Exists(Application.persistentDataPath + "/Bindings.json");

    ///<summary> create file if none exists (returns true) </summary>
    public bool CreateEmptyFile()
    {
        if (!IsFileExists())
        {
            SaveData(new BindingDataCollection());
            return true;
        }
        return false;
    }
}
