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

    #region SetPhotonRankAndTotalTimePlayed
    /// <summary>
    /// 0:Rank 1:TotalTimePlayed
    /// </summary>
    /// <param name="statsList"></param>
    public void SetPhotonRankAndTotalTimePlayed(List<int> statsList)
    {
        PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.StatisticKeys.Rank] = statsList[0];
        PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.StatisticKeys.TotalTimePlayed] = statsList[1];
    }
    #endregion

    #region SetPhotonPlayerRolesStats

    /// <summary>
    /// 0:AsSurvivor 1:AsDoctor 2:AsSheriff 3:AsSoldier 4:AsInfected 5:AsWitch
    /// </summary>
    /// <param name="statsList"></param>
    public void SetPhotonPlayerRolesStats(List<int> statsList)
    {
        PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.StatisticKeys.AsSurvivor] = statsList[0];
        PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.StatisticKeys.AsDoctor] = statsList[1];
        PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.StatisticKeys.AsSheriff] = statsList[2];
        PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.StatisticKeys.AsSoldier] = statsList[3];
        PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.StatisticKeys.AsInfected] = statsList[4];
        PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.StatisticKeys.AsWitch] = statsList[5];
    }
    #endregion

    #region SetPhotonPlayerLastRoomName
    public void SetPhotonPlayerLastRoomName(string roomName)
    {
        PlayerBaseConditions.LocalPlayer.CustomProperties[PlayerKeys.SetPlayersRoleKeys.RoomName] = roomName;
    }
    #endregion
}
