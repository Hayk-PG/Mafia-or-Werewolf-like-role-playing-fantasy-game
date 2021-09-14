using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerOnGameStart : MonoBehaviourPun
{   
    internal bool IsPlayerFirstTimeInThisRoom
    {
        get => !PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(PlayerKeys.SetPlayersRoleKeys.RoomName) || PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(PlayerKeys.SetPlayersRoleKeys.RoomName) && (string)PhotonNetwork.LocalPlayer.CustomProperties[PlayerKeys.SetPlayersRoleKeys.RoomName] != PhotonNetwork.CurrentRoom.Name;
    }
    internal bool IsPlayerRoleSet
    {
        get => PlayerBaseConditions.PlayerRoleName(PhotonNetwork.LocalPlayer.ActorNumber) != null;
    }
    PlayerUpdateStats _PlayerUpdateStats { get; set; }
    InformPlayerRole _InformPlayerRole { get; set; }
    IEnumerator UpdatePlayerStatsCoroutine { get; set; }


    void Awake()
    {
        _PlayerUpdateStats = GetComponent<PlayerUpdateStats>();
        _InformPlayerRole = FindObjectOfType<InformPlayerRole>();
    }

    void Start()
    {
        UpdatePlayerStatsCoroutine = _PlayerUpdateStats.UpdatePlayerStatsCoroutine();
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (IsPlayerFirstTimeInThisRoom)
            {
                if (IsPlayerRoleSet && !_PlayerUpdateStats._Conditions.isPlayerRoleSet)
                {
                    StartCoroutine(InformPlayerRolePopUp());
                    UpdatePlayedRolesStats();
                }               
            }
        }
    }

    #region UpdatePlayedRolesStats
    void UpdatePlayedRolesStats()
    {
        _PlayerUpdateStats.GetPlayerStats(delegate { StartCoroutine(UpdatePlayerStatsCoroutine); });
    }
    #endregion

    #region InformPlayerRolePopUp
    IEnumerator InformPlayerRolePopUp()
    {
        yield return new WaitForSeconds(1);
        _InformPlayerRole.OnPopUp("Your role is " + PlayerBaseConditions.PlayerRoleName(PhotonNetwork.LocalPlayer.ActorNumber) + "!");
    }
    #endregion
}
