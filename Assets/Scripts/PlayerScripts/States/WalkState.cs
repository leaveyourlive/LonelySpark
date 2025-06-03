using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : PlayerState
{
    private Vector2 movementInput;

    public WalkState(PlayerMovement player, Rigidbody2D rb, PlayerStats stats, Animator animator) 
        : base(player, rb, stats, animator) {}

    public override void Enter()
    {
        animator.SetBool("IsMoving", true);
        stats.canRegenerateStamina = true;
    }

    public override void Exit()
    {
        animator.SetBool("IsMoving", false);
    }

    public override void Update()
    {
        Vector2 input = player.movementInput;

        if (input == Vector2.zero)
        {
            player.StateMachine.ChangeState(new IdleState(player, rb, animator));
            return;
        }
        if (SettingsManager.GetKey(InputAction.Sprint) && stats.currentStamina > 2 && !player.runPenalty)
        {
            player.StateMachine.ChangeState(new RunState(player, rb, stats, animator));
            return;
        }
        if (player.RollInput && stats.currentStamina >= player.rollStaminaCost && !player.isRollOnCooldown)
        {
            player.StateMachine.ChangeState(new RollState(player, rb, stats, animator));
            return;
        }
    }

    public override void FixedUpdate()
    {
        if (player.isMovementEnabled && !player.isResting)
        {
            rb.velocity = player.movementInput.normalized * player.walkSpeed * player.speedAmplifier;
            player.ChangePlayerOrientation();
        }
    }

    // private bool ShouldRun() => 
    //     SettingsManager.GetKey((InputAction.Sprint)) && 
    //     stats.currentStamina > 0 && 
    //     movementInput != Vector2.zero;
    //
    // private bool CanRoll() => 
    //     !player.isRollOnCooldown && 
    //     !player.isRolling &&
    //     stats.currentStamina >= player.rollStaminaCost;
}
