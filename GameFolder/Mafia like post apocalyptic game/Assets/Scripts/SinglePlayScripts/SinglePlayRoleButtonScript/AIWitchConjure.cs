
public class AIWitchConjure : AICommonDayVote
{
    void OnEnable()
    {
        _SinglePlayGameController.OnAiNightVote += OnConjure;
    }

    void OnDisable()
    {
        _SinglePlayGameController.OnAiNightVote -= OnConjure;
    }

    void OnConjure()
    {
        Vote(SinglePlayGlobalConditions.CanAiParticipateInNightVote(_SinglePlayRoleButton), SinglePlayGlobalConditions.IsAiLizard(_SinglePlayRoleButton));
    }

    protected override void TargetIsSet(SinglePlayRoleButton Suspect)
    {
        Suspect.AIAbility(4);
        print(Suspect.Name + " got conjured");
        _SinglePlayRoleButton.HasVotedCondition(true);
    }

    protected override void SetRandomTarget()
    {
        for (int i = 0; i < _SinglePlayGameController._RolesClass.PlayersCount; i++)
        {
            if (RandomRoleButton() != this && RandomRoleButton().IsAlive)
            {
                RandomRoleButton().AIAbility(4);
                print(RandomRoleButton().Name + " got conjured");
                _SinglePlayRoleButton.HasVotedCondition(true);
                break;
            }
        }
    }
}
