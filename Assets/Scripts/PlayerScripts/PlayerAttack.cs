using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    private Item currentItem;

    [Header("Combo Settings")]
    [SerializeField] private float comboResetTime = 2f; // Если за это время не нажали атаку снова, комбо сбросится
    [SerializeField] private float comboEndExtraPause = 0.2f; // Доп. пауза после последнего удара

    [SerializeField] private int currentComboIndex = 0;   // какой шаг комбо следующий
    [SerializeField] private float lastAttackTime = 0f;   // время последнего удара

    [Header("Parry Settings")]
    [SerializeField] private float parryCooldown = 1f;
    [SerializeField] private float parryDuration = 0.2f;
    [SerializeField] private float parryStaminaUse = 14f;

    [Header("Microstun Settings")]
    public float microStunDuration = 0.5f;
    public bool isHitted = false;

    public bool isParrying;
    public bool isAttacking;
    public bool canDoShit = true; // ты знаешь что это делает

    private Animator animator;
    public AnimatorOverrideController woodenSwordController;
    public AnimatorOverrideController stoneSwordController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        lastAttackTime = 0f;
    }

    public void SetCurrentItem(Item item)
    {
        currentItem = item;
        if (item is Weapon weapon)
        {
            if (weapon.weaponType == WeaponType.Melee)
            {
                //animator.SetBool("HasSword", true);

                if (weapon.itemData.ItemName == "Wooden Sword")
                {
                    Debug.Log("Wooden Sword, animController = woodenSwordContoller(override)");
                    animator.runtimeAnimatorController = woodenSwordController;
                }
                else if (weapon.itemData.ItemName == "Stone Sword")
                {
                    Debug.Log("Stone Sword, animController = stoneSwordContoller(override)");
                    animator.runtimeAnimatorController = stoneSwordController;
                }
            }
            else
            {
                //animator.SetBool("HasSword", false);
            }
        }
    }

    void Update()
    {
        if (currentItem == null || !canDoShit || isHitted)
            return;

        switch (currentItem.itemData.ItemType)
        {
            case ItemType.Weapon:
                HandleWeaponInput();
                break;
        }
    }

    private void HandleWeaponInput()
    {
        if (currentItem is Weapon weapon)
        {
            if (currentItem is MeleeWeapon meleeWeapon)
            {
                // ЛКМ = комбо
                if (SettingsManager.GetKeyDown(InputAction.Attack) && !isParrying)
                {
                    TryComboAttack(meleeWeapon);
                }
                // ПКМ = парирование
                else if (SettingsManager.GetKeyDown(InputAction.Parry) && !isAttacking)
                {
                    TryParry();
                }
            }
        }
    }

    private void TryComboAttack(MeleeWeapon weapon)
    {
        if (Time.time - lastAttackTime > comboResetTime)
        {
            currentComboIndex = 0;
        }

        if (currentComboIndex >= weapon.comboSteps.Length)
        {
            currentComboIndex = 0;
        }

        // текущий шаг комбо
        MeleeWeapon.ComboStep step = weapon.comboSteps[currentComboIndex];

        if (Time.time < lastAttackTime + step.nextAttackCooldown)
        {
            Debug.Log("Слишком рано для следующего удара");
            return;
        }

        if (playerStats.currentStamina < step.staminaCost)
        {
            Debug.Log("Недостаточно стамины для удара комбо");
            return;
        }

        StartCoroutine(PerformComboStep(weapon, currentComboIndex));
    }

    private IEnumerator PerformComboStep(MeleeWeapon weapon, int stepIndex)
    {
        isAttacking = true;
        canDoShit = false; // на короткий момент блокируем другие действия (чтобы не прерывать сам удар)
        
        MeleeWeapon.ComboStep step = weapon.comboSteps[stepIndex];
        playerStats.UseStamina(step.staminaCost);

        // Запускаем анимацию атаки. Передаём ComboIndex в Animator
        animator.SetInteger("ComboIndex", stepIndex);
        animator.SetTrigger("Attack");
        animator.SetBool("IsAttacking", true);
        
        lastAttackTime = Time.time;
        currentComboIndex++;
        
        // Логика атаки в weapon
        // weapon.AttackComboStep(playerStats, stepIndex);
        yield return StartCoroutine(weapon.AttackComboStepRoutine(playerStats, stepIndex));
        // weapon.AttackComboStep(playerStats, stepIndex);
        // 26.02 => запихнул AttackComboStep в корутину, если вызвать weapon.AttackComboStep который    !!!
        // ее же вызывает то присылает ошибку                                                           !!!

        // Можно подождать чуть-чуть, пока идёт анимация конкретного удара.
        // Допустим, 0.1сек для "замаха".
        // Или можно использовать Animation Event, тогда корутина не нужна.

        isAttacking = false;
        canDoShit = true;
        animator.SetBool("IsAttacking", false);

        // Если теперь комбо достигло конца, делаем дополнительную паузу (comboEndExtraPause)
        // Чтобы следующий удар не пошёл мгновенно
        if (currentComboIndex >= weapon.comboSteps.Length)
        {
            // Можно тут же сбросить, если хотим «обнулять» сразу после финального удара
            // currentComboIndex = 0;
            // Или подождать чуть-чуть, чтобы игрок не мог мгновенно начать новое комбо
            canDoShit = false;
            yield return new WaitForSeconds(comboEndExtraPause);
            canDoShit = true;

            currentComboIndex = 0;
        }
    }

    private void TryParry()
    {
        // использует общий lastAttackTime
        // нужно сделать отдельную переменную lastParryTime
        if (Time.time >= lastAttackTime + parryCooldown)
        {
            StartCoroutine(PerformParry());
        }
        else
        {
            Debug.Log("Парирование на КД");
        }
    }

    private IEnumerator PerformParry()
    {
        isParrying = true;
        canDoShit = false;
        playerStats.UseStamina(parryStaminaUse);
        Debug.Log("Игрок пытается парировать");

        yield return new WaitForSeconds(parryDuration);

        isParrying = false;
        canDoShit = true;
        lastAttackTime = Time.time;
        Debug.Log("Закончил парировать");
    }

    // <summary>
    /// Вызываем извне, когда игрок получил удар и должен встать в стан.
    /// </summary>
    public void InterruptAttack(float duration)
    {
        if (isHitted) return;
        StartCoroutine(MicroStunCoroutine(duration));
        animator.SetTrigger("HitTrigger");
    }

    private IEnumerator MicroStunCoroutine(float duration)
    {
        isHitted = true;
        canDoShit = false;   // блокируем все действия

        yield return new WaitForSeconds(duration);

        isHitted = false;
        canDoShit = true;    // даём обратно управление
    }
}