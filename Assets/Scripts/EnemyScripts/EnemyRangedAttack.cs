using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRangedAttack : EnemyAttack
{
    [SerializeField] private GameObject projectilePrefab; // Префаб снаряда
    [SerializeField] private float projectileSpeed = 10f; // Скорость снаряда

    public override void Attack(PlayerStats playerStats, Transform enemyTransform, Vector2 attackDirection)
    {
        StartCoroutine(PerformRangedAttack(attackDirection));
    }

    private IEnumerator PerformRangedAttack(Vector2 attackDirection)
    {
        isEnemyAttacking = true;

        // Создаём снаряд и задаём его параметры
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.FromToRotation(transform.position, attackDirection));
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = attackDirection * projectileSpeed;
        projectile.GetComponent<ProjectileBehaviour>().projectileDamage = damage;

        yield return new WaitForSeconds(attackCooldown); // Ждём перезарядки

        isEnemyAttacking = false;
    }
}