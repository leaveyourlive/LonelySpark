using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemData itemData;
    public abstract void Activate();
}