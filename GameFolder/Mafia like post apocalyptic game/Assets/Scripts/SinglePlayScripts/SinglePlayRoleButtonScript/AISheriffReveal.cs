

public class AISheriffReveal : AISheriffDayVote
{
    void OnEnable()
    {
        _SinglePlayGameController.OnAiNightVote += OnReveal;
    }

    void OnDisable()
    {
        _SinglePlayGameController.OnAiNightVote -= OnReveal;
    }

    void OnReveal()
    {
        Process(SinglePlayGlobalConditions.IsAiSheriff(_SinglePlayRoleButton) && SinglePlayGlobalConditions.CanAiParticipateInNightVote(_SinglePlayRoleButton));
    }

    protected override void NoSuspect()
    {
        if (IsRevealedPlayerInfected())
        {
            Reveal(RevealedRoleButton());
        }
        else
        {
            for (int i = 0; i < _SinglePlayGameController._RolesClass.PlayersCount; i++)
            {
                if (RandomRoleButton() != this && RandomRoleButton().IsAlive)
                {
                    Reveal(RandomRoleButton());
                    break;
                }
            }
        }
    }

    protected override void SuspectFound(SinglePlayRoleButton Suspect)
    {
        if (IsRevealedPlayerInfected())
        {
            Reveal(RevealedRoleButton());
        }
        else
        {
            Reveal(Suspect);
        }
    }

    void Reveal(SinglePlayRoleButton Target)
    {
        Target.AIAbility(1);
        print(Target.Name + " got revealed");
        _SinglePlayRoleButton.HasVotedCondition(true);
    }
}
