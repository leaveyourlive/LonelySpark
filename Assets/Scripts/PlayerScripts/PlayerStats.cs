using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [Header("Атрибуты")]
    public int Vigor = 10; //Макс жизни
    public int Strength = 13;
    public int Endurance = 12; //Макс выносливость
    public int Agility = 9;
    public int Willpower = 6; //Макс мана
    public int Intelligence = 8;
    public int Faith = 9;

    [Header("Статы")]
    public float maxHealth;
    public float maxStamina;
    public float maxMana;
    public float currentHealth;
    public float currentStamina;
    public float currentMana;
    public float staminaRegenRate = 5f;
    public bool canRegenerateStamina = true;
    public bool isInvincible = false;

    [Header("Деньги")]
    public int money = 0;

    [Header("Броня")]
    public float armor = 0f;

    [Header("Бонусы")]
    public float physDamageAmplifier = 1f;
    public float magicDamageAmplifier = 1f;
    public float bonusHPAmplifier = 1f;
    [SerializeField] private float physDamageResistancePercent = 0f;
    public float physDamageResistanceFloat = 0f;
    public float magicDamageResistancePercent = 0f;
    public float magicDamageResistanceFloat = 0f;
    public float physAttackVampirismPercent = 0f;
    public bool isInvisible = false;

    [Header("Эффекты способностей")]
    [SerializeField] private GameObject suckEffectPrefab;
    private GameObject activeSuckEffect;
    [SerializeField] private GameObject bloodVampEffectPrefab;
    [SerializeField] private GameObject blackFireEffectPrefab;
    [SerializeField] private GameObject eyeOfAbyssEffectPrefab;
    private GameObject activeEyeOfAbyssEffect;

    private Vector3 startPosition;

    public static PlayerStats Instance { get; private set; }

    private void Awake()
    {
        startPosition = transform.position;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SaveSystem.LoadPlayerData(this);
            UpdateStats();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerData()
    {
        SaveSystem.SavePlayerData(this);
    }

    public void LoadPlayerData()
    {
        SaveSystem.LoadPlayerData(this);
    }

    private void Update()
    {
        RegenerateStamina();
    }

    // Метод получения урона
    public void TakeDamage(float damage, DamageType damageType)
    {
        if (!isInvincible)
        {
            float finalDamage = CalculateDamage(damage, damageType);
            currentHealth = Mathf.Clamp(currentHealth - finalDamage, 0, maxHealth);
            Debug.Log($"Игрок получил урон {finalDamage} типа {damageType}");

            if (gameObject.TryGetComponent(out PlayerAttack playerAttack))
            {
                playerAttack.InterruptAttack(playerAttack.microStunDuration);
            }

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    // Расчёт конечного урона с учётом сопротивлений
    private float CalculateDamage(float raw, DamageType type)
    {
        switch (type)
        {
            case DamageType.Physical:
                return raw * (1 - physDamageResistancePercent / 100) - physDamageResistanceFloat;
            case DamageType.Magical:
                return raw * (1 - magicDamageResistancePercent / 100) - magicDamageResistanceFloat;
            case DamageType.Pure:
                return raw;
            default:
                return raw;
        }
    }

    // Метод для лечения игрока
    public void Heal(float amount)
    {
        Debug.Log($"получено лечение {amount}");
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    // Использование выносливости
    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    // Использование маны
    public void UseMana(float amount)
    {
        currentMana -= amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
    }

    // Регенирация выносливости
    private void RegenerateStamina()
    {
        if (currentStamina < maxStamina && canRegenerateStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }

    // Обновление всех статов игрока
    private void UpdateStats()
    {
        maxHealth = Vigor * 20 * bonusHPAmplifier;
        maxStamina = Endurance * 18;
        maxMana = Willpower * 15;
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentMana = maxMana;

        staminaRegenRate = 9f; // Endurance || Agil * k + bonus

        // Устанавливаем сопротивление физическому урону через броню
        physDamageResistancePercent = armor / 2; // Можно изменить на более подходящее значение
    }

    // Добавление денег игроку
    public void AddMoney(int amount)
    {
        money += amount;
    }

    /// <summary>
    /// SUCK ability
    /// </summary>
    public void Suck(float duration, float HPCost, float damage, float range, float damageInterval)
    {
        TakeDamage(HPCost, DamageType.Pure);
        StartCoroutine(SuckRoutine(duration, damage, range, damageInterval));

        if (activeSuckEffect == null)
        {
            activeSuckEffect = Instantiate(suckEffectPrefab, transform.position, Quaternion.identity);
            activeSuckEffect.transform.localScale = new Vector3(range * 2, range * 2, 1);
            activeSuckEffect.transform.SetParent(transform, true); // Присоединяем, сохраняя текущий размер
        }

        activeSuckEffect.SetActive(true);

        Invoke(nameof(HideSuckEffect), duration);
    }

    private void HideSuckEffect()
    {
        if (activeSuckEffect != null)
        {
            activeSuckEffect.SetActive(false);
        }
    }

    // Рутину для всасывания
    private IEnumerator SuckRoutine(float duration, float damage, float range, float damageInterval)
    {
        int checkCount = Mathf.FloorToInt(duration / damageInterval);
        WaitForSeconds damageIntervalWait = new WaitForSeconds(damageInterval);

        for (int i = 0; i < checkCount; i++)
        {
            CheckSuckHits(damage, range);
            yield return damageIntervalWait;
        }
    }

    // Проверка попадания в радиус всасывания
    private void CheckSuckHits(float damage, float range)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0);
        foreach (var hit in hits)
        {
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy == null) continue;
            Instantiate(bloodVampEffectPrefab, enemy.transform.position, Quaternion.identity);
            enemy.TakeDamage(damage, DamageType.Magical);
            Debug.Log("Способность: соснул у противника");
            Heal(damage);
        }
    }

    private float drawSuckRange;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, drawSuckRange);
    }

    /// <summary>
    /// BLACKFIRE ability
    /// </summary>
    public void BlackFire(float duration, float damage, float damageInterval, float positionDelta)
    {
        StartCoroutine(AwaitingClickRoutine(duration, damage, damageInterval, positionDelta));
    }

    private IEnumerator AwaitingClickRoutine(float duration, float damage, float damageInterval, float positionDelta)
    {
        float elapsedTime = 0f;
        GameObject cursor = GameObject.FindWithTag("cursor");

        while (elapsedTime < duration)
        {
            GameObject[] enemyManagers = GameObject.FindGameObjectsWithTag("Spawner");
            foreach (GameObject enemyManager in enemyManagers)
            {
                EnemySpawner spawner = enemyManager.GetComponent<EnemySpawner>();
                if (spawner == null) continue;
                List<GameObject> enemyPool_ = spawner.enemyPool;
                foreach (GameObject enemy in enemyPool_)
                {
                    if (Vector3.Distance(cursor.transform.position, enemy.transform.position) < positionDelta)
                    {
                        StartCoroutine(BlackFireRoutine(duration, damage, damageInterval, enemy));
                        yield break; // Завершаем ожидание, так как способность применена
                    }
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Способность истекла, игрок не успел выбрать врага.");
    }

    private IEnumerator BlackFireRoutine(float duration, float damage, float damageInterval, GameObject enemy)
    {
        EnemyHealth enemyComponent = enemy.GetComponent<EnemyHealth>();
        if (enemyComponent == null) yield break;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            // Создаём визуальный эффект
            Instantiate(blackFireEffectPrefab, enemy.transform.position, Quaternion.identity);
            enemyComponent.TakeDamage(damage, DamageType.Magical);
            yield return new WaitForSeconds(damageInterval);
            elapsedTime += damageInterval;
        }
    }

    /// <summary>
    /// EYE OF ABYSS ability
    /// </summary>
    public void eyeOfAbyss(float duration, float range, float decelerationFactor, float accelerationСoefficient)
    {
        StartCoroutine(EyeOfAbyssRoutine(duration, range, decelerationFactor, accelerationСoefficient));
    }

    private IEnumerator EyeOfAbyssRoutine(float duration, float range, float decelerationFactor, float accelerationСoefficient)
    {
        float elapsedTime = 0f;
        List<EnemyBehaviour> affectedEnemies = new List<EnemyBehaviour>();

        // Отображаем круг визуально
        if (activeEyeOfAbyssEffect == null)
        {
            activeEyeOfAbyssEffect = Instantiate(eyeOfAbyssEffectPrefab, transform.position, Quaternion.identity);
            activeEyeOfAbyssEffect.transform.localScale = new Vector3(range * 2, range * 2, 1);
            activeEyeOfAbyssEffect.transform.SetParent(transform);
        }
        activeEyeOfAbyssEffect.SetActive(true);

        while (elapsedTime < duration)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);
            HashSet<EnemyBehaviour> currentEnemies = new HashSet<EnemyBehaviour>();

            foreach (var hit in hits)
            {
                EnemyBehaviour enemy = hit.GetComponent<EnemyBehaviour>();
                if (enemy != null)
                {
                    currentEnemies.Add(enemy);
                    if (!affectedEnemies.Contains(enemy))
                    {
                        enemy.SetSpeedMultiplier(decelerationFactor); // Замедление
                        affectedEnemies.Add(enemy);
                    }
                }
            }

            // Возвращаем нормальную скорость тем, кто вышел из круга
            for (int i = affectedEnemies.Count - 1; i >= 0; i--)
            {
                EnemyBehaviour enemy = affectedEnemies[i];
                if (!currentEnemies.Contains(enemy))
                {
                    enemy.SetSpeedMultiplier(1f);
                    affectedEnemies.RemoveAt(i);
                }
            }

            elapsedTime += 0.2f;
            yield return new WaitForSeconds(0.2f); // не каждый кадр, а раз в 0.2 сек
        }

        // Восстанавливаем скорость у всех
        foreach (var enemy in affectedEnemies)
        {
            if (enemy != null)
                enemy.SetSpeedMultiplier(1f);
        }

        if (activeEyeOfAbyssEffect != null)
            activeEyeOfAbyssEffect.SetActive(false);
    }

    private void Die()
    {
        money = 0;
        transform.position = startPosition;
        Debug.Log("Игрок умер");
    }
}
