using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;

public class Profile : MonoBehaviour
{
    public static Profile instance;
   
    [SerializeField] Global _Global;
    [SerializeField] FirstTab _FirstTab;
    [SerializeField] SecondTab _SecondTab;
    [SerializeField] ThirdTab _ThirdTab;
    [SerializeField] PlayedAsTab _PlayedAsTab;
    [SerializeField] PlayerVotesTab _PlayerVotesTab;
    [SerializeField] LogTab _LogTab;
    [SerializeField] FriendsTab _FriendsTab;
    [SerializeField] NotificationTab _NotificationTab;
    [SerializeField] FriendProfileTab _FriendProfileTab;
    [SerializeField] FirstTabMessageTab _FirstTabMessageTab;
    [SerializeField] Icons _Icons;
   
    [Serializable] class Global
    {
        [SerializeField] internal Image bgImage;
        [SerializeField] internal Button closeButton;
        [SerializeField] internal CanvasGroup canvasGroup;
    }
    [Serializable] class FirstTab
    {
        [SerializeField] internal Image flagImage;
        [SerializeField] internal Image profileImage;
        [SerializeField] internal Image rankImage;
        [SerializeField] internal Text rankNumberText;
        [SerializeField] internal Text rankSliderNumberText;
        [SerializeField] internal Slider rankSlider;
        [SerializeField] internal Sprite loadingProfilePic;
        [SerializeField] internal Text nameText;
        [SerializeField] internal Button sendFriendRequestButton;
        [SerializeField] internal Button deleteFriendButton;
        [SerializeField] internal Button sendMessageButton;
    }
    [Serializable] class SecondTab
    {
        [SerializeField] internal Text timePlayedCountText;
        [SerializeField] internal Text firstLoginDateText;
    }
    [Serializable] class ThirdTab
    {
        [SerializeField] internal Button playedAsTabButton;
        [SerializeField] internal Button playerVotesTabButton;
    }
    [Serializable] class PlayedAsTab
    {
        [SerializeField] internal Text playedAsSurvivorCountText;
        [SerializeField] internal Text playedAsDoctorCountText;
        [SerializeField] internal Text playedAsSheriffCountText;
        [SerializeField] internal Text playedAsSoldierCountText;
        [SerializeField] internal Text playedAsLizardCountText;
        [SerializeField] internal Text playedAsWendigoCountText;
        [SerializeField] internal CanvasGroup playedAsCanvasGroup;
    }
    [Serializable] class PlayerVotesTab
    {
        [SerializeField] internal Transform playerVotesTabContainer;
        [SerializeField] internal PlayerDisplayedVoteObj playerVotesPrefab;
        [SerializeField] internal CanvasGroup playerVotesCanvasGroup;
    }
    [Serializable] class LogTab
    {
        [SerializeField] internal Button logTabButton;
        [SerializeField] internal Button loginToAnotherAccButton;
        [SerializeField] internal Button logOutButton;
        [SerializeField] internal CanvasGroup logCanvasGroup;
    }
    [Serializable] class FriendsTab
    {
        [SerializeField] internal Button friendsTabButton;
        [SerializeField] internal FriendButtonScript friendsButtonPrefab;
        [SerializeField] internal Transform friendsListContainer;
        [SerializeField] internal CanvasGroup friendsCanvasGroup;
    }
    [Serializable] class NotificationTab
    {
        [SerializeField] internal FriendRequestMessageScript friendRequestMessagePrefab;
        [SerializeField] internal Button notificationTabButton;
        [SerializeField] internal Transform notificationsContainer;
        [SerializeField] internal CanvasGroup notificationCanvasGroup;
        [SerializeField] internal NotificationMessageTab _NotificationMessageTab;

        [Serializable]
        [SerializeField] internal class NotificationMessageTab
        {
            [SerializeField] internal CanvasGroup MessageTab;
            [SerializeField] internal Text messageText;
            [SerializeField] internal MessageScript messagePrefab;
            [SerializeField] internal Button closeMessageTabButton;
        }
    }
    [Serializable] class FriendProfileTab
    {
        [SerializeField] internal CanvasGroup friendProfileCanvasGroup;
        [SerializeField] internal Image friendProfilePic;
        [SerializeField] internal Image friendRankImage;
        [SerializeField] internal Button friendMessageButton;
        [SerializeField] internal Text friendNameText;
        [SerializeField] internal Text friendRankNumberText;
        [SerializeField] internal FriendMessageTab _FriendMessageTab;

        [Serializable]
        internal class FriendMessageTab
        {
            [SerializeField] internal CanvasGroup friendMessageCanvasGroup;
            [SerializeField] internal InputField friendMessageInputField;
            [SerializeField] internal Button sendMessageButton;
            [SerializeField] internal Button closeMessageButton;
        }
    }
    [Serializable] class FirstTabMessageTab
    {
        [SerializeField] internal Button sendMessageButton;
        [SerializeField] internal Button closeMessageTabButton;
        [SerializeField] internal InputField friendMessageInputField;
    }
    [Serializable] class Icons
    {
        [SerializeField] internal Sprite diagramIcon;
        [SerializeField] internal Sprite gamePadIcon;
        [SerializeField] internal Sprite megaphoneIcon;
        [SerializeField] internal Sprite doorIcon;
        [SerializeField] internal Sprite friendsIcon;
        [SerializeField] internal Sprite bellIcon;
     }

