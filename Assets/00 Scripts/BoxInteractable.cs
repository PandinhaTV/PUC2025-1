using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxInteractable : MonoBehaviour, IInteractable
{
    public string doorName = "Door";

    public string GetPromptText() => $"Next Level (E)";

    public void Interact(GameObject interactor)
    {
        Debug.Log($"{interactor.name} opened {doorName}!");
        SceneManager.LoadScene("Game/memories2 1");
        // Add your door animation, sound, or logic here
    }
}
