using PlayFab;
using PlayFab.AuthenticationModels;
using PlayFab.ProfilesModels;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayfabEntity : MonoBehaviour
{
    #region GetEntityToken
    public void GetEntityToken(Action<Dictionary<string, string>> GetEntityInfo)
    {
        GetEntityTokenRequest requestEntityInfo = new GetEntityTokenRequest();

        PlayFabAuthenticationAPI.GetEntityToken(requestEntityInfo,
            get =>
            {
                GetEntityInfo(new Dictionary<string, string> { { get.Entity.Id, get.Entity.Type } });

                SetEntityProfilePolicy(get.Entity.Id, get.Entity.Type);
            },
            error =>
            {
                print(error.ErrorMessage);
            });
    }
    #endregion

    #region SetEntityProfilePolicy + Play badge animation
    void SetEntityProfilePolicy(string entityId, string entityType)
    {
        SetEntityProfilePolicyRequest requestProfilePolicy = new SetEntityProfilePolicyRequest();
        requestProfilePolicy.Entity = new PlayFab.ProfilesModels.EntityKey { Id = entityId, Type = entityType };
        requestProfilePolicy.Statements = new List<EntityPermissionStatement>() { new EntityPermissionStatement
        { Action = "*",
          Effect = EffectType.Allow,
          Resource = string.Format("pfrn:data--title_player_account!{0}/Profile/Files/{1}", entityId, "*"),
          Principal = "*",
          Comment = null,
          Condition = null,
        }};

        PlayFabProfilesAPI.SetProfilePolicy(requestProfilePolicy, 
            succes => 
            {
                print("SUCCES");
                PlayerBaseConditions.NetworkManagerComponents.NetworkUI.PlayerBadgeButton.OnPlayerLoggedIn();
            }, 
            error => 
            {
                print(error.ErrorMessage);
            });
    }
    #endregion
}
