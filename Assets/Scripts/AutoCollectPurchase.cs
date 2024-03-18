using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        if (gameController.TryPurchase(price)) 
        {
            gameController.AutoCollectStatus = true;
            GetComponentInChildren<TextMeshPro>().text = "Max \n Lvl";
        }
    }
}
