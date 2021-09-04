using Photon.Pun;
using System;
using System.Collections.Generic;

public class GameManagerPlayerVotesController : MonoBehaviourPun
{
    [Serializable] public class Votes
    {
        public Dictionary<int, int> PlayersVotesAgainst { get; set; }
        public Dictionary<int, bool[]> PlayerVoteCondition { get; set; }
        public Dictionary<int, string> AgainstWhomPlayerVoted { get; set; }
    }

    public Votes _Votes;

    void Awake()
    {
        _Votes.PlayersVotesAgainst = new Dictionary<int, int>();
        _Votes.PlayerVoteCondition = new Dictionary<int, bool[]>();
        _Votes.AgainstWhomPlayerVoted = new Dictionary<int, string>();
    }

    public void TransferPlayersVotesToTheNewMaster()
    {
        _Votes.PlayersVotesAgainst = FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayersVotesAgainst;
        _Votes.PlayerVoteCondition = FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayerVoteCondition;
        _Votes.AgainstWhomPlayerVoted = FindObjectOfType<GameManagerPlayerVotesController>()._Votes.AgainstWhomPlayerVoted;
    }
}
