

public class AIOrcDayVote : SinglePlayAiController
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
        if (SinglePlayGlobalConditions.CanAiParticipateInDayVote(_SinglePlayRoleButton))
        {
            if (SinglePlayGlobalConditions.IsAiInfected(_SinglePlayRoleButton))
            {
                if (SinglePlayGlobalConditions.AmIInfected())
                {
                    InCaseIfPlayerAlsoIsAnInfected(false);
                }
                else
                {
                    InCaseIfPlayerIsNotAnInfected(false);
                }
            }
        }
    }

    protected void InCaseIfPlayerAlsoIsAnInfected(bool isNightVote)
    {
        if (HasPlayerVotedInTime())
        {
            PlayerPickedATarget(isNightVote);
        }
        else if (ShouldAIVoteIndependently())
        {
            SetVoteRandomTime(0, 4);

            if (IsItTimeToVoteForAI())
            {
                if (InfectedsDayVotesInfoContainsKey())
                {
                    AIPickedATarget(isNightVote, true);
                }
                else
                {
                    RandomTarget(isNightVote, true);
                }
            }
        }
    }

    protected void InCaseIfPlayerIsNotAnInfected(bool isNightVote)
    {
        SetVoteRandomTime(0, _SinglePlayGameController._TimerClass.Timer);

        if (IsItTimeToVoteForAI())
        {
            if (InfectedsDayVotesInfoContainsKey())
            {
                AIPickedATarget(isNightVote, false);
            }
            else
            {
                RandomTarget(isNightVote, false);
            }
        }
    }

    void PlayerPickedATarget(bool isNightVote)
    {
        SinglePlayVoteDatas.DayVotesInfo VotesInfo = _SinglePlayVoteDatas.InfectedsDayVotesInfo[RoleNames.Infected];

        SetVoteRandomTime(5, _SinglePlayGameController._TimerClass.Timer);

        if (IsItTimeToVoteForAI())
        {
            AddsVotesCount(SyncPlayerVotedName_AsAiInfected(VotesInfo.OtherPlayerName), !isNightVote, true, false);
        }
    }

    void AIPickedATarget(bool isNightVote, bool isPlayerInSameTeam)
    {
        string name = _SinglePlayVoteDatas.InfectedsDayVotesInfo[RoleNames.Infected].OtherPlayerName;

        if (RoleButtonByName(name) != _SinglePlayRoleButton && RoleButtonByName(name).IsAlive && !SinglePlayGlobalConditions.IsAiInfected(RoleButtonByName(name)))
        {
            AddsVotesCount(RoleButtonByName(name), !isNightVote, !isNightVote || isNightVote && isPlayerInSameTeam, false);
        }
    }

    void RandomTarget(bool isNightVote, bool isPlayerInSameTeam)
    {
        for (int i = 0; i < _SinglePlayGameController._RolesClass.PlayersCount; i++)
        {
            SinglePlayRoleButton RandomPlayer = RandomRoleButton();

            if (RandomPlayer.RoleName != RoleNames.Infected)
            {
                AddsVotesCount(RandomPlayer, !isNightVote, !isNightVote || isNightVote && isPlayerInSameTeam, true);
                break;
            }
        }
    }

    void AddsVotesCount(SinglePlayRoleButton Target, bool addDayVotesData, bool display, bool addInfectedsDayVotesInfo)
    {
        Target.AddVotesCount();
        print(Target.name + "/" + _SinglePlayRoleButton.Name);
        if (addDayVotesData) _SinglePlayVoteDatas.AddDayVotesData(_SinglePlayRoleButton, _SinglePlayGameController._TimerClass.DaysCount, Target);
        if (display) _SinglePlayRoleButton.DisplayVotesInfo(true, Target.Name);
        if (addInfectedsDayVotesInfo) _SinglePlayVoteDatas.AddInfectedsDayVotesInfo(_SinglePlayRoleButton.RoleName, new SinglePlayVoteDatas.DayVotesInfo(Target.Name, _SinglePlayGameController._TimerClass.DaysCount));
        _SinglePlayRoleButton.HasVotedCondition(true);
    }
}
