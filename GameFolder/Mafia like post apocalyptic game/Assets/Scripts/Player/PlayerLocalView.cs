using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class PlayerLocalView : MonoBehaviourPun
{
    [Header("COMPONENTS")]
    [SerializeField] PlayerSelfTimer playerSelfTimer;
    [SerializeField] GetTimer getTimer;


    void OnEnable()
    {
        playerSelfTimer.OnUpdateTimerText += PlayerSelfTimer_OnUpdateTimerText;

        if(PlayerBaseConditions._InstanceIsThis())
        {
            PlayerComponents.instance.PlayerEvents.OnSetOwnedAvatarButtonSprite += PlayerEvents_OnSetOwnedAvatarButtonSprite;
        }

        SubToEvents.SubscribeToEvents(delegate
        {
            PlayerBaseConditions._MyGameManager.OnDayVote += _MyGameManager_OnDayVote;
            PlayerBaseConditions._MyGameManager.OnNightVote += _MyGameManager_OnNightVote;
            PlayerBaseConditions._MyGameManager.OnLastSecondsOfVoting += _MyGameManager_OnLastSecondsOfVoting;
            PlayerBaseConditions._MyGameManager.OnNight += _MyGameManager_OnNight;
            PlayerBaseConditions._MyGameManager.OnDay += _MyGameManager_OnDay;

            PlayerBaseConditions._MyGameControllerComponents.VoteTab.OnVoteTabOpened += VoteTab_OnVoteTabOpened;
            PlayerBaseConditions._MyGameControllerComponents.VoteTab.OnVoteTabClosed += VoteTab_OnVoteTabClosed;
            PlayerBaseConditions._MyGameControllerComponents.DeathTab.OnDeathTabOpened += DeathTab_OnDeathTabOpened;
            PlayerBaseConditions._MyGameControllerComponents.DeathTab.OnDeathTabClosed += DeathTab_OnDeathTabClosed;
            PlayerBaseConditions._MyGameControllerComponents.GameControllerRPC.OnLostPlayer += GameControllerRPC_OnLostPlayer;

            PlayerBaseConditions.MyComponents.GetGameManagerEvents.OnClickNightVote += GetGameManagerEvents_OnClickNightVote;
            PlayerBaseConditions.MyComponents.GetGameManagerEvents.OnClickDayVote += GetGameManagerEvents_OnClickDayVote;
            PlayerBaseConditions.MyComponents.PlayerRPC.OnLizardMixPlayersNamesChars += PlayerRPC_OnLizardMixPlayersNamesChars;

            PlayerBaseConditions._MyGameControllerComponents.NetworkCallbacks.OnPlayerWelcome += NetworkCallbacks_OnPlayerWelcome;
            PlayerBaseConditions._MyGameControllerComponents.NetworkCallbacks.OnPlayerJoinedGame += NetworkCallbacks_OnPlayerJoinedGame;
            PlayerBaseConditions._MyGameControllerComponents.NetworkCallbacks.OnPlayerLeftGame += NetworkCallbacks_OnPlayerLeftGame;
            PlayerBaseConditions._MyGameControllerComponents.NetworkCallbacks.OnMasterSwitched += NetworkCallbacks_OnMasterSwitched;
        });
    }
   
    void OnDisable()
    {
        playerSelfTimer.OnUpdateTimerText -= PlayerSelfTimer_OnUpdateTimerText;

        if (PlayerBaseConditions._InstanceIsThis())
        {
            PlayerComponents.instance.PlayerEvents.OnSetOwnedAvatarButtonSprite -= PlayerEvents_OnSetOwnedAvatarButtonSprite;
        }

        if (PlayerBaseConditions._IsMyGameManagerNotNull)
        {
            PlayerBaseConditions._MyGameManager.OnDayVote -= _MyGameManager_OnDayVote;
            PlayerBaseConditions._MyGameManager.OnNightVote -= _MyGameManager_OnNightVote;
            PlayerBaseConditions._MyGameManager.OnLastSecondsOfVoting -= _MyGameManager_OnLastSecondsOfVoting;
            PlayerBaseConditions._MyGameManager.OnNight -= _MyGameManager_OnNight;
            PlayerBaseConditions._MyGameManager.OnDay -= _MyGameManager_OnDay;

            PlayerBaseConditions._MyGameControllerComponents.VoteTab.OnVoteTabOpened -= VoteTab_OnVoteTabOpened;
            PlayerBaseConditions._MyGameControllerComponents.VoteTab.OnVoteTabClosed -= VoteTab_OnVoteTabClosed;
            PlayerBaseConditions._MyGameControllerComponents.DeathTab.OnDeathTabOpened -= DeathTab_OnDeathTabOpened;
            PlayerBaseConditions._MyGameControllerComponents.DeathTab.OnDeathTabClosed -= DeathTab_OnDeathTabClosed;           
            PlayerBaseConditions._MyGameControllerComponents.GameControllerRPC.OnLostPlayer -= GameControllerRPC_OnLostPlayer;

            PlayerBaseConditions._MyGameControllerComponents.NetworkCallbacks.OnPlayerWelcome -= NetworkCallbacks_OnPlayerWelcome;
            PlayerBaseConditions._MyGameControllerComponents.NetworkCallbacks.OnPlayerJoinedGame -= NetworkCallbacks_OnPlayerJoinedGame;
            PlayerBaseConditions._MyGameControllerComponents.NetworkCallbacks.OnPlayerLeftGame -= NetworkCallbacks_OnPlayerLeftGame;
            PlayerBaseConditions._MyGameControllerComponents.NetworkCallbacks.OnMasterSwitched -= NetworkCallbacks_OnMasterSwitched;
        }

        if(PlayerBaseConditions.MyComponents != null)
        {
            PlayerBaseConditions.MyComponents.GetGameManagerEvents.OnClickNightVote -= GetGameManagerEvents_OnClickNightVote;
            PlayerBaseConditions.MyComponents.GetGameManagerEvents.OnClickDayVote -= GetGameManagerEvents_OnClickDayVote;
            PlayerBaseConditions.MyComponents.PlayerRPC.OnLizardMixPlayersNamesChars -= PlayerRPC_OnLizardMixPlayersNamesChars;
        }
    }
   
    #region PlayerSelfTimer_OnUpdateTimerText
    void PlayerSelfTimer_OnUpdateTimerText(byte second)
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            getTimer.Timer = second.ToString();
        }
    }
    #endregion

    #region PlayerEvents_OnSetOwnedAvatarButtonSprite + Coroutine
    void PlayerEvents_OnSetOwnedAvatarButtonSprite(int AvatarButtonIndex, string roleName)
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            StartCoroutine(SetOwnedAvatarButtonSpriteLocalView(AvatarButtonIndex));
        }
    }
    
    IEnumerator SetOwnedAvatarButtonSpriteLocalView(int AvatarButtonIndex)
    {
        InstantiatePlayers instPlayer = GameControllerComponents.instance.InstantiatePlayers;
        SetPlayerInfo playerInfo = GetComponent<SetPlayerInfo>();

        yield return new WaitForSeconds(1);

        string avatarButtonName = instPlayer.AvatarButtonController[0].AvatarButtonName;
        string avatarName = instPlayer.AvatarButtonController[0].AvatarName;
        Sprite avatarSprite = instPlayer.AvatarButtonController[0].AvatarSprite;
        Sprite hiddenAvatarSprite = instPlayer.AvatarButtonController[0].HiddenAvatarSprite;

        yield return null;

        instPlayer.AvatarButtonController[0].AvatarColor(Color.yellow);
        instPlayer.AvatarButtonController[0].AvatarButtonName = playerInfo.ActorNumber.ToString();
        instPlayer.AvatarButtonController[0].AvatarName = "(You)" + playerInfo.Name;
        instPlayer.AvatarButtonController[0].AvatarSprite = instPlayer.AvatarButtonController[AvatarButtonIndex].HiddenAvatarSprite;
        instPlayer.AvatarButtonController[0].HiddenAvatarSprite = instPlayer.AvatarButtonController[AvatarButtonIndex].HiddenAvatarSprite;


        yield return null;

        if(AvatarButtonIndex > 0)
        {
            instPlayer.AvatarButtonController[AvatarButtonIndex].AvatarButtonName = avatarButtonName;
            instPlayer.AvatarButtonController[AvatarButtonIndex].AvatarName = avatarName;
            instPlayer.AvatarButtonController[AvatarButtonIndex].AvatarSprite = avatarSprite;
            instPlayer.AvatarButtonController[AvatarButtonIndex].HiddenAvatarSprite = hiddenAvatarSprite;
        }       
    }
    #endregion

    #region _MyGameManager_OnDayVote + _MyGameManager_OnNightVote + VoteTab_OnVoteTabOpened + VoteTab_OnVoteTabClosed
    void _MyGameManager_OnDayVote()
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            if(PlayerBaseConditions.MyComponents.PlayerSerializeView.RoleName != RoleNames.Infected)
            {
                foreach (var avatar in PlayerBaseConditions.Avatars)
                {
                    avatar.IsPlayerAllowedToSee = true;
                }               
            }

            if (PlayerBaseConditions.MyComponents.PlayerGamePlayStatus.IsPlayerStillPlaying)
            {
                PlayerBaseConditions._MyGameControllerComponents.VoteTab.PlayVoteTabAnimation("Day " + PlayerBaseConditions._MostSyncedPlayerTimer.NightsCount);
            }
            else
            {
                return;
            }                     
        }
    }

    void _MyGameManager_OnNightVote()
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            if (PlayerBaseConditions.MyComponents.PlayerSerializeView.RoleName != RoleNames.Infected)
            {
                foreach (var avatar in PlayerBaseConditions.Avatars)
                {
                    avatar.IsPlayerAllowedToSee = false;
                }
            }          

            #region VoteTab
            string text =
                PlayerBaseConditions.MyComponents.PlayerSerializeView.RoleName == RoleNames.Citizen ? "\n \n Citizen!" :
                PlayerBaseConditions.MyComponents.PlayerSerializeView.RoleName == RoleNames.Infected ? "\n \n Choose a player you want to get rid of!" :
                PlayerBaseConditions.MyComponents.PlayerSerializeView.RoleName == RoleNames.Lizard ? "\n \n Choose a player you want to make incomprehensible for sheriff!" :
                PlayerBaseConditions.MyComponents.PlayerSerializeView.RoleName == RoleNames.Medic ? "\n \n Choose a player you want to save!" :
                PlayerBaseConditions.MyComponents.PlayerSerializeView.RoleName == RoleNames.Sheriff ? "\n \n Choose a player you want to discover!" :
                PlayerBaseConditions.MyComponents.PlayerSerializeView.RoleName == RoleNames.Soldier ? "\n \n Choose a player you want to get rid of!" :
                null;

            if (PlayerBaseConditions.MyComponents.PlayerSerializeView.RoleName == RoleNames.Citizen || !PlayerBaseConditions.MyComponents.PlayerGamePlayStatus.IsPlayerStillPlaying)
            {
                return;
            }
            else
            {
                PlayerBaseConditions._MyGameControllerComponents.VoteTab.PlayVoteTabAnimation("Night (" + PlayerBaseConditions.OrdinalNumbers(PlayerBaseConditions._MostSyncedPlayerTimer.NightsCount + 1) + ")" + text);
            }             
            #endregion
        }
    }

    void VoteTab_OnVoteTabOpened()
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            PlayerBaseConditions.HideUnhideVFXByTags(Tags.LostPlayerSignVFX, false);
        }
    }

    void VoteTab_OnVoteTabClosed()
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            foreach (var avatar in PlayerBaseConditions.Avatars)
            {
                if(int.TryParse(avatar.AvatarButtonName, out int actorNumber))
                {
                    if(PlayerBaseConditions._PlayerTagObject(actorNumber) != PlayerBaseConditions._LocalPlayerTagObject && PlayerBaseConditions._PlayerTagObject(actorNumber).GetComponent<PlayerGamePlayStatus>().IsPlayerStillPlaying)
                    {
                        PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(avatar.transform.position, 6);
                    }
                }
            }

            PlayerBaseConditions.HideUnhideVFXByTags(Tags.LostPlayerSignVFX, true);
        }            
    }

    #endregion

    #region _MyGameManager_OnLastSecondsOfVoting
    void _MyGameManager_OnLastSecondsOfVoting()
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            if (PlayerBaseConditions.MyComponents.PlayerGamePlayStatus.IsPlayerStillPlaying)
            {             
                PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(PlayerBaseConditions._MyGameControllerComponents.ObjectsHolder.TimerTickerSpawnPoint, 2);
            }
        }
    }
    #endregion

    #region GetGameManagerEvents_OnClickNightVote + GetGameManagerEvents_OnClickDayVote + DestroyVotePointVFX()
    void GetGameManagerEvents_OnClickNightVote(int obj)
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            DestroyVFX(Tags.VotePointsTag, true);
        }
    }

    void GetGameManagerEvents_OnClickDayVote(int obj)
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            DestroyVFX(Tags.VotePointsTag, true);
        }
    }

    void DestroyVFX(string tag, bool isDestroyingManual)
    {
        if(GameObject.FindGameObjectWithTag(tag) != null)
        {
            foreach (var vfx in GameObject.FindGameObjectsWithTag(tag))
            {
                if (isDestroyingManual)
                {
                    vfx.GetComponent<IParticleVFXBaseScript>()?.ManualDestroy();
                }
                else
                {
                    Destroy(vfx);
                }
            }
        }       
    }
    #endregion

    #region _MyGameManager_OnDay + _MyGameManager_OnNight + DestroyVFX + ResetMixedPlayersNamesChars
    void _MyGameManager_OnDay()
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            DestroyVFX(Tags.VotePointsTag, true);
            DestroyVFX(Tags.NightTimeVFX, false);            
        }
    }

    void _MyGameManager_OnNight()
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            DestroyVFX(Tags.VotePointsTag, true);

            ResetMixedPlayersNamesChars();
        }
    }
    #endregion

    #region PhotonNetwork.Player lostPlayer + GameControllerRPC_OnLostPlayer + DeathTab_OnDeathTabOpened + DeathTab_OnDeathTabClosed + AvatarUpdate + VFX + CreateBloodDecayVFX

    Player lostPlayer;

    void GameControllerRPC_OnLostPlayer(Player lostPlayer)
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            this.lostPlayer = lostPlayer;

            PlayerBaseConditions._MyGameControllerComponents.DeathTab.PlayDeathTabAnimation();
            PlayerBaseConditions._MyGameControllerComponents.DeathTab._Text = lostPlayer == PlayerBaseConditions.LocalPlayer ? "You Lost!" :
                lostPlayer.NickName + " Lost!";
        }
    }

    void DeathTab_OnDeathTabOpened()
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            PlayerBaseConditions.HideUnhideVFXByTags(Tags.LostPlayerSignVFX, false);
        }       
    }

    void DeathTab_OnDeathTabClosed()
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            foreach (var avatar in FindObjectsOfType<AvatarButtonController>())
            {
                if (avatar.AvatarButtonName == lostPlayer.ActorNumber.ToString())
                {
                    AvatarUpdate(avatar);

                    if (PlayerBaseConditions._PlayerTagObject(lostPlayer.ActorNumber).GetComponent<ISetPlayerRoleProps>().RoleName == RoleNames.Infected || PlayerBaseConditions._PlayerTagObject(lostPlayer.ActorNumber).GetComponent<ISetPlayerRoleProps>().RoleName == RoleNames.Lizard)
                    {
                        VFX(true, avatar);

                        PlayerBaseConditions.MyComponents.GetTeamsUI.UpdateTeamsCount(true);
                    }
                    else
                    {
                        VFX(false, avatar);

                        PlayerBaseConditions.MyComponents.GetTeamsUI.UpdateTeamsCount(false);
                    }                   
                }
            }

            PlayerBaseConditions.HideUnhideVFXByTags(Tags.LostPlayerSignVFX, true);
        }
    }

    void AvatarUpdate(AvatarButtonController avatar)
    {
        //avatar.LostTextObj.alpha = 1;
        avatar.AvatarColor(new Color32(207, 24, 30, 255));
        avatar.ShowHiddenAvatarSprite();
    }

    void VFX(bool infecteds, AvatarButtonController avatar)
    {
        PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(new Vector3(avatar.transform.position.x, avatar.transform.position.y, 0), 7);
        PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(new Vector3(avatar.transform.position.x, avatar.transform.position.y, 0), 13);
        CreateBloodDecayVFX(avatar);

        //if (infecteds)
        //{
        //    PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(new Vector3(avatar.transform.position.x, avatar.transform.position.y, 0), 12);
        //    PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(new Vector3(avatar.transform.position.x, avatar.transform.position.y, 0), 14);
        //}
        //else
        //{
        //    PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(new Vector3(avatar.transform.position.x, avatar.transform.position.y, 0), 7);
        //    PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(new Vector3(avatar.transform.position.x, avatar.transform.position.y, 0), 13);
        //}
    }

    void CreateBloodDecayVFX(AvatarButtonController avatar)
    {
        Transform parent = FindObjectOfType<GameUIComponents>().transform;
        Vector2 position = avatar.transform.position;
        int sibilingIndex = parent.Find("AvatarsContainer").GetSiblingIndex() + 1;
        int vfxIndex = 16;

        PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(parent, position, sibilingIndex, vfxIndex);
    }
    #endregion

    #region PlayerRPC_OnLizardMixPlayersNamesChars + MixPlayersNamesChars + ResetMixedPlayersNamesChars + MixPlayersNamesVFX
    void PlayerRPC_OnLizardMixPlayersNamesChars()
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            MixPlayersNamesChars();
        }           
    }

    void MixPlayersNamesChars()
    {
        if (PlayerBaseConditions.MyComponents.TemporaryDatas.AvatarRealNames.Count < 1)
        {
            foreach (var avatar in PlayerBaseConditions.Avatars)
            {
                if (int.TryParse(avatar.AvatarButtonName, out int actrNmbr))
                {
                    if (actrNmbr != PlayerBaseConditions.LocalPlayer.ActorNumber)
                    {
                        PlayerBaseConditions.MyComponents.TemporaryDatas.AvatarRealNames.Add(avatar.AvatarName);

                        string newAvatarName = null;

                        for (int i = 0; i < avatar.AvatarName.Length; i++)
                        {
                            newAvatarName += avatar.AvatarName[Random.Range(0, avatar.AvatarName.Length - 1)];
                        }

                        MixPlayersNamesVFX(avatar.transform.Find("NameBar").GetComponentInChildren<UnityEngine.UI.Text>().transform);
                        avatar.AvatarName = newAvatarName;
                    }
                }
            }
        }
    }

    void ResetMixedPlayersNamesChars()
    {
        if (PlayerBaseConditions.MyComponents != null)
        {
            if (PlayerBaseConditions.MyComponents.TemporaryDatas.AvatarRealNames.Count > 0)
            {
                foreach (var avatar in PlayerBaseConditions.Avatars)
                {
                    if (int.TryParse(avatar.AvatarButtonName, out int actrNmbr))
                    {
                        if (actrNmbr != PlayerBaseConditions.LocalPlayer.ActorNumber)
                        {
                            avatar.AvatarName = PlayerBaseConditions.MyComponents.TemporaryDatas.AvatarRealNames[0];
                            PlayerBaseConditions.MyComponents.TemporaryDatas.AvatarRealNames.RemoveAt(0);
                        }
                    }
                }
            }
        }
    }

    void MixPlayersNamesVFX(Transform nameTransform)
    {
        Vector3 pos = new Vector3(nameTransform.position.x, nameTransform.position.y, 0);
        PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(pos, 15);
    }

    #endregion

    #region NetworkCallbacks_OnPlayerWelcome + NetworkCallbacks_OnPlayerLeftGame + NetworkCallbacks_OnPlayerJoinedGame + UpdateOnlinePlayersList
    void NetworkCallbacks_OnPlayerWelcome(Player localPlayer)
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            string text = "<b>" + PlayerBaseConditions.Chat.ChatContainer.childCount + ") " + "<PAUTIK>" + "</b>" + "\n" + "Welcome! " + localPlayer.NickName;
            PlayerBaseConditions.Chat.InstantiateChatText(text, new Color32(242, 255, 0, 255), new Color32(255, 19, 0, 50), 1);

            UpdateOnlinePlayersList();
        }
    }
    private void NetworkCallbacks_OnPlayerLeftGame(Player player)
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            string text = "<b>" + PlayerBaseConditions.Chat.ChatContainer.childCount + ") " + "<PAUTIK>" + "</b>" + "\n" + player.NickName + " left the game!";
            PlayerBaseConditions.Chat.InstantiateChatText(text, new Color32(242, 255, 0, 255), new Color32(255, 19, 0, 50), 1);
        }

        UpdateOnlinePlayersList();
    }

    private void NetworkCallbacks_OnPlayerJoinedGame(Player player)
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            string text = "<b>" + PlayerBaseConditions.Chat.ChatContainer.childCount + ") " + "<PAUTIK>" + "</b>" + "\n" + player.NickName + " joined the game!";
            PlayerBaseConditions.Chat.InstantiateChatText(text, new Color32(242, 255, 0, 255), new Color32(255, 19, 0, 50), 1);
        }

        UpdateOnlinePlayersList();
    }

    void UpdateOnlinePlayersList()
    {
        if (PlayerBaseConditions._MyGameControllerComponents.OnlinePlayersList.GameStartCanvasGroup.interactable)
        {
            PlayerBaseConditions._MyGameControllerComponents.OnlinePlayersList.OnlinePlayersName = null;

            foreach (var player in PhotonNetwork.PlayerList)
            {
                PlayerBaseConditions._MyGameControllerComponents.OnlinePlayersList.OnlinePlayersName += player.NickName + " is online..." + "\n";
            }
        }
    }
    #endregion

    #region NetworkCallbacks_OnMasterSwitched + StartGameAsNewMaster
    void NetworkCallbacks_OnMasterSwitched(Player newMaster)
    {       
        if(PhotonNetwork.LocalPlayer == newMaster)
        {
            StartGameAsNewMaster(newMaster);
        }
    }

    void StartGameAsNewMaster(Player newMaster)
    {
        if (!PlayerBaseConditions._MyGameControllerComponents.GameStart.IsGameStarted)
        {
            PlayerBaseConditions._PlayerTagObject(newMaster.ActorNumber).GetComponent<PlayerComponents>().SetPlayersRoleAsMasterClient.enabled = true;
            PlayerBaseConditions._PlayerTagObject(newMaster.ActorNumber).GetComponent<PlayerComponents>().SetPlayersRoleAsMasterClient.OnMasterSwitched();
        }
    }
    #endregion
}
