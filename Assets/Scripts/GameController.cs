using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum ResourceType
{
    None,
    Coal,
    Stone,
    Iron,
    Clay,
    Gold,
    Diamond
}
enum UnlockPrice
{
    Coal = 500,
    Stone = 1500,
    Iron = 1000,
    Clay = 0,
    Gold = 2000,
    Diamond = 5000
}
public enum TileState
{
    Lock,
    ReadyToMine,
    ProcessMine,
    ResourceReady,
    Used,
    ResourceSpawning
}
public enum CharacterState
{
    Idle,
    Move,
    Busy
}
public enum TruckState
{
    Idle,
    Move,
    Full,
    Empty
}
public enum InventoryState
{
    Open,
    Close,
    Closing,
    Opening
}
public enum CellState
{
    Full,
    Empty
}
public class GameController : MonoBehaviour
{
    [SerializeField] private int balance = 100000;
    [SerializeField] private TextMeshPro balanceUI;
    [SerializeField] private GameObject character;
    [SerializeField] private List<GameObject> tiles;
    [SerializeField] private InventoryManager invManager;
    [SerializeField] private bool autoCollect;
    [SerializeField] private int inventorylvl = 1;
    [SerializeField] private GameObject truck;
    private GameObject tileMap;
   
    public  int GetBalance
    {
        get { return balance; }
        set 
        {
            if(value > 0) 
            {
                balance = value;
                DrawBalance();
            }
        }
    }
    public int MaxInventoryLvl { get => 5; }
    public GameObject GetTruck { get => truck; }
    public InventoryManager InventoryManager { get =>invManager; }
    public List<GameObject> GetTiles { get => tiles; }
    public GameObject TileMap { get => tileMap; }
    public GameObject Character { get => character; }
    public bool AutoCollectStatus { get => autoCollect; set => autoCollect = value; }
    public CharacterScript CharacterScr { get => character.GetComponent<CharacterScript>(); }
    public int InventoryLevel { get => inventorylvl; set => inventorylvl = value; }
    private void Start()
    {
        // зчитування балансу і інфу про відкриті плитки з файлу
        tileMap = GameObject.FindWithTag("TileMap");
        DrawBalance();
    }
    private bool IsPurchasePossible(int price) =>balance >= price;

    public bool TryPurchase(int price)
    {
        if (!IsPurchasePossible(price))
        {
            Debug.Log("Error! Not enough money");
            return false;
        }

        GetBalance -= price;
        return true;
    }
    private void DrawBalance() => balanceUI.text = $"Balance: {balance}$";
    void Update()
    {
        AutoCollect();
    }
    public void AutoCollect()
    {
        if (!autoCollect || CharacterScr.characterState != CharacterState.Idle) return;

        foreach (var tile in tiles) 
        {
            Tile_Instance tile_Instance = tile.GetComponent<Tile_Instance>();
            if (tile_Instance is Mine_Tile mineTile)
            {
                if (mineTile.tileState == TileState.ResourceReady) 
                {
                    mineTile.PickupResource();
                    break;
                }
            }
        }
    }


}

public static class ExtMethods<T>
{
    public static T Choose(params T[] values)
    {
        if(values.Length == 0) return default(T);
        return values[Random.Range(0, values.Length)];
    }
    public static bool IsAnimationPlaying(Animator animator,string animationName)
    {
        var animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return animatorStateInfo.IsName(animationName);
    }
    public static void SetAnimation(Animator animator, string anmName)
    {
        animator.Play(anmName);
    }
}

