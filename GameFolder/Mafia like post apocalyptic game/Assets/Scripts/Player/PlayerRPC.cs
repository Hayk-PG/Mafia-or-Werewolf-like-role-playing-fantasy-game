using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRPC : MonoBehaviourPun
{
    public static PlayerRPC MyRPC;

    #region Liazrd's events
    public event System.Action OnLizardMixPlayersNamesChars;
    #endregion


    void Awake()
    {
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID)) 
        {
            MyRPC = this;
        }
        else
        {
            enabled = false;
        }
    }

    void OnEnable()
    {
        if(PlayerBaseConditions._InstanceIsThis())
        {
            PlayerComponents.instance.PlayerEvents.OnTakeAvatarButtonOwnership += PlayerEvents_OnTakeAvatarButtonOwnership;
            PlayerComponents.instance.PlayerEvents.OnSetOwnedAvatarButtonSprite += PlayerEvents_OnSetOwnedAvatarButtonSprite;

            PlayerComponents.instance.GetGameManagerEvents.OnClickDayVote += GetGameManagerEvents_OnClickDayVote;
            PlayerComponents.instance.GetGameManagerEvents.OnClickNightVote += GetGameManagerEvents_OnClickNightVote;
        }

        SubToEvents.SubscribeToEvents(delegate
        {
            PlayerBaseConditions._MyGameControllerComponents.AbilityButtonsTabController.OnClickAbilityButtons += AbilityButtonsTabController_OnClickAbilityButtons;
            PlayerBaseConditions._MyGameControllerComponents.GlobalInputs.OnChat += GlobalInputs_OnChat;
        });
    }
 
    void OnDisable()
    {
        if (PlayerBaseConditions._InstanceIsThis())
        {
            PlayerComponents.instance.PlayerEvents.OnTakeAvatarButtonOwnership -= PlayerEvents_OnTakeAvatarButtonOwnership;
            PlayerComponents.instance.PlayerEvents.OnSetOwnedAvatarButtonSprite -= PlayerEvents_OnSetOwnedAvatarButtonSprite;

            PlayerComponents.instance.GetGameManagerEvents.OnClickDayVote -= GetGameManagerEvents_OnClickDayVote;
            PlayerComponents.instance.GetGameManagerEvents.OnClickNightVote -= GetGameManagerEvents_OnClickNightVote;
        }

        if (PlayerBaseConditions._IsMyGameManagerNotNull)
        {
            PlayerBaseConditions._MyGameControllerComponents.AbilityButtonsTabController.OnClickAbilityButtons -= AbilityButtonsTabController_OnClickAbilityButtons;
            PlayerBaseConditions._MyGameControllerComponents.GlobalInputs.OnChat -= GlobalInputs_OnChat;
        }
    }

    #region PlayerEvents.OnTakeAvatarButtonOwnership + RPC Call
    void PlayerEvents_OnTakeAvatarButtonOwnership(int RoleNumber,int AvatarButtonIndex)
    {
        MyRPC.photonView.RPC("OnTakeAvatarButtonOwnershipRPC", RpcTarget.All, RoleNumber, AvatarButtonIndex);
    }

    [PunRPC]
    void OnTakeAvatarButtonOwnershipRPC(int RoleNumber, int AvatarButtonIndex)
    {
        SetPlayerInfo playerInfo = GetComponent<SetPlayerInfo>();

        GameControllerComponents.instance.InstantiatePlayers.AvatarButtonController[AvatarButtonIndex].AvatarButtonName = playerInfo.ActorNumber.ToString();
        GameControllerComponents.instance.InstantiatePlayers.AvatarButtonController[AvatarButtonIndex].AvatarName = playerInfo.Name;
    }
    #endregion

    #region PlayerEvents_OnSetOwnedAvatarButtonSprite + RPC Call
    void PlayerEvents_OnSetOwnedAvatarButtonSprite(int AvatarButtonIndex, string roleName)
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            GameObject playerObj = (GameObject)player.TagObject;

            if (roleName == RoleNames.Infected)
            {
                if (playerObj.GetComponent<ISetPlayerRoleProps>().RoleName == RoleNames.Infected)
                {
                    MyRPC.photonView.RPC("OnSetOwnedAvatarButtonSpriteRPC", player, AvatarButtonIndex, true);
                }
                else
                {
                    MyRPC.photonView.RPC("OnSetOwnedAvatarButtonSpriteRPC", player, AvatarButtonIndex, false);
                }
            }
            else
            {
                MyRPC.photonView.RPC("OnSetOwnedAvatarButtonSpriteRPC", player, AvatarButtonIndex, false);
            }
        }
    }

    [PunRPC]
    void OnSetOwnedAvatarButtonSpriteRPC(int AvatarButtonIndex, bool isForInfectedTeamMembers)
    {
        InstantiatePlayers InstPlayer = GameControllerComponents.instance.InstantiatePlayers;
        ISetPlayerRoleProps playerProp = GetComponent<ISetPlayerRoleProps>();
        SetPlayerInfo playerInfo = GetComponent<SetPlayerInfo>();

        InstPlayer.SetPlayerRoleAvatar(playerProp.RoleName, playerInfo, AvatarButtonIndex, playerProp.GenderRandomNumber, isForInfectedTeamMembers);
    }
    #endregion

    #region GetGameManagerEvents_OnClickDayVote + NonLizardsRandomActorNumber + RPC Call
    void GetGameManagerEvents_OnClickDayVote(int actorNumber)
    {
        if (!PlayerBaseConditions._LocalPlayerTagObject.GetComponent<PlayerGamePlayStatus>().GotConfused)
        {
            MyRPC.photonView.RPC("PlayerVoteRPC", RpcTarget.All, actorNumber, PlayerBaseConditions.LocalPlayer.ActorNumber);
        }
        else
        {
            MyRPC.photonView.RPC("PlayerVoteRPC", RpcTarget.All, NonLizardsRandomActorNumber(), PlayerBaseConditions.LocalPlayer.ActorNumber);
        }
    }

    int NonLizardsRandomActorNumber()
    {
        List<int> randomActorNumbers = new List<int>();

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (PlayerBaseConditions._PlayerTagObject(player.ActorNumber).GetComponent<ISetPlayerRoleProps>().RoleName != RoleNames.Lizard)
            {
                randomActorNumbers.Add(player.ActorNumber);
            }
        }

        int rndmActrNmbr = randomActorNumbers[Random.Range(0, randomActorNumbers.Count - 1)];

        return rndmActrNmbr;
    }

    [PunRPC]
    void PlayerVoteRPC(int actorNumber, int ownPlayerActorNumber)
    {
        PlayerBaseConditions._PlayerTagObject(actorNumber).GetComponent<PlayerGamePlayStatus>().VotesCountThatPlayerGot++;
        PlayerBaseConditions._PlayerTagObject(ownPlayerActorNumber).GetComponent<PlayerGamePlayStatus>().VotedNames.Add(PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).NickName);
        PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(PlayerBaseConditions.Avatar(actorNumber).transform.position, 1);
        System.Array.Find(PlayerBaseConditions.Avatars, avatar => avatar.name == ownPlayerActorNumber.ToString()).VoteBarController(PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).NickName, true);
    }   
    #endregion

    #region GetGameManagerEvents_OnClickNightVote + ButtonPosition + RPC Calls
    void GetGameManagerEvents_OnClickNightVote(int actorNumber)
    {
        ISetPlayerRoleProps playerProps = PlayerBaseConditions.MyComponents.PlayerSerializeView;

        if(playerProps.RoleName == RoleNames.Medic)
        {
            Medic(actorNumber);
        }
        if(playerProps.RoleName == RoleNames.Sheriff)
        {
            Sheriff(actorNumber);
        }
        if(playerProps.RoleName == RoleNames.Soldier)
        {
            Soldier(actorNumber);
        }
        if(playerProps.RoleName == RoleNames.Lizard)
        {
            Lizard(actorNumber);
        }
        if(playerProps.RoleName == RoleNames.Infected)
        {
            Infected(actorNumber);
        }
    }

    Vector2 ButtonPosition(int actorNumber)
    {
        return System.Array.Find(FindObjectOfType<GlobalInputs>().AvatarButtons, obj => obj.name == actorNumber.ToString()).transform.position;
    }

    #region Medic + RPC
    void Medic(int actorNumber)
    {
        MyRPC.photonView.RPC("SavePlayerRPC", RpcTarget.All, actorNumber);
        MyRPC.photonView.RPC("DisplayHealVFXToPlayer", PhotonNetwork.CurrentRoom.GetPlayer(actorNumber), actorNumber);

        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(ButtonPosition(actorNumber), 8);
        }
    }

    [PunRPC]
    void SavePlayerRPC(int actorNumber)
    {
        GameObject SavedPlayer = (GameObject)PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).TagObject;
        SavedPlayer.GetComponent<PlayerGamePlayStatus>().GotSaved = true;
    }

    [PunRPC]
    void DisplayHealVFXToPlayer(int actorNumber)
    {
        PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(ButtonPosition(actorNumber), 9);
    }
    #endregion

    #region Sheriff + RPC + RandomSprite
    void Sheriff(int actorNumber)
    {
        //0: Not showing the hiddenAvatar 1: Shows wrong image
        int randomWay = Random.Range(0, 2);

        if (PlayerBaseConditions._PlayerTagObject(actorNumber).GetComponent<PlayerGamePlayStatus>().GotCompromised)
        {
            if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
            {
                if (randomWay == 0)
                {
                    PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(ButtonPosition(actorNumber), 11);
                }
                else
                {
                    PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(ButtonPosition(actorNumber), 11);

                    System.Array.Find(PlayerBaseConditions.Avatars, avatar => avatar.AvatarButtonName == actorNumber.ToString()).AvatarSprite = RandomSprite();
                }
            }           
        }
        else
        {
            MyRPC.photonView.RPC("DiscoverPlayerRPC", RpcTarget.All, actorNumber);
            MyRPC.photonView.RPC("DisplayEyeVFXToPlayer", PhotonNetwork.CurrentRoom.GetPlayer(actorNumber), actorNumber);

            if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
            {
                PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(ButtonPosition(actorNumber), 11);
                System.Array.Find(PlayerBaseConditions.Avatars, avatar => avatar.AvatarButtonName == actorNumber.ToString()).ShowHiddenAvatarSprite();
            }
        }             
    }

    [PunRPC]
    void DiscoverPlayerRPC(int actorNumber)
    {
        GameObject DiscoveredPlayer = (GameObject)PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).TagObject;
        DiscoveredPlayer.GetComponent<PlayerGamePlayStatus>().GotDiscovered = true;
    }

    [PunRPC]
    void DisplayEyeVFXToPlayer(int actorNumber)
    {
        PlayerBaseConditions._MyGameControllerComponents.VFXHolder.CreateVFX(ButtonPosition(actorNumber), 10);
    }

    Sprite RandomSprite()
    {
        int randomSpriteVariation = Random.Range(0, 4);

        Sprite randomSprite =
                        randomSpriteVariation == 0 ?
                        PlayerBaseConditions._MyGameControllerComponents.InstantiatePlayers.CitizenAvatar[Random.Range(0, PlayerBaseConditions._MyGameControllerComponents.InstantiatePlayers.CitizenAvatar.Count)] :
                        randomSpriteVariation == 1 ?
                        PlayerBaseConditions._MyGameControllerComponents.InstantiatePlayers.DoctorAvatar[Random.Range(0, PlayerBaseConditions._MyGameControllerComponents.InstantiatePlayers.DoctorAvatar.Count)] :
                        randomSpriteVariation == 2 ?
                        PlayerBaseConditions._MyGameControllerComponents.InstantiatePlayers.SoldiersAvatar[Random.Range(0, PlayerBaseConditions._MyGameControllerComponents.InstantiatePlayers.SoldiersAvatar.Count)] :
                        randomSpriteVariation == 3 ?
                        PlayerBaseConditions._MyGameControllerComponents.InstantiatePlayers.InfectedAvatar[Random.Range(0, PlayerBaseConditions._MyGameControllerComponents.InstantiatePlayers.InfectedAvatar.Count)] :
                        PlayerBaseConditions._MyGameControllerComponents.InstantiatePlayers.LizardAvatar[Random.Range(0, PlayerBaseConditions._MyGameControllerComponents.InstantiatePlayers.LizardAvatar.Count)];
        return randomSprite;
    }
    #endregion

    #region Soldier + RPC
    void Soldier(int actorNumber)
    {
        MyRPC.photonView.RPC("KillSheriffDiscoveredPlayer", RpcTarget.All, actorNumber);

        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            GameObject voteVFX = Instantiate(PlayerBaseConditions._MyGameControllerComponents.VFXHolder.VFX[1], ButtonPosition(actorNumber), Quaternion.identity);
        }
    }

    [PunRPC]
    void KillSheriffDiscoveredPlayer(int actorNumber)
    {
        GameObject otherPlayer = (GameObject)PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).TagObject;

        if (otherPlayer.GetComponent<PlayerGamePlayStatus>().GotDiscovered)
        {
            otherPlayer.GetComponent<PlayerGamePlayStatus>().VotesCountThatPlayerGot = 3;
        }
    }
    #endregion

    #region Lizard + OnClickAbilityButtonsAsLizard +  CompromisePlayerRPC + MixPlayersNamesCharsRPC + MakePlayerConfusedRPC
    void Lizard(int actorNumber)
    {
        PlayerBaseConditions._MyGameControllerComponents.AbilityButtonsTabController.OnClickToOpenAbilityButtonsTab(actorNumber, PlayerBaseConditions._LocalPlayerTagObject.GetComponent<ISetPlayerRoleProps>().RoleName);       
    }

    void OnClickAbilityButtonsAsLizard(int buttonIndex, int actorNumber)
    {
        if (buttonIndex == 0)
        {
            MyRPC.photonView.RPC("CompromisePlayerRPC", RpcTarget.All, actorNumber);
        }
        if(buttonIndex == 1)
        {
            MyRPC.photonView.RPC("MixPlayersNamesCharsRPC", PhotonNetwork.CurrentRoom.GetPlayer(actorNumber));
        }
        if(buttonIndex == 2)
        {
            MyRPC.photonView.RPC("MakePlayerConfusedRPC", PhotonNetwork.CurrentRoom.GetPlayer(actorNumber));
        }
               
        if (PlayerBaseConditions._IsPhotonviewMine(GetComponent<PhotonView>().ViewID))
        {
            GameObject voteVFX = Instantiate(PlayerBaseConditions._MyGameControllerComponents.VFXHolder.VFX[1], ButtonPosition(actorNumber), Quaternion.identity);
        }
    }

    [PunRPC]
    void CompromisePlayerRPC(int actorNumber)
    {
        GameObject CompromisedPlayer = (GameObject)PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).TagObject;
        CompromisedPlayer.GetComponent<PlayerGamePlayStatus>().GotCompromised = true;
    }

    [PunRPC]
    void MixPlayersNamesCharsRPC()
    {
        PlayerBaseConditions.MyComponents.PlayerRPC.OnLizardMixPlayersNamesChars?.Invoke();
    }

    [PunRPC]
    void MakePlayerConfusedRPC()
    {
        PlayerBaseConditions.MyComponents.PlayerGamePlayStatus.GotConfused = true;
    }
    #endregion

    #region Infected + RPCs
    void Infected(int actorNumber)
    {
        MyRPC.photonView.RPC("VoteToKillPlayerRPC", RpcTarget.All, actorNumber);

        foreach (var infectedPlayer in PhotonNetwork.PlayerList)
        {
            GameObject infected = (GameObject)infectedPlayer.TagObject;

            if(infected.GetComponent<ISetPlayerRoleProps>().RoleName == RoleNames.Infected)
            {
                MyRPC.photonView.RPC("OnlyForInfectedMembers", infectedPlayer, actorNumber);
            }
        }
    }

    [PunRPC]
    void VoteToKillPlayerRPC(int actorNumber)
    {
        GameObject otherPlayer = PlayerBaseConditions._PlayerTagObject(actorNumber);
        otherPlayer.GetComponent<PlayerGamePlayStatus>().VotesCountThatPlayerGot++;
    }

    [PunRPC]
    void OnlyForInfectedMembers(int actorNumber)
    {
        GameObject voteVFX = Instantiate(PlayerBaseConditions._MyGameControllerComponents.VFXHolder.VFX[1], ButtonPosition(actorNumber), Quaternion.identity);
    }
    #endregion

    #endregion

    #region AbilityButtonsTabController_OnClickAbilityButtons
    void AbilityButtonsTabController_OnClickAbilityButtons(int buttonIndex, int actorNumber, string localPlayerRoleName)
    {
        if(localPlayerRoleName == RoleNames.Lizard)
        {
            OnClickAbilityButtonsAsLizard(buttonIndex, actorNumber);
        }

        PlayerBaseConditions._MyGameControllerComponents.AbilityButtonsTabController.OnClickToCloseAbilityButtonsTab();
    }
    #endregion

    #region GlobalInputs_OnChat + SendTextRPC
    void GlobalInputs_OnChat(InputField chatField)
    {
        if(chatField.text.Length > 0)
        {
            MyRPC.photonView.RPC("SendTextRPC", RpcTarget.All, PlayerBaseConditions.LocalPlayer.ActorNumber, chatField.text);

            chatField.text = null;
        }
    }

    [PunRPC]
    void SendTextRPC(int localActorNumber, string text)
    {
        string chatText = FindObjectOfType<ChatController>()._GameObjects.ChatContainer.childCount + ") " + "<b>" + "<" + PhotonNetwork.CurrentRoom.GetPlayer(localActorNumber).NickName + ">" + "</b>" + "\n" + "<i>" + text + "</i>";
        FindObjectOfType<ChatController>().InstantiateChatText(chatText, new Color32(255, 255, 255, 255), new Color32(0, 0, 0, 50), 0);
    }
    #endregion
}
