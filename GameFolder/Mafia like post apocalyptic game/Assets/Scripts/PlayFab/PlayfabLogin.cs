using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;

public class PlayfabLogin : MonoBehaviour
{
    #region OnPlayfabLogin
    public void OnPlayfabLogin(string username, string password)
    {
        LoginWithPlayFabRequest loginRequest = new LoginWithPlayFabRequest { Username = username, Password = password };

        PlayFabClientAPI.LoginWithPlayFab(loginRequest,

            result =>
            {
                PlayerBaseConditions.PlayfabManager.PlayfabUserProfile.GetPlayerProfile(result.PlayFabId, CheckingIfAccountExists => 
                {
                    if(CheckingIfAccountExists.displayedName != null)
                    {
                        PlayerBaseConditions.PlayerCustomPropertiesController.SetPhotonPlayerName(username);
                        PlayerBaseConditions.PlayerCustomPropertiesController.SetPhotonPlayerID(result.PlayFabId);

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

                        #region Get player user data and store in player custom properties
                        PlayerBaseConditions.PlayfabManager.PlayfabUserData.GetUserData(result.PlayFabId,
                            get =>
                            {
                                PlayerBaseConditions.PlayerCustomPropertiesController.SetPhotonPlayerGender(get.gender);
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

                        #region Get player stats and store in player custom properties
                        PlayerBaseConditions.PlayfabManager.PlayfabStats.GetPlayerStats(result.PlayFabId,
                            get =>
                            {
                                PlayerBaseConditions.PlayerCustomPropertiesController.SetPhotonRankAndTotalTimePlayed(new List<int>
                                {
                                get.Rank,
                                get.TotalTimePlayed
                                });
                            });
                        #endregion

                        PlayerBaseConditions.NetworkManagerComponents?.NetworkManager.ConnectToPhoton(result.PlayFabId);
                        PlayerBaseConditions.NetworkManagerComponents.NetworkUI.OnLoggedIn();
                    }
                    else
                    {
                        PlayerBaseConditions.NetworkManagerComponents.SignInTab.OnPlayfabRegisterError("This account doesn't exist. Enter a different account or get a new one!");
                    }
                });  
            },

            error =>
            {
                PlayerBaseConditions.NetworkManagerComponents.SignInTab.OnPlayfabRegisterError(error.ErrorMessage);
            });
    }
    #endregion
}
