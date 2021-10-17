using Photon.Pun;
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
        if(photonView.IsMine && photonView.AmOwner)
        {
            int minHighestPoint = _GameManagerTimer._GameEndData.PlayersProfilePictureURL.Count * 10;
            string ownRoleName = _GameManagerTimer._GameEndData.PlayersRolesInLastRound[PhotonNetwork.LocalPlayer.UserId];

            _GameManagerTimer._PhasesIcons.IsNightPhaseIconsActive = false;
            _GameManagerTimer. _PhasesIcons.IsDayPhaseIconsActive = false;

            print(ownRoleName + "/" + humansWin);

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

    #region IfHumansWin
    void IfHumansWin(string ownRoleName, bool humansWin)
    {
        if (humansWin)
        {
            if (ownRoleName != RoleNames.Infected && ownRoleName != RoleNames.Lizard)
            {
                PlayerBaseConditions.UiSounds.PlaySoundFXinGame(9);
                _GameManagerVFXHolder.CreateVFX(1);
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
                _GameManagerVFXHolder.CreateVFX(1);
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
            PlayerBaseConditions.PlayerProfile.LoadProfilePicture(URL, Picture =>
            {
                _EndTab.InstantiateAwarededPlayerCard(Picture, _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], "Combat medic", 6, false);
                UpdateLocalPlayerStats(playfabId, 150);
            });
        }
    }
    #endregion

    #region Sherlock Holmes
    void SherlockHolmes(string playfabId, string URL, int minHighestPoint)
    {
        if (CheckDict(_GameManagerTimer._GameEndData.PointsOfTheSheriff, playfabId) && _GameManagerTimer._GameEndData.PointsOfTheSheriff[playfabId] > minHighestPoint)
        {
            PlayerBaseConditions.PlayerProfile.LoadProfilePicture(URL, Picture =>
            {
                _EndTab.InstantiateAwarededPlayerCard(Picture, _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], "Sherlock Holmes", 7, false);
                UpdateLocalPlayerStats(playfabId, 150);
            });
        }
    }
    #endregion

    #region BestSoldier
    void BestSoldier(string playfabId, string URL, int minHighestPoint)
    {
        if (CheckDict(_GameManagerTimer._GameEndData.PointsOfTheSoldier, playfabId) && _GameManagerTimer._GameEndData.PointsOfTheSoldier[playfabId] > minHighestPoint)
        {
            PlayerBaseConditions.PlayerProfile.LoadProfilePicture(URL, Picture =>
            {
                _EndTab.InstantiateAwarededPlayerCard(Picture, _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], "Best soldier", 8, false);
                UpdateLocalPlayerStats(playfabId, 150);
            });
        }
    }
    #endregion

    #region Predator
    void Predator(string playfabId, string URL, int minHighestPoint)
    {
        if (CheckDict(_GameManagerTimer._GameEndData.PointsOfTheInfected, playfabId) && _GameManagerTimer._GameEndData.PointsOfTheInfected[playfabId] > minHighestPoint)
        {
            PlayerBaseConditions.PlayerProfile.LoadProfilePicture(URL, Picture =>
            {
                _EndTab.InstantiateAwarededPlayerCard(Picture, _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], "Predator", 3, false);
                UpdateLocalPlayerStats(playfabId, 150);
            });
        }
    }
    #endregion

    #region TrickyBastard
    void TrickyBastard(string playfabId, string URL, int minHighestPoint)
    {
        if (CheckDict(_GameManagerTimer._GameEndData.PointsOfTheLizard, playfabId) && _GameManagerTimer._GameEndData.PointsOfTheLizard[playfabId] > minHighestPoint)
        {
            PlayerBaseConditions.PlayerProfile.LoadProfilePicture(URL, Picture =>
            {
                _EndTab.InstantiateAwarededPlayerCard(Picture, _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], "Tricky bastard", 4, false);
                UpdateLocalPlayerStats(playfabId, 150);
            });
        }
    }
    #endregion

    #region MVP
    void MVP(string playfabId, string URL, int minHighestPoint)
    {
        if (CheckDict(_GameManagerTimer._GameEndData.PointsForEveryone, playfabId) && _GameManagerTimer._GameEndData.PointsForEveryone[playfabId] > minHighestPoint)
        {
            PlayerBaseConditions.PlayerProfile.LoadProfilePicture(URL, Picture =>
            {
                _EndTab.InstantiateAwarededPlayerCard(Picture, _GameManagerTimer._GameEndData.PlayersCachedNames[playfabId], "MVP", 9, false);
                UpdateLocalPlayerStats(playfabId, 250);
            });
        }
    }
    #endregion

    #region Survivor
    void Survivor(string playfabId, string URL, int minHighestPoint)
    {
        if (playfabId == PhotonNetwork.LocalPlayer.UserId/* && _GameManagerTimer._GameEndData.PointsForEveryone[playfabId] <= Mathf.Abs(minHighestPoint / 2)*/)
        {
            PlayerBaseConditions.PlayerProfile.LoadProfilePicture(URL, Picture =>
            {
                _EndTab.InstantiateAwarededPlayerCard(Picture, "You", "Survivor", 0, true);
                UpdateLocalPlayerStats(playfabId, 25);
            });
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
                    UpdateStats.Statistics.Add(new PlayFab.ServerModels.StatisticUpdate { StatisticName = PlayerKeys.StatisticKeys.Points, Value = GetStats.Points += points });
                });
            });
        }
    }
}
