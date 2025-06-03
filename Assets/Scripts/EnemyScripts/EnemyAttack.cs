using System.Collections;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    [SerializeField] public float attackRange = 3f;
    [SerializeField] private protected float damage = 24f;
    [SerializeField] private protected float lastAttackTime;
    [SerializeField] private protected float attackCooldown = 3f;
    public bool isEnemyAttacking = false;
    public bool isHitted = false;
    public bool isStunned = false;

    private Animator animator;
    private EnemyHealth enemyHealth;

    public void Awake() 
    {
        lastAttackTime = 0f;
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    public void TryAttack(PlayerStats playerStats, Transform enemyTransform)
    {
        if (isEnemyAttacking || isHitted || isStunned) return;

        float distance = Vector2.Distance(enemyTransform.position, playerStats.transform.position);
        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            isEnemyAttacking = true;
            lastAttackTime = Time.time;
            animator.SetTrigger("AttackTrigger");
            Attack(playerStats, enemyTransform, (playerStats.transform.position - enemyTransform.position).normalized);

            enemyHealth.ResetHitCount();
        }
    }

    public virtual void InterruptAttack(float microStunDuration)
    {
        isEnemyAttacking = false;

        if (!isHitted)
        {
            StartCoroutine(MicroStunCoroutine(microStunDuration));
        }
        
        //animator.SetTrigger("HitTrigger");
    }

    private IEnumerator MicroStunCoroutine(float duration)
    {
        isHitted = true;

        yield return new WaitForSeconds(duration);

        isHitted = false;

        //animator.SetTrigger("RecoverTrigger");
    }

    public abstract void Attack(PlayerStats playerStats, Transform enemyTransform, Vector2 attackDirection);
}