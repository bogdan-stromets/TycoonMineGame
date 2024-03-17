using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Decor_Tile : Tile_Instance
{
    private int price = 100;
    private void OnEnable()
    {
        unlockPrice = price;
    }
/*    private void UnlockMineTile()
    {
        if (!IsTileLock()) return;

        if (TryToUnlockTile())
        {
            gameController.GetBalance -= price;
            UnlockTile();
        }
    }

    private bool TryToUnlockTile()
    {
        return gameController.GetBalance >= price;
    }
    private void OnMouseDown()
    {
        if (!IsTileLock()) return;
        UnlockMineTile();
    }*/
}
