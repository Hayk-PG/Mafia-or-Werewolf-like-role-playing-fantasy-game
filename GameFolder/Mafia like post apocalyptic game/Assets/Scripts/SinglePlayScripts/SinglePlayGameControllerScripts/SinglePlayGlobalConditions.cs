using UnityEngine;

public class SinglePlayGlobalConditions : MonoBehaviour
{
    static SinglePlayGameController _SPGC { get; set; }


    void Awake()
    {
        _SPGC = GetComponent<SinglePlayGameController>();
    }

    #region Roles related
    public static bool AmIMedic()
    {
        return _SPGC.PlayerRoleButton().RoleName == RoleNames.Medic;
    }

    public static bool AmISheriff()
    {
        return _SPGC.PlayerRoleButton().RoleName == RoleNames.Sheriff;
    }

    public static bool AmISoldier()
    {
        return _SPGC.PlayerRoleButton().RoleName == RoleNames.Soldier;
    }

    public static bool AmIInfected()
    {
        return _SPGC.PlayerRoleButton().RoleName == RoleNames.Infected;
    }

    public static bool AmILizard()
    {
        return _SPGC.PlayerRoleButton().RoleName == RoleNames.Lizard;
    }

    public static bool AmIKing()
    {
        return _SPGC.PlayerRoleButton().RoleName == RoleNames.HumanKing || _SPGC.PlayerRoleButton().RoleName == RoleNames.MonsterKing;
    }

    public static bool CanParticipateInNightVote()
    {
        bool isRoleSuitable = AmIMedic() || AmISheriff() || AmISoldier() || AmIInfected() || AmILizard();

        return _SPGC.PlayerRoleButton().IsAlive &&
            !_SPGC.PlayerRoleButton().HasVoted && isRoleSuitable;
    }

    public static bool CanParticipateInDayVote()
    {
        return _SPGC.PlayerRoleButton().IsAlive && !_SPGC.PlayerRoleButton().HasVoted;
    }
    
    public static bool IsOtherPlayerActive(SinglePlayRoleButton roleButton)
    {
        return !roleButton.IsPlayer && roleButton.IsAlive;
    }

    public static bool IsPlayerInHumansTeam(SinglePlayRoleButton roleButton)
    {
        return roleButton.RoleName != RoleNames.Infected &&
               roleButton.RoleName != RoleNames.Lizard &&
               roleButton.RoleName != RoleNames.MonsterKing;
    }
    #endregion

    #region Timers related
    public static bool IsVoteTime()
    {
        return _SPGC._TimerClass.Timer <= 30;
    }
    public static bool IsPhaseResetTime()
    {
        return _SPGC._TimerClass.Timer <= 0;
    }

    #endregion

    #region AI
    public static bool CanAiParticipateInDayVote(SinglePlayRoleButton aiRoleButton)
    {
        return aiRoleButton.IsPlayer == false && aiRoleButton.IsAlive == true && !aiRoleButton.HasVoted;
    }

    public static bool IsAiMedeic(SinglePlayRoleButton aiRoleButton)
    {
        return aiRoleButton.RoleName == RoleNames.Medic;
    }

    public static bool IsAiSheriff(SinglePlayRoleButton aiRoleButton)
    {
        return aiRoleButton.RoleName == RoleNames.Sheriff;
    }

    public static bool IsAiSoldier(SinglePlayRoleButton aiRoleButton)
    {
        return aiRoleButton.RoleName == RoleNames.Soldier;
    }

    public static bool IsAiInfected(SinglePlayRoleButton aiRoleButton)
    {
        return aiRoleButton.RoleName == RoleNames.Infected;
    }

    public static bool IsAiLizard(SinglePlayRoleButton aiRoleButton)
    {
        return aiRoleButton.RoleName == RoleNames.Lizard;
    }

    public static bool IsAiKing(SinglePlayRoleButton aiRoleButton)
    {
        return aiRoleButton.RoleName == RoleNames.HumanKing || aiRoleButton.RoleName == RoleNames.MonsterKing;
    }
    #endregion
}
