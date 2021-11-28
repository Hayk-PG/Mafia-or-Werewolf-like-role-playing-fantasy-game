

public class AiKnightKill : AICommonDayVote
{
    void OnEnable()
    {
        _SinglePlayGameController.OnAiNightVote += OnKill;
    }

    void OnDisable()
    {
        _SinglePlayGameController.OnAiNightVote -= OnKill;
    }

    void OnKill()
    {
        Vote(SinglePlayGlobalConditions.CanAiParticipateInNightVote(_SinglePlayRoleButton), SinglePlayGlobalConditions.IsAiSoldier(_SinglePlayRoleButton));
    }

    protected override void Vote(bool canParticipateInVote, bool isRoleSuited)
    {
        base.Vote(canParticipateInVote, isRoleSuited);
    }

    protected override void TargetIsSet(SinglePlayRoleButton Suspect)
    {
        Suspect.AIAbility(2);
        print(Suspect.Name + " got killed");
        _SinglePlayRoleButton.HasVotedCondition(true);
    }

    protected override void SetRandomTarget()
    {
        for (int i = 0; i < _SinglePlayGameController._RolesClass.PlayersCount; i++)
        {
            if (RandomRoleButton() != this && RandomRoleButton().IsAlive)
            {
                RandomRoleButton().AIAbility(2);
                print(RandomRoleButton().Name + " got killed");
                _SinglePlayRoleButton.HasVotedCondition(true);
                break;
            }
        }
    }
}
