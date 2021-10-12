﻿using Photon.Pun;
using System;
using System.Collections.Generic;

public class GameManagerPlayerVotesController : MonoBehaviourPun,IReset
{
    [Serializable] public class Votes
    {
        public Dictionary<int, int> PlayersVotesAgainst { get; set; }
        public Dictionary<int, bool> HealedPlayers { get; set; }
        public Dictionary<int, bool> DiscoverTheRole { get; set; }
        public Dictionary<int, int> InfectedVotesAgainst { get; set; }
        public Dictionary<int, int> SoldierVoteAgainst { get; set; }
        public Dictionary<int, bool> LizardVoteAgainst { get; set; }
        public Dictionary<int, bool[]> PlayerVoteCondition { get; set; }
        public Dictionary<int, string[]> AgainstWhomPlayerVoted { get; set; } 
    }

    public Votes _Votes;

    void Awake()
    {
        _Votes.PlayersVotesAgainst = new Dictionary<int, int>();
        _Votes.HealedPlayers = new Dictionary<int, bool>();
        _Votes.DiscoverTheRole = new Dictionary<int, bool>();
        _Votes.InfectedVotesAgainst = new Dictionary<int, int>();
        _Votes.SoldierVoteAgainst = new Dictionary<int, int>();
        _Votes.LizardVoteAgainst = new Dictionary<int, bool>();
        _Votes.PlayerVoteCondition = new Dictionary<int, bool[]>();
        _Votes.AgainstWhomPlayerVoted = new Dictionary<int, string[]>();
    }

    #region TransferPlayersVotesToTheNewMaster
    public void TransferPlayersVotesToTheNewMaster()
    {
        _Votes.PlayersVotesAgainst = FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayersVotesAgainst;
        _Votes.HealedPlayers = FindObjectOfType<GameManagerPlayerVotesController>()._Votes.HealedPlayers;
        _Votes.DiscoverTheRole = FindObjectOfType<GameManagerPlayerVotesController>()._Votes.DiscoverTheRole;
        _Votes.InfectedVotesAgainst = FindObjectOfType<GameManagerPlayerVotesController>()._Votes.InfectedVotesAgainst;
        _Votes.SoldierVoteAgainst = FindObjectOfType<GameManagerPlayerVotesController>()._Votes.SoldierVoteAgainst;
        _Votes.LizardVoteAgainst = FindObjectOfType<GameManagerPlayerVotesController>()._Votes.LizardVoteAgainst;
        _Votes.PlayerVoteCondition = FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayerVoteCondition;
        _Votes.AgainstWhomPlayerVoted = FindObjectOfType<GameManagerPlayerVotesController>()._Votes.AgainstWhomPlayerVoted;
    }
    #endregion

    #region IReset
    public void ResetWhileGameEndCoroutineIsRunning()
    {
        _Votes.AgainstWhomPlayerVoted = new Dictionary<int, string[]>();
        _Votes.DiscoverTheRole = new Dictionary<int, bool>();
        _Votes.HealedPlayers = new Dictionary<int, bool>();
        _Votes.InfectedVotesAgainst = new Dictionary<int, int>();
        _Votes.LizardVoteAgainst = new Dictionary<int, bool>();
        _Votes.PlayersVotesAgainst = new Dictionary<int, int>();
        _Votes.PlayerVoteCondition = new Dictionary<int, bool[]>();
        _Votes.SoldierVoteAgainst = new Dictionary<int, int>();
    }

    public void ResetAtTheEndOfTheGameEndCoroutine()
    {
        
    }
    #endregion
}
