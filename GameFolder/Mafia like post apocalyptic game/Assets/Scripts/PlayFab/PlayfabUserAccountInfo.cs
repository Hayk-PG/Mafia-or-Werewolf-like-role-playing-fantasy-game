using UnityEngine;
using PlayFab;
using PlayFab.ServerModels;
using System;

public class PlayfabUserAccountInfo : MonoBehaviour
{
    public AccountInfo _AccountInfo;

    public class AccountInfo
    {
        public string EntityId { get; set; }
        public string EntityType { get; set; }

        public AccountInfo()
        {

        }

        public AccountInfo(string entityId, string entityType)
        {
            EntityId = entityId;
            EntityType = entityType;
        }
    }

    public void GetUserAccountInfo(string playfabId, Action<AccountInfo> GetAccountInfo)
    {
        GetUserAccountInfoRequest getUserAccountInfo = new GetUserAccountInfoRequest();
        getUserAccountInfo.PlayFabId = playfabId;

        PlayFabServerAPI.GetUserAccountInfo(getUserAccountInfo, 
            get => 
            {
                GetAccountInfo(new AccountInfo(get.UserInfo.TitleInfo.TitlePlayerAccount.Id, get.UserInfo.TitleInfo.TitlePlayerAccount.Type));
            }, 
            error => 
            {
                print(error.ErrorMessage);
            });
    }
}
