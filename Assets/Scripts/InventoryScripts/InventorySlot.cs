using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    private Item currentItem;

    public void SetItem(Item item)
    {
        currentItem = item;
        itemIcon.sprite = item.itemData.Icon;
    }

    public void OnSlotClicked()
    {
        Debug.Log($"Кликнут слот инвентаря");

        // Получаем ссылку на PlayerStats (синглтон или компонент)
        PlayerStats playerStats = PlayerStats.Instance; // Или GetComponent<PlayerStats>() если это компонент на том же объекте

        // Передаем currentItem и playerStats в InventoryManager
        InventoryManager.Instance.EquipItem(currentItem, playerStats);
    }
}
