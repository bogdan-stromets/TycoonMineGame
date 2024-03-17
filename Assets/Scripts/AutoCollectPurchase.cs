using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCollectPurchase : MonoBehaviour
{
    private GameController gameController;
    private int price = 5000;

    private void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    private void OnMouseDown()
    {
        if (gameController.AutoCollectStatus) return;
        gameController.AutoCollectStatus = true;
    }
}
