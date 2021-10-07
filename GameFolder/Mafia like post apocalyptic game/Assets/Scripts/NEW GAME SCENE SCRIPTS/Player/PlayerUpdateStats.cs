using UnityEngine;
using System;
using Photon.Pun;
using System.Collections;

public class PlayerUpdateStats : MonoBehaviourPun
{
    [SerializeField] public StatsValue _StatsValue;
    [SerializeField] public Conditions _Conditions;
    [Serializable] public struct StatsValue
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
    [Serializable] public struct Conditions
    {
        [SerializeField] public bool isPlayerRoleSet;
    }


    void Awake()
    {
        _StatsValue = new StatsValue();
    }

    #region GetAndUpdatePlayfabStats
    internal void GetAndUpdatePlayfabStats(Action<PlayfabStats.StatsValue> GetPlayfabStats, Action UpdatePlayfabStats)
    {
        PlayerBaseConditions.PlayfabManager.PlayfabStats.GetPlayerStats(PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.UserID].ToString(), getPlayerStats =>
        {
            GetPlayfabStats?.Invoke(getPlayerStats);

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

    #region UpdatePlayerStatsCoroutine
    internal IEnumerator UpdatePlayerStatsCoroutine(Action UpdatePlayfabStats)
    {
        yield return new WaitForSeconds(1);
        UpdatePlayfabStats?.Invoke();
        _Conditions.isPlayerRoleSet = true;
    }
    #endregion
}
