using UnityEngine;

public class BoxInteractable : MonoBehaviour, IInteractable
{
    public string doorName = "Door";

    public string GetPromptText() => $"Open {doorName}";

    public void Interact(GameObject interactor)
    {
        Debug.Log($"{interactor.name} opened {doorName}!");
        // Add your door animation, sound, or logic here
    }
}
