using UnityEngine;
using System;
using Photon.Pun;
using System.Collections;

public class PlayerUpdateStats : MonoBehaviourPun, IReset
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

        public StatsValue(int rank, int totalTimePlayed, int points, int[] rolesPlayedCount)
        {
            Rank = rank;
            TotalTimePlayed = totalTimePlayed;
            Points = points;
            RolesPlayedCount = rolesPlayedCount;
        }
    }
    public struct Conditions
    {
        public bool isPlayerRoleSet { get; set; }
    }

    public StatsValue _StatsValue;
    public Conditions _Conditions;


    #region GetAndUpdatePlayfabStats
    internal void GetAndUpdatePlayfabStats(Action<PlayfabStats.StatsValue> GetPlayfabStats, Action UpdatePlayfabStats)
    {
        PlayerBaseConditions.PlayfabManager.PlayfabStats.GetPlayerStats(PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.UserID].ToString(), getPlayerStats =>
        {
            StartCoroutine(GetPlayfabStatsCoroutine(()=> GetPlayfabStats?.Invoke(getPlayerStats)));
            StartCoroutine(UpdatePlayerStatsCoroutine(UpdatePlayfabStats));
        });
    }
    #endregion

    #region PlayerStats
    public void PlayerStats(PlayFab.ServerModels.UpdatePlayerStatisticsRequest UpdatePlayerStats, string keyName, int value)
    {
        UpdatePlayerStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = keyName, Value = value });
    }
    #endregion

    #region GetPlayfabStatsCoroutine
    IEnumerator GetPlayfabStatsCoroutine(Action action)
    {
        yield return new WaitForSeconds(1);
        action();
    }
    #endregion

    #region UpdatePlayerStatsCoroutine
    internal IEnumerator UpdatePlayerStatsCoroutine(Action UpdatePlayfabStats)
    {
        yield return new WaitForSeconds(2);
        UpdatePlayfabStats?.Invoke();
        _Conditions.isPlayerRoleSet = true;
    }
    #endregion
   
    #region IReset
    public void ResetWhileGameEndCoroutineIsRunning()
    {
        _Conditions.isPlayerRoleSet = false;       
    }

    public void ResetAtTheEndOfTheGameEndCoroutine()
    {
        
    }
    #endregion
}
