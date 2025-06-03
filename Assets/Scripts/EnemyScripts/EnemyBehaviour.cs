using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Скорость патрулирования и погони")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float baseSpeed;
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private float patrolChangeInterval = 3f;

    [Header("Обнаружение")]
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float followRange = 12f;

    [Header("Домашний радиус")]
    [SerializeField] private float homeRange = 10f;
    [SerializeField] private float homeRangeBuffer = 0.5f;

    private Vector2 startPosition;
    private Vector2 patrolDirection;
    private bool playerDetected = false;

    private PlayerStats player;
    private Rigidbody2D rb;
    private EnemyAttack enemyAttack;
    private Animator animator;

    private Coroutine patrolCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAttack = GetComponent<EnemyAttack>();
        animator = GetComponent<Animator>();

        startPosition = transform.position;
        rb.velocity = Vector2.zero;

        patrolCoroutine = StartCoroutine(PatrolRoutine());
        baseSpeed = moveSpeed;
    }

    void OnEnable()
    {
        patrolCoroutine = StartCoroutine(PatrolRoutine());
    }

    void FixedUpdate()
    {
        if (!playerDetected)
        {
            Patrol();
            Scan();
        }
        else
        {
            ApproachThenAttack();
        }

        animator.SetBool("IsMoving", rb.velocity.magnitude > 0.1f);
    }

    private void Patrol()
    {
        // if (patrolDirection == Vector2.zero)
        // {
        //     patrolDirection = GetRandomDirection();
        // }
        //
        // Vector2 nextPosition = (Vector2)transform.position + patrolDirection * patrolSpeed * Time.fixedDeltaTime;
        //
        // // Проверяем, чтобы враг не выходил за пределы homeRange
        // if (Vector2.Distance(nextPosition, startPosition) > homeRange)
        // {
        //     patrolDirection = GetRandomDirection(); // Меняем направление, если вышли за границу
        // }
        //
        if (Vector2.Distance(transform.position, startPosition) > homeRange - homeRangeBuffer)
        {
            patrolDirection = (startPosition - (Vector2)transform.position).normalized;
        }
        
        //MoveTowards(patrolDirection, patrolSpeed); 
        rb.velocity = patrolDirection * patrolSpeed;
        FlipEnemy(rb.velocity.x);
    }

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            Vector2 target = startPosition + Random.insideUnitCircle * homeRange;
            patrolDirection = (target - (Vector2)transform.position).normalized;
        
            yield return new WaitForSeconds(patrolChangeInterval);
        }
    }

    private void ApproachThenAttack()
    {
        if (player == null)
        {
            playerDetected = false;
            ReturnToStart();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > followRange)
        {
            playerDetected = false;
            ReturnToStart();
        }
        else if (distanceToPlayer > enemyAttack.attackRange)
        {
            // if (!isTouchingEntity) {
            //     MoveTowards(player.transform.position, moveSpeed);
            // } else {
            //     rb.velocity = Vector2.zero;
            // }
            MoveTowards(player.transform.position, moveSpeed);
        }
        else
        {
            rb.velocity = Vector2.zero; // Останавливаемся для атаки
            FlipEnemy(player.transform.position.x - transform.position.x);
            enemyAttack.TryAttack(player, transform);
        }
    }

    private void ReturnToStart()
    {
        if (Vector2.Distance(transform.position, startPosition) > 0.1f)
        {
            MoveTowards(startPosition, moveSpeed);
        }
        else
        {
            rb.velocity = Vector2.zero; // Остановка после возврата домой
            patrolDirection = Vector2.zero; // Сбрасываем направление после возврата
            
            // перезапуск патрулирования
            if (patrolCoroutine != null)
            {
                StopCoroutine(patrolCoroutine);
                patrolCoroutine = StartCoroutine(PatrolRoutine());
            }
        }
    }

    private void MoveTowards(Vector2 targetPosition, float speed)
    {
        Vector2 dir = (targetPosition - rb.position).normalized;
        float distance = speed * Time.fixedDeltaTime;
        
        // Проверяем, есть ли препятствие впереди (игрок или другой враг)
        RaycastHit2D[] hits = new RaycastHit2D[1];
        int count = rb.Cast(dir, hits, distance);
        
        if (count == 0)
        {
            // свободно — двигаем
            rb.MovePosition(rb.position + dir * distance);
        }
        else
        {
            // столкновение — не двигаем вовсе
            rb.MovePosition(rb.position);
        }
        
        FlipEnemy(dir.x);
    }

    private void Scan()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, detectionRange, Vector2.zero, 0);
        foreach (var hit in hits)
        {
            PlayerStats scannedPlayer = hit.collider.GetComponent<PlayerStats>();
            if (scannedPlayer == null || scannedPlayer.isInvisible) continue;

            player = scannedPlayer;
            playerDetected = true;
            Debug.Log("Обнаружен игрок");
            
            StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
        }
    }

    private void FlipEnemy(float moveDirection)
    {
        if (Mathf.Abs(moveDirection) > 0.01f)
        {
            if (moveDirection > 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (moveDirection < 0 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    public void SetHomeRange(float range)
    {
        homeRange = range; // Позволяет спавнеру задавать homeRange
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
        moveSpeed = baseSpeed * speedMultiplier;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.DrawWireSphere(transform.position, followRange);
    }

    // private bool isTouchingEntity;
    // private HashSet<string> entityTags = new() {"Player", "Enemy"};

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (entityTags.Contains(collision.gameObject.tag))
    //     {
    //         isTouchingEntity = true;
    //         rb.velocity = Vector2.zero;
    //     }
    // }

    // void OnCollisionExit2D(Collision2D collision)
    // {
    //     if (collision.collider.TryGetComponent<PlayerMovement>(out _))
    //     {
    //         isTouchingEntity = false;
    //     }
    // }
}