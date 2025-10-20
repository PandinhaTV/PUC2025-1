using UnityEngine;

public interface IInteractable
{
    string GetPromptText();  // e.g. "Talk", "Open", "Inspect"
    void Interact(GameObject interactor);
}