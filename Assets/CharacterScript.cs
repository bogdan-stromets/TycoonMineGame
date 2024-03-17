using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public Tile_Instance target_tile { get; set; }
    public CharacterState characterState = CharacterState.Idle;
    private Animator animator;
    public GameController gameController { get; private set; }
    private void Awake()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        animator = GetComponent<Animator>();
    }
    private void SetAnimation(string animation)
    {
        animator.Play(animation);
    }
    public void SetIdle() => SetAnimation("idle");
    public void SetMove() => SetAnimation("walk");
    
}
