using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ProfileForMenuScene : MonoBehaviour
{
    void Start()
    {
        SubToEvents.SubscribeToEvents(PlayerBaseConditions.IsNetworkManagerComponentsNotNull, () =>
        {
            PlayerBaseConditions.NetworkManagerComponents.NetworkUIButtons.OnClickBadgeButton += NetworkUIButtons_OnClickBadgeButton;
        });
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
            UpdateNotificationTab();
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
        PlayerBaseConditions.PlayerProfile.TimePlayed = PlayerBaseConditions.LocalPlayer.CustomProperties.ContainsKey(PlayerKeys.StatisticKeys.TotalTimePlayed) ? PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.StatisticKeys.TotalTimePlayed].ToString() : "--";
        PlayerBaseConditions.PlayerProfile.RankNumber = PlayerBaseConditions.LocalPlayer.CustomProperties.ContainsKey(PlayerKeys.StatisticKeys.Rank) ? PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.StatisticKeys.Rank].ToString() : "--";
    }
    #endregion

    #region UpdateNotificationTab
    void UpdateNotificationTab()
    {
        PlayerBaseConditions.PlayfabManager.PlayfabFriends.ReceiveFriendRequest(PlayerBaseConditions.OwnPlayfabId,
            message =>
            {
                foreach (var item in message)
                {
                    if (PlayerBaseConditions.PlayerProfile.NotificationsContainer.transform.Find(item.Key) == null)
                    {
                        FriendRequestMessageScript friendRequestMessage = Instantiate(PlayerBaseConditions.PlayerProfile.FriendRequestMessagePrefab, PlayerBaseConditions.PlayerProfile.NotificationsContainer);
                        friendRequestMessage.Name = item.Key;
                        friendRequestMessage.Addressee = ConvertStrings.SubtractPlayfabIdFromFriendRequestKey(item.Key);
                        friendRequestMessage.Message = item.Value;
                    }
                }
            });
    }
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
}