    public Profile ProfileInstance
    {
        get => instance;
    }
    public Sprite BgImage
    {
        get => _Global.bgImage.sprite;
        set => _Global.bgImage.sprite = value;
    }
    public Sprite FlagImage
    {
        get => _FirstTab.flagImage.sprite;
        set => _FirstTab.flagImage.sprite = value;
    }
    public Sprite ProfileImage
    {
        get => _FirstTab.profileImage.sprite;
        set => _FirstTab.profileImage.sprite = value;
    }
    public Sprite FriendProfileImage
    {
        get => _FriendProfileTab.friendProfilePic.sprite;
        set => _FriendProfileTab.friendProfilePic.sprite = value;
    }
    public Sprite LoadingProfilePic
    {
        get => _FirstTab.loadingProfilePic;
    }
    public Sprite RankImage
    {
        get => _FirstTab.rankImage.sprite;
        set => _FirstTab.rankImage.sprite = value;
    }
    public Sprite FriendRankImage
    {
        get => _FriendProfileTab.friendRankImage.sprite;
        set => _FriendProfileTab.friendRankImage.sprite = value;
    }
    public Sprite DiagramIcon
    {
        get => _Icons.diagramIcon;
    }
    public Sprite GamePadIcon
    {
        get => _Icons.gamePadIcon;
    }
    public Sprite MegaphoneIcon
    {
        get => _Icons.megaphoneIcon;
    }
    public Sprite DoorIcon
    {
        get => _Icons.doorIcon;
    }
    public Sprite FriendsIcon
    {
        get => _Icons.friendsIcon;
    }
    public Sprite BellIcon
    {
        get => _Icons.bellIcon;
    }
    public string Name
    {
        get => _FirstTab.nameText.text;
        set => _FirstTab.nameText.text = value;
    }
    public string FriendProfileFriendName
    {
        get => _FriendProfileTab.friendNameText.text;
        set => _FriendProfileTab.friendNameText.text = value;
    }
    public string FriendMessageText
    {
        get => _FriendProfileTab._FriendMessageTab.friendMessageInputField.text;
        set => _FriendProfileTab._FriendMessageTab.friendMessageInputField.text = value;
    }
    public string NotificationMessageText
    {
        get => _NotificationTab._NotificationMessageTab.messageText.text;
        set => _NotificationTab._NotificationMessageTab.messageText.text = value;
    }
    public string TimePlayed
    {
        get => _SecondTab.timePlayedCountText.text;
        set => _SecondTab.timePlayedCountText.text = value;
    }
    public string FirstLoginDate
    {
        get => _SecondTab.firstLoginDateText.text;
        set => _SecondTab.firstLoginDateText.text = value;
    }
    public string AsSurvivorCount
    {
        get => _PlayedAsTab.playedAsSurvivorCountText.text;
        set => _PlayedAsTab.playedAsSurvivorCountText.text = value;
    }
    public string AsDoctorCount
    {
        get => _PlayedAsTab.playedAsDoctorCountText.text;
        set => _PlayedAsTab.playedAsDoctorCountText.text = value;
    }
    public string AsSheriffCount
    {
        get => _PlayedAsTab.playedAsSheriffCountText.text;
        set => _PlayedAsTab.playedAsSheriffCountText.text = value;
    }
    public string AsSoldierCount
    {
        get => _PlayedAsTab.playedAsSoldierCountText.text;
        set => _PlayedAsTab.playedAsSoldierCountText.text = value;
    }
    public string AsLizardCount
    {
        get => _PlayedAsTab.playedAsLizardCountText.text;
        set => _PlayedAsTab.playedAsLizardCountText.text = value;
    }
    public string AsInfectedCount
    {
        get => _PlayedAsTab.playedAsWendigoCountText.text;
        set => _PlayedAsTab.playedAsWendigoCountText.text = value;
    }
    public string RankNumber
    {
        get => _FirstTab.rankNumberText.text;
        set => _FirstTab.rankNumberText.text = value;
    }
    public string FriendProfileRankNumber
    {
        get => _FriendProfileTab.friendRankNumberText.text;
        set => _FriendProfileTab.friendRankNumberText.text = value;
    }
    public string FriendProfileFriendRankNumber
    {
        get => _FriendProfileTab.friendRankNumberText.text;
        set => _FriendProfileTab.friendRankNumberText.text = value;
    }
    public string RankSliderNumber
    {
        get => _FirstTab.rankSliderNumberText.text;
        set => _FirstTab.rankSliderNumberText.text = value;
    }
    public float RankSliderValue
    {
        get => _FirstTab.rankSlider.value;
        set => _FirstTab.rankSlider.value = value;
    }

