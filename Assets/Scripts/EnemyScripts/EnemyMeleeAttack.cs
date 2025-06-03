using System.Collections;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttack
{
    //[SerializeField] private float swingAngle = 90f;
    [SerializeField] private float swingTime = 0.6f;
    [SerializeField] private float parryWindowStart = 0.2f;
    [SerializeField] private float parryWindowEnd = 0.4f;
    [SerializeField] private float stunDuration = 5f;

    public override void Attack(PlayerStats playerStats, Transform enemyTransform, Vector2 attackDirection)
    {
        StartCoroutine(PerformMeleeAttack(playerStats, enemyTransform, attackDirection));
    }

    private IEnumerator PerformMeleeAttack(PlayerStats playerStats, Transform enemyTransform, Vector2 attackDirection)
    {
        // EnemyWeaponHolder weaponHolder = enemyTransform.GetComponentInChildren<EnemyWeaponHolder>();
        // weaponHolder?.AnimateMeleeAttack(attackDirection, swingAngle, swingTime);

        isEnemyAttacking = true;

        PlayerAttack playerAttack = playerStats.GetComponent<PlayerAttack>();

        // Ожидание до начала окна парирования
        yield return new WaitForSeconds(parryWindowStart);

        // Проверяем, находится ли игрок в состоянии парирования в окне
        if (playerAttack.isParrying)
        {
            Debug.Log("Парировал атаку противника в начале");
            Stun(enemyTransform);
        }
        else
        {
            // Ожидаем окончания окна парирования и проверяем снова
            yield return new WaitForSeconds(parryWindowEnd - parryWindowStart);
            if (!playerAttack.isParrying)
            {
                playerStats.TakeDamage(damage, DamageType.Physical);
            }
            else
            {
                Debug.Log("Парировал атаку в последний момент*");
                Stun(enemyTransform);
            }
        }

        // Ожидание завершения атаки
        yield return new WaitForSeconds(swingTime - parryWindowEnd);

        isEnemyAttacking = false;
    }

    private void Stun(Transform enemyTransform)
    {
        Debug.Log("Противник оглушен");

        isStunned = true;

        StartCoroutine(StunCoroutine(enemyTransform));
    }

    private IEnumerator StunCoroutine(Transform enemyTransform)
    {
        if (TryGetComponent<EnemyBehaviour>(out var enemyBehaviour))
        {
            enemyBehaviour.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            enemyBehaviour.enabled = false; // Отключаем логику на время стана
        }
        
        yield return new WaitForSeconds(stunDuration);

        if (enemyBehaviour != null)
        {
            enemyBehaviour.enabled = true;
        }

        Debug.Log("Противник вышел из стана");
        isStunned = false;
    }
}