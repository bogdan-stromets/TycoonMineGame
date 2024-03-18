using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryCell : MonoBehaviour
{
    [SerializeField] private GameObject item;
    private GameController gameController;
    public Vector3 defaultPos { get; private set; }
    public bool InDefaultPlace { get; set; }
    private bool isUnlocked = true;
    private bool isFree = true;
    private ResourceType resourceType;

    //public GameObject Item { get => item; }
    public bool Unlocked { get => isUnlocked; set => isUnlocked = value; }
    public bool IsFree { get => isFree; set => isFree = value; }
    public ResourceType Resource { get => resourceType; set => resourceType = value; }


    private void Awake()
    {
        defaultPos = transform.position;
        //item = gameObject.GetComponentInChildren<Outline>().gameObject;
    }
    private void OnEnable()
    {
    }
    public void ResetSelf()
    {
        resourceType = ResourceType.None;
        IsFree = true;
        item.SetActive(false);
    }
    public void PutItem()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        item.SetActive(true);
        item.GetComponent<Renderer>().material = gameController.InventoryManager.GetMaterial(resourceType);
    }


}
