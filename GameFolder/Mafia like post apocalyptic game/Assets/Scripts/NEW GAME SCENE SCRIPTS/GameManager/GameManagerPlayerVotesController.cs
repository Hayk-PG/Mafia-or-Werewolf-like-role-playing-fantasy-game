using Photon.Pun;
using System;
using System.Collections.Generic;

public class GameManagerPlayerVotesController : MonoBehaviourPun
{
    [Serializable] public class Votes
    {
        public Dictionary<int, int> PlayersVotesAgainst = new Dictionary<int, int>();
        public Dictionary<int, bool[]> PlayerVoteCondition = new Dictionary<int, bool[]>();
    }

    public Votes _Votes;

    void Update()
    {
        foreach (var item in _Votes.PlayersVotesAgainst)
        {
            print(item.Key + "/" + item.Value);
        }
        foreach (var item in _Votes.PlayerVoteCondition)
        {
            print(item.Key + "/" + item.Value[0] + "/" + item.Value[1]);
        }
    }

    public void TransferPlayersVotesToTheNewMaster()
    {
        _Votes.PlayersVotesAgainst = FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayersVotesAgainst;
        _Votes.PlayerVoteCondition = FindObjectOfType<GameManagerPlayerVotesController>()._Votes.PlayerVoteCondition;

        foreach (var item in _Votes.PlayerVoteCondition)
        {
            print(item + "/" + item.Value[0] + "/" + item.Value[1]);
        }
    }
}
