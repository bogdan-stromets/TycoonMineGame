using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;

public class JsonController : MonoBehaviour
{
    public Data data;
    GameController controller;

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
    private void Awake()
    {
        controller = GetComponent<GameController>();
        if (data == null) return;
        LoadData();
        controller.GetBalance = data.balance;
        controller.InventoryLevel = data.inventoryLevel;
        controller.AutoCollectStatus = data.autoCollect;
        controller.GetTruck.GetComponent<TruckController>().Getstate = data.truckState;
        for (int i = 0; i < controller.GetTiles.Count; i++)
            controller.GetTiles[i].GetComponent<Tile_Instance>().tileState = data.tileStates[i];
        for (int i = 0; i < controller.InventoryManager.GetAllCells.Count; i++)
        {
            controller.InventoryManager.GetAllCells[i].GetComponent<InventoryCell>().Unlocked = data.cellsUnlockState[i];
            controller.InventoryManager.GetAllCells[i].GetComponent<InventoryCell>().IsFree = data.cellsFreeState[i];
            controller.InventoryManager.GetAllCells[i].GetComponent<InventoryCell>().Resource = data.cellsResources[i];
        }
    }
    private void OnApplicationQuit()
    {
        List<TileState> tiles = new();
        List <bool> cellsUnlockState = new();
        List <bool> cellsFreeState = new();
        List<ResourceType> cellsResources = new();
        controller.GetTiles.ForEach(t => { tiles.Add(t.GetComponent<Tile_Instance>().tileState); });
        controller.InventoryManager.GetAllCells.ForEach(x => { cellsUnlockState.Add(x.GetComponent<InventoryCell>().Unlocked); });
        controller.InventoryManager.GetAllCells.ForEach(x => { cellsFreeState.Add(x.GetComponent<InventoryCell>().IsFree); });
        controller.InventoryManager.GetAllCells.ForEach(x => { cellsResources.Add(x.GetComponent<InventoryCell>().Resource); });
        data = new Data
        (
            controller.GetBalance,
            controller.InventoryLevel,
            controller.AutoCollectStatus,
            tiles.ToArray(),
            controller.GetTruck.GetComponent<TruckController>().Getstate,
            cellsUnlockState.ToArray(),
            cellsFreeState.ToArray(),
            cellsResources.ToArray()
        );
        SaveData();
    }
}
[System.Serializable]
public class Data
{
    public int balance;
    public int inventoryLevel;
    public bool autoCollect;
    public TileState[] tileStates;
    
    public TruckState truckState;
    public bool[] cellsUnlockState;
    public bool[] cellsFreeState;
    public ResourceType[] cellsResources;

    public Data(int balance,int inventoryLevel,bool autoCollect, TileState[]tileStates,
        TruckState truckState, bool[] cellsUnlockState, bool[] cellsFreeState, ResourceType[] cellsResources)
    {
        this.balance = balance;
        this.inventoryLevel = inventoryLevel;
        this.autoCollect = autoCollect;
        this.tileStates = tileStates;
        this.truckState = truckState;
        this.cellsUnlockState = cellsUnlockState;
        this.cellsFreeState = cellsFreeState;
        this.cellsResources = cellsResources;
    }
}
