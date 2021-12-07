﻿

public class AICommonDayVote : SinglePlayAiController
{
    void OnEnable()
    {
        _SinglePlayGameController.OnAiDayVote += OnDayVote;
    }

    void OnDisable()
    {
        _SinglePlayGameController.OnAiDayVote -= OnDayVote;
    }

    void OnDayVote()
    {
        Vote(SinglePlayGlobalConditions.CanAiParticipateInDayVote(_SinglePlayRoleButton), !SinglePlayGlobalConditions.IsAiInfected(_SinglePlayRoleButton) && !SinglePlayGlobalConditions.IsAiSheriff(_SinglePlayRoleButton));
    }

    protected virtual void Vote(bool canParticipateInVote, bool isRoleSuited)
    {
        if (canParticipateInVote)
        {
            if (isRoleSuited)
            {
                SetVoteRandomTime(0, _SinglePlayGameController._TimerClass.Timer);

                if (IsItTimeToVoteForAI())
                {
                    _DayVoteLogic.Vote(_SinglePlayRoleButton,
                            Suspect =>
                            {
                                TargetIsSet(Suspect);
                            },
                            SuspectFound =>
                            {
                                if (SuspectFound)
                                {
                                    SetRandomTarget();
                                }
                            }, null);
                }
            }
        }
    }

    protected virtual void TargetIsSet(SinglePlayRoleButton Suspect)
    {
        if (SinglePlayGlobalConditions.IsAiKing(_SinglePlayRoleButton)) Suspect.VotesCount += 2;
        else Suspect.AddVotesCount();
        _SinglePlayVoteDatas.AddDayVotesData(_SinglePlayRoleButton, _SinglePlayGameController._TimerClass.DaysCount, Suspect);
        _SinglePlayRoleButton.DisplayVotesInfo(true, Suspect.Name);
        _SinglePlayRoleButton.HasVotedCondition(true);
    }

    protected virtual void SetRandomTarget()
    {
        for (int i = 0; i < _SinglePlayGameController._RolesClass.PlayersCount; i++)
        {
            SinglePlayRoleButton RandomPlayer = RandomRoleButton();

            if (RandomPlayer != this && RandomPlayer.IsAlive)
            {
                if (SinglePlayGlobalConditions.IsAiKing(_SinglePlayRoleButton)) RandomPlayer.VotesCount += 2;
                else RandomPlayer.AddVotesCount();
                _SinglePlayVoteDatas.AddDayVotesData(_SinglePlayRoleButton, _SinglePlayGameController._TimerClass.DaysCount, RandomPlayer);
                _SinglePlayRoleButton.DisplayVotesInfo(true, RandomPlayer.Name);
                _SinglePlayRoleButton.HasVotedCondition(true);
                break;
            }
        }
    }
}
