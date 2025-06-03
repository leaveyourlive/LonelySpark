using UnityEngine;

public enum InteractPriority
{
    UI = 0,         // Высший приоритет
    NPC = 1,
    Item = 2,
    Campfire = 3,
    Environment = 4 // Низший приоритет
}

public interface IInteractable
{
    InteractPriority Priority { get; }
    void Interact();
    Vector3 Position { get; }
}
