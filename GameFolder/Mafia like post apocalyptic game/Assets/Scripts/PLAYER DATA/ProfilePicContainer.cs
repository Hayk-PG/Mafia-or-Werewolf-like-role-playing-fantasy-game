using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilePicContainer : MonoBehaviour
{
    public Dictionary<string, Sprite> CachedProfilePics;


    void Awake()
    {
        CachedProfilePics = new Dictionary<string, Sprite>();
    }

    public void CacheProfilePics(string playfabId, Sprite profilePic)
    {
        if (!CachedProfilePics.ContainsKey(playfabId))
        {
            CachedProfilePics.Add(playfabId, profilePic);
        }
    }

    public void DeleteCachedProfilePic(string playfabId)
    {
        if (CachedProfilePics.ContainsKey(playfabId))
        {
            CachedProfilePics.Remove(playfabId);
        }
    }
}
