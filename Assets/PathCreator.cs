using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

public class PathCreator : MonoBehaviour
{
    enum PathType
    {
        linear,
        loop
    }
    [SerializeField] private PathType pathType;
    [SerializeField] private int moveDirection = 1;
    [SerializeField] private GameObject character;
    [SerializeField] private Transform[] pathElements;
    private GameController gameController;

    public bool pathReversed { get; set; }
    private int movingTo = 0;

    public Transform[] PathElements { get { return pathElements; }}

    public int MovingTo { get => movingTo; set => movingTo = value; }

    public void OnDrawGizmos()
    {
        if (pathElements == null || pathElements.Length < 2) return;

        for (int i = 1; i < pathElements.Length; i++)
        {
            Gizmos.DrawLine(pathElements[i - 1].position, pathElements[i].position);
        }
        if (pathType == PathType.loop)
            Gizmos.DrawLine(pathElements[0].position, pathElements[pathElements.Length - 1].position);
    }
    public void ReversePath()
    {
        Array.Reverse(pathElements);
        pathReversed = !pathReversed;
        ResetPath();
    }
    private void ResetPath()
    {
        CharacterScript scr = character.GetComponent<CharacterScript>();
        scr.characterState = pathReversed ?  CharacterState.Busy : CharacterState.Idle;
        if (scr.characterState == CharacterState.Idle)
            scr.SetIdle();
        Destroy(character.GetComponent<FollowPath>());
    }
    public IEnumerator<Transform> GetNextPathPoint()
    {
        if (pathElements == null || pathElements.Length < 1) yield break;

        while (true) 
        {
            yield return pathElements[MovingTo];

            if (pathElements.Length == 1) break;

            if (pathType == PathType.linear)
            {
                if (MovingTo <= 0)
                    moveDirection = 1;
                else if (MovingTo >= pathElements.Length - 1) 
                {
                    ReversePath();
                    yield break;
                }
            }
            MovingTo += moveDirection;

            if (pathType == PathType.loop)
            {
                if (MovingTo >= pathElements.Length)
                    MovingTo = 0;
                if (MovingTo < 0)
                    MovingTo = pathElements.Length - 1;
            }
        }
    }


}
