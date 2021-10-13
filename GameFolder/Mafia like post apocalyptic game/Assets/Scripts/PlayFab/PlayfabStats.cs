using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ServerModels;
using System;

public class PlayfabStats : MonoBehaviour
{   
    public class StatsValue
    {
        internal int Rank { get; set; }
        internal int TotalTimePlayed { get; set; }
        internal int Points { get; set; }

        /// <summary>
        /// 0: asSurvivor 1: asDoctor 2: asSheriff 3: asSoldier 4: asInfected 5: asLizard
        /// </summary>
        internal int[] RolesPlayedCount { get; set; } = new int[6];

        public int Win { get; set; }
        public int Lost { get; set; }

        public StatsValue(int rank, int totalTimePlayed, int points, int win, int lost, int[] rolesPlayedCount)
        {
            Rank = rank;
            TotalTimePlayed = totalTimePlayed;
            Points = points;
            Win = win;
            Lost = lost;
            RolesPlayedCount = rolesPlayedCount;
        }
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
                    (
                    result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.Rank) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.Rank).Value : 0,
                    result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.TotalTimePlayed) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.TotalTimePlayed).Value : 0,
                    result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.Points) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.Points).Value : 0,
                    result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.Win) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.Win).Value : 0,
                    result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.Lost) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.Lost).Value : 0,

                    new int[] 
                    {
                        result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsSurvivor) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsSurvivor).Value : 0,
                        result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsDoctor) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsDoctor).Value : 0,
                        result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsSheriff) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsSheriff).Value : 0,
                        result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsSoldier) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsSoldier).Value : 0,
                        result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsInfected) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsInfected).Value : 0,
                        result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsWitch) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsWitch).Value : 0,
                    }));
            },
            error =>
            {
                print(error.ErrorMessage);
            });
    }
    #endregion 
}
