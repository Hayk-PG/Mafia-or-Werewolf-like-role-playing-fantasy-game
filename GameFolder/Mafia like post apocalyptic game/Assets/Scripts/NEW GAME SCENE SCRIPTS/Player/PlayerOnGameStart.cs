using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class PlayerOnGameStart : MonoBehaviourPun,IReset
{   
    internal bool IsPlayerFirstTimeInThisRoom
    {
        get => !PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(PlayerKeys.SetPlayersRoleKeys.RoomName) || PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(PlayerKeys.SetPlayersRoleKeys.RoomName) && (string)PhotonNetwork.LocalPlayer.CustomProperties[PlayerKeys.SetPlayersRoleKeys.RoomName] != PhotonNetwork.CurrentRoom.Name;
    }
    internal bool IsPlayerRoleSet
    {
        get => PlayerBaseConditions.PlayerRoleName(PhotonNetwork.LocalPlayer.ActorNumber) != null;
    }
    PlayerUpdateStats _PlayerUpdateStats { get; set; }
    InformPlayerRole _InformPlayerRole { get; set; }


    void Awake()
    {
        _PlayerUpdateStats = GetComponent<PlayerUpdateStats>();
        _InformPlayerRole = FindObjectOfType<InformPlayerRole>();
    }

    void Update()
    {
        if (photonView.IsMine && photonView.AmOwner)
        {
            if (IsPlayerFirstTimeInThisRoom)
            {
                if (IsPlayerRoleSet && !_PlayerUpdateStats._Conditions.isPlayerRoleSet)
                {
                    StartCoroutine(InformPlayerRolePopUp());
                    UpdatePlayedRolesStats();
                    PlayerCustomPropertiesController.PCPC.SetPhotonPlayerLastRoomName(PhotonNetwork.CurrentRoom.Name);
                }               
            }
        }
    }

    #region UpdatePlayedRolesStats
    void UpdatePlayedRolesStats()
    {
        _PlayerUpdateStats.GetAndUpdatePlayfabStats(
            Stats => 
            {
                _PlayerUpdateStats._StatsValue = new PlayerUpdateStats.StatsValue
                (
                    Stats.Rank < 1 ? 1 : Stats.Rank,
                    Stats.TotalTimePlayed +1,
                    Stats.Points + 25,
                    new int[] 
                    {
                        Stats.RolesPlayedCount[0] += PlayerBaseConditions.PlayerRoleName(PhotonNetwork.LocalPlayer.ActorNumber) == RoleNames.Citizen ? 1:0,
                        Stats.RolesPlayedCount[1] += PlayerBaseConditions.PlayerRoleName(PhotonNetwork.LocalPlayer.ActorNumber) == RoleNames.Medic ? 1:0,
                        Stats.RolesPlayedCount[2] += PlayerBaseConditions.PlayerRoleName(PhotonNetwork.LocalPlayer.ActorNumber) == RoleNames.Sheriff ? 1:0,
                        Stats.RolesPlayedCount[3] += PlayerBaseConditions.PlayerRoleName(PhotonNetwork.LocalPlayer.ActorNumber) == RoleNames.Soldier ? 1:0,
                        Stats.RolesPlayedCount[4] += PlayerBaseConditions.PlayerRoleName(PhotonNetwork.LocalPlayer.ActorNumber) == RoleNames.Infected ? 1:0,
                        Stats.RolesPlayedCount[5] += PlayerBaseConditions.PlayerRoleName(PhotonNetwork.LocalPlayer.ActorNumber) == RoleNames.Lizard ? 1:0,
                    });
            }, 
            ()=> 
            {
                PlayerBaseConditions.PlayfabManager.PlayfabStats.UpdatePlayerStats(PhotonNetwork.LocalPlayer.UserId, UpdatePlayerStats =>
                {
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.Rank, _PlayerUpdateStats._StatsValue.Rank); // Rank
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.TotalTimePlayed, _PlayerUpdateStats._StatsValue.TotalTimePlayed); // TotalTimePlayed
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.Points, _PlayerUpdateStats._StatsValue.Points); // Points

                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsSurvivor, _PlayerUpdateStats._StatsValue.RolesPlayedCount[0]); // AsSurvivor 
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsDoctor, _PlayerUpdateStats._StatsValue.RolesPlayedCount[1]); // AsDoctor 
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsSheriff, _PlayerUpdateStats._StatsValue.RolesPlayedCount[2]); // AsSheriff
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsSoldier, _PlayerUpdateStats._StatsValue.RolesPlayedCount[3]); // AsSoldier
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsInfected, _PlayerUpdateStats._StatsValue.RolesPlayedCount[4]); // AsInfected
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsWitch, _PlayerUpdateStats._StatsValue.RolesPlayedCount[5]); // AsWitch
                });
            });
    }
    #endregion

    #region InformPlayerRolePopUp
    IEnumerator InformPlayerRolePopUp()
    {
        yield return new WaitForSeconds(1);
        _InformPlayerRole.OnPopUp("Your role is " + PlayerBaseConditions.PlayerRoleName(PhotonNetwork.LocalPlayer.ActorNumber) + "!",
            Array.Find(FindObjectOfType<GameManagerSetPlayersRoles>()._RoleButtonControllers.RoleButtons, roleButton => roleButton._OwnerInfo.OwnerActorNumber == Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber)._UI.RoleImage);
    }
    #endregion

    #region IReset
    public void ResetWhileGameEndCoroutineIsRunning()
    {
        PlayerBaseConditions.LocalPlayer.CustomProperties.Remove(PlayerKeys.SetPlayersRoleKeys.RoomName);
    }

    public void ResetAtTheEndOfTheGameEndCoroutine()
    {
        throw new NotImplementedException();
    }
    #endregion
}
