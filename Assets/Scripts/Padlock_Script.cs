using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Padlock_Script : MonoBehaviour
{
    private Tile_Instance tile;
    private int unlockPrice;
    void Start()
    {
        tile = transform.parent.GetComponent<Tile_Instance>();
        unlockPrice = tile.unlockPrice;
        DrawPrice();
    }
    private void OnMouseDown()
    {
        tile.TryToUnlockTile(unlockPrice);
    }
    private void DrawPrice()
    {
        GetComponentInChildren<TextMeshPro>().text = $"{unlockPrice}$";
    }
    private void Update()
    {
        if (tile.tileState != TileState.Lock) 
            Destroy(gameObject);
    }
}
