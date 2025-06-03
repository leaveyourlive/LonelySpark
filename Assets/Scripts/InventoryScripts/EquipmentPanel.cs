using System.Collections.Generic;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField] private Transform weaponSlot; // Слот для оружия (правый)
    [SerializeField] private Transform potionSlot; // Слот для зелий (нижний)
    [SerializeField] private Transform bombSlot;   // Слот для бомб (верхний)
    [SerializeField] private Transform spellSlot;  // Слот для заклинаний (левый)
    [SerializeField] private Transform arrowSlot;  // Слот для стрел (левый верх)

    private Dictionary<ItemType, Transform> slotMapping;

    // Объекты для экипировки
    private GameObject equippedWeapon;
    private GameObject equippedPotion;
    private GameObject equippedBomb;
    private GameObject equippedSpell;
    private GameObject equippedArrow;

    private void Awake()
    {
        slotMapping = new Dictionary<ItemType, Transform>
        {
            { ItemType.Weapon, weaponSlot },
            { ItemType.Spell, spellSlot },
        };
    }

    public void EquipItem(Item item, PlayerStats playerStats)
    {
        Debug.Log($"Вызвано EquipmentPanel.EquipItem");
        switch (item.itemData.ItemType)
        {
            case ItemType.Weapon:
                EquipInSlot(ref equippedWeapon, item, weaponSlot);
                break;
            case ItemType.Spell:
                EquipInSlot(ref equippedSpell, item, spellSlot);
                break;
            default:
                Debug.LogWarning("Этот предмет не может быть экипирован в панели.");
                break;
        }
    }

    private void EquipInSlot(ref GameObject currentItemObject, Item item, Transform slot)
    {
        if (currentItemObject != null)
        {
            Destroy(currentItemObject);
        }

        if (item.itemData.Icon != null)
        {
            var iconObject = new GameObject("ItemIcon");
            var image = iconObject.AddComponent<UnityEngine.UI.Image>();
            image.sprite = item.itemData.Icon;

            iconObject.transform.SetParent(slot, false);
            currentItemObject = iconObject;
        }
    }
}
