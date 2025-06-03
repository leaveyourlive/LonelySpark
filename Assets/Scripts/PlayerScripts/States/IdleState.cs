using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerMovement player, Rigidbody2D rb, Animator animator)
        : base(player, rb, null, animator) {}

    public override void Enter()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsRunning", false);
    }

    public override void Update()
    {
        // Переход в MoveState при наличии ввода
        if (player.movementInput != Vector2.zero && player.isMovementEnabled)
        {
            player.StateMachine.ChangeState(new WalkState(
                player, 
                rb, 
                player.playerStats, 
                animator
            ));
        }
    }
}
