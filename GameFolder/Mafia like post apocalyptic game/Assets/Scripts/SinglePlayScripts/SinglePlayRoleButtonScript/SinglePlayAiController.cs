using System;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayAiController : MonoBehaviour
{
    protected SinglePlayRoleButton _SinglePlayRoleButton { get; set; }
    protected SinglePlayGameController _SinglePlayGameController { get; set; }
    protected SinglePlayVoteDatas _SinglePlayVoteDatas { get; set; }
    protected DayVoteLogic _DayVoteLogic { get; set; }
    protected MedicNightAction _MedicNightAction { get; set; }

    protected bool IsRandomSecondSet { get; set; }
    protected int RandomSecond { get; set; }

    protected void Awake()
    {
        _SinglePlayRoleButton = GetComponent<SinglePlayRoleButton>();
        _DayVoteLogic = GetComponent<DayVoteLogic>();
        _MedicNightAction = GetComponent<MedicNightAction>();

        _SinglePlayGameController = FindObjectOfType<SinglePlayGameController>();
        _SinglePlayVoteDatas = FindObjectOfType<SinglePlayVoteDatas>();       
    }

    void OnEnable()
    {
        _SinglePlayGameController.OnPhaseReset += _SinglePlayGameController_OnPhaseReset;
    }
  
    void OnDisable()
    {
        _SinglePlayGameController.OnPhaseReset -= _SinglePlayGameController_OnPhaseReset;
    }

    void _SinglePlayGameController_OnPhaseReset(bool obj)
    {
        IsRandomSecondSet = false;
    }

    #region Conditions
    protected bool HasPlayerVotedInTime()
    {
        return _SinglePlayGameController._TimerClass.Timer >= 5 && _SinglePlayGameController.PlayerRoleButton().HasVoted;
    }

    protected bool ShouldAIVoteIndependently()
    {
        return _SinglePlayGameController._TimerClass.Timer < 5 && !_SinglePlayGameController.PlayerRoleButton().HasVoted;
    }

    protected bool IsItTimeToVoteForAI()
    {
        return RandomSecond == _SinglePlayGameController._TimerClass.Timer && IsRandomSecondSet;
    }

    protected bool InfectedsDayVotesInfoContainsKey()
    {
        return _SinglePlayVoteDatas.InfectedsDayVotesInfo.ContainsKey(RoleNames.Infected);
    }

    protected void SetVoteRandomTime(int min, int max)
    {
        if (!IsRandomSecondSet)
        {
            RandomSecond = UnityEngine.Random.Range(min, max);
            IsRandomSecondSet = true;
        }
    }
    #endregion

    #region RoleButtonByName
    protected SinglePlayRoleButton RoleButtonByName(string name)
    {
        return Array.Find(_SinglePlayGameController._RolesClass.RoleButtons.ToArray(), roleButton => roleButton.Name == name);
    }
    #endregion

    #region RandomRoleButton
    protected SinglePlayRoleButton RandomRoleButton()
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
    protected SinglePlayRoleButton RevealedRoleButton()
    {
        return Array.Find(_SinglePlayGameController._RolesClass.RoleButtons.ToArray(), revealed => revealed.IsRevealed == true);
    }
    #endregion

    #region SyncPlayerVotedName_AsAiInfected
    protected SinglePlayRoleButton SyncPlayerVotedName_AsAiInfected(string otherPlayerName)
    {
        return Array.Find(_SinglePlayGameController._RolesClass.RoleButtons.ToArray(), voted => voted.Name == otherPlayerName);
    }
    #endregion
}