    /// <summary>
    /// 0:PlayedAsTabButton 1:PlayerVotesTabButton 2:LogTabButton 3:FriendsTabButton 4:NotificationTabButton
    /// </summary>
    public Button[] TabButtons { get; set; }
    public Button CloseButton
    {
        get => _Global.closeButton;
    }    
    public Button LoginToAnotherAccButton
    {
        get => _LogTab.loginToAnotherAccButton;
    }
    public Button LogOutButton
    {
        get => _LogTab.logOutButton;
    }
    public Button SendFriendRequestButton
    {
        get => _FirstTab.sendFriendRequestButton;
    }
    public Button DeleteFriendButton
    {
        get => _FirstTab.deleteFriendButton;
    }
    public Button SendMessageButton
    {
        get => _FirstTab.sendMessageButton;
    }
    public Button FriendProfileFriendMessageButton
    {
        get => _FriendProfileTab.friendMessageButton;
    }
    public Button SendFriendMessageButton
    {
        get => _FriendProfileTab._FriendMessageTab.sendMessageButton;
    }
    public Button CloseFriendMessageButton
    {
        get => _FriendProfileTab._FriendMessageTab.closeMessageButton;
    } 
    public Button CloseNotificationMessageButton
    {
        get => _NotificationTab._NotificationMessageTab.closeMessageTabButton;
    }

    /// <summary>
    /// 0: CanvasGroup 1: PlayedAsTab 2: PlayerVotesTab 3:PlayerLogTab 4:FriendsTab 5:NotificationTab 6:FriendProfileTab 7:FriendMessageTab 8: NoticiationMessageTab 9:FriendMessageTab
    /// </summary>
    public CanvasGroup[] CanvasGroups;    
    public Transform PlayerVotesTabContainer
    {
        get => _PlayerVotesTab.playerVotesTabContainer;
    }
    public Transform FriendsListContainer
    {
        get => _FriendsTab.friendsListContainer;
    }
    public Transform NotificationsContainer
    {
        get => _NotificationTab.notificationsContainer;
    }


