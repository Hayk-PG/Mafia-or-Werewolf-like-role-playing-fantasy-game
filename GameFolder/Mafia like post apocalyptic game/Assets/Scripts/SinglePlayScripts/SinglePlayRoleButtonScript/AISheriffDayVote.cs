

public class AISheriffDayVote : SinglePlayAiController
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
        Process(SinglePlayGlobalConditions.IsAiSheriff(_SinglePlayRoleButton) && SinglePlayGlobalConditions.CanAiParticipateInDayVote(_SinglePlayRoleButton));
    }

    protected virtual void Process(bool canParticipateInVote)
    {
        if (canParticipateInVote)
        {
            SetVoteRandomTime(0, _SinglePlayGameController._TimerClass.Timer);

            if (IsItTimeToVoteForAI())
            {
                _DayVoteLogic.Vote(_SinglePlayRoleButton, null,
                    SuspectNotFound =>
                    {
                        if (SuspectNotFound)
                        {
                            NoSuspect();
                        }
                    },
                    Suspect =>
                    {
                        SuspectFound(Suspect);
                    });
            }
        }
    }

    protected virtual void NoSuspect()
    {
        if (IsRevealedPlayerInfected())
        {
            VoteAgainstRevealedPlayer();
        }
        else
        {
            for (int i = 0; i < _SinglePlayGameController._RolesClass.PlayersCount; i++)
            {
                SinglePlayRoleButton RandomPlayer = RandomRoleButton();

                if (RandomPlayer != this && RandomPlayer.IsAlive)
                {
                    RandomPlayer.AddVotesCount();
                    _SinglePlayRoleButton.DisplayVotesInfo(true, RandomPlayer.Name);
                    _SinglePlayRoleButton.HasVotedCondition(true);
                    break;
                }
            }
        }
    }

    protected virtual void SuspectFound(SinglePlayRoleButton Suspect)
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
    }

    protected bool IsRevealedPlayerInfected()
    {
        return RevealedRoleButton() != null && RevealedRoleButton() != this && RevealedRoleButton().IsAlive && RevealedRoleButton().RoleName == RoleNames.Infected;
    }

    void VoteAgainstRevealedPlayer()
    {
        RevealedRoleButton().AddVotesCount();
        _SinglePlayRoleButton.DisplayVotesInfo(true, RevealedRoleButton().Name);
        _SinglePlayRoleButton.HasVotedCondition(true);
    }
}
