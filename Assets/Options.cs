using UnityEngine;

public class Options : MonoBehaviour
{
    
    public void SubtitlesActivate()
    {

        SubtitleManager.Instance.activateSubtitles = !SubtitleManager.Instance.activateSubtitles;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
