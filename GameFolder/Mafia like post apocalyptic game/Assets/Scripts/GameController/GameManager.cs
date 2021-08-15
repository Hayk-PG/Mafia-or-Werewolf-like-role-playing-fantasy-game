using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPun
{
    public static GameManager instance;

    public event Action OnNightVote;
    public event Action OnDayVote;
    public event Action OnRoundChange;
    public event Action OnCalculatePlayerVotes;
    public event Action OnLastSecondsOfVoting;
    public event Action OnNight;
    public event Action OnDay;

    public bool IsNightVoteInvoked { get; set; }
    public bool IsDayVoteInvoked { get; set; }
    public bool IsRoundChangeInvoked { get; set; }
    public bool IsLastSecondsOfVotingInvoked { get; set; }
    public bool IsNightInvoked { get; set; }
    public bool IsDayInvoked { get; set; }



    void Awake()
    {
        if (PlayerBaseConditions._IsLocalPlayer)
        {
            instance = this;
        }
        else
        {
            enabled = false;
        }
    }

    void Update()
    {
        if(PlayerBaseConditions._IsMyGameManagerNotNull)
        {
            instance.InvokeVoteEvents();
            instance.InvokeRoundChangeEvent();
            instance.InvokeOnLastSecondsOfVoting();
            instance.InvokeOnNight();
            instance.InvokeOnDay();
        }
    }
  
    void InvokeVoteEvents()
    {
        if (PlayerBaseConditions._IsTimeToDayVote && !IsDayVoteInvoked)
        {
            OnDayVote?.Invoke();

            IsDayVoteInvoked = true;
            IsNightVoteInvoked = false;
            IsRoundChangeInvoked = false;
            IsLastSecondsOfVotingInvoked = false;
        }
        if (PlayerBaseConditions._IsTimeToNightVote && !IsNightVoteInvoked)
        {
            OnNightVote?.Invoke();

            IsNightVoteInvoked = true;
            IsDayVoteInvoked = false;
            IsRoundChangeInvoked = false;
            IsLastSecondsOfVotingInvoked = false;
        }
    }

    void InvokeRoundChangeEvent()
    {
        if (PlayerBaseConditions._HasRoundBeenChanged && !IsRoundChangeInvoked)
        {
            OnRoundChange?.Invoke();
            OnCalculatePlayerVotes?.Invoke();

            IsRoundChangeInvoked = true;           
        }
    }

    void InvokeOnLastSecondsOfVoting()
    {
        if(PlayerBaseConditions.IsVotesLastSeconds && !IsLastSecondsOfVotingInvoked)
        {
            OnLastSecondsOfVoting?.Invoke();

            IsLastSecondsOfVotingInvoked = true;
        }
    }

    void InvokeOnNight()
    {
        if (PlayerBaseConditions.Night && !IsNightInvoked)
        {
            OnNight?.Invoke();

            IsNightInvoked = true;
            IsDayInvoked = false;
        }
    }

    void InvokeOnDay()
    {
        if (PlayerBaseConditions.Day && !IsDayInvoked)
        {
            OnDay?.Invoke();

            IsDayInvoked = true;
            IsNightInvoked = false;
        }
    }



}
