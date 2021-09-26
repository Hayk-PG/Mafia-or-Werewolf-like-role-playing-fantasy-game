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

        internal int asSurvivor;
        internal int asDoctor;
        internal int asSheriff;
        internal int asSoldier;
        internal int asInfected;
        internal int asLizard;

        internal int overallSkills;
        internal int survivorSkills;
        internal int doctorSkills;
        internal int sheriffSkills;
        internal int soldierSkills;
        internal int infectedSkills;
        internal int lizardSkills;

        internal int winAsSurvivor;
        internal int lostAsSurvivor;
        internal int winAsInfected;
        internal int lostAsInfected;
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
                    rank = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.Rank) != null? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.Rank).Value: 0,
                    totalTimePlayed = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.TotalTimePlayed) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.TotalTimePlayed).Value : 0,
                    points = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.Points) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.Points).Value : 0,

                    asSurvivor = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsSurvivor) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsSurvivor).Value : 0,
                    asDoctor = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsDoctor) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsDoctor).Value : 0,
                    asSheriff = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsSheriff) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsSheriff).Value : 0,
                    asSoldier = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsSoldier) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsSoldier).Value : 0,
                    asInfected = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsInfected) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsInfected).Value : 0,
                    asLizard = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsWitch) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.AsWitch).Value : 0,

                    overallSkills = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.OverallSkills) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.OverallSkills).Value : 0,
                    survivorSkills = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.SurvivorSkills) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.SurvivorSkills).Value : 0,
                    doctorSkills = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.DoctorSkills) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.DoctorSkills).Value : 0,
                    sheriffSkills = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.SheriffSkills) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.SheriffSkills).Value : 0,
                    soldierSkills = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.SoldierSkills) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.SoldierSkills).Value : 0,
                    infectedSkills = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.InfectedSkills) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.InfectedSkills).Value : 0,
                    lizardSkills = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.LizardSkills) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.LizardSkills).Value : 0,

                    winAsSurvivor = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.WinAsSurvivor) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.WinAsSurvivor).Value : 0,
                    lostAsSurvivor = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.LostAsSurvivor) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.LostAsSurvivor).Value : 0,
                    winAsInfected = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.WinAsInfected) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.WinAsInfected).Value : 0,
                    lostAsInfected = result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.LostAsInfected) != null ? result.Statistics.Find(stats => stats.StatisticName == PlayerKeys.StatisticKeys.LostAsInfected).Value : 0,
                }); 
            },
            error =>
            {
                print(error.ErrorMessage);
            });
    }
    #endregion 
}
