
public class AIMedicHeal : SinglePlayAiController
{
    void OnEnable()
    {
        _SinglePlayGameController.OnAiNightVote += OnHeal;
    }

    void OnDisable()
    {
        _SinglePlayGameController.OnAiNightVote -= OnHeal;
    }

    void OnHeal()
    {
        if (SinglePlayGlobalConditions.IsAiMedeic(_SinglePlayRoleButton) && SinglePlayGlobalConditions.CanAiParticipateInNightVote(_SinglePlayRoleButton))
        {
            SetVoteRandomTime(0, _SinglePlayGameController._TimerClass.Timer);

            if (IsItTimeToVoteForAI())
            {
                _MedicNightAction.Vote(_SinglePlayRoleButton,
                    PlayerWhomWeWantToHeal =>
                    {
                        Heal(PlayerWhomWeWantToHeal);
                    },
                    PlayerWhomWeWantToHealNotFound =>
                    {
                        Heal(RandomRoleButton());
                    }, null);
            }
        }
    }

    void Heal(SinglePlayRoleButton Target)
    {
        Target.AIAbility(0);
        print(Target.Name + " got healed");
        _SinglePlayRoleButton.HasVotedCondition(true);
    }
}
