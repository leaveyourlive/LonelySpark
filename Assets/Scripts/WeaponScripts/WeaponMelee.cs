using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [Serializable]
    public struct ComboStep
    {
        public float staminaCost;        // Сколько стамины стоит удар
        public float damage;             // Урон удара
        public float attackRange;        // Радиус удара (CircleCast)
        public float attackAngle;        // Угол атаки (для проверки попадания)
        public float nextAttackCooldown; // Кулдаун до следующего удара
        public float stepDistance;       // Дистанция подшага (рывка)
        public float attackAnimTime;
        public float animWithoutHittingTime;
    }

    [Header("Combo Settings")]
    public ComboStep[] comboSteps = new ComboStep[3]
    {
        new ComboStep 
        { 
            staminaCost = 5f,
            damage = 10f,
            attackRange = 3f,
            attackAngle = 60f,
            nextAttackCooldown = 0.617f,
            stepDistance = 1f,
            attackAnimTime = 0.517f,
            animWithoutHittingTime = 0.400f
        },

        new ComboStep 
        {
            staminaCost = 5f,
            damage = 12f,
            attackRange = 3f,
            attackAngle = 60f,
            nextAttackCooldown = 0.533f,
            stepDistance = 1f,
            attackAnimTime = 0.433f,
            animWithoutHittingTime = 0.317f
        },

        new ComboStep 
        {
            staminaCost = 8f,
            damage = 20f,
            attackRange = 3f,
            attackAngle = 60f,
            nextAttackCooldown = 0.533f,
            stepDistance = 1f,
            attackAnimTime = 0.433f,
            animWithoutHittingTime = 0.317f
        }
    };

    private Vector2 playerPosition;
    private Vector2 attackDirection;
    private Vector2 mousePos;

    private float angleCoefficient = 1.5f;

    public override void Attack(PlayerStats playerStats) { }

    // разобраться
    public void AttackComboStep(PlayerStats playerStats, int comboIndex)
    {
        StartCoroutine(AttackComboStepRoutine(playerStats, comboIndex));
    }

    public IEnumerator AttackComboStepRoutine(PlayerStats playerStats, int comboIndex)
    {
        if (comboIndex < 0 || comboIndex >= comboSteps.Length)
        {
            Debug.Log("Неверный индекс удара в комбо");
            yield break;
        }

        var step = comboSteps[comboIndex];
        var hitEnemies = new HashSet<EnemyHealth>(); 

        Recalculate();

        // Выполняем подшаг перед атакой
        Debug.Log("Сейчас будет подходить");
        PlayerMovement pm = playerStats.GetComponent<PlayerMovement>();
        pm.AttackStep(attackDirection, step.stepDistance);
        Recalculate();

        // Анимация атаки
        // Debug.Log("Закончил подшаг, анимируем атаку");
        // WeaponHolder weaponHolder = playerStats.GetComponentInChildren<WeaponHolder>();
        // weaponHolder.AnimateAttack(step.attackAngle, step.attackAnimTime);

        yield return new WaitForSeconds(step.animWithoutHittingTime);

        // Проверяем попадания
        float checkInterval = 0.05f; // Проверяем 20 раз за секунду
        WaitForSeconds checkIntervalWait = new WaitForSeconds(checkInterval); // кэшируем

        int checksCount = Mathf.FloorToInt( (step.attackAnimTime - step.animWithoutHittingTime) / checkInterval );
        
        for (int i = 0; i < checksCount; i++)
        {
            Recalculate(); // Обновляем позицию и направление перед каждой проверкой
            CheckHits(
                playerStats, 
                playerPosition, 
                attackDirection,
                step.damage, 
                step.attackRange,
                step.attackAngle,
                hitEnemies
            );
            yield return checkIntervalWait;
        }

        Debug.Log("Атака закончена");
    }

    private void Recalculate()
    {
        Physics2D.SyncTransforms(); // для визуальной точности
        playerPosition = PlayerStats.Instance.transform.position;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        attackDirection = (mousePos - playerPosition).normalized;
    }

    private void CheckHits(
        PlayerStats playerStats,
        Vector2 playerPosition,
        Vector2 attackDirection,
        float damage,
        float attackRange,
        float attackAngle,
        HashSet<EnemyHealth> hitEnemies)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(playerPosition, attackRange, Vector2.zero, 0);
        foreach (var hit in hits)
        {
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy == null || hitEnemies.Contains(enemy)) continue;

            Vector2 directionToEnemy = ((Vector2)enemy.transform.position - playerPosition).normalized;
            float angle = Vector2.Angle(attackDirection, directionToEnemy);

            if (angle <= attackAngle / angleCoefficient)
            {
                enemy.TakeDamage(damage * playerStats.physDamageAmplifier, DamageType.Physical);
                hitEnemies.Add(enemy);
                Debug.Log($"{enemy.name} получил урон от Melee удара игрока");
            }
        }
    }
}