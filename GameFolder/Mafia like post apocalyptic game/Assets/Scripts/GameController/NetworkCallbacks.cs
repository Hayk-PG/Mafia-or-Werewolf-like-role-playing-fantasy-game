using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

public class NetworkCallbacks : MonoBehaviourPunCallbacks
{
    public delegate void Callback();
    public Callback UpdateOnlinePlayersListCallback;


    void Start()
    {
        UpdateOnlinePlayersListCallback?.Invoke();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateOnlinePlayersListCallback?.Invoke();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateOnlinePlayersListCallback?.Invoke();
    }

   
}
