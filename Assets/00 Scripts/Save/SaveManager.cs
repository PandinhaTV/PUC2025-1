using System.Collections;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public SaveData saveData;
    public ProfileData currentProfile;
    private float autosaveInterval = 300f; // 5 minutes = 300 seconds
    string savePath;

    void Start()
    {
       // CreateProfile("test");
    }
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;


        savePath = Path.Combine(Application.persistentDataPath, "save.json");
        Load();
        StartCoroutine(AutoSaveRoutine());
    }

    // ---------- LOAD ----------
    public void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            saveData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            saveData = new SaveData();
            Save();
        }
        
    }

    // ---------- SAVE ----------
    public void Save()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
    }
    public void CreateProfile(string playerName)
    {
        ProfileData profile = new ProfileData
        {
            profileId = System.Guid.NewGuid().ToString(),
            level = 1
        };
        Debug.Log(profile.profileId);
        saveData.profiles.Add(profile);
        SelectProfile(profile.profileId);
        Save();
    }
    public void SelectProfile(string profileId)
    {
        currentProfile = saveData.profiles.Find(p => p.profileId == profileId);

        if (currentProfile != null)
        {
            saveData.lastUsedProfileId = profileId;
            Save();
        }
        else
        {
            Debug.LogError("Profile not found!");
        }
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(autosaveInterval);

            Save();
        }
    }
}
    