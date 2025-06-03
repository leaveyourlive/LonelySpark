using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIPanel : MonoBehaviour
{
    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private Transform slotParent;
    [SerializeField] private Text categoryName;

    private List<InventorySlot> slots = new List<InventorySlot>();

    public List<Item> lastItemsDisplayed = new();

    public void UpdateInventoryUI(List<Item> itemsToDisplay)
    {
        Debug.Log($"Вызвано InventoryUIPanel.UpdateInventoryUI");

        // Очистка старых слотов
        foreach (var slot in slots)
        {
            Destroy(slot.gameObject);
        }
        slots.Clear();

        // Добавление новых слотов
        foreach (var item in itemsToDisplay)
        {
            GameObject newSlot = Instantiate(inventorySlotPrefab, slotParent);
            InventorySlot slotComponent = newSlot.GetComponent<InventorySlot>();
            slotComponent.SetItem(item);
            slots.Add(slotComponent);
        }

        Canvas.ForceUpdateCanvases();
    }

    // Кнопки для показа по категориям
    public void ShowAllItems()
    {
        ChangeCategoryName("Все предметы");
        lastItemsDisplayed = InventoryManager.Instance.GetItemsByType();
        UpdateInventoryUI(lastItemsDisplayed);
    }

    public void ShowMeleeWeapons()
    {
        ChangeCategoryName("Ближний бой");
        List<Item> meleeWeapons = InventoryManager.Instance.GetWeaponsByType(WeaponType.Melee);
        lastItemsDisplayed = meleeWeapons;
        UpdateInventoryUI(lastItemsDisplayed);
    }

    public void ChangeCategoryName(string newName)
    {
        categoryName.text = newName;
    }
}