using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [HideInInspector] public Vector2 movementInput;
    public float walkSpeed = 6f;
    public float runSpeed = 9f;
    private float runPenaltyDuration = 3f;
    public float speedAmplifier = 1f;
    private Vector2 lastDirection;

    [Header("Stamina Costs")]
    public float runStaminaCost = 30f;
    public float rollStaminaCost = 16f;

    [Header("Roll Settings")]
    // [SerializeField] private float invincibilityDuration = 0.5f;
    public float rollSpeed = 16f;
    public float rollDuration = 0.4f;
    public float rollRecovery = 0.2f;
    
    private Vector2 rollDirection;
    public bool RollInput => SettingsManager.GetKeyDown(InputAction.Roll);
    public bool isRollOnCooldown = false;
    
    [Header("Step Time")]
    public float stepTime = 0.05f;

    [Header("Global Flags")]
    public bool isMovementEnabled = true;
    public bool isRolling = false;
    public bool isResting = false;
    public bool runPenalty = false;
    
    // Ссылки на компоненты
    public Rigidbody2D rb;
    public PlayerStats playerStats;
    private GameObject cursor;
    private PlayerAttack playerAttack;
    private SpriteRenderer childRenderer;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trail;
    private Animator animator;
    public PlayerStateMachine StateMachine;

    private void Awake()
    {
        // Инициализация компонентов
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
        cursor = GameObject.FindWithTag("cursor");
        playerAttack = GetComponent<PlayerAttack>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trail = GetComponent<TrailRenderer>();
        trail.emitting = false;
        animator = GetComponent<Animator>();
        
        // Создание состояний
        StateMachine = new PlayerStateMachine();
        var idleState = new IdleState(this, rb, animator);
        var moveState = new WalkState(this, rb, playerStats, animator);
        var runState = new RunState(this, rb, playerStats, animator);
        var rollState = new RollState(this, rb, playerStats, animator);
        
        // Инициализация начального
        StateMachine.Initialize(idleState);

        // Изменяем скорость анимаций
        animator.SetFloat("idleAnimSpeed", animator.GetFloat("idleAnimSpeed"));
        animator.SetFloat("walkAnimSpeed", animator.GetFloat("walkAnimSpeed"));
        animator.SetFloat("runAnimSpeed", animator.GetFloat("runAnimSpeed"));
        animator.SetFloat("rollAnimSpeed", animator.GetFloat("rollAnimSpeed"));
        animator.SetFloat("attack1AnimSpeed", animator.GetFloat("attack1AnimSpeed"));
        animator.SetFloat("attack2AnimSpeed", animator.GetFloat("attack2AnimSpeed"));
        animator.SetFloat("attack3AnimSpeed", animator.GetFloat("attack3AnimSpeed"));
        animator.SetFloat("attack4AnimSpeed", animator.GetFloat("attack4AnimSpeed"));
    }

    private void Update()
    {
        HandleInput();
        StateMachine.Update();
    }
    
    private void FixedUpdate() => StateMachine.FixedUpdate();

    private void HandleInput()
    {
        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    public void ChangePlayerOrientation()
    {
        // Сохраняем последнее направление только при движении
        if (movementInput != Vector2.zero)
        {
            lastDirection = movementInput;
        }
        
        // Визуальный поворот спрайта
        if (lastDirection.x < 0) 
            spriteRenderer.flipX = true;
        else if (lastDirection.x > 0) 
            spriteRenderer.flipX = false;
    }

    private void HandleRolling()
    {
        if (SettingsManager.GetKeyDown(InputAction.Roll) && (playerStats.currentStamina >= rollStaminaCost) && (!isRolling) && (!isRollOnCooldown) && (!isResting))
        {
            if (movementInput == Vector2.zero)
            {
                rollDirection = spriteRenderer.flipX ? Vector2.right : Vector2.left;
            }
            else
            {
                rollDirection = movementInput.normalized;
            }

            playerStats.UseStamina(rollStaminaCost);
            StartCoroutine(ImmunityFramesRoutine());
        }

        if (playerStats.isInvincible)
        {
            rb.velocity = rollDirection * rollSpeed;
        }
    }

    public void Roll()
    {
        HandleRolling();
    }

    private IEnumerator ImmunityFramesRoutine()
    {
        isRollOnCooldown = true;

        playerStats.canRegenerateStamina = false;
        isRolling = true;
        playerStats.isInvincible = true;
        playerAttack.canDoShit = false;

        animator.ResetTrigger("RollTrigger"); // Сброс предыдущего триггера
        animator.SetTrigger("RollTrigger");
        animator.SetBool("IsRolling", true); 

        yield return new WaitForSeconds(rollDuration);

        playerStats.isInvincible = false;
        playerStats.canRegenerateStamina = true;
        isRolling = false;
        playerAttack.canDoShit = true;

        yield return new WaitForSeconds(rollRecovery);
        isRollOnCooldown = false;
    }

    /// <summary>
    /// Attack Step
    /// </summary>

    public void AttackStep(Vector2 direction, float distance)
    {
        StartCoroutine(AttackStepRoutine(direction, distance));
    }

    private IEnumerator AttackStepRoutine(Vector2 direction, float distance)
    {
        isMovementEnabled = false;

        rb.velocity = direction*(distance/stepTime);

        yield return new WaitForSeconds(stepTime);
        
        rb.velocity = Vector2.zero;

        isMovementEnabled = true;
    }

    /// <summary>
    /// Run Penalty
    /// </summary>

    public void Penalty()
    {
        StartCoroutine(RunPenaltyRoutine(runPenaltyDuration));
    }

    private IEnumerator RunPenaltyRoutine(float time)
    {
        runPenalty = true;

        yield return new WaitForSeconds(time);

        runPenalty = false;
    }

    /// <summary>
    /// Wings Of Despair ability
    /// "Big Jump"
    /// </summary>

    public void BigJump(float time, float range, float damage, GameObject circle, float arcHeight)
    {
        StartCoroutine(BigJumpRoutine(time, range, damage, circle, arcHeight));
    }

    public IEnumerator BigJumpRoutine(
        float time,
        float range, 
        float damage, 
        GameObject circle,
        float arcHeight
    )
    {
        // I.

        playerAttack.canDoShit = false;
        isMovementEnabled = false;
        playerStats.isInvincible = true;
        // Анимация? Эффекты?

        // II.

        Vector3 _cursorPos = cursor.transform.position;

        GameObject landingHighlighting = Instantiate(circle, _cursorPos, Quaternion.identity);
        landingHighlighting.transform.localScale = new Vector3(range, range, 0f);

        // III.
        gameObject.layer = LayerMask.NameToLayer("PlayerDashing");

        // движение по параболе
        Vector2 startPos = transform.position;

        float timePassed = 0f;
        while(timePassed < time)
        {
            float t = timePassed / time;
            
            // Горизонтальное движение
            float x = Mathf.Lerp(startPos.x, _cursorPos.x, t);
            
            // Вертикальное движение по параболе
            float y = startPos.y + arcHeight * (1 - Mathf.Pow(2*t - 1, 2));
            
            rb.MovePosition(new Vector2(x, y));
            
            timePassed += Time.deltaTime;
            yield return null; // т.к. yield return null ждет ровно 1 кадр
        }
        
        rb.MovePosition(_cursorPos);
        //

        Destroy(landingHighlighting);

        rb.velocity = Vector2.zero;
        gameObject.layer = LayerMask.NameToLayer("PlayerDefault");

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0);
        foreach (var hit in hits)
        {
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy == null) continue;

            enemy.TakeDamage(damage, DamageType.Physical);
        }

        // IV.

        playerAttack.canDoShit = true;
        isMovementEnabled = true;
        playerStats.isInvincible = false;
    }

}