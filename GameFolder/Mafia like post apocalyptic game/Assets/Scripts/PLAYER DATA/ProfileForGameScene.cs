using UnityEngine;

public class ProfileForGameScene : MonoBehaviour
{
    int previousActorNumber;
    bool isResetPhase;

    GameManagerTimer _GameManagerTimer;


    void Awake()
    {
        _GameManagerTimer = FindObjectOfType<GameManagerTimer>();
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
        if (!isResetPhase)
        {
            MyCanvasGroups.CanvasGroupActivity(PlayerBaseConditions.PlayerProfile.CanvasGroups[0], false);
        }
    }

    void CanViewTheProfile(bool isResetPhase)
    {       
        this.isResetPhase = isResetPhase;
    }

    #region OnClickRoleButton
    public void OnClickRoleButton(bool canPlayerViewTheProfile, int actorNumber, string playfabId)
    {
        if (canPlayerViewTheProfile)
        {
            MyCanvasGroups.CanvasGroupActivity(PlayerBaseConditions.PlayerProfile.CanvasGroups[0], true);

            GameScene();
            UpdatePlayerProfile(actorNumber);
            UpdatePlaterStats(playfabId);
            CheckPlayerVotes(actorNumber);
            IfPlayerIsFriend(playfabId);
            ShowPlayerProfilePicture(actorNumber);
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

    #region CheckPlayerProfile + SetDefaultPage
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
    void IfPlayerIsFriend(string playfabId)
    {
        PlayerBaseConditions.PlayerProfile.SendFriendRequestButton.gameObject.SetActive(false);

        if (PlayerBaseConditions.OwnPlayfabId != playfabId)
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
        if(previousActorNumber != actorNumber)
        {
            PlayerBaseConditions.PlayerProfile.ProfileImage = PlayerBaseConditions.PlayerProfile.LoadingProfilePic;
            PlayerBaseConditions.PlayerProfile.ShowPlayerProfilePic(false, actorNumber);
        }

        previousActorNumber = actorNumber;
    }
    #endregion
}
