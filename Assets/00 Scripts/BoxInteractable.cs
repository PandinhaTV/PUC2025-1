using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxInteractable : MonoBehaviour, IInteractable
{
    public string doorName = "Door";

    public AudioSource audioSource;
    
    public SubtitleData subtitleData;
    public string GetPromptText() => $"Next Level (E)";

    public void Interact(GameObject interactor)
    {
        Debug.Log($"{interactor.name} opened {doorName}!");
        audioSource.clip = subtitleData.voiceClip;
        audioSource.Play();
        SubtitleManager.Instance.PlaySubtitles(subtitleData, audioSource);

        // Add your door animation, sound, or logic here
    }
}
