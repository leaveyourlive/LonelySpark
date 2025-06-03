using UnityEngine;
using UnityEngine.UI;

public class PlayerEquipment : MonoBehaviour
{
    public static PlayerEquipment Instance { get; private set; }

    [SerializeField] private PlayerAttack playerAttack;

    private GameObject currentItemPrefab;

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

    public void EquipItem(Item item)
    {
        Debug.Log($"Вызвано PlayerEquipment.EquipItem");
        if (currentItemPrefab != null && item.itemData.ItemType != ItemType.Spell)
        {
            Destroy(currentItemPrefab);
        }

        if (item != null && item.itemData.Prefab != null)
        {
            if (item.itemData.ItemType != ItemType.Spell)
            {
                //currentItemPrefab = Instantiate(item.itemData.Prefab, itemSlot.position, itemSlot.rotation, itemSlot);
                playerAttack.SetCurrentItem(item);
            }
        }
    }
}
