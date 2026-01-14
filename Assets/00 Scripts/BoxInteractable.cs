using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxInteractable : MonoBehaviour, IInteractable
{
    public string doorName = "Door";

    public AudioSource audioSource;
    public SubtitleManager subtitleManager;
    public SubtitleData subtitleData;
    public string GetPromptText() => $"Next Level (E)";

    public void Interact(GameObject interactor)
    {
        Debug.Log($"{interactor.name} opened {doorName}!");
        audioSource.clip = subtitleData.voiceClip;
        audioSource.Play();
        subtitleManager.PlaySubtitles(subtitleData, audioSource);

        // Add your door animation, sound, or logic here
    }
}
