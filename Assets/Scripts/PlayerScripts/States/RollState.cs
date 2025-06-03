using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : PlayerState
{
    private float rollTime;
    private Vector2 rollDirection;

    public RollState(PlayerMovement player, Rigidbody2D rb, PlayerStats stats, Animator animator) 
        : base(player, rb, stats, animator) {}

    public override void Enter()
    {
        rollDirection = player.movementInput.normalized;
        rollTime = player.rollDuration;
        stats.UseStamina(player.rollStaminaCost);
        player.Roll();
    }

    public override void FixedUpdate()
    {
        // rb.velocity = rollDirection * player.rollSpeed;
        // rollTime -= Time.fixedDeltaTime;

        if (!player.isRolling) // (rollTime <= 0)
        {
            player.StateMachine.ChangeState(new WalkState(player, rb, stats, animator));
        }
    }

    public override void Exit()
    {
        animator.ResetTrigger("RollTrigger");
        animator.SetBool("IsRolling", false);
    }
}
