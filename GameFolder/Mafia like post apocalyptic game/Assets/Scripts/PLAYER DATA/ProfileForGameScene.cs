using UnityEngine;
using UnityEngine.UI;

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
            UpdatePlaterStats(actorNumber);
            //CheckPlayerVotes(actorNumber);
            SendFriendButtonActivity(playfabId);
            ShowPlayerProfilePicture(actorNumber);
        }
    }
    #endregion

    #region GameScene
    void GameScene()
    {
        PlayerBaseConditions.PlayerProfile.SetDefaultPage();
        PlayerBaseConditions.PlayerProfile.TabButtons[3].gameObject.SetActive(false);
        PlayerBaseConditions.PlayerProfile.TabButtons[2].gameObject.SetActive(false);
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
    void UpdatePlaterStats(int actorNumber)
    {
        PlayerBaseConditions.PlayerProfile.TimePlayed = PlayerBaseConditions.Player(actorNumber).CustomProperties[PlayerKeys.StatisticKeys.TotalTimePlayed].ToString();
        PlayerBaseConditions.PlayerProfile.RankNumber = PlayerBaseConditions.Player(actorNumber).CustomProperties[PlayerKeys.StatisticKeys.Rank].ToString();
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
                Destroy(PlayerBaseConditions.PlayerProfile.PlayerVotesTabContainer.GetChild(0).gameObject);
            }
        }
        #endregion

        for (int i = 0; i < PlayerBaseConditions._PlayerTagObject(actorNumber).GetComponent<PlayerGamePlayStatus>().VotedNames.Count; i++)
        {
            PlayerDisplayedVoteObj playerVote = Instantiate(PlayerBaseConditions.PlayerProfile.PlayerVotesPrefab, PlayerBaseConditions.PlayerProfile.PlayerVotesTabContainer);
            playerVote.Name = PlayerBaseConditions._PlayerTagObject(actorNumber).GetComponent<PlayerGamePlayStatus>().VotedNames[i];
        }
    }
    #endregion

    #region SendFriendButtonActivity
    void SendFriendButtonActivity(string playfabId)
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
                        SetSendFriendRequestButtonName(playfabId);
                    }
                    else
                    {
                        PlayerBaseConditions.PlayerProfile.SendFriendRequestButton.gameObject.SetActive(false);
                    }
                });
        }
    }
    #endregion

    #region SetSendFriendRequestButtonName
    void SetSendFriendRequestButtonName(string playfabId)
    {
        PlayerBaseConditions.PlayerProfile.SendFriendRequestButton.name = playfabId;
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
