using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ServerModels;
using System;

public class PlayfabStats : MonoBehaviour
{
    public StatsValue _StatsValue;  
    public struct StatsValue
    {
        internal int rank;
        internal int totalTimePlayed;
        internal int points;       
        internal int winAsSurvivor;
        internal int lostAsSurvivor;
        internal int winAsInfected;
        internal int lostAsInfected;
        internal int countPlayedAsSurvivor;
        internal int countPlayedAsDoctor;
        internal int countPlayedAsSheriff;
        internal int countPlayedAsSoldier;
        internal int countPlayedAsLizard;
        internal int countPlayedAsInfected;
    }

    #region UpdatePlayerStats
    public void UpdatePlayerStats(string playfabId, Action<UpdatePlayerStatisticsRequest> UpdatePlayerStats)
    {
        UpdatePlayerStatisticsRequest requestUpdatePlayerStats = new UpdatePlayerStatisticsRequest();
        requestUpdatePlayerStats.PlayFabId = playfabId;
        requestUpdatePlayerStats.Statistics = new List<StatisticUpdate>();

        UpdatePlayerStats(requestUpdatePlayerStats);

        PlayFabServerAPI.UpdatePlayerStatistics(requestUpdatePlayerStats,
            result =>
            {
                
            },
            error =>
            {
                print(error.ErrorMessage);
            });
    }
    #endregion

    #region GetPlayerStats
    public void GetPlayerStats(string playfabId, Action<StatsValue> GetPlayerStats)
    {
        GetPlayerStatisticsRequest getPlayerStats = new GetPlayerStatisticsRequest();
        getPlayerStats.PlayFabId = playfabId;

        PlayFabServerAPI.GetPlayerStatistics(getPlayerStats,

            result =>
            {
                GetPlayerStats(new StatsValue
                {
                    rank = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.Rank).Value,
                    totalTimePlayed = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.TotalTimePlayed).Value
                });
            },
            error =>
            {
                print(error.ErrorMessage);
            });
    }
    #endregion


   
}
