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
                _PlayerUpdateStats._StatsValue.rank = Stats.rank < 1 ? 1 : Stats.rank;
                _PlayerUpdateStats._StatsValue.totalTimePlayed = Stats.totalTimePlayed + 1;
                _PlayerUpdateStats._StatsValue.points = Stats.points + 25;

                PlayerBaseConditions.StatsByRoles(RoleName => 
                {
                    if (RoleName == RoleNames.Citizen)
                    {
                        _PlayerUpdateStats._StatsValue.asSurvivor = Stats.asSurvivor + 1;
                        _PlayerUpdateStats._StatsValue.survivorSkills = Stats.survivorSkills + 5;
                    }
                    if (RoleName == RoleNames.Medic)
                    {
                        _PlayerUpdateStats._StatsValue.asDoctor = Stats.asDoctor + 1;
                        _PlayerUpdateStats._StatsValue.doctorSkills = Stats.doctorSkills + 5;
                    }
                    if (RoleName == RoleNames.Sheriff)
                    {
                        _PlayerUpdateStats._StatsValue.asSheriff = Stats.asSheriff + 1;
                        _PlayerUpdateStats._StatsValue.sheriffSkills = Stats.sheriffSkills + 5;
                    }
                    if (RoleName == RoleNames.Soldier)
                    {
                        _PlayerUpdateStats._StatsValue.asSoldier = Stats.asSoldier + 1;
                        _PlayerUpdateStats._StatsValue.soldierSkills = Stats.soldierSkills + 5;
                    }
                    if (RoleName == RoleNames.Infected)
                    {
                        _PlayerUpdateStats._StatsValue.asInfected = Stats.asInfected + 1;
                        _PlayerUpdateStats._StatsValue.infectedSkills = Stats.infectedSkills + 5;
                    }
                    if (RoleName == RoleNames.Lizard)
                    {
                        _PlayerUpdateStats._StatsValue.asLizard = Stats.asLizard + 1;
                        _PlayerUpdateStats._StatsValue.lizardSkills = Stats.lizardSkills + 5;
                    }
                });
            }, 
            ()=> 
            {
                PlayerBaseConditions.PlayfabManager.PlayfabStats.UpdatePlayerStats(PhotonNetwork.LocalPlayer.UserId, UpdatePlayerStats => 
                {
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.Rank, _PlayerUpdateStats._StatsValue.rank); // Rank
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.TotalTimePlayed, _PlayerUpdateStats._StatsValue.totalTimePlayed); // TotalTimePlayed
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.Points, _PlayerUpdateStats._StatsValue.points); // Points

                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsSurvivor, _PlayerUpdateStats._StatsValue.asSurvivor); // AsSurvivor 
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsDoctor, _PlayerUpdateStats._StatsValue.asDoctor); // AsDoctor 
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsSheriff, _PlayerUpdateStats._StatsValue.asSheriff); // AsSheriff
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsSoldier, _PlayerUpdateStats._StatsValue.asSoldier); // AsSoldier
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsInfected, _PlayerUpdateStats._StatsValue.asInfected); // AsInfected
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsWitch, _PlayerUpdateStats._StatsValue.asLizard); // AsWitch

                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.OverallSkills, _PlayerUpdateStats._StatsValue.overallSkills); // OverallSkills
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.SurvivorSkills, _PlayerUpdateStats._StatsValue.survivorSkills); // SurvivorSkills
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.DoctorSkills, _PlayerUpdateStats._StatsValue.doctorSkills); // DoctorSkills
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.SheriffSkills, _PlayerUpdateStats._StatsValue.sheriffSkills); // SheriffSkills
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.SoldierSkills, _PlayerUpdateStats._StatsValue.soldierSkills); // SoldierSkills
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.InfectedSkills, _PlayerUpdateStats._StatsValue.infectedSkills); // InfectedSkills
                    _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.LizardSkills, _PlayerUpdateStats._StatsValue.lizardSkills); // LizardSkills
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
