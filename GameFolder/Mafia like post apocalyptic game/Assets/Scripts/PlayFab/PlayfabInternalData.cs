using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ServerModels;
using System;

public class PlayfabInternalData : MonoBehaviour
{
    #region GetPlayerUserInternalData
    public void GetPlayerUserInternalData(string playfabId, Action<Dictionary<string, UserDataRecord>> InternalDataDict)
    {
        GetUserDataRequest getData = new GetUserDataRequest();
        getData.PlayFabId = playfabId;

        PlayFabServerAPI.GetUserInternalData(getData,
            get =>
            {
                InternalDataDict(get.Data);
            },
            error =>
            {
                print(error.ErrorMessage);
            });
    }
    #endregion

    #region UpdatePlayerInternalData
    public void UpdatePlayerInternalData(string playfabId, string key, string value)
    {
        GetPlayerUserInternalData(playfabId, 
            data => 
            {
                RequestInternalDataUpdate(playfabId, key, value, PlayerKeys.InternalData.MessageEndPoint + data.Keys.Count + UnityEngine.Random.Range(0,1000000));
            });
    }

    void RequestInternalDataUpdate(string playfabId, string key, string value, string count)
    {
        UpdateUserInternalDataRequest requestUserInternalData = new UpdateUserInternalDataRequest();
        requestUserInternalData.PlayFabId = playfabId;
        requestUserInternalData.Data = new Dictionary<string, string>()
        {
            {key + count, value }
        };

        PlayFabServerAPI.UpdateUserInternalData(requestUserInternalData,
            update =>
            {
                
            },
            error =>
            {
                print(error.ErrorMessage);
            });
    }
    #endregion
    
    #region DeleteData
    public void DeleteData(string playfabID, string key, string value)
    {
        GetPlayerUserInternalData(playfabID, InternalDataDict =>
        {
            foreach (var data in InternalDataDict)
            {
                if (data.Key == key && data.Value.Value == value)
                {
                    RemoveKeyFromInternalData(playfabID, data.Key);
                }
            }
        });
    }

    public void RemoveKeyFromInternalData(string playfabID, string key)
    {
        UpdateUserInternalDataRequest updateInternalData = new UpdateUserInternalDataRequest();
        updateInternalData.PlayFabId = playfabID;
        updateInternalData.KeysToRemove = new List<string>();
        updateInternalData.KeysToRemove.Add(key);

        if (PlayerBaseConditions.PlayfabManager.PlayfabIsLoggedIn.IsPlayfabLoggedIn())
        {
            PlayFabServerAPI.UpdateUserInternalData(updateInternalData,
                deleted =>
                {
                    print("DELETED");
                },
                error =>
                {
                    print(error.ErrorMessage);
                });
        }
    }
    #endregion
}
