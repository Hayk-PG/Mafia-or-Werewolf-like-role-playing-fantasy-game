using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Collections;

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

                #region Get entity info and store in player custom properties + Upload profile pic
                PlayerBaseConditions.PlayfabManager.PlayfabEntity.GetEntityToken(
                    get => 
                    {
                        foreach (var info in get)
                        {
                            StartCoroutine(UploadProfileImageCoroutine(info.Key, info.Value));
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

                #region Create player stats
                PlayerBaseConditions.PlayfabManager.PlayfabStats.UpdatePlayerStats(result.PlayFabId, 
                    update =>
                    {
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.Rank, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.TotalTimePlayed, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.Points, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.Scores, Value = 0 });

                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsSurvivor, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsDoctor, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsSheriff, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsSoldier, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsInfected, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.AsWitch, Value = 0 });

                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.OverallSkills, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.SurvivorSkills, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.DoctorSkills, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.SheriffSkills, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.SoldierSkills, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.InfectedSkills, Value = 0 });
                        update.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.LizardSkills, Value = 0 });
                    });
                #endregion

                PlayerBaseConditions.ConnectToPhoton(result.PlayFabId);
                PlayerBaseConditions.NetworkManagerComponents.NetworkUI.OnLoggedIn();
            },

            error =>
            {
                PlayerBaseConditions.NetworkManagerComponents.SignUpTab.OnPlayfabRegisterError(error.ErrorMessage);
            }
        );
    }
    #endregion

    IEnumerator UploadProfileImageCoroutine(string entityId, string entityType)
    {
        yield return new WaitForSeconds(2);
        AndroidGoodiesExamples.OtherGoodiesTest AndroidGoodies = FindObjectOfType<AndroidGoodiesExamples.OtherGoodiesTest>();
        PlayerBaseConditions.PlayfabManager.PlayfabUploadProfileImage.UploadProfileImage(entityId, entityType, AndroidGoodies.profilePic.sprite);
    }
}
