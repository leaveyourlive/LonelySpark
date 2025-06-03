using UnityEngine;

public enum ItemType
{
    Weapon,
    Spell,
    MoneyItem,
}

[CreateAssetMenu(fileName = "New ItemData", menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public string Description;
    public ItemType ItemType;
    public float Value;
    public Sprite Icon;
    public GameObject Prefab;
}