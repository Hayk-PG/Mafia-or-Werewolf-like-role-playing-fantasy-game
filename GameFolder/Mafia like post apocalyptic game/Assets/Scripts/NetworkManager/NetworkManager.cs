using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public event Action OnLobbyJoined;
    public event Action OnRoomCreated;    
    public event Action OnRoomJoined;
    public event Action<string> OnCreateRoomError;

    List<RoomInfo> roomInfo = new List<RoomInfo>();


    void Start()
    {
        ConnectToPlayfab();
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

    #region ConnectToPlayfab
    void ConnectToPlayfab()
    {
        if (!PlayerBaseConditions.PlayfabManager.PlayfabIsLoggedIn.IsPlayfabLoggedIn())
        {
            if (PlayerBaseConditions.PlayerSavedData.AreUsernameAndPasswordSaved())
            {
                PlayerBaseConditions.PlayfabManager.PlayfabSignIn.OnPlayfabLogin(PlayerPrefs.GetString(PlayerKeys.UsernameKey), PlayerPrefs.GetString(PlayerKeys.PasswordKey));
            }
            else
            {
                PlayerBaseConditions.NetworkManagerComponents.NetworkUI.OnLoggedOut();
            }
        }
    }
    #endregion

    #region Connect
    public void ConnectToPhoton(string playfabId)
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.AuthValues = new AuthenticationValues();
            PhotonNetwork.AuthValues.UserId = playfabId;
        }

        #region ConnectionCheck
        ConnectionUI.instance.ConnectionCheck(5, ConnectionUI.Connected.IsConnected, null, ()=> 
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.AuthValues = new AuthenticationValues();
            PhotonNetwork.AuthValues.UserId = playfabId;
        });
        #endregion
    }
    #endregion

    #region OnConnectedToMaster
    public override void OnConnectedToMaster()
    {
        if (PlayerBaseConditions.PlayfabManager.PlayfabIsLoggedIn.IsPlayfabLoggedIn())
        {
            PhotonNetwork.JoinLobby();
        }
        else
        {
            PlayerBaseConditions.NetworkManagerComponents.NetworkUI.OnLoggedOut();
        }
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
