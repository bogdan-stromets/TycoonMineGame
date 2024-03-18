using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCell : MonoBehaviour
{
    public Vector3 defaultPos { get; private set; }
    public bool InDefaultPlace { get; set; }
    private bool isUnlocked = true;
    public bool Unlocked { get => isUnlocked; set => isUnlocked = value; }

    private void Awake()
    {
        defaultPos = transform.position;
    }
}
