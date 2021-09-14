using UnityEngine;
using System;
using System.Collections.Generic;
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
        [SerializeField] internal int asSurvivor;
        [SerializeField] internal int asDoctor;
        [SerializeField] internal int asSheriff;
        [SerializeField] internal int asSoldier;
        [SerializeField] internal int asInfected;
        [SerializeField] internal int asLizard;
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

            if (PlayerBaseConditions.PlayerRoleName(PhotonNetwork.LocalPlayer.ActorNumber) == RoleNames.Citizen) _StatsValue.asSurvivor = getPlayerStats.countPlayedAsSurvivor + 1; 
            if (PlayerBaseConditions.PlayerRoleName(PhotonNetwork.LocalPlayer.ActorNumber) == RoleNames.Medic) _StatsValue.asDoctor = getPlayerStats.countPlayedAsDoctor + 1;
            if (PlayerBaseConditions.PlayerRoleName(PhotonNetwork.LocalPlayer.ActorNumber) == RoleNames.Sheriff) _StatsValue.asSheriff = getPlayerStats.countPlayedAsSheriff + 1;
            if (PlayerBaseConditions.PlayerRoleName(PhotonNetwork.LocalPlayer.ActorNumber) == RoleNames.Soldier) _StatsValue.asSoldier = getPlayerStats.countPlayedAsSoldier + 1;
            if (PlayerBaseConditions.PlayerRoleName(PhotonNetwork.LocalPlayer.ActorNumber) == RoleNames.Infected) _StatsValue.asInfected = getPlayerStats.countPlayedAsInfected + 1;
            if (PlayerBaseConditions.PlayerRoleName(PhotonNetwork.LocalPlayer.ActorNumber) == RoleNames.Lizard) _StatsValue.asLizard = getPlayerStats.countPlayedAsLizard + 1;

            PlayerCustomPropertiesController.PCPC.SetPhotonPlayerRolesStats(new List<int>()
            {
                 _StatsValue.asSurvivor,
                 _StatsValue.asDoctor,
                 _StatsValue.asSheriff,
                 _StatsValue.asSoldier,
                 _StatsValue.asInfected,
                 _StatsValue.asLizard
            });

            UpdateCoroutine?.Invoke();
        });
    }
    #endregion

    #region UpdatePlayerStats
    internal void UpdatePlayerStats()
    {
        PlayerBaseConditions.PlayfabManager.PlayfabStats.UpdatePlayerStats(PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.UserID].ToString(), updatePlayerStats =>
        {
            updatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.Rank, Value = _StatsValue.rank });
            updatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.TotalTimePlayed, Value = _StatsValue.totalTimePlayed });

            updatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsSurvivor, Value = _StatsValue.asSurvivor });
            updatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsDoctor, Value = _StatsValue.asDoctor });
            updatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsSheriff, Value = _StatsValue.asSheriff });
            updatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsSoldier, Value = _StatsValue.asSoldier });
            updatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsInfected, Value = _StatsValue.asInfected });
            updatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsWitch, Value = _StatsValue.asLizard });
        });
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
