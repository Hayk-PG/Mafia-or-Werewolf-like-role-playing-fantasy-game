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
                _GameManagerTimer.AddOrRemovePoints(PhotonNetwork.LocalPlayer.ActorNumber, _GameManagerTimer._GameEndData.PointsForEveryone, 50);
                PlayerBaseConditions.UiSounds.PlaySoundFXinGame(9);
                _EndTab._UI.Title = "Your team won!";
            }
        }
        else
        {
            if (ownRoleName != RoleNames.Infected && ownRoleName != RoleNames.Lizard)
            {
                _GameManagerTimer.AddOrRemovePoints(PhotonNetwork.LocalPlayer.ActorNumber, _GameManagerTimer._GameEndData.PointsForEveryone, -30);
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
                _GameManagerTimer.AddOrRemovePoints(PhotonNetwork.LocalPlayer.ActorNumber, _GameManagerTimer._GameEndData.PointsForEveryone, 50);
                PlayerBaseConditions.UiSounds.PlaySoundFXinGame(9);
                _EndTab._UI.Title = "Your team won!";
            }
        }
        else
        {
            if (ownRoleName == RoleNames.Infected || ownRoleName == RoleNames.Lizard)
            {
                _GameManagerTimer.AddOrRemovePoints(PhotonNetwork.LocalPlayer.ActorNumber, _GameManagerTimer._GameEndData.PointsForEveryone, -30);
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
            _EndTab.DisplayPublicScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], _GameManagerTimer._GameEndData.PointsOfTheDoctor[playfabId].ToString());
        }
    }
    #endregion

    #region Sherlock Holmes
    void SherlockHolmes(string playfabId, string URL, int minHighestPoint)
    {
        if (CheckDict(_GameManagerTimer._GameEndData.PointsOfTheSheriff, playfabId) && _GameManagerTimer._GameEndData.PointsOfTheSheriff[playfabId] > minHighestPoint)
        {
            _EndTab.DisplayPublicScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], _GameManagerTimer._GameEndData.PointsOfTheSheriff[playfabId].ToString());
        }
    }
    #endregion

    #region BestSoldier
    void BestSoldier(string playfabId, string URL, int minHighestPoint)
    {
        if (CheckDict(_GameManagerTimer._GameEndData.PointsOfTheSoldier, playfabId) && _GameManagerTimer._GameEndData.PointsOfTheSoldier[playfabId] > minHighestPoint)
        {
            _EndTab.DisplayPublicScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], _GameManagerTimer._GameEndData.PointsOfTheSoldier[playfabId].ToString());
        }
    }
    #endregion

    #region Predator
    void Predator(string playfabId, string URL, int minHighestPoint)
    {
        if (CheckDict(_GameManagerTimer._GameEndData.PointsOfTheInfected, playfabId) && _GameManagerTimer._GameEndData.PointsOfTheInfected[playfabId] > minHighestPoint)
        {
            _EndTab.DisplayPublicScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], _GameManagerTimer._GameEndData.PointsOfTheInfected[playfabId].ToString());
        }
    }
    #endregion

    #region TrickyBastard
    void TrickyBastard(string playfabId, string URL, int minHighestPoint)
    {
        if (CheckDict(_GameManagerTimer._GameEndData.PointsOfTheLizard, playfabId) && _GameManagerTimer._GameEndData.PointsOfTheLizard[playfabId] > minHighestPoint)
        {
            _EndTab.DisplayPublicScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], _GameManagerTimer._GameEndData.PointsOfTheLizard[playfabId].ToString());
        }
    }
    #endregion

    #region MVP
    void MVP(string playfabId, string URL, int minHighestPoint)
    {
        if (CheckDict(_GameManagerTimer._GameEndData.PointsForEveryone, playfabId) && _GameManagerTimer._GameEndData.PointsForEveryone[playfabId] > minHighestPoint)
        {
            _EndTab.DisplayPublicScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], _GameManagerTimer._GameEndData.PointsForEveryone[playfabId].ToString());
        }
    }
    #endregion

    #region Survivor
    void Survivor(string playfabId, string URL, int minHighestPoint)
    {
        if (playfabId == PhotonNetwork.LocalPlayer.UserId)
        {
            int score = 0;     

            if (_GameManagerTimer._GameEndData.PointsOfTheDoctor.ContainsKey(PhotonNetwork.LocalPlayer.UserId))
            {
                score += _GameManagerTimer._GameEndData.PointsOfTheDoctor[PhotonNetwork.LocalPlayer.UserId];

                _EndTab.DisplayYourScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], score.ToString());
            }

            if (_GameManagerTimer._GameEndData.PointsOfTheSheriff.ContainsKey(PhotonNetwork.LocalPlayer.UserId))
            {
                score += _GameManagerTimer._GameEndData.PointsOfTheSheriff[PhotonNetwork.LocalPlayer.UserId];

                _EndTab.DisplayYourScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], score.ToString());
            }

            if (_GameManagerTimer._GameEndData.PointsOfTheSoldier.ContainsKey(PhotonNetwork.LocalPlayer.UserId))
            {
                score += _GameManagerTimer._GameEndData.PointsOfTheSoldier[PhotonNetwork.LocalPlayer.UserId];

                _EndTab.DisplayYourScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], score.ToString());
            }

            if (_GameManagerTimer._GameEndData.PointsOfTheInfected.ContainsKey(PhotonNetwork.LocalPlayer.UserId))
            {
                score += _GameManagerTimer._GameEndData.PointsOfTheInfected[PhotonNetwork.LocalPlayer.UserId];

                _EndTab.DisplayYourScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], score.ToString());
            }

            if (_GameManagerTimer._GameEndData.PointsOfTheLizard.ContainsKey(PhotonNetwork.LocalPlayer.UserId))
            {
                score += _GameManagerTimer._GameEndData.PointsOfTheLizard[PhotonNetwork.LocalPlayer.UserId];

                _EndTab.DisplayYourScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], score.ToString());
            }

            if (_GameManagerTimer._GameEndData.PointsForEveryone.ContainsKey(PhotonNetwork.LocalPlayer.UserId))
            {
                score += _GameManagerTimer._GameEndData.PointsForEveryone[PhotonNetwork.LocalPlayer.UserId] + 15;

                _EndTab.DisplayYourScores(_GameManagerTimer._GameEndData.PlayersRolesInLastRound[playfabId], _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], score.ToString());

                UpdateLocalPlayerStats(playfabId, score);
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
