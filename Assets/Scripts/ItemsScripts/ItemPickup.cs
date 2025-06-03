using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [Header("Item Settings")]
    [SerializeField] private Item[] items;
    [SerializeField] private InteractPriority interactionPriority = InteractPriority.Item;

    [Header("UI References")]
    [SerializeField] private PickedItemInfoUI itemInfoUI;

    public InteractPriority Priority => interactionPriority;
    public Vector3 Position => transform.position;

    private void Awake()
    {
        if (itemInfoUI == null)
        {
            itemInfoUI = FindObjectOfType<PickedItemInfoUI>();
        }
    }

    public void Interact()
    {
        Destroy(gameObject);

        foreach (var item in items)
        {
            if (item.itemData.ItemType == ItemType.MoneyItem)
            {
                item.Activate();
            }
            else
            {
                itemInfoUI.ShowItemInfo(item, null);
                InventoryManager.Instance.AddItem(item);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var interactionSystem = other.GetComponent<PlayerInteraction>();
            if (interactionSystem != null)
            {
                interactionSystem.RegisterInteractable(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var interactionSystem = other.GetComponent<PlayerInteraction>();
            if (interactionSystem != null)
            {
                interactionSystem.UnregisterInteractable(this);
            }
        }
    }
}
