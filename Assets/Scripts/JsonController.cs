using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;

public class JsonController : MonoBehaviour
{
    public Data data;
    private string fileName = "gameData.json";
    [ContextMenu("Load")]
    public void LoadData()
    {
        data = JsonUtility.FromJson<Data>(File.ReadAllText(Application.streamingAssetsPath + "/" + fileName));
    }  
    [ContextMenu("Save")]
    public void SaveData()
    {
        File.WriteAllText(Application.streamingAssetsPath + "/" + fileName, JsonUtility.ToJson(data));
    }
}
[System.Serializable]
public class Data
{
    
}
