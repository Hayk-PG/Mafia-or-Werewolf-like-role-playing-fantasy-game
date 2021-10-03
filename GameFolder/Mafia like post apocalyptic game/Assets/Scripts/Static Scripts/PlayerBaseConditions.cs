using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class PlayerBaseConditions : MonoBehaviourPun
{
    #region Player

    /// <summary>
    /// Checking if the player is a local 
    /// </summary>
    /// <returns></returns>
    public static bool _InstanceIsThis()
    {
        return PlayerComponents.instance;
    }

    /// <summary>
    /// An instance of PlayerComponents
    /// </summary>
    public static PlayerComponents MyComponents
    {
        get
        {
            if (PlayerComponents.instance != null)
            {
                return PlayerComponents.instance;
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Another way of checking if this player is the local one by photonView
    /// </summary>
    /// <param name="viewId"></param>
    /// <returns></returns>
    public static bool _IsPhotonviewMine(int viewId)
    {
        PhotonView photonView = PhotonNetwork.GetPhotonView(viewId);
        return photonView.IsMine;
    }

    /// <summary>
    /// Player's photonViewId
    /// </summary>
    public static int ViewId
    {
        get
        {
            return _LocalPlayerTagObject.GetComponent<PhotonView>().ViewID;
        }
    }
  
    /// <summary>
    /// Checking if this player is the master client
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <returns></returns>
    public static bool _PlayerIsMasterClient(int actorNumber)
    {
        return PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).IsMasterClient;
    }

    public static PhotonView MasterClient
    {
        get
        {
            GameObject masterClientObj = PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.MasterClientId).TagObject as GameObject;
            return masterClientObj.GetComponent<PhotonView>();
        }
    }

    /// <summary>
    /// Local player's gameObject
    /// </summary>
    public static GameObject _LocalPlayerTagObject
    {
        get
        {
            return (GameObject)PhotonNetwork.LocalPlayer.TagObject;
        }
    }

    /// <summary>
    /// Get any player by actor number
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <returns></returns>
    public static GameObject _PlayerTagObject(int actorNumber)
    {
        return PhotonNetwork.CurrentRoom.GetPlayer(actorNumber) != null ? (GameObject)PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).TagObject : null;
    }

    /// <summary>
    /// PhotonNetwork LocalPlayer
    /// </summary>
    public static Player LocalPlayer
    {
        get
        {
            return PhotonNetwork.LocalPlayer;
        }
    }  

    /// <summary>
    /// Photonnetwork player
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <returns></returns>
    public static Player Player(int actorNumber)
    {
        return PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);
    }

    /// <summary>
    /// Checking if provided actor number belongs to local player
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <returns></returns>
    public static bool IsActorNumberMine(int actorNumber)
    {
        return PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).IsLocal;
    }

    /// <summary>
    /// Checking if given actor number exists in general
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <returns></returns>
    public static bool IsActorNumberNotNull(int actorNumber)
    {
        return PhotonNetwork.CurrentRoom.GetPlayer(actorNumber) != null ? true : false;
    }

    /// <summary>
    /// Checking if LocalPlayer exists
    /// </summary>
    public static bool _IsLocalPlayerNotNull
    {
        get
        {
            return _LocalPlayerTagObject;
        }
    }

    /// <summary>
    /// Checking if player is the local player by Photonnetwok.IsLocal
    /// </summary>
    public static bool _IsLocalPlayer
    {
        get
        {
            return PhotonNetwork.LocalPlayer.IsLocal;
        }
    }

    /// <summary>
    /// Local player's playfabId
    /// </summary>
    /// <returns></returns>
    public static string OwnPlayfabId
    {
        get => PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(PlayerKeys.UserID)? PhotonNetwork.LocalPlayer.CustomProperties[PlayerKeys.UserID].ToString() : null;
    }

    /// <summary>
    /// Other player's playfabId
    /// </summary>
    /// <param name="actorNumber"></param>
    /// <returns></returns>
    public static string PlayerPlayfabId(int actorNumber)
    {
        return PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).CustomProperties.ContainsKey(PlayerKeys.UserID) ? PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).CustomProperties[PlayerKeys.UserID].ToString() : null;
    }

    #endregion

    #region CanPlayerVote
    /// <summary>
    /// Can player vote
    /// </summary>
    public static bool _CanPlayerVoteGlobal
    {
        get
        {
            return MyComponents.PlayerGamePlayStatus.CanPlayerVote && MyComponents.PlayerGamePlayStatus.IsPlayerStillPlaying;
        }
    }
    #endregion

    #region GameManager

    /// <summary>
    /// An instance of GameManager
    /// </summary>
    public static GameManager _MyGameManager
    {
        get
        {
            if(GameManager.instance != null)
            {
                return GameManager.instance;
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Checking if an instance of MyGameManager exists
    /// </summary>
    public static bool _IsMyGameManagerNotNull
    {
        get
        {
            return _MyGameManager;
        }
    }

    #endregion

    #region GameControllerComponents

    /// <summary>
    /// An instance of GameControllerComponents
    /// </summary>
    public static GameControllerComponents _MyGameControllerComponents
    {
        get
        {
            if(GameControllerComponents.instance != null)
            {
                return GameControllerComponents.instance;
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Checking if an instance of GameControllerComponents exists
    /// </summary>
    public static bool _IsMyGameControllerComponentesNotNull
    {
        get
        {
            return _MyGameControllerComponents;
        }
    }

    #endregion

    #region NetworkManagerComponents
    public static NetworkManagerComponents NetworkManagerComponents
    {
        get
        {
            if (NetworkManagerComponents.Instance != null)
            {
                return NetworkManagerComponents.Instance;
            }
            else
            {
                return null;
            }
        }
    }
    
    public static bool IsNetworkManagerComponentsNotNull
    {
        get => NetworkManagerComponents;
    }
    #endregion

    #region Time && Vote

    /// <summary>
    /// Getting MostSyncedPlayerTimer directly from PlayerBaseConditions
    /// </summary>
    public static PlayerSelfTimer _MostSyncedPlayerTimer
    {
        get
        {
            if(_IsMyGameControllerComponentesNotNull && _MyGameControllerComponents.SyncPlayersTimer.MostSyncedPlayerTimer != null)
            {
                return _MyGameControllerComponents.SyncPlayersTimer.MostSyncedPlayerTimer;
            }
            else
            {
                if(MyComponents != null)
                {
                    return MyComponents.PlayerSelfTimer;
                }
                else
                {
                    return null;
                }
            }           
        }
    }

    /// <summary>
    /// Checking if an instance of _MostSyncedPlayerTimer exists
    /// </summary>
    public static bool IsMostSyncedPlayerTimerNotNull
    {
        get
        {
            return _MostSyncedPlayerTimer;
        }
    }

    /// <summary>
    /// Checking if it's time to vote in a day phase
    /// </summary>
    public static bool _IsTimeToDayVote
    {
        get
        {
            if (IsMostSyncedPlayerTimerNotNull)
            {
                return !_MostSyncedPlayerTimer.IsNight && _MostSyncedPlayerTimer.Second >= 60;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Checking if it's time to vote in a night phase
    /// </summary>
    public static bool _IsTimeToNightVote
    {
        get
        {
            if (IsMostSyncedPlayerTimerNotNull)
            {
                return _MostSyncedPlayerTimer.IsNight && _MostSyncedPlayerTimer.Second >= 30;
            }
            else
            {
                return false;
            }
        }
    }

    public static bool _HasRoundBeenChanged
    {
        get
        {
            if (IsMostSyncedPlayerTimerNotNull)
            {
                return _MostSyncedPlayerTimer.IsNight && _MostSyncedPlayerTimer.Second < 30 || !_MostSyncedPlayerTimer.IsNight && _MostSyncedPlayerTimer.Second < 60;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Detecting last 10 seconds of voting
    /// </summary>
    public static bool IsVotesLastSeconds
    {
        get
        {
            if (IsMostSyncedPlayerTimerNotNull)
            {
                if(_MostSyncedPlayerTimer.IsNight && _MostSyncedPlayerTimer.Second >= 45 || !_MostSyncedPlayerTimer.IsNight && _MostSyncedPlayerTimer.Second >= 75)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    public static bool Night
    {
        get
        {
            if (IsMostSyncedPlayerTimerNotNull)
            {
                return _MostSyncedPlayerTimer.IsNight;
            }
            else
            {
                return false;
            }
        } 
    }
    public static bool Day
    {
        get
        {
            if (IsMostSyncedPlayerTimerNotNull)
            {
                return !_MostSyncedPlayerTimer.IsNight;
            }
            else
            {
                return false;
            }
        }
    }

    #endregion

    #region Avatars

    /// <summary>
    /// Get all avatars controller
    /// </summary>
    public static AvatarButtonController[] Avatars
    {
        get
        {
            if (_IsMyGameControllerComponentesNotNull)
            {
                return _MyGameControllerComponents.InstantiatePlayers.AvatarButtonController;
            }
            else
            {
                return null;
            }
        }
    }
    public static AvatarButtonController Avatar(int actorNumber)
    {
        return System.Array.Find(Avatars, avatar => avatar.name == actorNumber.ToString());
    }
    #endregion

    #region OrdinalNumbers
    public static string OrdinalNumbers(int number)
    {
        if(number == 1)
        {
            return "First";
        }
        if(number == 2)
        {
            return "Second";
        }
        if (number == 3)
        {
            return "Third";
        }
        if (number == 4)
        {
            return "Fourth";
        }
        if (number == 5)
        {
            return "Fifth";
        }
        if (number == 6)
        {
            return "Sixth";
        }
        if (number == 7)
        {
            return "Seventh";
        }
        if (number == 8)
        {
            return "Eighth";
        }
        if (number == 9)
        {
            return "Ninth";
        }
        if (number == 10)
        {
            return "Tenth";
        }
        if (number == 11)
        {
            return "Eleventh";
        }
        if (number == 12)
        {
            return "Twelfth";
        }
        if (number == 13)
        {
            return "Thirteenth";
        }
        if (number == 14)
        {
            return "Fourteenth";
        }
        if (number == 15)
        {
            return "Fifteenth";
        }
        if (number == 16)
        {
            return "Sixteenth";
        }
        if (number == 17)
        {
            return "Seventeenth";
        }
        if (number == 18)
        {
            return "Eighteenth";
        }
        if (number == 19)
        {
            return "Nineteenth";
        }
        if (number == 20)
        {
            return "Twentieth";
        }
        if (number == 21)
        {
            return "Twenty-first";
        }
        else
        {
            return null;
        }
    }
    #endregion

    #region HideUnhideVFXByTags
    public static void HideUnhideVFXByTags(string tag, bool isActive)
    {
        if(GameObject.FindGameObjectWithTag(tag) != null)
        {
            foreach (var vfxObj in GameObject.FindGameObjectsWithTag(tag))
            {
                vfxObj.GetComponent<IHideUnhideVfx>().Activity(isActive);
            }
        }       
    }
    #endregion

    #region Playfab
    public static PlayfabManager PlayfabManager => PlayfabManager.PM;
    #endregion

    #region PlayerData
    public static PlayerSavedData PlayerSavedData
    {
        get
        {
            return PlayerSavedData.PSD;
        }
    }
    public static PlayerCustomPropertiesController PlayerCustomPropertiesController
    {
        get
        {
            return PlayerCustomPropertiesController.PCPC;
        }
    }
    #endregion

    #region PlayerProfile
    public static Profile PlayerProfile
    {
        get => Profile.instance;
    }
    #endregion


    //NEW

    #region Role && RoleButton
    public static string PlayerRoleName(int actorNumber)
    {        
        string playerRoleName = 
            System.Array.Find(FindObjectOfType<GameManagerSetPlayersRoles>()._RoleButtonControllers.RoleButtons, roleButton => roleButton._OwnerInfo.OwnerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber) != null &&
            System.Array.Find(FindObjectOfType<GameManagerSetPlayersRoles>()._RoleButtonControllers.RoleButtons, roleButton => roleButton._OwnerInfo.OwnerActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)._GameInfo.RoleName != null?
            System.Array.Find(FindObjectOfType<GameManagerSetPlayersRoles>()._RoleButtonControllers.RoleButtons, roleButton => roleButton._OwnerInfo.OwnerActorNumber == PhotonNetwork.CurrentRoom.GetPlayer(actorNumber).ActorNumber)._GameInfo.RoleName : null;
        return playerRoleName;
    }
    public static RoleButtonController GetRoleButton(int actorNumber)
    {
        return System.Array.Find(FindObjectOfType<GameManagerSetPlayersRoles>()._RoleButtonControllers.RoleButtons, roleButton => roleButton._OwnerInfo.OwnerActorNumber == actorNumber);
    }
    public static int RoleIndex(int actorNumber)
    {
        if (PlayerRoleName(actorNumber) != null)
        {
            return PlayerRoleName(actorNumber) == RoleNames.Citizen ? 0 : PlayerRoleName(actorNumber) == RoleNames.Medic ? 1 : PlayerRoleName(actorNumber) == RoleNames.Sheriff ? 2 : PlayerRoleName(actorNumber) == RoleNames.Soldier ? 3 : PlayerRoleName(actorNumber) == RoleNames.Infected ? 4 : 5;
        }
        else return 0;
    }
    #endregion
   
    #region Photonview
    public static bool IsPhotonviewMine(PhotonView photonView)
    {
        return photonView.IsMine;
    }

    public static bool AmOwner(PhotonView photonView)
    {
        return photonView.AmOwner;
    }
    #endregion

    #region UISounds
    public static UiSoundsBaseScript UiSounds
    {
        get
        {
            if(_MySceneManager.CurrentScene().name == SceneNames.MenuScene)
                return FindObjectOfType<UISounds>();

            if (_MySceneManager.CurrentScene().name == SceneNames.GameScene)
                return FindObjectOfType<UISoundsInGame>();

            else return null;
        }
    }
    #endregion
}