    public PlayerDisplayedVoteObj PlayerVotesPrefab
    {
        get => _PlayerVotesTab.playerVotesPrefab;
    }
    public FriendButtonScript FriendButtonPrefab
    {
        get => _FriendsTab.friendsButtonPrefab;
    }
    public FriendRequestMessageScript FriendRequestMessagePrefab
    {
        get => _NotificationTab.friendRequestMessagePrefab;
    }
    public MessageScript MessagePrefab
    {
        get => _NotificationTab._NotificationMessageTab.messagePrefab;
    }


    public Color32 releasedTabButtonColor;
    public Color32 clickedTabButtonColor;

    ProfilePicContainer ProfilePicContainer { get; set; }


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        TabButtons = new Button[5] 
        {
         _ThirdTab.playedAsTabButton,
         _ThirdTab.playerVotesTabButton,
         _LogTab.logTabButton,
         _FriendsTab.friendsTabButton,
         _NotificationTab.notificationTabButton
        };

        ProfilePicContainer = GetComponent<ProfilePicContainer>();
    }

    void Update()
    {
        OnClickButton(CloseButton);      
        OnClickButton(LogOutButton);
        OnClickButton(LoginToAnotherAccButton);

        OnClickTabButtons(TabButtons[0]);
        OnClickTabButtons(TabButtons[1]);
        OnClickTabButtons(TabButtons[2]);
        OnClickTabButtons(TabButtons[3]);
        OnClickTabButtons(TabButtons[4]);

        OnClickSendFriendRequestButton(SendFriendRequestButton);
        OnClickDeleteFriendButton(DeleteFriendButton);
        OnClickSendMessageButton(SendMessageButton);

        OnClickFirstTabMessageTabsButtons(_FirstTabMessageTab.sendMessageButton);
        OnClickFirstTabMessageTabsButtons(_FirstTabMessageTab.closeMessageTabButton);

        OnClickFriendProfileButtons(FriendProfileFriendMessageButton);
        OnClickFriendProfileButtons(SendFriendMessageButton);
        OnClickFriendProfileButtons(CloseFriendMessageButton);

        OnNotificationButtons(CloseNotificationMessageButton);
    }

    #region OnClickButton
    public void OnClickButton(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            if (button == CloseButton)
            {
                MyCanvasGroups.CanvasGroupActivity(CanvasGroups[0], false);
            }                     
            if (button == LogOutButton)
            {
                PlayerBaseConditions.PlayfabManager.PlayfabLogOut.LogOut(() => PlayerBaseConditions.NetworkManagerComponents.NetworkUI.OnLoggedOut());                              
            }
            if (button == LoginToAnotherAccButton)
            {
                PlayerBaseConditions.PlayfabManager.PlayfabLogOut.LogOut(() => PlayerBaseConditions.NetworkManagerComponents.NetworkUI.OnLoginToAnotherAccount());
            }
        });
    }
    #endregion

    #region OnClickTabButtons
    void OnClickTabButtons(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => 
        {
            TabButtonsColor(button);

            if (button == TabButtons[0])
            {
                DisableCanvasGroups();
                MyCanvasGroups.CanvasGroupActivity(CanvasGroups[1], true);
                BgImage = GamePadIcon;
            }
            if (button == TabButtons[1])
            {
                DisableCanvasGroups();
                MyCanvasGroups.CanvasGroupActivity(CanvasGroups[2], true);
                BgImage = MegaphoneIcon;
            }
            if (button == TabButtons[2])
            {
                DisableCanvasGroups();
                MyCanvasGroups.CanvasGroupActivity(CanvasGroups[3], true);
                BgImage = DoorIcon;
            }
            if (button == TabButtons[3])
            {
                DisableCanvasGroups();
                MyCanvasGroups.CanvasGroupActivity(CanvasGroups[4], true);
                BgImage = FriendsIcon;
            }
            if (button == TabButtons[4])
            {
                DisableCanvasGroups();
                MyCanvasGroups.CanvasGroupActivity(CanvasGroups[5], true);
                BgImage = BellIcon;
            }
        });       
    }
    #endregion

    #region OnClickSendFriendRequestButton
    void OnClickSendFriendRequestButton(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => 
        {
            PlayerBaseConditions.PlayfabManager.PlayfabFriends.SendFriendRequest(button.name);
        });
    }
    #endregion

    #region OnClickDeleteFriendButton
    void OnClickDeleteFriendButton(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            PlayerBaseConditions.PlayfabManager.PlayfabFriends.UnFriend(PlayerBaseConditions.OwnPlayfabId, button.name);
            PlayerBaseConditions.PlayfabManager.PlayfabFriends.UnFriend(button.name, PlayerBaseConditions.OwnPlayfabId);
        });
    }
    #endregion

    #region OnClickSendMessageButton
    void OnClickSendMessageButton(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            MyCanvasGroups.CanvasGroupActivity(CanvasGroups[9], true);
        });
    }
    #endregion

    #region OnClickFirstTabMessageTabsButtons
    void OnClickFirstTabMessageTabsButtons(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            if(button == _FirstTabMessageTab.sendMessageButton)
            {
                if (!String.IsNullOrEmpty(_FirstTabMessageTab.friendMessageInputField.text))
                {
                    PlayerBaseConditions.PlayfabManager.PlayfabInternalData.UpdatePlayerInternalData(button.name,
                        PlayerKeys.InternalData.MessageKey + PlayerBaseConditions.LocalPlayer.NickName, _FirstTabMessageTab.friendMessageInputField.text);

                    _FirstTabMessageTab.friendMessageInputField.text = null;
                }
            }
            if (button == _FirstTabMessageTab.closeMessageTabButton)
            {
                MyCanvasGroups.CanvasGroupActivity(CanvasGroups[9], false);
            }          
        });
    }
    #endregion

    #region OnClickFriendMessageButton
    void OnClickFriendProfileButtons(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => 
        {
            if(button == FriendProfileFriendMessageButton)
            {
                MyCanvasGroups.CanvasGroupActivity(CanvasGroups[7], true);
                SendFriendMessageButton.name = button.name;
            }
            if(button == SendFriendMessageButton)
            {
                if (!String.IsNullOrEmpty(FriendMessageText))
                {
                    PlayerBaseConditions.PlayfabManager.PlayfabInternalData.UpdatePlayerInternalData(button.name,
                        PlayerKeys.InternalData.MessageKey + PlayerBaseConditions.LocalPlayer.NickName, _FriendProfileTab._FriendMessageTab.friendMessageInputField.text);

                    FriendMessageText = null;
                }                
            }
            if(button == CloseFriendMessageButton)
            {
                MyCanvasGroups.CanvasGroupActivity(CanvasGroups[7], false);
            }
        });
    }
    #endregion

    #region OnNotificationButtons
    void OnNotificationButtons(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => 
        {
            if(button == CloseNotificationMessageButton)
            {
                MyCanvasGroups.CanvasGroupActivity(CanvasGroups[8], false);
            }
        });
    }
    #endregion

    #region TabButtonsColor
    void TabButtonsColor(Button pressedTabButton)
    {
        foreach (var tabButton in TabButtons)
        {
            if(tabButton != pressedTabButton)
            {
                tabButton?.GetComponent<TabButtons>().ImagesColor(releasedTabButtonColor);
            }
            else
            {
                tabButton?.GetComponent<TabButtons>().ImagesColor(clickedTabButtonColor);
            }
        }
    }
    #endregion

    #region DisableCanvasGroups
    void DisableCanvasGroups()
    {
        foreach (var canvasGroup in CanvasGroups)
        {
            if (canvasGroup != CanvasGroups[0])
                MyCanvasGroups.CanvasGroupActivity(canvasGroup, false);
        }
    }
    #endregion

    #region SetDefaultPage
    public void SetDefaultPage()
    {
        DisableCanvasGroups();
        TabButtonsColor(this.TabButtons[0]);
        MyCanvasGroups.CanvasGroupActivity(CanvasGroups[1], true);
        BgImage = GamePadIcon;     
    }
    #endregion

    #region ShowPlayerAvatar
    /// <summary>
    /// Get profile pic by player's actor number
    /// </summary>
    /// <param name="isInMenuScene"></param>
    /// <param name="actorNumber"></param>
    public void ShowPlayerProfilePic(bool isInMenuScene, int actorNumber)
    {
        Photon.Realtime.Player Player = isInMenuScene ? PlayerBaseConditions.LocalPlayer : PlayerBaseConditions.Player(actorNumber);

        if (Player.CustomProperties.ContainsKey(PlayerKeys.EntityId))
        {
            PlayerBaseConditions.PlayfabManager.PlayfabFile.GetPlayfabFile(Player.CustomProperties[PlayerKeys.EntityId].ToString(), Player.CustomProperties[PlayerKeys.EntityType].ToString(),
            ProfilePictureURL =>
            {
                if (ProfilePicContainer.CachedProfilePics.ContainsKey(Player.CustomProperties[PlayerKeys.UserID].ToString()))
                {
                    ProfileImage = ProfilePicContainer.CachedProfilePics[Player.CustomProperties[PlayerKeys.UserID].ToString()];
                }
                else
                {
                    StartCoroutine(ShowPlayerProfilePicCoroutine(ProfilePictureURL, 
                        ProfilePic => 
                        {
                            ProfileImage = ProfilePic;
                            ProfilePicContainer.CacheProfilePics(Player.CustomProperties[PlayerKeys.UserID].ToString(), ProfilePic);
                        }));
                }
            });
        }        
    }

    /// <summary>
    /// Get profile pic by player's playfabId
    /// </summary>
    /// <param name="playfabId"></param>
    public void ShowPlayerProfilePic(string playfabId)
    {
        PlayerBaseConditions.PlayfabManager.PlayfabUserAccountInfo.GetUserAccountInfo(playfabId, 
            GetAccountInfo => 
            {
                PlayerBaseConditions.PlayfabManager.PlayfabFile.GetPlayfabFile(GetAccountInfo.TitleInfo.TitlePlayerAccount.Id, GetAccountInfo.TitleInfo.TitlePlayerAccount.Type, 
                    ProfilePictureURL => 
                    {
                        if (ProfilePicContainer.CachedProfilePics.ContainsKey(playfabId))
                        {
                            FriendProfileImage = ProfilePicContainer.CachedProfilePics[playfabId];
                        }
                        else
                        {
                            StartCoroutine(ShowPlayerProfilePicCoroutine(ProfilePictureURL,
                                FriendProfilePic =>
                                {
                                    FriendProfileImage = FriendProfilePic;
                                    ProfilePicContainer.CacheProfilePics(playfabId, FriendProfilePic);
                                }));
                        }                      
                    });
            });
    }

    IEnumerator ShowPlayerProfilePicCoroutine(string url, Action<Sprite>ProfilePic)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        if (!request.isNetworkError && !request.isHttpError)
        {
            Texture2D downloadedAvatar = ((DownloadHandlerTexture)request.downloadHandler).texture;
            ProfilePic(Sprite.Create(downloadedAvatar, new Rect(0, 0, downloadedAvatar.width, downloadedAvatar.height), new Vector2(0.5f, 0.5f), 100));
        }
    }
    #endregion

    #region ShowPlayerStats
    public void ShowPlayerStats(string playfabId)
    {
        PlayerBaseConditions.PlayfabManager.PlayfabStats.GetPlayerStats(playfabId, Stats => 
        {
            TimePlayed = Stats.totalTimePlayed.ToString();
            RankNumber = Stats.rank.ToString();
            AsSurvivorCount = Stats.countPlayedAsSurvivor.ToString();
            AsDoctorCount = Stats.countPlayedAsDoctor.ToString();
            AsSheriffCount = Stats.countPlayedAsSheriff.ToString();
            AsSoldierCount = Stats.countPlayedAsSoldier.ToString();
            AsInfectedCount = Stats.countPlayedAsInfected.ToString();
            AsLizardCount = Stats.countPlayedAsLizard.ToString();
        });
    }
    #endregion
}
