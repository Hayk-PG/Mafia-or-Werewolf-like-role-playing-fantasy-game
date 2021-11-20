using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayAiController : MonoBehaviour
{
    SinglePlayRoleButton _SinglePlayRoleButton { get; set; }
    SinglePlayGameController _SinglePlayGameController { get; set; }
    SinglePlayVoteDatas _SinglePlayVoteDatas { get; set; }
    DayVoteLogic _DayVoteLogic { get; set; }

    bool IsRandomSecondSet { get; set; }
    int RandomSecond { get; set; }

    void Awake()
    {
        _SinglePlayRoleButton = GetComponent<SinglePlayRoleButton>();
        _DayVoteLogic = GetComponent<DayVoteLogic>();
        _SinglePlayGameController = FindObjectOfType<SinglePlayGameController>();
        _SinglePlayVoteDatas = FindObjectOfType<SinglePlayVoteDatas>();       
    }

    void OnEnable()
    {
        _SinglePlayGameController.OnAiDayVote += _SinglePlayGameController_OnAiDayVote;
        _SinglePlayGameController.OnPhaseReset += _SinglePlayGameController_OnPhaseReset;
    }

    void OnDisable()
    {
        _SinglePlayGameController.OnAiDayVote -= _SinglePlayGameController_OnAiDayVote;
        _SinglePlayGameController.OnPhaseReset -= _SinglePlayGameController_OnPhaseReset;
    }

    void _SinglePlayGameController_OnAiDayVote()
    {
        if (SinglePlayGlobalConditions.CanAiParticipateInDayVote(_SinglePlayRoleButton))
        {
            OnDayVoteAsInfected();
            OnDayVoteAsNonInfected();
            OnDayVoteAsSheriff();
        }
    }

    void _SinglePlayGameController_OnPhaseReset(bool obj)
    {
        IsRandomSecondSet = false;
    }

    bool HasPlayerHasVotedInTime()
    {
        return _SinglePlayGameController._TimerClass.Timer >= 5 && _SinglePlayGameController.PlayerRoleButton().HasVoted;
    } 

    bool ShouldAIVoteIndependently()
    {
        return _SinglePlayGameController._TimerClass.Timer < 5 && !_SinglePlayGameController.PlayerRoleButton().HasVoted;
    }

    bool IsItTimeToVoteForAI()
    {
        return RandomSecond == _SinglePlayGameController._TimerClass.Timer && IsRandomSecondSet;
    }

    bool InfectedsDayVotesInfoContainsKey()
    {
        return _SinglePlayVoteDatas.InfectedsDayVotesInfo.ContainsKey(RoleNames.Infected);
    }

    void SetVoteRandomTime(int min, int max)
    {
        if (!IsRandomSecondSet)
        {
            RandomSecond = UnityEngine.Random.Range(min, max);
            IsRandomSecondSet = true;
        }
    }

    #region OnDayVoteAsInfected
    void OnDayVoteAsInfected()
    {
        if (SinglePlayGlobalConditions.IsAiInfected(_SinglePlayRoleButton))
        {
            if (_SinglePlayGameController.PlayerRoleButton().RoleName == RoleNames.Infected)
            {
                PlayerIsInfected();
            }
            else
            {
                PlayerIsNotInfected();
            }
        }
    }
    
    void PlayerIsInfected()
    {
        if (HasPlayerHasVotedInTime())
        {
            SinglePlayVoteDatas.DayVotesInfo VotesInfo = _SinglePlayVoteDatas.InfectedsDayVotesInfo[RoleNames.Infected];

            SetVoteRandomTime(5, _SinglePlayGameController._TimerClass.Timer);

            if (IsItTimeToVoteForAI())
            {
                SyncPlayerVotedName_AsAiInfected(VotesInfo.OtherPlayerName).AddVotesCount();
                _SinglePlayVoteDatas.AddDayVotesData(_SinglePlayRoleButton, _SinglePlayGameController._TimerClass.DaysCount, SyncPlayerVotedName_AsAiInfected(VotesInfo.OtherPlayerName));
                _SinglePlayRoleButton.DisplayVotesInfo(true, VotesInfo.OtherPlayerName);
                _SinglePlayRoleButton.HasVotedCondition(true);
            }
        }
        else if(ShouldAIVoteIndependently())
        {
            SetVoteRandomTime(0, 4);

            if (IsItTimeToVoteForAI())
            {
                if (InfectedsDayVotesInfoContainsKey())
                {
                    string name = _SinglePlayVoteDatas.InfectedsDayVotesInfo[RoleNames.Infected].OtherPlayerName;

                    if (RoleButtonByName(name) != _SinglePlayRoleButton && RoleButtonByName(name).IsAlive && !SinglePlayGlobalConditions.IsAiInfected(RoleButtonByName(name)))
                    {
                        RoleButtonByName(name).AddVotesCount();
                        _SinglePlayVoteDatas.AddDayVotesData(_SinglePlayRoleButton, _SinglePlayGameController._TimerClass.DaysCount, RoleButtonByName(name));
                        _SinglePlayRoleButton.DisplayVotesInfo(true, name);
                        _SinglePlayRoleButton.HasVotedCondition(true);
                    }
                }
                else
                {
                    for (int i = 0; i < _SinglePlayGameController._RolesClass.PlayersCount; i++)
                    {
                        RandomRoleButton().AddVotesCount();
                        _SinglePlayVoteDatas.AddDayVotesData(_SinglePlayRoleButton, _SinglePlayGameController._TimerClass.DaysCount, RandomRoleButton());
                        _SinglePlayRoleButton.DisplayVotesInfo(true, RandomRoleButton().Name);
                        _SinglePlayVoteDatas.AddInfectedsDayVotesInfo(_SinglePlayRoleButton.RoleName, new SinglePlayVoteDatas.DayVotesInfo(RandomRoleButton().Name, _SinglePlayGameController._TimerClass.DaysCount));
                        _SinglePlayRoleButton.HasVotedCondition(true);
                        break;
                    }
                }
            }
        }
    }

    void PlayerIsNotInfected()
    {
        SetVoteRandomTime(0, _SinglePlayGameController._TimerClass.Timer);

        if (IsItTimeToVoteForAI())
        {
            if (InfectedsDayVotesInfoContainsKey())
            {
                string name = _SinglePlayVoteDatas.InfectedsDayVotesInfo[RoleNames.Infected].OtherPlayerName;

                if (RoleButtonByName(name) != _SinglePlayRoleButton && RoleButtonByName(name).IsAlive && !SinglePlayGlobalConditions.IsAiInfected(RoleButtonByName(name)))
                {
                    RoleButtonByName(name).AddVotesCount();
                    _SinglePlayVoteDatas.AddDayVotesData(_SinglePlayRoleButton, _SinglePlayGameController._TimerClass.DaysCount, RoleButtonByName(name));
                    _SinglePlayRoleButton.DisplayVotesInfo(true, name);
                    _SinglePlayRoleButton.HasVotedCondition(true);
                }
            }
            else
            {
                for (int i = 0; i < _SinglePlayGameController._RolesClass.PlayersCount; i++)
                {
                    RandomRoleButton().AddVotesCount();
                    _SinglePlayVoteDatas.AddDayVotesData(_SinglePlayRoleButton, _SinglePlayGameController._TimerClass.DaysCount, RandomRoleButton());
                    _SinglePlayRoleButton.DisplayVotesInfo(true, RandomRoleButton().Name);
                    _SinglePlayVoteDatas.AddInfectedsDayVotesInfo(_SinglePlayRoleButton.RoleName, new SinglePlayVoteDatas.DayVotesInfo(RandomRoleButton().Name, _SinglePlayGameController._TimerClass.DaysCount));
                    _SinglePlayRoleButton.HasVotedCondition(true);
                    break;
                }
            }
        }
    }
    #endregion

    #region OnDayVoteAsNonInfected
    void OnDayVoteAsNonInfected()
    {
        if (!SinglePlayGlobalConditions.IsAiInfected(_SinglePlayRoleButton) && !SinglePlayGlobalConditions.IsAiSheriff(_SinglePlayRoleButton))
        {
            if (!IsRandomSecondSet)
            {
                RandomSecond = UnityEngine.Random.Range(0, _SinglePlayGameController._TimerClass.Timer);
                IsRandomSecondSet = true;
            }

            if (RandomSecond == _SinglePlayGameController._TimerClass.Timer && IsRandomSecondSet)
            {
                _DayVoteLogic.Vote(_SinglePlayRoleButton,
                        Suspect =>
                        {
                            if (SinglePlayGlobalConditions.IsAiKing(_SinglePlayRoleButton)) Suspect.VotesCount += 2;
                            else Suspect.AddVotesCount();
                            _SinglePlayVoteDatas.AddDayVotesData(_SinglePlayRoleButton, _SinglePlayGameController._TimerClass.DaysCount, Suspect);
                            _SinglePlayRoleButton.DisplayVotesInfo(true, Suspect.Name);
                            _SinglePlayRoleButton.HasVotedCondition(true);
                        },
                        SuspectFound =>
                        {
                            if (SuspectFound == false)
                            {
                                for (int i = 0; i < _SinglePlayGameController._RolesClass.PlayersCount; i++)
                                {
                                    if (RandomRoleButton() != this && RandomRoleButton().IsAlive)
                                    {
                                        if (SinglePlayGlobalConditions.IsAiKing(_SinglePlayRoleButton)) RandomRoleButton().VotesCount += 2;
                                        else RandomRoleButton().AddVotesCount();
                                        _SinglePlayVoteDatas.AddDayVotesData(_SinglePlayRoleButton, _SinglePlayGameController._TimerClass.DaysCount, RandomRoleButton());
                                        _SinglePlayRoleButton.DisplayVotesInfo(true, RandomRoleButton().Name);
                                        _SinglePlayRoleButton.HasVotedCondition(true);
                                        break;
                                    }
                                }
                            }
                        }, null);
            }
        }
    }
    #endregion

    #region OnDayVoteAsSheriff
    void OnDayVoteAsSheriff()
    {
        if (SinglePlayGlobalConditions.IsAiSheriff(_SinglePlayRoleButton))
        {
            if (!IsRandomSecondSet)
            {
                RandomSecond = UnityEngine.Random.Range(0, _SinglePlayGameController._TimerClass.Timer);
                IsRandomSecondSet = true;
            }

            if(RandomSecond == _SinglePlayGameController._TimerClass.Timer && IsRandomSecondSet)
            {
                _DayVoteLogic.Vote(_SinglePlayRoleButton, null, 
                    SuspectFound => 
                    {
                        if(SuspectFound == false)
                        {
                            if (IsRevealedPlayerInfected())
                            {
                                VoteAgainstRevealedPlayer();
                            }
                            else
                            {
                                for (int i = 0; i < _SinglePlayGameController._RolesClass.PlayersCount; i++)
                                {
                                    if (RandomRoleButton() != this && RandomRoleButton().IsAlive)
                                    {
                                        RandomRoleButton().AddVotesCount();
                                        _SinglePlayRoleButton.DisplayVotesInfo(true, RandomRoleButton().Name);
                                        _SinglePlayRoleButton.HasVotedCondition(true);
                                        break;
                                    }
                                }
                            }
                        }
                    },
                    Suspect => 
                    {
                        if (IsRevealedPlayerInfected())
                        {
                            VoteAgainstRevealedPlayer();
                        }
                        else
                        {
                            Suspect.AddVotesCount();
                            _SinglePlayVoteDatas.AddDayVotesData(_SinglePlayRoleButton, _SinglePlayGameController._TimerClass.DaysCount, Suspect);
                            _SinglePlayRoleButton.DisplayVotesInfo(true, Suspect.Name);
                            _SinglePlayRoleButton.HasVotedCondition(true);
                        }
                    });
            }
        }
    }

    bool IsRevealedPlayerInfected()
    {
        return RevealedRoleButton() != null && RevealedRoleButton() != this && RevealedRoleButton().IsAlive && RevealedRoleButton().RoleName == RoleNames.Infected;
    }

    void VoteAgainstRevealedPlayer()
    {
        RevealedRoleButton().AddVotesCount();
        _SinglePlayRoleButton.DisplayVotesInfo(true, RevealedRoleButton().Name);
        _SinglePlayRoleButton.HasVotedCondition(true);
    }
    #endregion

    #region RoleButtonByName
    SinglePlayRoleButton RoleButtonByName(string name)
    {
        return Array.Find(_SinglePlayGameController._RolesClass.RoleButtons.ToArray(), roleButton => roleButton.Name == name);
    }
    #endregion

    #region RandomRoleButton
    SinglePlayRoleButton RandomRoleButton()
    {
        List<SinglePlayRoleButton> roleButtons = new List<SinglePlayRoleButton>();

        foreach (var roleButton in _SinglePlayGameController._Players.ActivePlayers)
        {
            if (roleButton != _SinglePlayRoleButton) roleButtons.Add(roleButton);
        }

        int randomIndex = UnityEngine.Random.Range(0, roleButtons.Count);
        return roleButtons[randomIndex];
    }
    #endregion

    #region RevealedRoleButton
    SinglePlayRoleButton RevealedRoleButton()
    {
        return System.Array.Find(_SinglePlayGameController._RolesClass.RoleButtons.ToArray(), revealed => revealed.IsRevealed == true);
    }
    #endregion

    #region SyncPlayerVotedName_AsAiInfected
    SinglePlayRoleButton SyncPlayerVotedName_AsAiInfected(string otherPlayerName)
    {
        return System.Array.Find(_SinglePlayGameController._RolesClass.RoleButtons.ToArray(), voted => voted.Name == otherPlayerName);
    }
    #endregion
}
