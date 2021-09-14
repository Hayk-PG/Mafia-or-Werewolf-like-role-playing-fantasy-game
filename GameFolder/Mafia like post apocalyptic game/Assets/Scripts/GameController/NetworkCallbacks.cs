using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

public class NetworkCallbacks : MonoBehaviourPunCallbacks
{
    public delegate void Callback();
    public delegate void ChatMessageCallback(string playerName, bool hasNewPlayerJoinded);
    public Callback UpdateOnlinePlayersListCallback;
    public ChatMessageCallback UpdateChatMessage;


    void Start()
    {
        UpdateOnlinePlayersListCallback?.Invoke();
        UpdateChatMessage?.Invoke(null, false);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateOnlinePlayersListCallback?.Invoke();
        UpdateChatMessage?.Invoke(newPlayer.NickName, true);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateOnlinePlayersListCallback?.Invoke();
        UpdateChatMessage?.Invoke(otherPlayer.NickName, false);
    }

   
}
