using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSavedData : MonoBehaviour
{
    public static PlayerSavedData PSD;

    void Awake()
    {
        PSD = this;
    }

    void Start()
    {
        print(PlayerPrefs.GetString(PlayerKeys.UsernameKey)
        + "/" + PlayerPrefs.GetString(PlayerKeys.PasswordKey)
        + "/" + PlayerPrefs.GetString(PlayerKeys.UserID));
    }

    #region SaveUsernameAndPassword
    public void SaveUsernameAndPassword(string username, string password)
    {
        PlayerPrefs.SetString(PlayerKeys.UsernameKey, username);
        PlayerPrefs.SetString(PlayerKeys.PasswordKey, password);
    }
    #endregion

    #region DeleteUsernameAndPassword
    public void DeleteUsernameAndPassword()
    {
        PlayerPrefs.DeleteKey(PlayerKeys.UsernameKey);
        PlayerPrefs.DeleteKey(PlayerKeys.PasswordKey);
    }
    #endregion

    #region AreUsernameAndPasswordSaved
    public bool AreUsernameAndPasswordSaved()
    {
        return PlayerPrefs.HasKey(PlayerKeys.UsernameKey) && PlayerPrefs.HasKey(PlayerKeys.PasswordKey);
    }
    #endregion
}
