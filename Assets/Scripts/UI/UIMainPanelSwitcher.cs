using UnityEngine;
using UnityEngine.UI;

public class UIPanelSwitcher : MonoBehaviour
{
    [Header("Tab Buttons")]
    [Tooltip("Кнопка «Инвентарь»")]
    public Button inventoryButton;
    [Tooltip("Кнопка «Экипировка»")]
    public Button equipmentButton;
    [Tooltip("Кнопка «Статы»")]
    public Button statsButton;

    [Header("Sub-Panels")]
    [Tooltip("InventoryBGPanel")]
    public GameObject inventoryPanel;
    [Tooltip("EquipmentPanel")]
    public GameObject equipmentPanel;
    [Tooltip("StatsBGPanel")]
    public GameObject statsPanel;

    private GameObject currentPanel;

    void Start()
    {
        // Подпишемся на клики
        inventoryButton.onClick.AddListener(() => ShowPanel(inventoryPanel));
        equipmentButton.onClick.AddListener(() => ShowPanel(equipmentPanel));
        statsButton.onClick.AddListener(() => ShowPanel(statsPanel));

        // По умолчанию сразу показываем инвентарь
        ShowPanel(inventoryPanel);
    }

    // Этот метод включает нужную панель и выключает остальные
    public void ShowPanel(GameObject panelToShow)
    {
        // Если уже выбрана — ничего не делаем
        if (currentPanel == panelToShow) return;

        // Гасим старую вкладку
        if (currentPanel != null)
            currentPanel.SetActive(false);

        // Включаем новую
        panelToShow.SetActive(true);
        currentPanel = panelToShow;
    }
}