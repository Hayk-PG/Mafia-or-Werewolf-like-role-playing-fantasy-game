using Photon.Pun;
using System;
using UnityEngine;

public class PlayerPoints : MonoBehaviourPun
{
    delegate bool Filter();

    [Serializable]
    class Prefab
    {
        [SerializeField] PointsPrefab pointsPrefab;
        internal Transform GameUI
        {
            get => GameObject.Find("GameUI").transform;
        }
        internal PointsPrefab PointsPrefab
        {
            get => pointsPrefab;
        }
    }
    class CustomPropKeys
    {
        internal string NightAlive = "NightAlive";
        internal string DayAlive = "DayAlive";
    }

    [SerializeField] Prefab _Prefab;
    CustomPropKeys _CustomPropKeys { get; set; }
    GameManagerTimer _GameManagerTimer { get; set; }
    PlayerUpdateStats _PlayerUpdateStats { get; set; }


    void Awake()
    {
        if(photonView.IsMine && photonView.AmOwner)
        {
            enabled = true;
        }
        else
        {
            enabled = false;
        }

        _CustomPropKeys = new CustomPropKeys();

        _GameManagerTimer = FindObjectOfType<GameManagerTimer>();
        _PlayerUpdateStats = GetComponent<PlayerUpdateStats>();
    }

    void Update()
    {
        GetPointsForStayingAliveInNightPhase(() => _GameManagerTimer._Timer.NightTime && _GameManagerTimer._Timer.Seconds <= 55 && _GameManagerTimer._Timer.NightsCount > 0);
        GetPointsForStayingAliveInDayPhase(() => _GameManagerTimer._Timer.DayTime && _GameManagerTimer._Timer.Seconds <= 85);
    }

    #region HasNightAlivePointReceived
    bool HasNightAlivePointReceived()
    {
        return PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(_CustomPropKeys.NightAlive) && (bool)PhotonNetwork.LocalPlayer.CustomProperties[_CustomPropKeys.NightAlive];
    }
    #endregion

    #region HasDayAlivePointReceived
    bool HasDayAlivePointReceived()
    {
        return PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(_CustomPropKeys.DayAlive) && (bool)PhotonNetwork.LocalPlayer.CustomProperties[_CustomPropKeys.DayAlive];
    }
    #endregion

    #region GetPointsForStayingAliveInNightPhase
    void GetPointsForStayingAliveInNightPhase(Filter filter)
    {
        if (filter())
        {
            if (!HasNightAlivePointReceived() && PlayerBaseConditions.GetRoleButton(PhotonNetwork.LocalPlayer.ActorNumber)._GameInfo.IsPlayerAlive)
            {
                _PlayerUpdateStats.GetAndUpdatePlayfabStats(
                    GetPlayfabStats =>
                    {
                        _PlayerUpdateStats._StatsValue.points = GetPlayfabStats.points + 25;
                        InstantiatePointsText("+" + 25, PlayerBaseConditions.GetRoleButton(PhotonNetwork.LocalPlayer.ActorNumber).transform.position);
                    },
                    () =>
                    {
                        PlayerBaseConditions.PlayfabManager.PlayfabStats.UpdatePlayerStats(PhotonNetwork.LocalPlayer.UserId,
                            UpdatePlayerStats =>
                            {
                                _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.Points, _PlayerUpdateStats._StatsValue.points);
                            });
                    });

                PhotonNetwork.LocalPlayer.CustomProperties[_CustomPropKeys.NightAlive] = true;
                PhotonNetwork.LocalPlayer.CustomProperties[_CustomPropKeys.DayAlive] = false;
            }
        }
    }
    #endregion

    #region GetPointsForStayingAliveInDayPhase
    void GetPointsForStayingAliveInDayPhase(Filter filter)
    {
        if (filter())
        {
            if (!HasDayAlivePointReceived() && PlayerBaseConditions.GetRoleButton(PhotonNetwork.LocalPlayer.ActorNumber)._GameInfo.IsPlayerAlive)
            {
                _PlayerUpdateStats.GetAndUpdatePlayfabStats(
                    GetPlayfabStats =>
                    {
                        _PlayerUpdateStats._StatsValue.points = GetPlayfabStats.points + 25;
                        InstantiatePointsText("+" + 25, PlayerBaseConditions.GetRoleButton(PhotonNetwork.LocalPlayer.ActorNumber).transform.position);
                    },
                    () =>
                    {
                        PlayerBaseConditions.PlayfabManager.PlayfabStats.UpdatePlayerStats(PhotonNetwork.LocalPlayer.UserId,
                            UpdatePlayerStats =>
                            {
                                _PlayerUpdateStats.PlayerStats(UpdatePlayerStats, PlayerKeys.StatisticKeys.Points, _PlayerUpdateStats._StatsValue.points);
                            });
                    });

                PhotonNetwork.LocalPlayer.CustomProperties[_CustomPropKeys.DayAlive] = true;
                PhotonNetwork.LocalPlayer.CustomProperties[_CustomPropKeys.NightAlive] = false;
            }
        }
    }
    #endregion

    #region InstantiatePointsText
    public void InstantiatePointsText(string text, Vector3 pos)
    {
        PointsPrefab prefabCopy = Instantiate(_Prefab.PointsPrefab, pos, Quaternion.identity, _Prefab.GameUI);
        PlayerBaseConditions.UiSounds.PlaySoundFXinGame(6);
        prefabCopy.transform.SetSiblingIndex(_Prefab.GameUI.childCount - 1);
        prefabCopy.Text = text;
    }
    #endregion
}
 