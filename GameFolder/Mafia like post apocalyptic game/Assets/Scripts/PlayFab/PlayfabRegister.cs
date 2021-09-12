using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

public class PlayfabRegister : MonoBehaviour
{
    public event Action<string> OnPlayfabRegisterError;

    #region OnPlayfabRegister
    public void OnPlayfabRegister(string username, string password, SignUpTab.Gender gender)
    {
        RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest();

        registerRequest.Username = username;
        registerRequest.DisplayName = username;
        registerRequest.Password = password;
        registerRequest.RequireBothUsernameAndEmail = false;

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest,

            result =>
            {
                PlayerBaseConditions.PlayerCustomPropertiesController.SetPhotonPlayerName(username);
                PlayerBaseConditions.PlayerCustomPropertiesController.SetPhotonPlayerID(result.PlayFabId);
                PlayerBaseConditions.PlayerCustomPropertiesController.SetPhotonPlayerGender(gender.ToString());

                #region Get entity info and store in player custom properties
                PlayerBaseConditions.PlayfabManager.PlayfabEntity.GetEntityToken(
                    get => 
                    {
                        foreach (var info in get)
                        {
                            PlayerBaseConditions.PlayerCustomPropertiesController.SetPhotonPlayerEntity(info.Key, info.Value);
                        }
                    });
                #endregion

                #region Update user data and store in player custom properties
                PlayerBaseConditions.PlayfabManager.PlayfabUserData.UpdateUserData(
                    update =>
                    {
                        update.Data = new Dictionary<string, string>
                        {
                                {PlayerKeys.UserID, result.PlayFabId },
                                {PlayerKeys.GenderKey, gender.ToString() }
                        };
                    });
                #endregion

                #region Get player profile and store in player custom properties
                PlayerBaseConditions.PlayfabManager.PlayfabUserProfile.GetPlayerProfile(result.PlayFabId,
                    get =>
                    {
                        PlayerBaseConditions.PlayerCustomPropertiesController.SetPhotonPlayerCountryCode(get.countryCode);
                        PlayerBaseConditions.PlayerCustomPropertiesController.SetPhotonPlayerRegDate(get.regDate);
                    });
                #endregion

                #region Update player stats and store in player custom properties
                PlayerBaseConditions.PlayfabManager.PlayfabStats.UpdatePlayerStats(result.PlayFabId, 
                    update =>
                    {
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.Rank, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.TotalTimePlayed, Value = 0 });

                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsSurvivor, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsDoctor, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsSheriff, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsSoldier, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsInfected, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsWitch, Value = 0 });

                        PlayerBaseConditions.PlayerCustomPropertiesController.SetPhotonRankAndTotalTimePlayed(new List<int> { 0, 0 });
                    });
                #endregion

                PlayerBaseConditions.NetworkManagerComponents?.NetworkManager.ConnectToPhoton(result.PlayFabId);
                PlayerBaseConditions.NetworkManagerComponents.NetworkUI.OnLoggedIn();
            },

            error =>
            {
                PlayerBaseConditions.NetworkManagerComponents.SignUpTab.OnPlayfabRegisterError(error.ErrorMessage);
            }
        );
    }
    #endregion
}
