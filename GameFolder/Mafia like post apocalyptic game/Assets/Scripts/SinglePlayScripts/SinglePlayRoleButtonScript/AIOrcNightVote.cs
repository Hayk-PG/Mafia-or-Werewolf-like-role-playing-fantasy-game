

public class AIOrcNightVote : AIOrcDayVote
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
        if (SinglePlayGlobalConditions.CanAiParticipateInNightVote(_SinglePlayRoleButton))
        {
            if (SinglePlayGlobalConditions.IsAiInfected(_SinglePlayRoleButton))
            {
                if (SinglePlayGlobalConditions.AmIInfected())
                {
                    InCaseIfPlayerAlsoIsAnInfected(true);
                }
                else
                {
                    InCaseIfPlayerIsNotAnInfected(true);
                }
            }
        }
    }
}
