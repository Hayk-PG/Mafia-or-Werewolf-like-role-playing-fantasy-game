using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class NetworkCallbacks : MonoBehaviourPunCallbacks
{
    public event Action<Player> OnPlayerWelcome;
    public event Action<Player> OnPlayerJoinedGame;
    public event Action<Player> OnPlayerLeftGame;
    public event Action<Player> OnMasterSwitched;

    void Start()
    {
        SubToEvents.SubscribeToEvents(()=> OnPlayerWelcome?.Invoke(PhotonNetwork.LocalPlayer));       
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnPlayerJoinedGame?.Invoke(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnPlayerLeftGame?.Invoke(otherPlayer);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        OnMasterSwitched?.Invoke(newMasterClient);
    }







}
