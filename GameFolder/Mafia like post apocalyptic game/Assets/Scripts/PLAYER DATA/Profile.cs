﻿using UnityEngine;
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
    }
    [Serializable] class FriendProfileTab
    {
        [SerializeField] internal CanvasGroup friendProfileCanvasGroup;
        [SerializeField] internal Image friendProfilePic;
        [SerializeField] internal Image friendRankImage;
        [SerializeField] internal Button friendMessageButton;
        [SerializeField] internal Text friendNameText;
        [SerializeField] internal Text friendRankNumberText;
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
    public string AsWendigoCount
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
    public Button FriendProfileFriendMessageButton
    {
        get => _FriendProfileTab.friendMessageButton;
    }

    /// <summary>
    /// 0: CanvasGroup 1: PlayedAsTab 2: PlayerVotesTab 3:PlayerLogTab 4:FriendsTab 5:NotificationTab 6:FriendProfileTab
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

    Color32 releasedTabButtonColor => new Color32(155, 126, 80, 255);
    Color32 clickedTabButtonColor => new Color32(6, 255, 0, 255);


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
            StartCoroutine(ShowPlayerProfilePicCoroutine(ProfilePictureURL, ProfilePic => { ProfileImage = ProfilePic; }));
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
                PlayerBaseConditions.PlayfabManager.PlayfabFile.GetPlayfabFile(GetAccountInfo.EntityId, GetAccountInfo.EntityType, 
                    ProfilePictureURL => 
                    {
                        StartCoroutine(ShowPlayerProfilePicCoroutine(ProfilePictureURL, FriendProfilePic => { FriendProfileImage = FriendProfilePic; }));
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
}
