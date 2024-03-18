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
}
