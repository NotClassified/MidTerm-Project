using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BindingDataManager : MonoBehaviour
{
    BindingDataCollection currentData;

    public void SaveData(BindingDataCollection collection)
    {
        currentData = collection;
        string json = JsonUtility.ToJson(collection);
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
}
