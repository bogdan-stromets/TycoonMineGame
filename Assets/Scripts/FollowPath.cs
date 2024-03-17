using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    enum MovementType
    {
        Moving,
        Lerping
    }
    [SerializeField] private MovementType movementType = MovementType.Moving;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float maxDistance = 0.01f;
    private PathCreator path;
    private IEnumerator<Transform> pointInPath;

    public PathCreator Path { get => path; set => path = value; }
    private void Start()
    {
        if (path == null)
        {
            Debug.Log("Path not exist!");
            return;
        }

        path.MovingTo = 0;
        CharacterScript scr = gameObject.GetComponent<CharacterScript>();
        scr.characterState = CharacterState.Move;
        scr.SetMove();
        pointInPath = path.GetNextPathPoint();
        pointInPath.MoveNext();

        if (pointInPath.Current == null)
        {
            Debug.Log("Not enough point!");
            return;
        }

        transform.position = new Vector3(pointInPath.Current.position.x, transform.position.y, pointInPath.Current.position.z);
    }
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (pointInPath == null || pointInPath.Current == null)
            return;
            
        switch (movementType) 
        {
            case MovementType.Moving:
                Vector3 direction = transform.position - new Vector3(pointInPath.Current.position.x, transform.position.y, pointInPath.Current.position.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-direction), Time.deltaTime * (speed * 3));
                transform.position = Vector3.MoveTowards(transform.position,new Vector3(pointInPath.Current.position.x, transform.position.y,pointInPath.Current.position.z),Time.deltaTime * speed);
                //transform.LookAt(new Vector3(pointInPath.Current.position.x, transform.position.y,pointInPath.Current.position.z));
                break;
            case MovementType.Lerping:
                transform.position = Vector3.Lerp(transform.position, new Vector3(pointInPath.Current.position.x, transform.position.y, pointInPath.Current.position.z), Time.deltaTime * speed);
            break;
        }
       

        float distanceSquare = (transform.position - new Vector3(pointInPath.Current.position.x, transform.position.y, pointInPath.Current.position.z)).sqrMagnitude;
        if (distanceSquare < Mathf.Pow(maxDistance, 2))
            pointInPath.MoveNext();
        
    }

    
}
