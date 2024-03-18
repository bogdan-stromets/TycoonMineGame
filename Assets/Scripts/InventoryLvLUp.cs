using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryLvLUp : MonoBehaviour
{
    [SerializeField] private int price = 500;
    private GameController gameController;
    private void Awake()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }
    private void OnMouseDown()
    {
        if (gameController.InventoryLevel == gameController.MaxInventoryLvl) return;

        if (gameController.TryPurchase(price * gameController.InventoryLevel))
        {
            if(gameController.InventoryManager.state == InventoryState.Open)
                gameController.InventoryManager.OnMouseDown();
            gameController.InventoryLevel++;
            if(gameController.InventoryLevel == gameController.MaxInventoryLvl)
                GetComponentInChildren<TextMeshPro>().text = $"MaxLvl";
            else
                GetComponentInChildren<TextMeshPro>().text = $"↑v↑ {gameController.InventoryLevel}";
        }
    }
}
