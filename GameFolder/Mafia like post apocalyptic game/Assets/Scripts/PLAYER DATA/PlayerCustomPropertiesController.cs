using Photon.Pun;
using System.Collections.Generic;

public class PlayerCustomPropertiesController : MonoBehaviourPun
{
    public static PlayerCustomPropertiesController PCPC;

    void Awake()
    {
        PCPC = this;
    }

    #region SetPhotonPlayerName
    public void SetPhotonPlayerName(string username)
    {
        PlayerBaseConditions.LocalPlayer.NickName = username;
    }
    #endregion

    #region SetPhotonPlayerID
    public void SetPhotonPlayerID(string id)
    {
        PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.UserID] = id;
    }
    #endregion

    #region SetPhotonPlayerGender
    public void SetPhotonPlayerGender(string gender)
    {
        PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.GenderKey] = gender;
    }
    #endregion

    #region SetPhotonPlayerCountryCode
    public void SetPhotonPlayerCountryCode(string countryCode)
    {
        PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.LocationKey] = countryCode;
    }
    #endregion

    #region SetPhotonPlayerRegDate
    public void SetPhotonPlayerRegDate(string createDate)
    {
        PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.RegDateKey] = createDate;
    }
    #endregion

    #region SetPhotonPlayerEntity
    public void SetPhotonPlayerEntity(string entityId, string entityType)
    {
        PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.EntityId] = entityId;
        PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.EntityType] = entityType;
    }
    #endregion

    #region SetPhotonPlayerStats
    /// <summary>
    /// 0:Rank 1:TotalTimePlayed
    /// </summary>
    /// <param name="statsList"></param>
    public void SetPhotonPlayerStats(List<int> statsList)
    {
        PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.StatisticKeys.Rank] = statsList[0];
        PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.StatisticKeys.TotalTimePlayed] = statsList[1];
    }
    #endregion
}
