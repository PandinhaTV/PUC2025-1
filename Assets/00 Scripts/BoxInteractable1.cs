using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxInteractable1 : MonoBehaviour, IInteractable
{
    public string doorName = "Door";

    public string GetPromptText() => prompttext;
    public string prompttext = "Next Level (E)";
    public void Interact(GameObject interactor)
    {
        Debug.Log($"{interactor.name} opened {doorName}!");
        Application.Quit();
        // Add your door animation, sound, or logic here
    }
}
