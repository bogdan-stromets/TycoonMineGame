using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<InventoryCell> inventoryCells = new List<InventoryCell>();
    [SerializeField] private Material[] materials;
    private List<InventoryCell> activeCells;
    private List<InventoryCell> freeCells = new List<InventoryCell>();
    public GameController gameController;
    public InventoryState state = InventoryState.Close;
    private float speed = 3f;

    public List<InventoryCell> GetActiveCells { get => activeCells; }
    public Material GetMaterial(ResourceType rt)
    {
        if (materials == null)
        {
            Debug.Log("Error! Materials not found");
            return null;
        }
        return materials[(int)rt - 1];
    }
    private void Start()
    {
        UnActiveCells();
    }
    public void UnActiveCells()
    {
        foreach (InventoryCell cell in inventoryCells) { cell.Unlocked = true; }
        for (int i = gameController.InventoryLevel; i < inventoryCells.Count; i++) 
        {
            inventoryCells[i].gameObject.SetActive(false);
            inventoryCells[i].Unlocked = false;
        }

        activeCells = inventoryCells.Where(x => x.Unlocked).ToList();
    }
    private void OnMouseDown()
    {
        switch (state) 
        {
            case InventoryState.Close:
                ProcessOpening();
            break;
            case InventoryState.Open:
                ProcessClosing();
            break;
        }
    }
    public void ProcessClosing() => StartCoroutine(Closing());
    public void ProcessOpening() => StartCoroutine(Opening());
    private IEnumerator Closing()
    {
        UnActiveCells();
        state = InventoryState.Closing;
        GetComponentInChildren<TextMeshPro>().text = "Open";
        InventoryCell lastCell = activeCells[activeCells.Count - 1];
        int cellsOnPlace = 1;
        while (true)
        {
            int ind = activeCells.Count - 1 - cellsOnPlace < 0 ? 0 : activeCells.Count - 1 - cellsOnPlace;
            if (cellsOnPlace == gameController.InventoryLevel)
            {
                state = InventoryState.Close;
                lastCell.gameObject.SetActive(false);
                yield break;
            }
            if (lastCell.transform.position == activeCells[ind].transform.position) 
            {
                activeCells[ind].gameObject.SetActive(false);
                cellsOnPlace++;
            }
            lastCell.transform.position = Vector3.MoveTowards(lastCell.transform.position, activeCells[ind].transform.position, Time.deltaTime * speed);
            yield return null;
        }
    }

    private IEnumerator Opening()
    {
        UnActiveCells();
        state = InventoryState.Opening;
        GetComponentInChildren<TextMeshPro>().text = "Close";
        Vector3 firstCellPos = activeCells[0].transform.position;
        foreach (InventoryCell cell in activeCells) 
        {
            if(cell.Unlocked)
                cell.gameObject.SetActive(true);
            cell.gameObject.transform.position = firstCellPos;
            cell.InDefaultPlace = false;
        }

        while (true)
        {
            int cellsOnPlace = 0;
            foreach (InventoryCell cell in activeCells)
            {
                if (cell.InDefaultPlace) 
                {
                    cellsOnPlace++;
                    if (cellsOnPlace == gameController.InventoryLevel)
                    {
                        state = InventoryState.Open;
                        yield break;
                    }
                    
                    continue;
                }
                cell.transform.position = Vector3.MoveTowards(cell.transform.position, cell.defaultPos, Time.deltaTime * speed);
                if (cell.transform.position == cell.defaultPos) 
                    cell.InDefaultPlace = true;
            }
            yield return null;
        }    

    }

    private void GetFreeCell()
    {
        freeCells = activeCells.Where(x => x.IsFree).ToList();
    }
    private bool HasFreeCell() => freeCells.Count != 0;
    public void CheckFullTruck()
    {
        GetFreeCell();
        if (!HasFreeCell() && gameController.GetTruck.GetComponent<TruckController>().Getstate != TruckState.Full)
        {
            gameController.GetTruck.GetComponent<TruckController>().Getstate = TruckState.Full;
            return;
        }
    }
    public void TryFillCell(ResourceType rt)
    {
        // тут закінчив

        //CheckFullTruck();
        freeCells[0].IsFree = false;
        freeCells[0].Resource = rt;
        freeCells[0].PutItem();
        freeCells.RemoveAt(0);
    }

    public void ResetCells()
    {
        for (int i = 0; i < inventoryCells.Count; i++)
            inventoryCells[i].ResetSelf();

    }
}
