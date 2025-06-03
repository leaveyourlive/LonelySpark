using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public EnemyHealthBar enemyHealthBar;
    private Animator animator;
    private EnemyAttack enemyAttack;

    private float currentHealth;
    [SerializeField] private float maxHealth = 25f;
    [SerializeField] private float armor = 0f;
    [SerializeField] private float magicResistance = 0f;

    [SerializeField] private float respawnTime = 5f;

    private int receivedHitsCount = 0;
    [SerializeField] private float singleHitLifetime = 0.35f;
    // [SerializeField] private float allHitsLifetime = 3f;

    void Awake()
    {
        currentHealth = maxHealth;
        enemyHealthBar = GetComponent<EnemyHealthBar>();
        enemyHealthBar.UpdateHealthBar(currentHealth, maxHealth);
        animator = GetComponent<Animator>();
        enemyAttack = GetComponent<EnemyAttack>();
    }

    public void TakeDamage(float damage, DamageType damageType)
    {
        {
            float finalDamage = CalculateDamage(damage, damageType);
            currentHealth = Mathf.Clamp(currentHealth - finalDamage, 0, maxHealth);
            enemyHealthBar.UpdateHealthBar(currentHealth, maxHealth);
            Debug.Log($"{gameObject.name} получил урон {finalDamage} типа {damageType}");

            PlayerStats playerStats = PlayerStats.Instance;
            if (playerStats.physAttackVampirismPercent > 0)
            {
                playerStats.Heal(finalDamage * playerStats.physAttackVampirismPercent);
                Debug.Log($"Вампиризм: Игрок излечился на {finalDamage} * {playerStats.physAttackVampirismPercent} = {finalDamage * playerStats.physAttackVampirismPercent}");
            }

            receivedHitsCount++;
            if (receivedHitsCount <= 3)
            {
                enemyAttack.InterruptAttack(singleHitLifetime);
            }

            if (currentHealth <= 0)
            {
                StartCoroutine(TimeOut(respawnTime));
                currentHealth = maxHealth;
            }

            animator.SetTrigger("HitTrigger");
        }
    }

    private float CalculateDamage(float raw, DamageType type)
    {
        switch (type)
        {
            case DamageType.Physical:
                return raw*(1 - armor/100);
            case DamageType.Magical:
                return raw*(1 - magicResistance/100);
            case DamageType.Pure:
                return raw;
            default:
                return raw;
        }
    }

    private IEnumerator TimeOut(float time)
    {
        gameObject.SetActive(false);

        yield return new WaitForSeconds(time);

        gameObject.SetActive(true);
    }

    public void ResetHitCount() {
        receivedHitsCount = 0;
    }
}