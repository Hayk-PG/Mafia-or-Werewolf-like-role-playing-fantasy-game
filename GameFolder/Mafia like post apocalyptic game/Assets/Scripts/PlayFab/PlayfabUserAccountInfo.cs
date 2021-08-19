using UnityEngine;
using PlayFab;
using PlayFab.ServerModels;
using System;

public class PlayfabUserAccountInfo : MonoBehaviour
{   
    public void GetUserAccountInfo(string playfabId, Action<UserAccountInfo> GetAccountInfo)
    {
        GetUserAccountInfoRequest getUserAccountInfo = new GetUserAccountInfoRequest();
        getUserAccountInfo.PlayFabId = playfabId;

        PlayFabServerAPI.GetUserAccountInfo(getUserAccountInfo, 
            get => 
            {
                GetAccountInfo(get.UserInfo);
            }, 
            error => 
            {
                print(error.ErrorMessage);
            });
    }
}
