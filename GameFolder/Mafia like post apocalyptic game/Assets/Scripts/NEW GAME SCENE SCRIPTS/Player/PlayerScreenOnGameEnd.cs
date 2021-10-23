using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScreenOnGameEnd : MonoBehaviourPun
{
    EndTab _EndTab { get; set; }
    GameManagerTimer _GameManagerTimer { get; set; }
    GameManagerVFXHolder _GameManagerVFXHolder { get; set; }


    void Awake()
    {
        _EndTab = FindObjectOfType<EndTab>();
        _GameManagerTimer = FindObjectOfType<GameManagerTimer>();
        _GameManagerVFXHolder = FindObjectOfType<GameManagerVFXHolder>();
    }

    public void Screen(string playfabId, string URL, bool humansWin)
    {
        StartCoroutine(ScreenCoroutine(playfabId, URL, humansWin));
    }

    IEnumerator ScreenCoroutine(string playfabId, string URL, bool humansWin)
    {
        yield return new WaitUntil(IsScreenReady);

        if (photonView.IsMine && photonView.AmOwner)
        {
            int minHighestPoint = _GameManagerTimer._GameEndData.PlayersProfilePictureURL.Count * 10;
            string ownRoleName = _GameManagerTimer._GameEndData.PlayersRolesInLastRound[PhotonNetwork.LocalPlayer.UserId];

            _GameManagerTimer._PhasesIcons.IsNightPhaseIconsActive = false;
            _GameManagerTimer._PhasesIcons.IsDayPhaseIconsActive = false;

            IfHumansWin(ownRoleName, humansWin);
            IfInfectedsWin(ownRoleName, humansWin);
            CombatMedic(playfabId, URL, minHighestPoint);
            SherlockHolmes(playfabId, URL, minHighestPoint);
            BestSoldier(playfabId, URL, minHighestPoint);
            Predator(playfabId, URL, minHighestPoint);
            TrickyBastard(playfabId, URL, minHighestPoint);
            MVP(playfabId, URL, minHighestPoint);
            Survivor(playfabId, URL, minHighestPoint);
        }
    }

    bool IsScreenReady()
    {
        return _EndTab._UI.BackgroundColor == Color.white;
    }

    #region IfHumansWin
    void IfHumansWin(string ownRoleName, bool humansWin)
    {
        if (humansWin)
        {
            if (ownRoleName != RoleNames.Infected && ownRoleName != RoleNames.Lizard)
            {
                PlayerBaseConditions.UiSounds.PlaySoundFXinGame(9);
                //_GameManagerVFXHolder.CreateVFX(1);
                _EndTab._UI.Title = "Your team won!";
                PlayerBaseConditions.UiSounds.PlaySoundFXinGame(10);
            }
        }
        else
        {
            if (ownRoleName != RoleNames.Infected && ownRoleName != RoleNames.Lizard)
            {
                PlayerBaseConditions.UiSounds.PlaySoundFXinGame(8);
                _EndTab._UI.Title = "Your team lost!";
            }
        }
    }
    #endregion

    #region IfInfectedsWin
    void IfInfectedsWin(string ownRoleName, bool humansWin)
    {
        if (!_GameManagerTimer._GameEndData.HumansWin)
        {
            if (ownRoleName == RoleNames.Infected || ownRoleName == RoleNames.Lizard)
            {
                PlayerBaseConditions.UiSounds.PlaySoundFXinGame(9);
                //_GameManagerVFXHolder.CreateVFX(1);
                _EndTab._UI.Title = "Your team won!";
                PlayerBaseConditions.UiSounds.PlaySoundFXinGame(10);
            }
        }
        else
        {
            if (ownRoleName == RoleNames.Infected || ownRoleName == RoleNames.Lizard)
            {
                PlayerBaseConditions.UiSounds.PlaySoundFXinGame(8);
                _EndTab._UI.Title = "Your team lost!";
            }
        }
    }
    #endregion

    #region CombatMedic
    void CombatMedic(string playfabId, string URL, int minHighestPoint)
    {
        if (CheckDict(_GameManagerTimer._GameEndData.PointsOfTheDoctor, playfabId) && _GameManagerTimer._GameEndData.PointsOfTheDoctor[playfabId] > minHighestPoint)
        {
            UpdateLocalPlayerStats(playfabId, 150);
            _EndTab.DisplayPublicScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], "150");
        }
    }
    #endregion

    #region Sherlock Holmes
    void SherlockHolmes(string playfabId, string URL, int minHighestPoint)
    {
        if (CheckDict(_GameManagerTimer._GameEndData.PointsOfTheSheriff, playfabId) && _GameManagerTimer._GameEndData.PointsOfTheSheriff[playfabId] > minHighestPoint)
        {
            UpdateLocalPlayerStats(playfabId, 150);
            _EndTab.DisplayPublicScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], "150");
        }
    }
    #endregion

    #region BestSoldier
    void BestSoldier(string playfabId, string URL, int minHighestPoint)
    {
        if (CheckDict(_GameManagerTimer._GameEndData.PointsOfTheSoldier, playfabId) && _GameManagerTimer._GameEndData.PointsOfTheSoldier[playfabId] > minHighestPoint)
        {
            UpdateLocalPlayerStats(playfabId, 150);
            _EndTab.DisplayPublicScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], "150");
        }
    }
    #endregion

    #region Predator
    void Predator(string playfabId, string URL, int minHighestPoint)
    {
        if (CheckDict(_GameManagerTimer._GameEndData.PointsOfTheInfected, playfabId) && _GameManagerTimer._GameEndData.PointsOfTheInfected[playfabId] > minHighestPoint)
        {
            UpdateLocalPlayerStats(playfabId, 150);
            _EndTab.DisplayPublicScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], "150");
        }
    }
    #endregion

    #region TrickyBastard
    void TrickyBastard(string playfabId, string URL, int minHighestPoint)
    {
        if (CheckDict(_GameManagerTimer._GameEndData.PointsOfTheLizard, playfabId) && _GameManagerTimer._GameEndData.PointsOfTheLizard[playfabId] > minHighestPoint)
        {
            UpdateLocalPlayerStats(playfabId, 150);
            _EndTab.DisplayPublicScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], "150");
        }
    }
    #endregion

    #region MVP
    void MVP(string playfabId, string URL, int minHighestPoint)
    {
        if (CheckDict(_GameManagerTimer._GameEndData.PointsForEveryone, playfabId) && _GameManagerTimer._GameEndData.PointsForEveryone[playfabId] > minHighestPoint)
        {
            UpdateLocalPlayerStats(playfabId, 250);
            _EndTab.DisplayPublicScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], "250 (MVP)");
        }
    }
    #endregion

    #region Survivor
    void Survivor(string playfabId, string URL, int minHighestPoint)
    {
        if (playfabId == PhotonNetwork.LocalPlayer.UserId)
        {
            if (playfabId == PhotonNetwork.LocalPlayer.UserId)
            {
                UpdateLocalPlayerStats(playfabId, 50);
                _EndTab.DisplayYourScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], "50");
            }
        }
    }
    #endregion

    bool CheckDict<T1, T2>(Dictionary<T1, T2> Dict, T1 key)
    {
        return Dict != null && Dict.ContainsKey(key);
    }

    void UpdateLocalPlayerStats(string playfabId, int points)
    {
        if (playfabId == PlayerBaseConditions.LocalPlayer.UserId)
        {
            PlayerBaseConditions.PlayfabManager.PlayfabStats.GetPlayerStats(playfabId, GetStats =>
            {
                PlayerBaseConditions.PlayfabManager.PlayfabStats.UpdatePlayerStats(playfabId, UpdateStats =>
                {
                    int scoresForLeaderboard = Mathf.Abs(points + GetStats.Points + (GetStats.Win - GetStats.Lost));

                    UpdateStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.Points, Value = GetStats.Points += points });
                    UpdateStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.Scores, Value = scoresForLeaderboard });
                });
            });
        }
    }

    #region Deprecated
    void LoadProfilePicture(string playfabId, string URL)
    {
        PlayerBaseConditions.PlayerProfile.LoadProfilePicture(URL, Picture =>
        {
            _EndTab.InstantiateAwarededPlayerCard(Picture, _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], "Combat medic", 6, false);
            UpdateLocalPlayerStats(playfabId, 150);
        });
    }
    #endregion
}
