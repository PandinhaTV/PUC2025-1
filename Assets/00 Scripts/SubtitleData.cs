using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Subtitle Data")]
public class SubtitleData : ScriptableObject
{
    public AudioClip voiceClip;
    public SubtitleLine[] lines;
}

[System.Serializable]
public class SubtitleLine
{
    public string text;
    public float startTime;
    public float endTime;
}