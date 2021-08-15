﻿using UnityEngine;
using System;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;

public class PlayerUpdateStats : MonoBehaviour
{
    [SerializeField] StatsValue _StatsValue;
    [Serializable] struct StatsValue
    {
        [SerializeField] internal int rank;
        [SerializeField] internal int totalTimePlayed;
    }

    void Awake()
    {
        _StatsValue = new StatsValue();
    }

    void Start()
    {
        StartCoroutine(UpdatePlayerStatsCoroutine());
    }

    #region GetPlayerStats
    void GetPlayerStats()
    {
        print(PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.UserID].ToString());

        PlayerBaseConditions.PlayfabManager.PlayfabStats.GetPlayerStats(getPlayerStats =>
        {
            _StatsValue.rank = getPlayerStats.rank;
            _StatsValue.totalTimePlayed = getPlayerStats.totalTimePlayed;

            PlayerBaseConditions.PlayerCustomPropertiesController.SetPhotonPlayerStats(new List<int>
            { _StatsValue.rank,
              _StatsValue.totalTimePlayed
            });
        });
    }
    #endregion

    #region UpdatePlayerStats
    void UpdatePlayerStats()
    {
        PlayerBaseConditions.PlayfabManager.PlayfabStats.UpdatePlayerStats(updatePlayerStats =>
        {
            updatePlayerStats.Statistics.Add(new StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.Rank, Value = _StatsValue.rank });
            updatePlayerStats.Statistics.Add(new StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.TotalTimePlayed, Value = _StatsValue.totalTimePlayed + 1 });           
        });
    }
    #endregion

    #region UpdatePlayerStatsCoroutine
    IEnumerator UpdatePlayerStatsCoroutine()
    {
        GetPlayerStats();

        yield return new WaitForSeconds(1);

        UpdatePlayerStats();
    }
    #endregion
}
