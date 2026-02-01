using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProfileData
{
    [Header("Profile")]
    public string profileId;
    public string level;

}

[Serializable]
public class SaveData
{
    public int saveVersion = 1;
    public string lastUsedProfileId;
    public List<ProfileData> profiles = new List<ProfileData>();
}