using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSetPlayersRoles : MonoBehaviourPun
{
    [Serializable] public class RoleButtonControllers
    {
        [SerializeField] RoleButtonController[] roleButtons;

        public RoleButtonController[] RoleButtons
        {
            get => roleButtons;
        }
    }
    [Serializable] public class ListOfRoles
    {
        [SerializeField]
        List<string> playersRolesNames = new List<string>(20)
        {
            RoleNames.Citizen, RoleNames.Citizen, RoleNames.Infected, RoleNames.Medic, RoleNames.Infected, RoleNames.Soldier, RoleNames.Infected, RoleNames.Citizen,
            RoleNames.Lizard, RoleNames.Sheriff, RoleNames.Citizen, RoleNames.Infected, RoleNames.Citizen, RoleNames.Infected, RoleNames.Citizen, RoleNames.Infected,
            RoleNames.Citizen, RoleNames.Infected, RoleNames.Soldier, RoleNames.Lizard
        };

        public List<string> PlayersRolesNames
        {
            get => playersRolesNames;
        }
    }
    [Serializable] public class ImageOfRoles
    {
        [SerializeField] Sprite[] citizenRoleSprites;
        [SerializeField] Sprite[] doctorRoleSprites;
        [SerializeField] Sprite[] sheriffRoleSprites;
        [SerializeField] Sprite[] soldierRoleSprite;
        [SerializeField] Sprite[] infectedRoleSprites;
        [SerializeField] Sprite[] witcherRoleSprite;
        [SerializeField] Sprite[] defaultSprites;

        public Sprite[] CitizenRoleSprites
        {
            get => citizenRoleSprites;
        }
        public Sprite[] DoctorRoleSprites
        {
            get => doctorRoleSprites;
        }
        public Sprite[] SheriffRoleSprites
        {
            get => sheriffRoleSprites;
        }
        public Sprite[] SoldierRoleSprite
        {
            get => soldierRoleSprite;
        }
        public Sprite[] InfectedRoleSprites
        {
            get => infectedRoleSprites;
        }
        public Sprite[] WitcherRoleSprites
        {
            get => witcherRoleSprite;
        }
        public Sprite[] DefaultSprite
        {
            get => defaultSprites;
        }
    }
    [Serializable] public class Condition
    {
        [SerializeField] bool hasPlayersRolesBeenSet;

        public bool HasPlayersRolesBeenSet
        {
            get => hasPlayersRolesBeenSet;
            set => hasPlayersRolesBeenSet = value;
        }
    }

    public RoleButtonControllers _RoleButtonControllers;
    public ListOfRoles _ListOfRoles;
    public ImageOfRoles _ImageOfRoles;
    public Condition _Condition;


    public void SetPlayersRoles()
    {
        StartCoroutine(SetPlayersRolesCoroutine());
    }

    IEnumerator SetPlayersRolesCoroutine()
    {
        while(photonView.IsMine && !_Condition.HasPlayersRolesBeenSet)
        {
            List<int> rolesIndexList = new List<int>();
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                rolesIndexList.Add(i);
            }

            yield return null;

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Player player = PhotonNetwork.PlayerList[i];

                int randomIndex = UnityEngine.Random.Range(0, rolesIndexList.Count);
                int roleIndex = rolesIndexList[randomIndex];

                photonView.RPC("PlayersRolesRPC", RpcTarget.AllBufferedViaServer, player.ActorNumber, roleIndex, i);

                rolesIndexList.Remove(roleIndex);
                yield return new WaitForSeconds(0.1f);
            }

            _Condition.HasPlayersRolesBeenSet = true;
            PhotonNetwork.CurrentRoom.IsOpen = !_Condition.HasPlayersRolesBeenSet;
        }
    } 

    [PunRPC]
    public void PlayersRolesRPC(int actorNumber, int roleIndex, int roleButtonIndex)
    {
        Player player = PhotonNetwork.CurrentRoom.GetPlayer(actorNumber);

        //For everyone

        _RoleButtonControllers.RoleButtons[roleButtonIndex]._OwnerInfo.OwenrUserId = player.UserId;
        _RoleButtonControllers.RoleButtons[roleButtonIndex]._OwnerInfo.OwnerActorNumber = player.ActorNumber;
        _RoleButtonControllers.RoleButtons[roleButtonIndex]._OwnerInfo.OwnerName = player.NickName;

        _RoleButtonControllers.RoleButtons[roleButtonIndex].ObjName = player.UserId;

        _RoleButtonControllers.RoleButtons[roleButtonIndex]._UI.Name = player.NickName;
        _RoleButtonControllers.RoleButtons[roleButtonIndex]._UI.VotesCount = 0;
        _RoleButtonControllers.RoleButtons[roleButtonIndex]._UI.RoleImage = RoleSprite(player.CustomProperties[PlayerKeys.GenderKey].ToString() == PlayerKeys.Male ? 0 : 1, _ListOfRoles.PlayersRolesNames[roleIndex]);
        _RoleButtonControllers.RoleButtons[roleButtonIndex]._UI.VisibleToEveryoneImage = _ImageOfRoles.DefaultSprite[player.CustomProperties[PlayerKeys.GenderKey].ToString() == PlayerKeys.Male ? 0 : 1];

        _RoleButtonControllers.RoleButtons[roleButtonIndex]._GameInfo.RoleIndex = roleIndex;
        _RoleButtonControllers.RoleButtons[roleButtonIndex]._GameInfo.RoleName = _ListOfRoles.PlayersRolesNames[roleIndex];

        SubToEvents.SubscribeToEvents(player.TagObject != null, () => 
        {
            GameObject MyTagObject = player.TagObject as GameObject;

            if (MyTagObject.GetComponent<PhotonView>().IsMine)
            {
                if (roleButtonIndex != 0)
                {
                    #region Other player
                    _RoleButtonControllers.RoleButtons[roleButtonIndex]._OwnerInfo.OwenrUserId = _RoleButtonControllers.RoleButtons[0]._OwnerInfo.OwenrUserId;
                    _RoleButtonControllers.RoleButtons[roleButtonIndex]._OwnerInfo.OwnerActorNumber = _RoleButtonControllers.RoleButtons[0]._OwnerInfo.OwnerActorNumber;
                    _RoleButtonControllers.RoleButtons[roleButtonIndex]._OwnerInfo.OwnerName = _RoleButtonControllers.RoleButtons[0]._OwnerInfo.OwnerName;
                    _RoleButtonControllers.RoleButtons[roleButtonIndex]._OwnerInfo.OwnerObj = null;

                    _RoleButtonControllers.RoleButtons[roleButtonIndex].ObjName = _RoleButtonControllers.RoleButtons[0].ObjName;

                    _RoleButtonControllers.RoleButtons[roleButtonIndex]._UI.Name = _RoleButtonControllers.RoleButtons[0]._UI.Name;
                    _RoleButtonControllers.RoleButtons[roleButtonIndex]._UI.VotesCount = _RoleButtonControllers.RoleButtons[0]._UI.VotesCount;
                    _RoleButtonControllers.RoleButtons[roleButtonIndex]._UI.RoleImage = _RoleButtonControllers.RoleButtons[0]._UI.RoleImage;
                    _RoleButtonControllers.RoleButtons[roleButtonIndex]._UI.VisibleToEveryoneImage = _RoleButtonControllers.RoleButtons[0]._UI.VisibleToEveryoneImage;

                    _RoleButtonControllers.RoleButtons[roleButtonIndex]._GameInfo.RoleIndex = _RoleButtonControllers.RoleButtons[0]._GameInfo.RoleIndex;
                    _RoleButtonControllers.RoleButtons[roleButtonIndex]._GameInfo.RoleName = _RoleButtonControllers.RoleButtons[0]._GameInfo.RoleName;
                    #endregion                    
                }

                #region Local player
                _RoleButtonControllers.RoleButtons[0]._OwnerInfo.OwenrUserId = player.UserId;
                _RoleButtonControllers.RoleButtons[0]._OwnerInfo.OwnerActorNumber = player.ActorNumber;
                _RoleButtonControllers.RoleButtons[0]._OwnerInfo.OwnerName = player.NickName;
                _RoleButtonControllers.RoleButtons[0]._OwnerInfo.OwnerObj = null;

                _RoleButtonControllers.RoleButtons[0].ObjName = player.UserId;

                _RoleButtonControllers.RoleButtons[0]._UI.Name = player.NickName;
                _RoleButtonControllers.RoleButtons[0]._UI.VotesCount = 0;
                _RoleButtonControllers.RoleButtons[0]._UI.RoleImage = RoleSprite(player.CustomProperties[PlayerKeys.GenderKey].ToString() == PlayerKeys.Male ? 0 : 1, _ListOfRoles.PlayersRolesNames[roleIndex]);
                _RoleButtonControllers.RoleButtons[0]._UI.VisibleToEveryoneImage = RoleSprite(player.CustomProperties[PlayerKeys.GenderKey].ToString() == PlayerKeys.Male ? 0 : 1, _ListOfRoles.PlayersRolesNames[roleIndex]);

                _RoleButtonControllers.RoleButtons[0]._GameInfo.RoleIndex = roleIndex;
                _RoleButtonControllers.RoleButtons[0]._GameInfo.RoleName = _ListOfRoles.PlayersRolesNames[roleIndex];
                #endregion
            }
        });
    }

    Sprite RoleSprite(int gender, string roleName)
    {
        Sprite roleImage = null;

        switch (roleName)
        {
            case RoleNames.Citizen: roleImage = _ImageOfRoles.CitizenRoleSprites[gender]; break;
            case RoleNames.Medic: roleImage = _ImageOfRoles.DoctorRoleSprites[gender]; break;
            case RoleNames.Sheriff: roleImage = _ImageOfRoles.SheriffRoleSprites[gender]; break;
            case RoleNames.Soldier: roleImage = _ImageOfRoles.SoldierRoleSprite[gender]; break;
            case RoleNames.Infected: roleImage = _ImageOfRoles.InfectedRoleSprites[gender]; break;
            case RoleNames.Lizard: roleImage = _ImageOfRoles.WitcherRoleSprites[gender]; break;
        }

        return roleImage;
    }







}



