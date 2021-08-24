using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

public class NetworkCallbacks : MonoBehaviourPunCallbacks
{
    public event Action<Player> OnPlayerWelcome;
    public event Action<Player> OnPlayerJoinedGame;
    public event Action<Player> OnPlayerLeftGame;
    public event Action<Player> OnMasterSwitched;

    string RoomName { get; set; }

    void Start()
    {
        SubToEvents.SubscribeToEvents(()=> OnPlayerWelcome?.Invoke(PhotonNetwork.LocalPlayer));

        RoomName = PhotonNetwork.InRoom ? PhotonNetwork.CurrentRoom.Name : null;
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

    public override void OnDisconnected(DisconnectCause cause)
    {
        StartCoroutine(Test());
    }

    public override void OnJoinedRoom()
    {
        print("Joined");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print(message);
    }

    IEnumerator Test()
    {
        while (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Reconnect();
            yield return new WaitForSeconds(3);
        }

        yield return new WaitForSeconds(3);

        while (!PhotonNetwork.InRoom)
        {
            if (RoomName != null) PhotonNetwork.RejoinRoom(RoomName);
            yield return new WaitForSeconds(3);
        }        
    }



}
