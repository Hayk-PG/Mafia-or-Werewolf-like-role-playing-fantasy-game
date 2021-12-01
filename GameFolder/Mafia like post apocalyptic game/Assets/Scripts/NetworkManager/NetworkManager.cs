﻿using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using System;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public enum ConnectionType {Auto, Manual}
    public ConnectionType connectionType;

    public event Action OnLobbyJoined;
    public event Action OnRoomCreated;    
    public event Action OnRoomJoined;
    public event Action<string> OnCreateRoomError;

    List<RoomInfo> roomInfo = new List<RoomInfo>();


    void Start()
    {
        PlayerBaseConditions.ConnectToPlayfab(ConnectionType.Auto);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        GetComponent<NetworkManagerComponents>().NetworkManagerCreatedRoomProperties.OnClickConfirmRoomButton += NetworkManagerCreatedRoomProperties_OnClickConfirmRoomButton;
        GetComponent<NetworkManagerComponents>().NetworkUIButtons.OnClickRoomButton += NetworkUIButtons_OnClickRoomButton;
    }
   
    public override void OnDisable()
    {
        base.OnDisable();
        GetComponent<NetworkManagerComponents>().NetworkManagerCreatedRoomProperties.OnClickConfirmRoomButton -= NetworkManagerCreatedRoomProperties_OnClickConfirmRoomButton;
        GetComponent<NetworkManagerComponents>().NetworkUIButtons.OnClickRoomButton -= NetworkUIButtons_OnClickRoomButton;
    }
         
    #region OnConnectedToMaster
    public override void OnConnectedToMaster()
    {
        if (connectionType == ConnectionType.Manual && !PhotonNetwork.InLobby) PlayerBaseConditions.JoinLobby();
        if (connectionType == ConnectionType.Auto && !PlayerBaseConditions.IsOptionsOpened()) Options.instance.OnPressedOptionButton();
    }
    #endregion

    #region OnJoinedLobby
    public override void OnJoinedLobby()
    {
        OnLobbyJoined?.Invoke();
    }
    #endregion

    #region NetworkManagerCreatedRoomProperties_OnClickConfirmRoomButton
    void NetworkManagerCreatedRoomProperties_OnClickConfirmRoomButton(string roomName, bool isPasswordSet, string pinNumber, int minRequiredCount)
    {
        RoomOptions options = new RoomOptions();

        options.CleanupCacheOnLeave = false;
        options.IsOpen = true;
        options.IsVisible = true;
        options.MaxPlayers = 20;
        options.EmptyRoomTtl = 60000;
        options.PlayerTtl = 60000;
        options.PublishUserId = true;

        options.CustomRoomPropertiesForLobby = new string[3] { RoomCustomProperties.IsPasswordSet, RoomCustomProperties.PinNumber, RoomCustomProperties.MinRequiredCount };
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable(3) { { RoomCustomProperties.IsPasswordSet, isPasswordSet }, { RoomCustomProperties.PinNumber, pinNumber }, { RoomCustomProperties.MinRequiredCount, minRequiredCount } };

        PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
    }
    #endregion

    #region NetworkUIButtons_OnClickRoomButton
    void NetworkUIButtons_OnClickRoomButton(IRoomButton roomButton)
    {
        if(roomButton.Pin == "")
        {
            PhotonNetwork.JoinRoom(roomButton.RoomName);
        }
        else
        {
            roomButton.EnablePasswordCanvasGroup();
        }        
    }
    #endregion

    #region OnCreatedRoom
    public override void OnCreatedRoom()
    {
        OnRoomCreated?.Invoke();
    }
    #endregion

    #region OnCreateRoomFailed
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        OnCreateRoomError?.Invoke(message);

        print(returnCode);
    }
    #endregion

    #region OnJoinedRoom
    public override void OnJoinedRoom()
    {
        OnRoomJoined?.Invoke();
    }
    #endregion

    #region OnJoinRoomFailed
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print(returnCode + "/" + message);

        if(returnCode == 32749)
        {
            FindObjectOfType<JoinRoomErrorTab>().OnError("Unable to join: The room is currently not available,please try again later!");
        }

        PlayerBaseConditions.UiSounds.PlaySoundFX(7);
    }
    #endregion

    #region OnRoomListUpdate
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        roomInfo = roomList;

        foreach (var room in roomInfo)
        {
            bool isRoomAlreadyCreated = NetworkManagerComponents.Instance.NetworkObjectsHolder.roomsContainer.transform.Find(room.Name) != null;

            if (!isRoomAlreadyCreated)
            {
                CreateRoom(room);
            }
            else
            {
                UpdateRoom(room);
            }

            DestroyRoomButton(room);
        }       
    }

    void CreateRoom(RoomInfo room)
    {
        if (room.CustomProperties[RoomCustomProperties.IsPasswordSet] != null && room.CustomProperties[RoomCustomProperties.PinNumber] != null)
        {
            IRoomButton button = Instantiate(NetworkManagerComponents.Instance.NetworkObjectsHolder.RoomButtonPrefab, NetworkManagerComponents.Instance.NetworkObjectsHolder.roomsContainer);
            button.UpdateRoomButton(room.Name, room.PlayerCount, room.MaxPlayers, room.IsOpen, (bool)room.CustomProperties[RoomCustomProperties.IsPasswordSet], (string)room.CustomProperties[RoomCustomProperties.PinNumber]);
        }
    }

    void UpdateRoom(RoomInfo room)
    {
        IRoomButton button = NetworkManagerComponents.Instance.NetworkObjectsHolder.roomsContainer.transform.Find(room.Name).GetComponent<IRoomButton>();
        button.UpdateRoomButton(room.PlayerCount, room.MaxPlayers, room.IsOpen);
    }

    void DestroyRoomButton(RoomInfo room)
    {
        if (room.RemovedFromList)
        {
            if (NetworkManagerComponents.Instance.NetworkObjectsHolder.roomsContainer.Find(room.Name) != null)
            {
                Destroy(NetworkManagerComponents.Instance.NetworkObjectsHolder.roomsContainer.Find(room.Name).gameObject);                
            }
        }
    }
    #endregion
}
