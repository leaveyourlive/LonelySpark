using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    private List<Item> weaponItems = new List<Item>();
    private int currentWeaponIndex = 0;

    private void Start()
    {
        // Получаем список оружий из инвентаря
        UpdateInventoryData();
        if (weaponItems.Count > 0)
        {
            EquipWeapon(0);
        }
    }

    private void Update()
    {
        UpdateInventoryData();

        if (SettingsManager.GetKeyDown(InputAction.SwitchWeapon))
        {
            SwitchToNextWeapon();
        }
        // else if (scrollInput < 0f)
        // {
        //     SwitchToPreviousWeapon();
        // }
    }

    private void SwitchToNextWeapon()
    {
        if (weaponItems.Count == 0) return;

        currentWeaponIndex = (currentWeaponIndex + 1) % weaponItems.Count;
        EquipWeapon(currentWeaponIndex);
    }

    private void SwitchToPreviousWeapon()
    {
        if (weaponItems.Count == 0) return;

        currentWeaponIndex = (currentWeaponIndex - 1 + weaponItems.Count) % weaponItems.Count;
        EquipWeapon(currentWeaponIndex);
    }

    private void EquipWeapon(int index)
    {
        Debug.Log($"Смена оружия на: {weaponItems[index].itemData.ItemName}");

        // Получаем ссылку на PlayerStats (через синглтон или компонент)
        PlayerStats playerStats = PlayerStats.Instance; // или если не синглтон, то через GetComponent

        // Передаем в InventoryManager объект PlayerStats
        InventoryManager.Instance.EquipItem(weaponItems[index], playerStats);
    }

    private void UpdateInventoryData()
    {
        // Обновляем список оружия из инвентаря
        weaponItems = InventoryManager.Instance.GetItemsByType(ItemType.Weapon);
    }
}
