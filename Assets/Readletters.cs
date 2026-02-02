using UnityEngine;

public class Readletters : MonoBehaviour, IInteractable
{
    public AudioSource audioSource;
    
    public SubtitleData subtitleData;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        
        audioSource.clip = subtitleData.voiceClip;
        audioSource.Play();
        SubtitleManager.Instance.PlaySubtitles(subtitleData, audioSource);
        
    }

    public string GetDescription()
    {
        return "Read letter";
    }
}
