using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayfabLeaderboard : MonoBehaviour
{   
    public void ShowLeaderboard(Action<PlayerLeaderboardEntry> Result)
    {
        GetLeaderboardRequest leaderboardRequest = new GetLeaderboardRequest();
        leaderboardRequest.StartPosition = 0;
        leaderboardRequest.StatisticName = PlayerKeys.StatisticKeys.Scores;
        leaderboardRequest.MaxResultsCount = 100;
        leaderboardRequest.ProfileConstraints = new PlayerProfileViewConstraints();
        leaderboardRequest.ProfileConstraints.ShowStatistics = true;
        leaderboardRequest.ProfileConstraints.ShowDisplayName = true;

        PlayFabClientAPI.GetLeaderboard(leaderboardRequest, 
            get => 
            {
                foreach (var item in get.Leaderboard)
                {
                    //StatisticModel totalTimePlayed = new StatisticModel { Name = PlayerKeys.StatisticKeys.TotalTimePlayed };

                    //print(item.Profile.Statistics.Find(s => s.Name == totalTimePlayed.Name).Value);
                    //print(item.Position + 1 + ") " + item.DisplayName + "/" + item.StatValue);

                    Result?.Invoke(item);
                }
            }, 
            error => 
            {
                print(error.ErrorMessage);
            });
    }
}
