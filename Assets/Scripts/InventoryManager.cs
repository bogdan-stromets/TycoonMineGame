using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject cell;
    private List<GameObject> cells = new List<GameObject>();
    private int maxCells = 5;
    private int inventoryLvl = 5;
    void Start()
    {
        FillCellsList();
    }
    private void FillCellsList()
    {
        Vector3 rootPos = Vector3.zero;
        for (int i = 0; i < inventoryLvl; i++) 
        {
            cells.Add(Instantiate(cell,rootPos,Quaternion.identity));
            cells[i].transform.SetParent(transform,true);
            //rootPos = cells[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
