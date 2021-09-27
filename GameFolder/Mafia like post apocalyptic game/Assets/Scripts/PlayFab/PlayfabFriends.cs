using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ServerModels;
using System;
using Photon.Pun;

public class PlayfabFriends : MonoBehaviourPunCallbacks
{ 
    #region SendFriendRequest
    /// <summary>
    /// Adding a friend request message to the desired player internal data, with the request message and playfabID.
    /// </summary>
    /// <param name="playfabID"></param>
    public void SendFriendRequest(string playfabID)
    {
        UpdateUserInternalDataRequest updateInternalData = new UpdateUserInternalDataRequest();
        updateInternalData.PlayFabId = playfabID;
        updateInternalData.Data = new Dictionary<string, string>();
        updateInternalData.Data.Add(
            PlayerKeys.FriendRequest.FR + PlayerBaseConditions.OwnPlayfabId,
            PlayerBaseConditions.LocalPlayer.NickName + " sent you a friend request." + PlayerKeys.FriendRequest.ID + PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.UserID]
            );

        if (PlayerBaseConditions.PlayfabManager.PlayfabIsLoggedIn.IsPlayfabLoggedIn())
        {
            PlayFabServerAPI.UpdateUserInternalData(updateInternalData,
           sent =>
           {
               print("SENT");
           },
           error =>
           {
               print(error.ErrorMessage);
           });
        }
    }
    #endregion

    #region ReceiveFriendRequest
    /// <summary>
    /// If someone sent a friend request to a player, it appears in the internal data of that player. Player will get a notification to accept or deny the request.
    /// </summary>
    /// <param name="playfabID"></param>
    public void ReceiveFriendRequest(string playfabID, Action<Dictionary<string, string>> GetMessage)
    {
        GetUserDataRequest getUserData = new GetUserDataRequest();
        getUserData.PlayFabId = playfabID;

        Dictionary<string, string> messageDinctionary = new Dictionary<string, string>();

        if (PlayerBaseConditions.PlayfabManager.PlayfabIsLoggedIn.IsPlayfabLoggedIn())
        {
            PlayFabServerAPI.GetUserInternalData(getUserData,
            get =>
            {
                foreach (var data in get.Data)
                {
                    if (ConvertStrings.SubtractFriendRequestKey(data.Key) == PlayerKeys.FriendRequest.FR)
                    {
                        #region Getting data value
                        string subtract = PlayerKeys.FriendRequest.ID;
                        int messageStart = 0;
                        int messageLength = data.Value.Value.IndexOf(subtract);
                        int idStart = data.Value.Value.IndexOf(subtract) + PlayerKeys.FriendRequest.ID.Length;
                        int idLength = data.Value.Value.Length - idStart;
                        string message = data.Value.Value.Substring(messageStart, messageLength);
                        string possibleFriendID = data.Value.Value.Substring(idStart, idLength);
                        string myID = PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.UserID].ToString();
                        #endregion

                        messageDinctionary.Add(data.Key, message);
                    }
                }

                GetMessage(messageDinctionary);
            },
            error =>
            {
                print(error.ErrorMessage);
            });
        }
    }
    #endregion

    #region AddFriend
    /// <summary>
    /// Adding a friend
    /// </summary>
    /// <param name="playfabID"></param>
    /// <param name="friendPlayfabID"></param>
    public void AddFriend(string playfabID, string friendPlayfabID)
    {
        AddFriendRequest friend = new AddFriendRequest();
        friend.PlayFabId = playfabID;
        friend.FriendPlayFabId = friendPlayfabID;

        PlayFabServerAPI.AddFriend(friend,
            added =>
            {
                print("ADDED");
            },
            error =>
            {
                print(error.ErrorMessage);
            });
    }
    #endregion

    #region UnFriend
    public void UnFriend(string playfabID, string friendPlayfabID)
    {
        RemoveFriendRequest removeFriend = new RemoveFriendRequest();
        removeFriend.PlayFabId = playfabID;
        removeFriend.FriendPlayFabId = friendPlayfabID;


        PlayFabServerAPI.RemoveFriend(removeFriend, 
            removed => 
            {
                print("Removed");
            }, 
            error => 
            {
                print(error.ErrorMessage);
            });
    }
    #endregion

    #region DeleteData
    /// <summary>
    /// Deleting the friend request from the internal data
    /// </summary>
    /// <param name="playfabID"></param>
    /// <param name="key"></param>
    public void DeleteData(string playfabID, string key)
    {
        UpdateUserInternalDataRequest updateInternalData = new UpdateUserInternalDataRequest();
        updateInternalData.PlayFabId = playfabID;
        updateInternalData.KeysToRemove = new List<string>();
        updateInternalData.KeysToRemove.Add(key);

        if (PlayerBaseConditions.PlayfabManager.PlayfabIsLoggedIn.IsPlayfabLoggedIn())
        {
            PlayFabServerAPI.UpdateUserInternalData(updateInternalData, 
                deleted => 
                {
                    print("DELETED");
                }, 
                error => 
                {
                    print(error.ErrorMessage);
                });
        }
    }
    #endregion

    #region SearchPlayerInFriendsList
    public void SearchPlayerInFriendsList(string playfabId, string possibleFriendPlayfabId, Action<string> Friend)
    {
        GetFriendsListRequest friendsList = new GetFriendsListRequest();
        friendsList.PlayFabId = playfabId;

        PlayFabServerAPI.GetFriendsList(friendsList, 
            get => 
            {
                if(get.Friends.Count == 0)
                {
                    Friend(null);
                }
                else
                {
                    foreach (var friend in get.Friends)
                    {
                        if (friend.FriendPlayFabId == possibleFriendPlayfabId)
                        {
                            Friend(friend.FriendPlayFabId);
                            break;
                        }
                        else
                        {
                            Friend(null);
                        }
                    }
                }               
            }, 
            error => 
            {
                print(error.ErrorMessage);
            });
    }
    #endregion

    #region UpdateFriendsList
    public void UpdateFriendsList(string playfabId, Action<Dictionary<string,PlayerProfileModel>> Friends)
    {
        GetFriendsListRequest friendsList = new GetFriendsListRequest();
        friendsList.ProfileConstraints = new PlayerProfileViewConstraints();
        friendsList.PlayFabId = playfabId;

        Dictionary<string, PlayerProfileModel> friendsInfoDictionary = new Dictionary<string, PlayerProfileModel>();

        PlayFabServerAPI.GetFriendsList(friendsList,
            get =>
            {
                foreach (var friend in get.Friends)
                {
                    friendsInfoDictionary.Add(friend.TitleDisplayName, friend.Profile);
                }

                Friends(friendsInfoDictionary);
            },
            error =>
            {
                print(error.ErrorMessage);
            });
    }
    #endregion

    #region PhotonNetworkFriends
    public void PhotonNetworkFriends(string[] friends)
    {
        if (friends.Length > 0)
            PhotonNetwork.FindFriends(friends);
    }

    public override void OnFriendListUpdate(List<Photon.Realtime.FriendInfo> friendList)
    {
        foreach (Photon.Realtime.FriendInfo friend in friendList)
        {
            for (int i = 0; i < PlayerBaseConditions.PlayerProfile.FriendsListContainer.childCount; i++)
            {
                if (PlayerBaseConditions.PlayerProfile.FriendsListContainer.GetChild(i).GetComponent<FriendButtonScript>()?.Name == friend.UserId)
                {
                    PlayerBaseConditions.PlayerProfile.FriendsListContainer.GetChild(i).GetComponent<FriendButtonScript>().StatusImageColor = friend.IsOnline ? PlayerBaseConditions.PlayerProfile.clickedTabButtonColor : new Color32(255, 0, 64, 255);
                    PlayerBaseConditions.PlayerProfile.FriendsListContainer.GetChild(i).GetComponent<FriendButtonScript>()._Room.IsInRoom = friend.IsInRoom;
                    PlayerBaseConditions.PlayerProfile.FriendsListContainer.GetChild(i).GetComponent<FriendButtonScript>()._Room.RoomName = friend.Room;
                }
            }
        }
    }

    #endregion
}
