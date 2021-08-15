using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class PlayfabUserProfile : MonoBehaviour
{
    public Profile _Profile;
    public struct Profile
    {
        internal string countryCode;
        internal string regDate;
        internal string avatarURL;
    }

    #region GetPlayerProfile
    public void GetPlayerProfile(string playfabID, Action<Profile> GetPlayerProfile)
    {
        GetPlayerProfileRequest profileRequest = new GetPlayerProfileRequest();

        profileRequest.PlayFabId = playfabID;
        profileRequest.ProfileConstraints = new PlayerProfileViewConstraints();
        profileRequest.ProfileConstraints.ShowCreated = true;
        profileRequest.ProfileConstraints.ShowDisplayName = true;
        profileRequest.ProfileConstraints.ShowLocations = true;
        profileRequest.ProfileConstraints.ShowAvatarUrl = true;

        PlayFabClientAPI.GetPlayerProfile(profileRequest,

            result =>
            {
                GetPlayerProfile(new Profile
                {
                    countryCode = result.PlayerProfile.Locations[result.PlayerProfile.Locations.Count - 1].CountryCode.ToString(),
                    regDate = result.PlayerProfile.Created.Value.ToLocalTime().ToString(),
                    avatarURL = result.PlayerProfile.AvatarUrl
                });
            },
            error =>
            {
                print(error.ErrorMessage);
            });
    }
    #endregion
}
