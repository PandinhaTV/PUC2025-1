using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    
    public SubtitleData subtitleData;
    public bool played = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (played == true) return;
        if (other.gameObject.tag != "Player") return;
        audioSource.clip = subtitleData.voiceClip;
        audioSource.Play();
        SubtitleManager.Instance.PlaySubtitles(subtitleData, audioSource);
    }
}
