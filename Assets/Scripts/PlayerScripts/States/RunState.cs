using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : PlayerState
{
    private Vector2 movementInput;
    
    public RunState(PlayerMovement player, Rigidbody2D rb, PlayerStats stats, Animator animator) 
        : base(player, rb, stats, animator) {}

    public override void Enter()
    {
        animator.SetBool("IsRunning", true);
    }

    public override void Exit()
    {
        animator.SetBool("IsRunning", false);
        stats.canRegenerateStamina = true;
    }

    public override void Update()
    {
        // Обновление ввода
        movementInput = player.movementInput;
        
        // Условия выхода из состояния
        if (!IsRunInput() || movementInput == Vector2.zero || player.runPenalty)
        {
            ExitToMovementState();
        }
        else if (stats.currentStamina <= 2)
        {
            player.Penalty();
        }
        else
        {
            stats.UseStamina(player.runStaminaCost * Time.deltaTime);
            stats.canRegenerateStamina = false;
        }
        
        // Проверка перехода в Roll
        if (player.RollInput && CanRoll())
        {
            player.StateMachine.ChangeState(new RollState(player, rb, stats, animator));
        }
    }

    public override void FixedUpdate()
    {
        if (player.isMovementEnabled && !player.isResting)
        {
            rb.velocity = movementInput.normalized * player.runSpeed * player.speedAmplifier;
            player.ChangePlayerOrientation();
        }
    }

    private bool IsRunInput() => 
        SettingsManager.GetKey(InputAction.Sprint) && 
        movementInput != Vector2.zero;

    private void ExitToMovementState()
    {
        if (movementInput != Vector2.zero)
            player.StateMachine.ChangeState(new WalkState(player, rb, stats, animator));
        else
            player.StateMachine.ChangeState(new IdleState(player, rb, animator));
    }
    
    private bool CanRoll() => 
        !player.isRollOnCooldown && 
        !player.isRolling &&
        stats.currentStamina >= player.rollStaminaCost;
}