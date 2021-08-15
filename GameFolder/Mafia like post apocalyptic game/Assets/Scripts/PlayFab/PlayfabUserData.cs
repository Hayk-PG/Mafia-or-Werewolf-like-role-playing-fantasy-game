using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayfabUserData : MonoBehaviour
{
    public Data _Data;
    public class Data
    {
        internal string playfabId;
        internal string gender;
    }

    #region UpdateUserData
    public void UpdateUserData(Action<UpdateUserDataRequest> UpdateUserData)
    {
        UpdateUserDataRequest dataRequest = new UpdateUserDataRequest();

        UpdateUserData(dataRequest);

        PlayFabClientAPI.UpdateUserData(dataRequest,

            result =>
            {
                
            },
            error =>
            {

            });
    }
    #endregion

    #region GetUserData
    public void GetUserData(string PlayFabId, Action<Data> GetUserData)
    {
        GetUserDataRequest dataRequest = new GetUserDataRequest();
        dataRequest.PlayFabId = PlayFabId;

        PlayFabClientAPI.GetUserData(dataRequest,

            result =>
            {              
                GetUserData(new Data
                {
                    gender = result.Data[PlayerKeys.GenderKey].Value
                });
            },
            error =>
            {

            });
    }
    #endregion


}
