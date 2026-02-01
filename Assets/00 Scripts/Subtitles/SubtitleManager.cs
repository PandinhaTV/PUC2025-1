using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    #region Singleton
    public static SubtitleManager Instance;
    void Awake()
    {
        if (Instance != null &&  Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    #endregion
    public TextMeshProUGUI subtitleText;
    private SubtitleData currentData;
    private AudioSource audioSource;
    private int currentLineIndex;
    public bool activateSubtitles = true;

    void Update()
    {
        if (currentData == null || audioSource == null || !audioSource.isPlaying)
            return;

        float time = audioSource.time;

        if (currentLineIndex < currentData.lines.Length)
        {
            var line = currentData.lines[currentLineIndex];

            if (time >= line.startTime && time <= line.endTime)
            {
                subtitleText.text = line.text;
            }
            else if (time > line.endTime)
            {
                subtitleText.text = "";
                currentLineIndex++;
            }
        }
    }

    public void PlaySubtitles(SubtitleData data, AudioSource source)
    {
        if (activateSubtitles)
        {
           currentData = data;
                   audioSource = source;
                   currentLineIndex = 0;
                   subtitleText.text = ""; 
        }
        
    }
}


