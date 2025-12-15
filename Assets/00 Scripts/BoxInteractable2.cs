using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxInteractable2 : MonoBehaviour, IInteractable
{
    public string doorName = "Door";

    public string GetPromptText() => prompttext;
    public string prompttext = "Next Level (E)";
    public void Interact(GameObject interactor)
    {
        Debug.Log($"{interactor.name} opened {doorName}!");
        SceneManager.LoadScene("Game/memories2");
        // Add your door animation, sound, or logic here
    }
}
