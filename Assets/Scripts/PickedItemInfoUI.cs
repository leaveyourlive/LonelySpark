using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class PickedItemInfoUI : MonoBehaviour
{
    [SerializeField] private Text itemName;
    [SerializeField] private Text itemQuantity;
    [SerializeField] private Image itemIcon;
    [SerializeField] private float appearDuration = 0.3f;
    [SerializeField] private float hideDuration = 0.01f;

    private CanvasGroup canvasGroup;
    private Action confirmationCallback;

    public static bool IsUIOpen { get; private set; } = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        // Всегда активен, но прозрачен и не кликаемый
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowItemInfo(Item item, Action onConfirmed)
    {
        confirmationCallback = onConfirmed;
        StartCoroutine(ShowRoutine(item));
    }

    private IEnumerator ShowRoutine(Item item)
    {
        // 1) Флаг открыт
        IsUIOpen = true;

        // 2) Заполнение данных
        itemName.text     = item.itemData.ItemName;
        itemQuantity.text = "1";
        itemIcon.sprite   = item.itemData.Icon;

        // 3) Плавный fade-in
        yield return StartCoroutine(FadeUI(0f, 1f, appearDuration));
        canvasGroup.interactable  = true;
        canvasGroup.blocksRaycasts = true;

        // 4) Ждём: либо кнопка Use, либо открытие другого меню
        var useKey = SettingsManager.GetKeyByAction(InputAction.Use);
        while (true)
        {
            if (Input.GetKeyDown(useKey) || UIBlocker.IsAnyMenuOpen)
                break;
            yield return null;
        }

        // 5) Сразу «подтверждаем»
        confirmationCallback?.Invoke();

        // 6) Запрет кликов, fade-out
        canvasGroup.interactable  = false;
        canvasGroup.blocksRaycasts = false;
        yield return StartCoroutine(FadeUI(1f, 0f, hideDuration));
        IsUIOpen = false;
    }

    private IEnumerator FadeUI(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = to;
    }

    // IInteractable.Interact не нужен
}