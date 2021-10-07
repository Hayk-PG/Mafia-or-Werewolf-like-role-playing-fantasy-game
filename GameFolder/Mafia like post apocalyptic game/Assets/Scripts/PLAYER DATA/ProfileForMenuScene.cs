using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProfileForMenuScene : MonoBehaviour
{
    delegate bool Filter(string dataKey);


    void Start()
    {
        SubToEvents.SubscribeToEvents(PlayerBaseConditions.IsNetworkManagerComponentsNotNull, () =>
        {
            PlayerBaseConditions.NetworkManagerComponents.NetworkUIButtons.OnClickBadgeButton += NetworkUIButtons_OnClickBadgeButton;
        });

        StartCoroutine(NotificationCoroutine());
    }
    
    void OnDisable()
    {
        if (PlayerBaseConditions.IsNetworkManagerComponentsNotNull)
        {
            PlayerBaseConditions.NetworkManagerComponents.NetworkUIButtons.OnClickBadgeButton -= NetworkUIButtons_OnClickBadgeButton;
        }
    }

    #region NetworkUIButtons_OnClickBadgeButton
    void NetworkUIButtons_OnClickBadgeButton()
    {
        if (PlayerBaseConditions.IsNetworkManagerComponentsNotNull && Photon.Pun.PhotonNetwork.IsConnected && PlayerBaseConditions.PlayfabManager.PlayfabIsLoggedIn.IsPlayfabLoggedIn())
        {
            MyCanvasGroups.CanvasGroupActivity(PlayerBaseConditions.PlayerProfile.CanvasGroups[0], true);

            MenuScene();
            UpdatePlayerProfile();
            UpdatePlayerStats();                      
            //UpdateNotificationTab();
            UpdateFriendsList();

        }
    }
    #endregion

    #region MenuScene
    void MenuScene()
    {
        PlayerBaseConditions.PlayerProfile.SendFriendRequestButton.gameObject.SetActive(false);
        PlayerBaseConditions.PlayerProfile.SetDefaultPage();
    }
    #endregion

    #region UpdatePlayerProfile
    void UpdatePlayerProfile()
    {
        PlayerBaseConditions.PlayerProfile.ShowPlayerProfilePic(true, 0);
        PlayerBaseConditions.PlayerProfile.Name = PlayerBaseConditions.LocalPlayer.NickName;
        PlayerBaseConditions.PlayerProfile.FlagImage = System.Array.Find(Flags.instance.FlagSprites, flag => flag.name.Substring(5, flag.name.Length - 5) == ConvertStrings.GetCountryCode((string)PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.LocationKey]));
        PlayerBaseConditions.PlayerProfile.FirstLoginDate = PlayerBaseConditions.LocalPlayer.CustomProperties.ContainsKey(PlayerKeys.RegDateKey) ? PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.RegDateKey].ToString() : "--";
    }
    #endregion

    #region UpdatePlayerStats
    void UpdatePlayerStats()
    {
        PlayerBaseConditions.PlayerProfile.ShowPlayerStats(PlayerBaseConditions.OwnPlayfabId);
    }
    #endregion

    #region UpdateNotificationTab OUTDATED
    void UpdateNotificationTab()
    {
        for (int i = 0; i < PlayerBaseConditions.PlayerProfile.NotificationsContainer.childCount; i++)
        {
            Destroy(PlayerBaseConditions.PlayerProfile.NotificationsContainer.GetChild(i).gameObject);
        }

        PlayerBaseConditions.PlayfabManager.PlayfabInternalData.GetPlayerUserInternalData(PlayerBaseConditions.OwnPlayfabId,
            InternalDataDict =>
            {
                foreach (var data in InternalDataDict)
                {
                    FriendRequestNotification(data.Key, data.Value.Value, ConvertStrings.SubtractFriendRequestKey(data.Key) == PlayerKeys.FriendRequest.FR);
                    MessageNotification(data.Key, data.Value.Value, ConvertStrings.SubtractMessageDataFromPlayerInternalData(data.Key, true) == PlayerKeys.InternalData.MessageKey);                   
                }
            });
    }

    #region FriendRequestNotification
    void FriendRequestNotification(string key, string value, bool isFriendRequest)
    {
        if (isFriendRequest)
        {
            if(PlayerBaseConditions.PlayerProfile.NotificationsContainer.Find(key) == null)
            {
                FriendRequestMessageScript friendRequestMessage = Instantiate(PlayerBaseConditions.PlayerProfile.FriendRequestMessagePrefab, PlayerBaseConditions.PlayerProfile.NotificationsContainer);
                friendRequestMessage.Name = key;
                friendRequestMessage.Addressee = ConvertStrings.SubtractPlayfabIdFromFriendRequestKey(key);
                friendRequestMessage.Message = value;
            } 
        }
    }
    #endregion

    #region MessageNotification
    void MessageNotification(string key, string value, bool isMessage)
    {
        if (isMessage)
        {
            MessageScript message = Instantiate(PlayerBaseConditions.PlayerProfile.MessagePrefab, PlayerBaseConditions.PlayerProfile.NotificationsContainer);
            message.gameObject.name = key;
            message.InternalDataKey = key;

            message.MessageSentFrom = "Message from " + ConvertStrings.SubtractMessageDataFromPlayerInternalData(key, false) + ".";
            message.InternalDataValue = value;           
        }
    }
    #endregion
    #endregion

    #region UpdateFriendsList
    void UpdateFriendsList()
    {
        PlayerBaseConditions.PlayfabManager.PlayfabFriends.UpdateFriendsList(PlayerBaseConditions.OwnPlayfabId,
            Friends =>
            {
                string[] friendsArray = new string[Friends.Count];
                int index = 0;

                foreach (var friend in Friends)
                {
                    if(PlayerBaseConditions.PlayerProfile.FriendsListContainer.Find(friend.Value.PlayerId) == null)
                    {
                        FriendButtonScript friendButton = Instantiate(PlayerBaseConditions.PlayerProfile.FriendButtonPrefab, PlayerBaseConditions.PlayerProfile.FriendsListContainer);
                        friendButton.Name = friend.Value.PlayerId;
                        friendButton.FriendName = friend.Key;                        
                    }

                    friendsArray[index] = friend.Value.PlayerId;
                    index++;                  
                }

                PlayerBaseConditions.PlayfabManager.PlayfabFriends.PhotonNetworkFriends(friendsArray);
            });
    }
    #endregion

    #region NotificationCoroutine
    IEnumerator NotificationCoroutine()
    {       
        yield return new WaitUntil(() => PlayerBaseConditions.PlayfabManager.PlayfabIsLoggedIn.IsPlayfabLoggedIn() && PlayerBaseConditions.LocalPlayer.UserId != null);

        bool isSoundPlayed = false;

        LoopInternalDataDict(IsNotification,
                UpdatedNotification =>
                {
                    if (PlayerBaseConditions.PlayerProfile.NotificationsContainer.Find(UpdatedNotification[0]) == null)
                    {
                        if (!isSoundPlayed) { PlayerBaseConditions.UiSounds.PlaySoundFX(8); isSoundPlayed = true; }                       
                        MessageNotification(UpdatedNotification[0], UpdatedNotification[1], ConvertStrings.SubtractMessageDataFromPlayerInternalData(UpdatedNotification[0], true) == PlayerKeys.InternalData.MessageKey);
                        FriendRequestNotification(UpdatedNotification[0], UpdatedNotification[1], ConvertStrings.SubtractFriendRequestKey(UpdatedNotification[0]) == PlayerKeys.FriendRequest.FR);
                        MyCanvasGroups.CanvasGroupActivity(PlayerBaseConditions.PlayerProfile.NotifcationsCountCanvasgroup, true);
                        PlayerBaseConditions.PlayerProfile.NotificationsCount = PlayerBaseConditions.PlayerProfile.NotificationsContainer.childCount;
                    } 
                },null);


        yield return new WaitForSeconds(1);       

        while (PlayerBaseConditions.PlayfabManager.PlayfabIsLoggedIn.IsPlayfabLoggedIn())
        {
            LoopInternalDataDict(IsNotification,
                UpdatedNotification =>
                {
                    if (PlayerBaseConditions.PlayerProfile.NotificationsContainer.Find(UpdatedNotification[0]) == null)
                    {
                        PlayerBaseConditions.UiSounds.PlaySoundFX(8);
                        MessageNotification(UpdatedNotification[0], UpdatedNotification[1], ConvertStrings.SubtractMessageDataFromPlayerInternalData(UpdatedNotification[0], true) == PlayerKeys.InternalData.MessageKey);
                        FriendRequestNotification(UpdatedNotification[0], UpdatedNotification[1], ConvertStrings.SubtractFriendRequestKey(UpdatedNotification[0]) == PlayerKeys.FriendRequest.FR);
                        MyCanvasGroups.CanvasGroupActivity(PlayerBaseConditions.PlayerProfile.NotifcationsCountCanvasgroup, true);
                        PlayerBaseConditions.PlayerProfile.NotificationsCount = PlayerBaseConditions.PlayerProfile.NotificationsContainer.childCount;
                    }
                }, 
                delegate 
                {                   
                    PlayerBaseConditions.PlayerProfile.NotificationsCount = PlayerBaseConditions.PlayerProfile.NotificationsContainer.childCount;
                    if (PlayerBaseConditions.PlayerProfile.NotificationsCount < 1) MyCanvasGroups.CanvasGroupActivity(PlayerBaseConditions.PlayerProfile.NotifcationsCountCanvasgroup, false);
                });

            yield return new WaitForSeconds(3);
        }
    }    
    #endregion

    #region LoopInternalDataDict
    void LoopInternalDataDict(Filter filter, Action<string[]> DoInsideTheLoop, Action DoOutsideTheLoop)
    {
        PlayerBaseConditions.PlayfabManager.PlayfabInternalData.GetPlayerUserInternalData(PlayerBaseConditions.OwnPlayfabId, InternalDataDict => 
        {
            foreach (var data in InternalDataDict)
            {             
                if (filter(data.Key) == true)
                {
                    DoInsideTheLoop.Invoke(new string[] { data.Key, data.Value.Value});
                }
            }

            DoOutsideTheLoop?.Invoke();
        });
    }

    bool IsNotification(string dataKey)
    {
        return ConvertStrings.SubtractFriendRequestKey(dataKey) == PlayerKeys.FriendRequest.FR  
            || ConvertStrings.SubtractMessageDataFromPlayerInternalData(dataKey, true) == PlayerKeys.InternalData.MessageKey ? true : false;
    }
    #endregion
}
