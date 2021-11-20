using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VotingBaseScript: MonoBehaviour
{
    protected SinglePlayVoteDatas _SinglePlayVoteDatas { get; set; }
    protected SinglePlayGameController _SinglePlayGameController { get; set; }

    protected List<SinglePlayRoleButton> ExcludedPlayers { get; set; } = new List<SinglePlayRoleButton>();
    protected List<SinglePlayRoleButton> IncludedPlayers { get; set; } = new List<SinglePlayRoleButton>();    
    protected List<SinglePlayRoleButton> PrioritizedPlayers { get; set; } = new List<SinglePlayRoleButton>();
    protected List<SinglePlayRoleButton> MainSuspects { get; set; } = new List<SinglePlayRoleButton>();


    protected int DaysCount
    {
        get => _SinglePlayGameController._TimerClass.DaysCount;
    }
    protected int PreviousDaysCount
    {
        get => DaysCount > 0 ? DaysCount - 1 : 0;
    }


    protected void Awake()
    {
        _SinglePlayVoteDatas = FindObjectOfType<SinglePlayVoteDatas>();
        _SinglePlayGameController = FindObjectOfType<SinglePlayGameController>();
    }

    public abstract void Vote(SinglePlayRoleButton owner, Action<SinglePlayRoleButton> Vote, Action<bool> RandomVote, Action<SinglePlayRoleButton> SheriffVote);

    protected void AddMainSuspectPlayers(SinglePlayRoleButton roleButton)
    {
        if (!MainSuspects.Contains(roleButton)) MainSuspects.Add(roleButton);
    }

    protected void AddPrioritizedPlayers(SinglePlayRoleButton roleButton)
    {
        if (!PrioritizedPlayers.Contains(roleButton)) PrioritizedPlayers.Add(roleButton);   
    }

    protected void ExcludePlayers(SinglePlayRoleButton player)
    {
        if (!ExcludedPlayers.Contains(player)) ExcludedPlayers.Add(player);
    }

    protected void IncludePlayers(SinglePlayRoleButton player)
    {
        if (!IncludedPlayers.Contains(player)) IncludedPlayers.Add(player);
    }

    protected void RemoveFromList(SinglePlayRoleButton player, List<SinglePlayRoleButton> List)
    {
        if (List.Contains(player)) List.Remove(player);
    }

    protected void LoopActivePlayers(Action<SinglePlayRoleButton> Player)
    {
        foreach (var player in _SinglePlayGameController._Players.ActivePlayers)
        {
            Player?.Invoke(player);
        }
    }

    protected void LoopLostPlayers(Action<SinglePlayRoleButton> Player)
    {
        foreach (var player in _SinglePlayGameController._Players.LostPlayers)
        {
            Player?.Invoke(player);
        }
    }

    protected void LoopDayVoteData(Action<KeyValuePair<SinglePlayRoleButton, Dictionary<int, SinglePlayRoleButton>>> DayVoteData)
    {
        foreach (var data in _SinglePlayVoteDatas.DayVotesData)
        {
            DayVoteData?.Invoke(data);
        }
    }

    protected void LoopDayVoteDataValue(KeyValuePair<SinglePlayRoleButton, Dictionary<int, SinglePlayRoleButton>> DayVoteData, Action<KeyValuePair<int, SinglePlayRoleButton>> Value)
    {
        foreach (var value in DayVoteData.Value)
        {
            Value?.Invoke(value);
        }
    }

    protected bool IsOtherPlayer(SinglePlayRoleButton roleButton, SinglePlayRoleButton owner)
    {
        return roleButton != owner;
    }

    protected bool IsPlayerALive(SinglePlayRoleButton roleButton)
    {
        return _SinglePlayGameController._Players.ActivePlayers.Contains(roleButton);
    }
}
