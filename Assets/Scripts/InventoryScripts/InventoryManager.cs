using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private InventoryUIPanel inventoryUIPanel; // Ссылка на панель инвентаря
    [SerializeField] private EquipmentPanel equipmentPanel; // Ссылка на панель экипировки

    private List<Item> inventoryItems = new List<Item>(); // Список всех предметов в инвентаре

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(Item item)
    {
        Debug.Log($"Вызвано InventoryManager.AddItem");
        inventoryItems.Add(item);
        inventoryUIPanel.UpdateInventoryUI(inventoryUIPanel.lastItemsDisplayed);
    }

    // Метод для экипировки предмета
    public void EquipItem(Item item, PlayerStats playerStats)
    {
        Debug.Log($"Вызвано InventoryManager.EquipItem");
        equipmentPanel.EquipItem(item, playerStats);
        
        if (item is Weapon weapon)
        {
            PlayerEquipment.Instance.EquipItem(item);
        }
    }

    // Получение всех предметов по типу
    public List<Item> GetItemsByType(ItemType? type = null)
    {
        if (type.HasValue)
        {
            return inventoryItems.FindAll(item => item.itemData.ItemType == type.Value);
        }
        else
        {
            return inventoryItems;
        }
    }

    // Получение всех предметов оружия по типу
    public List<Item> GetWeaponsByType(WeaponType weaponType)
    {
        return inventoryItems
            .Where(item => item is Weapon weapon && weapon.weaponType == weaponType)
            .Cast<Item>()
            .ToList();
    }
}
