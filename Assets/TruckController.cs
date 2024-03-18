using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private int speed = 3;
    [SerializeField] private TruckState state = TruckState.Empty; // temp
    GameController gameController;
    private Vector3 defaultPosition;
    private bool canMove, isBackMove;
    private int money;
    public TruckState Getstate {get => state; set => state = value;}
    private InventoryManager inventoryManager;
    private void Start()
    {
        defaultPosition = transform.position;
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        inventoryManager = GetComponentInChildren<InventoryManager>();
    }
    private void OnMouseDown()
    {
        if (state == TruckState.Move || gameController.CharacterScr.characterState != CharacterState.Idle) return;
        canMove = true;
        state = TruckState.Move;
        if(inventoryManager.state == InventoryState.Open)
            inventoryManager.ProcessClosing();
    }

    private void Update()
    {
        Move();
    }
    private int ResourcesPrice(InventoryCell cell)
    {
        switch (cell.Resource) 
        {
            case ResourceType.Clay: return 5;
            case ResourceType.Coal: return 10;
            case ResourceType.Iron: return 50;
            case ResourceType.Stone: return 100;
            case ResourceType.Gold: return 200;
            case ResourceType.Diamond: return 500;
            default: return 0;
        }
    }
    private void MoneyCount()
    {
        int sum = 0;
        var listActive = gameController.InventoryManager.GetActiveCells;
        for (int i = 0; i < listActive.Count; i++)
            sum += ResourcesPrice(listActive[i]);

        money = sum;
    }
    private void Move()
    {
        if (!canMove) return; 
        
        if (!isBackMove)
            transform.position = Vector3.MoveTowards(transform.position,target.transform.position, Time.deltaTime * speed);
        else
            transform.position = Vector3.MoveTowards(transform.position, defaultPosition, Time.deltaTime * speed);

        if (transform.position == target.transform.position) isBackMove = true;
        if (transform.position == defaultPosition)
        {
            canMove = false;
            isBackMove = false;
            state = TruckState.Idle;
            MoneyCount();
            gameController.InventoryManager.ResetCells();
            gameController.GetBalance += money;
        }

    }
}
