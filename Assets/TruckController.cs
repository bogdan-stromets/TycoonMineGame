using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private int speed = 3;
    private Vector3 defaultPosition;
    private bool canMove, isBackMove;
    private TruckState state = TruckState.Idle; //{get; private set;}
    private void Start()
    {
        defaultPosition = transform.position;
        state = TruckState.Full;
    }
    private void OnMouseDown()
    {
        if (state == TruckState.Move) return;
        canMove = true;
        state = TruckState.Move;
    }

    private void Update()
    {
        Move();
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
        }

    }
}
