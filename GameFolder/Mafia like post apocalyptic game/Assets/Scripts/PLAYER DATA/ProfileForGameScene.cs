using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ProfileForGameScene : MonoBehaviourPun
{
    GameManagerTimer _GameManagerTimer { get; set; }
    Profile _Profile { get; set; }
    bool IsResetPhase { get; set;}
    

    void Awake()
    {
        _GameManagerTimer = FindObjectOfType<GameManagerTimer>();
        _Profile = FindObjectOfType<Profile>();
    }

    void OnEnable()
    {
        _GameManagerTimer.IsResetPhaseActive += CanViewTheProfile;
    }

    void OnDisable()
    {
        _GameManagerTimer.IsResetPhaseActive -= CanViewTheProfile;
    }

    void Update()
    {
        if (!IsResetPhase)
        {
            MyCanvasGroups.CanvasGroupActivity(PlayerBaseConditions.PlayerProfile.CanvasGroups[0], false);
        }
    }

    void CanViewTheProfile(bool isResetPhase)
    {       
        this.IsResetPhase = isResetPhase;
    }

    #region OnClickRoleButton
    public void OnClickRoleButton(bool canPlayerViewTheProfile, int actorNumber, string playfabId)
    {
        if (canPlayerViewTheProfile)
        {
            MyCanvasGroups.CanvasGroupActivity(PlayerBaseConditions.PlayerProfile.CanvasGroups[0], true);

            GameScene();
            //EnableFriendsTab(actorNumber);
            //UpdateFriendsList();
            UpdatePlayerProfile(actorNumber);
            UpdatePlaterStats(playfabId);
            CheckPlayerVotes(actorNumber);
            IfPlayerIsFriend(playfabId, actorNumber);
            IfFriendRequestAlreadySent(playfabId, actorNumber);
            ShowPlayerProfilePicture(actorNumber);

            if (_Profile._DatasDownloadCheck.Coroutine != null)
            {
                StopCoroutine(_Profile._DatasDownloadCheck.Coroutine);
            }

            _Profile._DatasDownloadCheck.Coroutine = _Profile.CheckIfAllDatasDownloadCompleted(actorNumber);
            StartCoroutine(_Profile._DatasDownloadCheck.Coroutine);
        }
    }
    #endregion

    #region GameScene
    void GameScene()
    {
        PlayerBaseConditions.PlayerProfile.SetDefaultPage();

        for (int i = 0; i < PlayerBaseConditions.PlayerProfile.TabButtons.Length; i++)
        {
            if (i < 2)
            {
                PlayerBaseConditions.PlayerProfile.TabButtons[i].gameObject.SetActive(true);
            }
            else
            {
                PlayerBaseConditions.PlayerProfile.TabButtons[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region EnableFriendsTab
    void EnableFriendsTab(int actorNumber)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            if (PhotonNetwork.LocalPlayer.TagObject != null)
            {
                GameObject localPlayerObj = PhotonNetwork.LocalPlayer.TagObject as GameObject;

                if (localPlayerObj.GetComponent<PhotonView>().IsMine && localPlayerObj.GetComponent<PhotonView>().AmOwner)
                {
                    _Profile.TabButtons[3].gameObject.SetActive(true);
                }
            }
        }
        else
        {
            _Profile.TabButtons[3].gameObject.SetActive(false);
        }     
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
                    if (PlayerBaseConditions.PlayerProfile.FriendsListContainer.Find(friend.Value.PlayerId) == null)
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

    #region UpdatePlayerProfile
    void UpdatePlayerProfile(int actorNumber)
    {        
        PlayerBaseConditions.PlayerProfile.Name = PlayerBaseConditions.GetRoleButton(actorNumber)._OwnerInfo.OwnerName;
        PlayerBaseConditions.PlayerProfile.FlagImage = System.Array.Find(Flags.instance.FlagSprites, flag => flag.name.Substring(5, flag.name.Length - 5) == ConvertStrings.GetCountryCode(PlayerBaseConditions.Player(actorNumber).CustomProperties[PlayerKeys.LocationKey].ToString()));
        PlayerBaseConditions.PlayerProfile.FirstLoginDate = PlayerBaseConditions.Player(actorNumber).CustomProperties[PlayerKeys.RegDateKey].ToString();       
    }
    #endregion

    #region UpdatePlaterStats
    void UpdatePlaterStats(string playfabId)
    {
        PlayerBaseConditions.PlayerProfile.ShowPlayerStats(playfabId);
    }
    #endregion

    #region CheckPlayerVotes
    void CheckPlayerVotes(int actorNumber)
    {
        #region Reset votedNamesContainer 
        if (PlayerBaseConditions.PlayerProfile.PlayerVotesTabContainer.childCount > 0)
        {
            for (int i = 0; i < PlayerBaseConditions.PlayerProfile.PlayerVotesTabContainer.childCount; i++)
            {
                Destroy(PlayerBaseConditions.PlayerProfile.PlayerVotesTabContainer.GetChild(i).gameObject);
            }
        }
        #endregion

        if (FindObjectOfType<GameManagerPlayerVotesController>()._Votes.AgainstWhomPlayerVoted.ContainsKey(actorNumber))
        {
            for (int i = 0; i < FindObjectOfType<GameManagerPlayerVotesController>()._Votes.AgainstWhomPlayerVoted[actorNumber].Length; i++)
            {
                PlayerDisplayedVoteObj playerVote = Instantiate(PlayerBaseConditions.PlayerProfile.PlayerVotesPrefab, PlayerBaseConditions.PlayerProfile.PlayerVotesTabContainer);
                playerVote.Name = FindObjectOfType<GameManagerPlayerVotesController>()._Votes.AgainstWhomPlayerVoted[actorNumber][i];
            }
        }
    }
    #endregion

    #region IfPlayerIsFriend
    void IfPlayerIsFriend(string playfabId, int actorNumber)
    {
        PlayerBaseConditions.PlayerProfile.SendFriendRequestButton.gameObject.SetActive(false);

        if (PlayerBaseConditions.OwnPlayfabId != playfabId && PlayerBaseConditions.LocalPlayer.ActorNumber != actorNumber)
        {
            PlayerBaseConditions.PlayfabManager.PlayfabFriends.SearchPlayerInFriendsList(PlayerBaseConditions.OwnPlayfabId, playfabId,
                friend =>
                {
                    if (friend == null)
                    {
                        PlayerBaseConditions.PlayerProfile.SendFriendRequestButton.gameObject.SetActive(true);
                        PlayerBaseConditions.PlayerProfile.SendMessageButton.gameObject.SetActive(false);
                        PlayerBaseConditions.PlayerProfile.DeleteFriendButton.gameObject.SetActive(false);
                        SetSendFriendRequestButtonName(playfabId);
                    }
                    else
                    {
                        PlayerBaseConditions.PlayerProfile.SendFriendRequestButton.gameObject.SetActive(false);
                        PlayerBaseConditions.PlayerProfile.SendMessageButton.gameObject.SetActive(true);
                        PlayerBaseConditions.PlayerProfile.DeleteFriendButton.gameObject.SetActive(true);
                        PlayerBaseConditions.PlayerProfile.FriendRequestAlreadySentIcon.gameObject.SetActive(false);
                        SetMessageButtonName(playfabId);
                        SetDeleteFriendButtonName(playfabId);
                    }
                });
        }
        else
        {
            PlayerBaseConditions.PlayerProfile.SendFriendRequestButton.gameObject.SetActive(false);
            PlayerBaseConditions.PlayerProfile.SendMessageButton.gameObject.SetActive(false);
            PlayerBaseConditions.PlayerProfile.DeleteFriendButton.gameObject.SetActive(false);
        }

        PlayerBaseConditions.PlayerProfile.ProfileDatasCompleteCheck(Profile.DatasDownloadCheck.DatasDownloadStatus.FriendCheckingDownloaded);
    }
    #endregion

    #region IfFriendRequestAlreadySent
    void IfFriendRequestAlreadySent(string playfabId, int actorNumber)
    {
        PlayerBaseConditions.PlayfabManager.PlayfabInternalData.GetPlayerUserInternalData(playfabId, InternalDataDict => 
        {
            if(InternalDataDict.ContainsKey(PlayerKeys.FriendRequest.FR + PlayerBaseConditions.OwnPlayfabId))
            {
                PlayerBaseConditions.PlayerProfile.OnPlayerRequestAlreadySent();
            }
            else
            {
                IfPlayerIsFriend(playfabId, actorNumber);
            }

            PlayerBaseConditions.PlayerProfile.ProfileDatasCompleteCheck(Profile.DatasDownloadCheck.DatasDownloadStatus.FriendRequestDataDownloaded);
        });
    }
    #endregion

    #region SetSendFriendRequestButtonName
    void SetSendFriendRequestButtonName(string playfabId)
    {
        PlayerBaseConditions.PlayerProfile.SendFriendRequestButton.name = playfabId;
    }
    #endregion

    #region SetMessageButtonName
    void SetMessageButtonName(string playfabId)
    {
        PlayerBaseConditions.PlayerProfile.SendMessageButton.name = playfabId;
    }
    #endregion

    #region SetDeleteFriendButtonName
    void SetDeleteFriendButtonName(string playfabId)
    {
        PlayerBaseConditions.PlayerProfile.DeleteFriendButton.name = playfabId;
    }
    #endregion

    #region ShowPlayerProfilePicture
    void ShowPlayerProfilePicture(int actorNumber)
    {
        if(PlayerBaseConditions.PlayerProfile.ProfileImage.name != actorNumber.ToString())
        {
            PlayerBaseConditions.PlayerProfile.ProfileImage = PlayerBaseConditions.PlayerProfile.LoadingProfilePic;
            PlayerBaseConditions.PlayerProfile.ShowPlayerProfilePic(false, actorNumber);
        }
    }
    #endregion
}
