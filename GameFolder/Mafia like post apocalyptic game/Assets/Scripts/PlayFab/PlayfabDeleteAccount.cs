using UnityEngine;
using PlayFab;
using PlayFab.ServerModels;
using System;

public class PlayfabDeleteAccount : MonoBehaviour
{   
    public void DeleteAccount(string playfabId, Action<bool> Deleted)
    {
        DeletePlayerRequest deletePlayer = new DeletePlayerRequest();
        deletePlayer.PlayFabId = playfabId;

        PlayFabServerAPI.DeletePlayer(deletePlayer, 
            delete => 
            {
                print("Player was successfuly deleted");
                Deleted?.Invoke(true);
            }, 
            error => 
            {
                print(error.ErrorMessage);
                Deleted?.Invoke(false);
            });
    }
}
