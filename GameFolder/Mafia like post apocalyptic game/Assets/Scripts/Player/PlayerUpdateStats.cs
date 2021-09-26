using UnityEngine;
using System;
using Photon.Pun;
using System.Collections;

public class PlayerUpdateStats : MonoBehaviourPun
{
    [SerializeField] StatsValue _StatsValue;
    [SerializeField] internal Conditions _Conditions;
    [Serializable] struct StatsValue
    {
        [SerializeField] internal int rank;
        [SerializeField] internal int totalTimePlayed;
        [SerializeField] internal int points;

        [SerializeField] internal int asSurvivor;
        [SerializeField] internal int asDoctor;
        [SerializeField] internal int asSheriff;
        [SerializeField] internal int asSoldier;
        [SerializeField] internal int asInfected;
        [SerializeField] internal int asLizard;
        
        [SerializeField] internal int overallSkills;
        [SerializeField] internal int survivorSkills;
        [SerializeField] internal int doctorSkills;
        [SerializeField] internal int sheriffSkills;
        [SerializeField] internal int soldierSkills;
        [SerializeField] internal int infectedSkills;
        [SerializeField] internal int lizardSkills;

        [SerializeField] internal int winAsSurvivor;
        [SerializeField] internal int lostAsSurvivor;
        [SerializeField] internal int winAsInfected;
        [SerializeField] internal int lostAsInfected;

        [SerializeField] internal int countPlayedAsSurvivor;
        [SerializeField] internal int countPlayedAsDoctor;
        [SerializeField] internal int countPlayedAsSheriff;
        [SerializeField] internal int countPlayedAsSoldier;
        [SerializeField] internal int countPlayedAsLizard;
        [SerializeField] internal int countPlayedAsInfected;
    }
    [Serializable] internal struct Conditions
    {
        [SerializeField] internal bool isPlayerRoleSet;
    }


    void Awake()
    {
        _StatsValue = new StatsValue();
    }

    #region GetPlayerStats
    internal void GetPlayerStats(Action UpdateCoroutine)
    {
        PlayerBaseConditions.PlayfabManager.PlayfabStats.GetPlayerStats(PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.UserID].ToString(), getPlayerStats =>
        {
            _StatsValue.rank = getPlayerStats.rank;
            _StatsValue.totalTimePlayed = getPlayerStats.totalTimePlayed + 1;
            _StatsValue.points = getPlayerStats.points + 25;

            StatsByRoles(RoleName => 
            {
                if (RoleName == RoleNames.Citizen)
                {
                    _StatsValue.asSurvivor = getPlayerStats.asSurvivor + 1;
                    _StatsValue.survivorSkills = getPlayerStats.survivorSkills + 5;
                }
                if (RoleName == RoleNames.Medic)
                {
                    _StatsValue.asDoctor = getPlayerStats.asDoctor + 1;
                    _StatsValue.doctorSkills = getPlayerStats.doctorSkills + 5;
                }
                if (RoleName == RoleNames.Sheriff)
                {
                    _StatsValue.asSheriff = getPlayerStats.asSheriff + 1;
                    _StatsValue.sheriffSkills = getPlayerStats.sheriffSkills + 5;
                }
                if (RoleName == RoleNames.Soldier)
                {
                    _StatsValue.asSoldier = getPlayerStats.asSoldier + 1;
                    _StatsValue.soldierSkills = getPlayerStats.soldierSkills + 5;
                }
                if (RoleName == RoleNames.Infected)
                {
                    _StatsValue.asInfected = getPlayerStats.asInfected + 1;
                    _StatsValue.infectedSkills = getPlayerStats.infectedSkills + 5;
                }
                if (RoleName == RoleNames.Lizard)
                {
                    _StatsValue.asLizard = getPlayerStats.asLizard + 1;
                    _StatsValue.lizardSkills = getPlayerStats.lizardSkills + 5;
                }
            });

            UpdateCoroutine?.Invoke();
        });
    }
    #endregion

    #region StatsByRoles
    internal void StatsByRoles(Action<string> Roles)
    {
        Roles?.Invoke(PlayerBaseConditions.PlayerRoleName(PhotonNetwork.LocalPlayer.ActorNumber));
    }
    #endregion

    #region UpdatePlayerStats
    internal void UpdatePlayerStats()
    {
        PlayerBaseConditions.PlayfabManager.PlayfabStats.UpdatePlayerStats(PhotonNetwork.LocalPlayer.UserId, UpdatePlayerStats =>
        {           
            PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.Rank, _StatsValue.rank); // Rank
            PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.TotalTimePlayed, _StatsValue.totalTimePlayed); // TotalTimePlayed
            PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.Points, _StatsValue.points); // Points

            PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsSurvivor, _StatsValue.asSurvivor); // AsSurvivor 
            PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsDoctor, _StatsValue.asDoctor); // AsDoctor 
            PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsSheriff, _StatsValue.asSheriff); // AsSheriff
            PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsSoldier, _StatsValue.asSoldier); // AsSoldier
            PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsInfected, _StatsValue.asInfected); // AsInfected
            PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.AsWitch, _StatsValue.asLizard); // AsWitch
            
            PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.OverallSkills, _StatsValue.overallSkills); // OverallSkills
            PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.SurvivorSkills, _StatsValue.survivorSkills); // SurvivorSkills
            PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.DoctorSkills, _StatsValue.doctorSkills); // DoctorSkills
            PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.SheriffSkills, _StatsValue.sheriffSkills); // SheriffSkills
            PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.SoldierSkills, _StatsValue.soldierSkills); // SoldierSkills
            PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.InfectedSkills, _StatsValue.infectedSkills); // InfectedSkills
            PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.LizardSkills, _StatsValue.lizardSkills); // LizardSkills
        });
    }
    #endregion

    #region PlayerStats
    void PlayerStats(PlayFab.ServerModels.UpdatePlayerStatisticsRequest UpdatePlayerStats, string keyName, int value)
    {
        UpdatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = keyName, Value = value });
    }
    #endregion

    #region UpdatePlayerStatsCoroutine
    internal IEnumerator UpdatePlayerStatsCoroutine()
    {
        yield return new WaitForSeconds(1);
        UpdatePlayerStats();
        _Conditions.isPlayerRoleSet = true;
    }
    #endregion
}
