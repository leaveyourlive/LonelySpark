using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private List<IInteractable> availableInteractions = new List<IInteractable>();

    private void Update()
    {
        if (PickedItemInfoUI.IsUIOpen || UIBlocker.IsAnyMenuOpen)
            return;

        if (Input.GetKeyDown(SettingsManager.GetKeyByAction(InputAction.Use)))
        {
            TryInteractWithHighestPriority();
        }
    }

    private void TryInteractWithHighestPriority()
    {
        if (availableInteractions.Count == 0) return;

        var highestPriority = FindHighestPriorityInteractable();
        highestPriority?.Interact();
    }

    private IInteractable FindHighestPriorityInteractable()
    {
        IInteractable bestInteractable = null;
        int highestPriority = int.MaxValue;

        foreach (var interactable in availableInteractions)
        {
            int currentPriority = (int)interactable.Priority;
            if (currentPriority < highestPriority)
            {
                highestPriority = currentPriority;
                bestInteractable = interactable;
            }
        }

        return bestInteractable;
    }

    public void RegisterInteractable(IInteractable interactable)
    {
        if (!availableInteractions.Contains(interactable))
        {
            availableInteractions.Add(interactable);
        }
    }

    public void UnregisterInteractable(IInteractable interactable)
    {
        if (availableInteractions.Contains(interactable))
        {
            availableInteractions.Remove(interactable);
        }
    }
}