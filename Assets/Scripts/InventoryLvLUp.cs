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
    private void Start()
    {
        if (gameController.InventoryLevel == gameController.MaxInventoryLvl)
            GetComponentInChildren<TextMeshPro>().text = $"Max \n Lvl";
        else
            GetComponentInChildren<TextMeshPro>().text = $"↑v↑ {gameController.InventoryLevel}";
    }
    private void OnMouseDown()
    {
        if (gameController.InventoryLevel == gameController.MaxInventoryLvl) return;

        if (gameController.TryPurchase(price * gameController.InventoryLevel))
        {
            if (gameController.InventoryManager.state == InventoryState.Open)
                gameController.InventoryManager.ProcessClosing(); ;
            gameController.InventoryLevel++;
            if(gameController.InventoryLevel == gameController.MaxInventoryLvl)
                GetComponentInChildren<TextMeshPro>().text = $"Max \n Lvl";
            else
                GetComponentInChildren<TextMeshPro>().text = $"↑v↑ {gameController.InventoryLevel}";
            gameController.GetTruck.GetComponentInChildren<TruckController>().Getstate = TruckState.Empty;
            gameController.InventoryManager.UnActiveCells();

        }
    }
}
