using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.Networking;
using Photon.Pun;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Profile : MonoBehaviourPun
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
    [SerializeField] internal DatasDownloadCheck _DatasDownloadCheck;
    [SerializeField] internal EditProfilePicTab _EditProfilePicTab;


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
        [SerializeField] internal Sprite loadingProfilePic;
        [SerializeField] internal Text nameText;
        [SerializeField] internal Button sendFriendRequestButton;
        [SerializeField] internal Button deleteFriendButton;
        [SerializeField] internal Button sendMessageButton;
        [SerializeField] internal Button updateProfilePictureButton;
        [SerializeField] internal Image friendReuqestAlreadySentIcon;
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
        [SerializeField] internal Text rankNumberText;
        [SerializeField] internal Text points;
        [SerializeField] internal Image rankImage;
        [SerializeField] internal Image winImage;
        [SerializeField] internal Slider rankSlider;

        [SerializeField] internal Text playedAsSurvivorCountText;
        [SerializeField] internal Text playedAsDoctorCountText;
        [SerializeField] internal Text playedAsSheriffCountText;
        [SerializeField] internal Text playedAsSoldierCountText;
        [SerializeField] internal Text playedAsLizardCountText;
        [SerializeField] internal Text playedAsWendigoCountText;

        [SerializeField] internal Text winsPercentText;
        [SerializeField] internal Text skillValueText;
        [SerializeField] internal Text winsCountText;
        [SerializeField] internal Text lossesCountText;

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
        [SerializeField] internal Button deleteAccountButton;
        [SerializeField] internal Button confirmAccountDeletionButton;
        [SerializeField] internal Button dontDeleteAccountButton;
        [SerializeField] internal CanvasGroup logCanvasGroup;

        [SerializeField] internal CanvasGroup logTabButtonsCanvasGroup;
        [SerializeField] internal CanvasGroup confirmDeletionButtonsCanvasGroup;
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
        [SerializeField] internal CanvasGroup notificationsCountCanvasGroup;
        [SerializeField] internal Text notificationsCountText;
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
        [SerializeField] internal Button joinRoomButton;
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
    [Serializable] internal class DatasDownloadCheck
    {
        internal enum DatasDownloadStatus { PlayerStatsDownloaded, FriendCheckingDownloaded, FriendRequestDataDownloaded}
        internal DatasDownloadStatus _DatasDownloadStatus;

        [SerializeField] GameObject loadingScreen;

        [SerializeField] bool hasPlayerStatsDownloaded;
        [SerializeField] bool hasFriendCheckingDownloaded;
        [SerializeField] bool hasFriendRequestDataDownloaded;
        [SerializeField] bool hasProfilePicDownloaded;

        internal IEnumerator Coroutine { get; set; }
        internal GameObject LoadingScreen
        {
            get => loadingScreen;
        }

        internal bool HasPlayerStatsDownloaded
        {
            get => hasPlayerStatsDownloaded;
            set => hasPlayerStatsDownloaded = value;
        }
        internal bool HasFriendCheckingDownloaded
        {
            get => hasFriendCheckingDownloaded;
            set => hasFriendCheckingDownloaded = value;
        }
        internal bool HasFriendRequestDataDownloaded
        {
            get => hasFriendRequestDataDownloaded;
            set => hasFriendRequestDataDownloaded = value;
        }
    }
    [Serializable] internal class EditProfilePicTab
    {
        [SerializeField] Image profilePic;
        [SerializeField] CanvasGroup editProfileTabCanvasGroups;

        internal Sprite ProfilePicSprite
        {
            get => profilePic.sprite;
            set => profilePic.sprite = value;
        }
        internal CanvasGroup EditProfileTabCanvasGroups
        {
            get => editProfileTabCanvasGroups;
        }
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
        get => _PlayedAsTab.rankImage.sprite;
        set => _PlayedAsTab.rankImage.sprite = value;
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
        get => _PlayedAsTab.rankNumberText.text;
        set => _PlayedAsTab.rankNumberText.text = value;
    }
    public string Points
    {
        get => _PlayedAsTab.points.text;
        set => _PlayedAsTab.points.text = value;
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
    public int NotificationsCount
    {
        get => int.Parse(_NotificationTab.notificationsCountText.text);
        set => _NotificationTab.notificationsCountText.text = value.ToString();
    }
    public int PointsSliderValue
    {
        get => (int)_PlayedAsTab.rankSlider.value;
        set => _PlayedAsTab.rankSlider.value = value;
    }
    public int WinsPercent
    {
        get => int.Parse(_PlayedAsTab.winsPercentText.text.Substring(0, _PlayedAsTab.winsPercentText.text.Length - 1));
        set => _PlayedAsTab.winsPercentText.text = value.ToString() + "%";
    }
    public int SkillValue
    {
        get => int.Parse(_PlayedAsTab.skillValueText.text);
        set => _PlayedAsTab.skillValueText.text = value.ToString();
    }
    public int WinsCount
    {
        get => int.Parse(_PlayedAsTab.winsCountText.text);
        set => _PlayedAsTab.winsCountText.text = value.ToString();
    }
    public int LossesCount
    {
        get => int.Parse(_PlayedAsTab.lossesCountText.text);
        set => _PlayedAsTab.lossesCountText.text = value.ToString();
    }

    public float WinImageAmountValue
    {
        get => _PlayedAsTab.winImage.fillAmount;
        set => _PlayedAsTab.winImage.fillAmount = value;
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
    public Button DeleteAccountButton
    {
        get => _LogTab.deleteAccountButton;
    }
    public Button ConfirmAccountDeletionButton
    {
        get => _LogTab.confirmAccountDeletionButton;
    }
    public Button DontDeleteAccountButton
    {
        get => _LogTab.dontDeleteAccountButton;
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
    public Button JoinFriendsRoomButton
    {
        get => _FriendProfileTab.joinRoomButton;
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
    public Button EditProfilePictureButton
    {
        get => _FirstTab.updateProfilePictureButton;
    }
    public Image FriendRequestAlreadySentIcon
    {
        get => _FirstTab.friendReuqestAlreadySentIcon;
    }

    /// <summary>
    /// 0: CanvasGroup 1: PlayedAsTab 2: PlayerVotesTab 3:PlayerLogTab 4:FriendsTab 5:NotificationTab 6:FriendProfileTab 7:FriendMessageTab 8: NoticiationMessageTab 9:FriendMessageTab
    /// </summary>
    public CanvasGroup[] CanvasGroups;  
    public CanvasGroup LogTabButtonsCanvasGroup
    {
        get => _LogTab.logTabButtonsCanvasGroup;
    }
    public CanvasGroup ConfirmAccountDeletionButtonsCanvasGroup
    {
        get => _LogTab.confirmDeletionButtonsCanvasGroup;
    }
    public CanvasGroup NotifcationsCountCanvasgroup
    {
        get => _NotificationTab.notificationsCountCanvasGroup;
    }
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

    public ProfilePicContainer ProfilePicContainer { get; set; }


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
        OnClickButton(DeleteAccountButton);
        OnClickButton(ConfirmAccountDeletionButton);
        OnClickButton(DontDeleteAccountButton);
        OnClickButton(EditProfilePictureButton);

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
        OnClickToJoinFriendsRoomButton(JoinFriendsRoomButton);

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
            if (button == DeleteAccountButton)
            {
                MyCanvasGroups.CanvasGroupActivity(ConfirmAccountDeletionButtonsCanvasGroup, true);
                MyCanvasGroups.CanvasGroupActivity(LogTabButtonsCanvasGroup, false);
            }
            if(button == ConfirmAccountDeletionButton)
            {
                PlayerBaseConditions.PlayfabManager.PlayfabDeleteAccount.DeleteAccount(PhotonNetwork.LocalPlayer.UserId, Delete =>
                {
                    if (Delete == true)
                    {
                        MyCanvasGroups.CanvasGroupActivity(ConfirmAccountDeletionButtonsCanvasGroup, false);
                        MyCanvasGroups.CanvasGroupActivity(LogTabButtonsCanvasGroup, true);
                        PlayerBaseConditions.PlayfabManager.PlayfabLogOut.LogOut(() => PlayerBaseConditions.NetworkManagerComponents.NetworkUI.OnLoggedOut());
                    }
                    else
                    {
                        MyCanvasGroups.CanvasGroupActivity(ConfirmAccountDeletionButtonsCanvasGroup, false);
                        MyCanvasGroups.CanvasGroupActivity(LogTabButtonsCanvasGroup, true);
                    }
                });               
            }
            if(button == DontDeleteAccountButton)
            {
                MyCanvasGroups.CanvasGroupActivity(ConfirmAccountDeletionButtonsCanvasGroup, false);
                MyCanvasGroups.CanvasGroupActivity(LogTabButtonsCanvasGroup, true);
            }
            if(button == EditProfilePictureButton)
            {
                MyCanvasGroups.CanvasGroupActivity(_EditProfilePicTab.EditProfileTabCanvasGroups, true);
                //AndroidGoodiesExamples.OtherGoodiesTest Android = FindObjectOfType<AndroidGoodiesExamples.OtherGoodiesTest>();
                //Android.OnPickGalleryImage(Android.profilePictureUploadMethod = AndroidGoodiesExamples.OtherGoodiesTest.ProfilePictureUploadMethod.Profile);
            }

            PlayerBaseConditions.UiSounds.PlaySoundFX(0);
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

            PlayerBaseConditions.UiSounds.PlaySoundFX(2);
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
            OnPlayerRequestAlreadySent();
            PlayerBaseConditions.UiSounds.PlaySoundFX(5);
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
            PlayerBaseConditions.UiSounds.PlaySoundFX(5);
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
            PlayerBaseConditions.UiSounds.PlaySoundFX(0);
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

            PlayerBaseConditions.UiSounds.PlaySoundFX(0);
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
                PlayerBaseConditions.UiSounds.PlaySoundFX(0);
            }
            if(button == SendFriendMessageButton)
            {
                if (!String.IsNullOrEmpty(FriendMessageText))
                {
                    PlayerBaseConditions.PlayfabManager.PlayfabInternalData.UpdatePlayerInternalData(button.name,
                        PlayerKeys.InternalData.MessageKey + PlayerBaseConditions.LocalPlayer.NickName, _FriendProfileTab._FriendMessageTab.friendMessageInputField.text);

                    FriendMessageText = null;
                    PlayerBaseConditions.UiSounds.PlaySoundFX(5);
                }                
            }
            if(button == CloseFriendMessageButton)
            {
                MyCanvasGroups.CanvasGroupActivity(CanvasGroups[7], false);
                PlayerBaseConditions.UiSounds.PlaySoundFX(0);
            }         
        });
    }
    #endregion

    #region OnClickToJoinFriendsRoomButton
    void OnClickToJoinFriendsRoomButton(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => 
        {
            if (!PhotonNetwork.InRoom || PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.Name != button.gameObject.name)
            {
                PhotonNetwork.JoinRoom(button.gameObject.name);
            }

            PlayerBaseConditions.UiSounds.PlaySoundFX(5);
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

            PlayerBaseConditions.UiSounds.PlaySoundFX(0);
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
                    ProfileImage.name = actorNumber.ToString();
                }
                else
                {
                    StartCoroutine(ShowPlayerProfilePicCoroutine(ProfilePictureURL, 
                        ProfilePic => 
                        {
                            ProfileImage = ProfilePic;
                            ProfileImage.name = actorNumber.ToString();
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
            RankNumber = RankNumberValueByPoints(Stats.points).ToString();
            Points = Stats.points.ToString("N0") + "/550,000";
            WinsPercent = 0 / int.Parse(TimePlayed) * 100;
            WinImageAmountValue = WinsPercent;
            WinsCount = 0;
            LossesCount = 0;
            SkillValue = Stats.overallSkills;
            PointsSliderController(int.Parse(RankNumber));
            PointsSliderValue = Stats.points;


            AsSurvivorCount = Stats.asSurvivor.ToString();
            AsDoctorCount = Stats.asDoctor.ToString();
            AsSheriffCount = Stats.asSheriff.ToString();
            AsSoldierCount = Stats.asSoldier.ToString();
            AsInfectedCount = Stats.asInfected.ToString();
            AsLizardCount = Stats.asLizard.ToString();

            ProfileDatasCompleteCheck(DatasDownloadCheck.DatasDownloadStatus.PlayerStatsDownloaded);
        });
    }

    int RankNumberValueByPoints(int points)
    {
        return points <= 2500 ? 1 : points > 2500 && points <= 7250 ? 2 : points > 7250 && points <= 15100 ? 3 : points > 15100 && points <= 24725 ? 4 :
               points > 24725 && points <= 37725 ? 5 : points > 37725 && points <= 53580 ? 6 : points > 53580 && points <= 74605 ? 7 : points > 74605 && points <= 100130 ? 8 :
               points > 100130 && points <= 127185 ? 9 : points > 127185 && points <= 157185 ? 10 : points > 157185 && points <= 188435 ? 11 : points > 188435 && points <= 223185 ? 12 :
               points > 223185 && points <= 263185 ? 13 : points > 263185 && points <= 305435 ? 14 : points > 305435 && points <= 348435 ? 15 : points > 348435 && points <= 392935 ? 16 :
               points > 392935 && points <= 440760 ? 17 : points > 440760 && points <= 490760 ? 18 : points > 490760 && points <= 550000 ? 19 : 19;
    }

    void PointsSliderController(int rankNumber)
    {
        if(rankNumber == 1)
        {
            _PlayedAsTab.rankSlider.minValue = 0;
            _PlayedAsTab.rankSlider.maxValue = 2500;
        }
        if (rankNumber == 2)
        {
            _PlayedAsTab.rankSlider.minValue = 2500;
            _PlayedAsTab.rankSlider.maxValue = 7250;
        }
        if (rankNumber == 3)
        {
            _PlayedAsTab.rankSlider.minValue = 7250;
            _PlayedAsTab.rankSlider.maxValue = 15100;
        }
        if (rankNumber == 4)
        {
            _PlayedAsTab.rankSlider.minValue = 15100;
            _PlayedAsTab.rankSlider.maxValue = 24725;
        }
        if (rankNumber == 5)
        {
            _PlayedAsTab.rankSlider.minValue = 24725;
            _PlayedAsTab.rankSlider.maxValue = 37725;
        }
        if (rankNumber == 6)
        {
            _PlayedAsTab.rankSlider.minValue = 37725;
            _PlayedAsTab.rankSlider.maxValue = 53580;
        }
        if (rankNumber == 7)
        {
            _PlayedAsTab.rankSlider.minValue = 53580;
            _PlayedAsTab.rankSlider.maxValue = 74605;
        }
        if (rankNumber == 8)
        {
            _PlayedAsTab.rankSlider.minValue = 74605;
            _PlayedAsTab.rankSlider.maxValue = 100130;
        }
        if (rankNumber == 9)
        {
            _PlayedAsTab.rankSlider.minValue = 100130;
            _PlayedAsTab.rankSlider.maxValue = 127185;
        }
        if (rankNumber == 10)
        {
            _PlayedAsTab.rankSlider.minValue = 127185;
            _PlayedAsTab.rankSlider.maxValue = 157185;
        }
        if (rankNumber == 11)
        {
            _PlayedAsTab.rankSlider.minValue = 157185;
            _PlayedAsTab.rankSlider.maxValue = 188435;
        }
        if (rankNumber == 12)
        {
            _PlayedAsTab.rankSlider.minValue = 188435;
            _PlayedAsTab.rankSlider.maxValue = 223185;
        }
        if (rankNumber == 13)
        {
            _PlayedAsTab.rankSlider.minValue = 223185;
            _PlayedAsTab.rankSlider.maxValue = 263185;
        }
        if (rankNumber == 14)
        {
            _PlayedAsTab.rankSlider.minValue = 263185;
            _PlayedAsTab.rankSlider.maxValue = 305435;
        }
        if (rankNumber == 15)
        {
            _PlayedAsTab.rankSlider.minValue = 305435;
            _PlayedAsTab.rankSlider.maxValue = 348435;
        }
        if (rankNumber == 16)
        {
            _PlayedAsTab.rankSlider.minValue = 348435;
            _PlayedAsTab.rankSlider.maxValue = 392935;
        }
        if (rankNumber == 17)
        {
            _PlayedAsTab.rankSlider.minValue = 392935;
            _PlayedAsTab.rankSlider.maxValue = 440760;
        }
        if (rankNumber == 18)
        {
            _PlayedAsTab.rankSlider.minValue = 440760;
            _PlayedAsTab.rankSlider.maxValue = 490760;
        }
        if (rankNumber == 19)
        {
            _PlayedAsTab.rankSlider.minValue = 490760;
            _PlayedAsTab.rankSlider.maxValue = 550000;
        }
    }
    #endregion

    #region OnPlayerRequestAlreadySent
    public void OnPlayerRequestAlreadySent()
    {
        SendFriendRequestButton.gameObject.SetActive(false);
        FriendRequestAlreadySentIcon.gameObject.SetActive(true);
    }
    #endregion

    #region ProfileDatasCompleteCheck + ResetProfileDatasCompleteCheck
    internal void ProfileDatasCompleteCheck(DatasDownloadCheck.DatasDownloadStatus downloadStatus)
    {
        if (downloadStatus == DatasDownloadCheck.DatasDownloadStatus.PlayerStatsDownloaded) _DatasDownloadCheck.HasPlayerStatsDownloaded = true;
        if (downloadStatus == DatasDownloadCheck.DatasDownloadStatus.FriendCheckingDownloaded) _DatasDownloadCheck.HasFriendCheckingDownloaded = true;
        if (downloadStatus == DatasDownloadCheck.DatasDownloadStatus.FriendRequestDataDownloaded) _DatasDownloadCheck.HasFriendRequestDataDownloaded = true;
    }

    internal void ResetProfileDatasCompleteCheck()
    {
        _DatasDownloadCheck.HasPlayerStatsDownloaded = false;
        _DatasDownloadCheck.HasFriendCheckingDownloaded = false;
        _DatasDownloadCheck.HasFriendRequestDataDownloaded = false;
    }

    internal IEnumerator CheckIfAllDatasDownloadCompleted(int actorNumber)
    {
        _DatasDownloadCheck.LoadingScreen.SetActive(true);
        PlayerBaseConditions.PlayerProfile.ResetProfileDatasCompleteCheck();

        yield return new WaitUntil(() => _DatasDownloadCheck.HasFriendCheckingDownloaded && _DatasDownloadCheck.HasFriendRequestDataDownloaded && _DatasDownloadCheck.HasPlayerStatsDownloaded && ProfileImage.name == actorNumber.ToString());

        _DatasDownloadCheck.LoadingScreen.SetActive(false);

        _DatasDownloadCheck.Coroutine = null;
    }
    #endregion
}
